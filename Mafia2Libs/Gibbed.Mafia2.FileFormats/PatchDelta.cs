using System;
using System.Collections.Generic;

namespace Gibbed.Mafia2.FileFormats
{
    /// <summary>
    /// Decoder for the binary-delta payloads found in classic Mafia II (.sds.patch, version 2)
    /// resource entries. Reverse-engineered from the retail Mafia2.exe delta applier
    /// (sub_1865080 / sub_185D6F0 / sub_183B5B0). A delta reconstructs a full resource from the
    /// corresponding base SDS resource.
    ///
    /// Layout (all integers big-endian):
    ///   header : flag(=1) u32, srcTotal u32, tgtTotal u32, deltaTotal u32
    ///   windows: winSrcLen u32, winTgtLen u32, winInstrLen u32, instr[winInstrLen]
    ///            (source segment is capped at 0x100000 bytes per window)
    ///
    /// Each instruction is a control byte followed by variable-width big-endian operands whose
    /// widths are 1..4 bytes, chosen by 2-bit size classes packed into the control byte:
    ///   bit7 set  -> ADD  : write literal bytes into the output window at a given offset
    ///   bit7 clear-> COPY : copy bytes from the base resource (or, for back-references, from
    ///                       already-produced output) into the output window at a given offset
    /// </summary>
    public static class PatchDelta
    {
        /// <summary>Size (in bytes) of the base resource data this delta expects (srcTotal).</summary>
        public static uint ReadSourceSize(byte[] payload)
        {
            if (payload == null || payload.Length < 16)
            {
                return 0;
            }
            return ReadBE(payload, 4, 4);
        }

        /// <summary>Applies the delta payload to the base resource data, returning the full resource.</summary>
        public static byte[] Apply(byte[] payload, byte[] baseData)
        {
            int p = 0;

            uint flag = ReadBE(payload, p, 4); p += 4;
            if (flag != 1)
            {
                throw new FormatException($"Unexpected patch delta flag {flag} (expected 1).");
            }
            uint srcTotal = ReadBE(payload, p, 4); p += 4;
            uint tgtTotal = ReadBE(payload, p, 4); p += 4;
            /* deltaTotal */ ReadBE(payload, p, 4); p += 4;

            var output = new byte[tgtTotal];
            int outPos = 0;
            int srcBase = 0; // start of the current window's source segment within baseData

            while (outPos < tgtTotal)
            {
                uint winSrc = ReadBE(payload, p, 4); p += 4;
                uint winTgt = ReadBE(payload, p, 4); p += 4;
                uint winInstr = ReadBE(payload, p, 4); p += 4;
                int instrEnd = p + (int)winInstr;

                var win = new byte[winTgt];
                int srcPos = 0;          // bytes consumed from this window's source segment
                long srcAddr = 0;        // running (delta-coded) source address accumulator
                var segments = new List<int[]>(); // recorded COPY segments {srcAddr, destOff, len}

                while (p < instrEnd)
                {
                    byte c = payload[p++];
                    if ((c & 0x80) != 0)
                    {
                        // ADD: literal bytes copied from the instruction stream into the window.
                        int dest = (int)ReadBE(payload, ref p, ((c >> 4) & 3) + 1);
                        int len = ((c & 0x40) != 0)
                            ? (c & 0xF) + 1
                            : (int)ReadBE(payload, ref p, ((c >> 2) & 3) + 1);
                        Buffer.BlockCopy(payload, p, win, dest, len);
                        p += len;
                    }
                    else
                    {
                        // COPY: from the base source, or from already-produced output (back-reference).
                        srcAddr += ReadBE(payload, ref p, ((c >> 4) & 3) + 1);
                        int reqSrc = (int)srcAddr;
                        int dest = (int)ReadBE(payload, ref p, ((c >> 2) & 3) + 1);
                        int len = (int)ReadBE(payload, ref p, (c & 3) + 1);

                        if (srcPos < reqSrc)
                        {
                            // Skip forward over unused source bytes.
                            srcPos = reqSrc;
                        }

                        int recordSrc = reqSrc;
                        if (srcPos > reqSrc)
                        {
                            // Back-reference: copy the overlapping part from prior output.
                            int back = Math.Min(srcPos - reqSrc, len);
                            ResolveBackReference(win, segments, reqSrc, dest, back);
                            dest += back;
                            recordSrc = reqSrc + back;
                            len -= back;
                        }

                        if (len > 0)
                        {
                            Buffer.BlockCopy(baseData, srcBase + srcPos, win, dest, len);
                            srcPos += len;
                        }

                        if ((c & 0x40) != 0)
                        {
                            segments.Add(new[] { recordSrc, dest, len });
                        }
                    }
                }

                Buffer.BlockCopy(win, 0, output, outPos, (int)winTgt);
                outPos += (int)winTgt;
                srcBase += (int)winSrc;
            }

            return output;
        }

        // Mirrors sub_183B5B0: resolves a copy whose source address lies within regions already
        // written to the output window, by mapping it through previously recorded COPY segments.
        private static void ResolveBackReference(byte[] win, List<int[]> segments, int srcAddr, int dest, int len)
        {
            while (len > 0)
            {
                bool matched = false;
                foreach (var seg in segments)
                {
                    int segSrc = seg[0], segDest = seg[1], segLen = seg[2];
                    if (segSrc <= srcAddr && srcAddr < segSrc + segLen)
                    {
                        int take = Math.Min(len, segSrc + segLen - srcAddr);
                        int from = srcAddr + segDest - segSrc;
                        Buffer.BlockCopy(win, from, win, dest, take);
                        len -= take;
                        srcAddr += take;
                        dest += take;
                        matched = true;
                        break;
                    }
                }
                if (!matched)
                {
                    throw new FormatException("Unresolved patch delta back-reference.");
                }
            }
        }

        private static uint ReadBE(byte[] d, int offset, int count)
        {
            uint v = 0;
            for (int i = 0; i < count; i++)
            {
                v = (v << 8) | d[offset + i];
            }
            return v;
        }

        private static uint ReadBE(byte[] d, ref int offset, int count)
        {
            uint v = ReadBE(d, offset, count);
            offset += count;
            return v;
        }
    }
}
