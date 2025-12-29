using Gibbed.Illusion.FileFormats.Hashing;
using Newtonsoft.Json;
using ResourceTypes.City;
using ResourceTypes.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils.Language;
using Utils.Logging;
using Utils.Settings;
using static ResourceTypes.Misc.StreamMapLoader;
namespace Mafia2Tool
{
    public partial class StreamEditor : Form
    {
        private CityAreas cityAreas;
        private FileInfo file;
        private StreamMapLoader stream;
        private static object clipboard;
        private bool bIsFileEdited = false;
        private Dictionary<uint, string> _knownHashes = new Dictionary<uint, string>();
        private Dictionary<uint, string> _knownHashes32 = new Dictionary<uint, string>();
        private Dictionary<ulong, string> _knownHashes64 = new Dictionary<ulong, string>();
        Dictionary<ulong, string> areaNameHashes = new Dictionary<ulong, string>();

        public StreamEditor(FileInfo file)
        {
            InitializeComponent();
            Localise();
            this.file = file;
            BuildData();
            Show();
            ToolkitSettings.UpdateRichPresence("Using the Stream editor.");
        }
        private void InitializeHashDictionary()
        {
            try
            {
                Encoding encoding;
                try
                {
                    encoding = Encoding.GetEncoding(1252);
                }
                catch
                {
                    encoding = Encoding.ASCII;
                }
                string[] names = {
                                "AREA000_1_Odevy",
                                "AREA001_0_Odevy",
                                "AREA001_PORT_SOUTHPORT",
                                "AREA002_SICCESTA_SICMESTOA",
                                "AREA002_SOUTHPORT_OYSTERBAY",
                                "AREA003_SICMESTOA_sicradnice",
                                "AREA003_SOUTHPORT_port",
                                "AREA004_SICMESTOA_SICRADNICE",
                                "AREA004_SOUTHPORT_PORT",
                                "AREA005_SICMESTOA_SICRADNICE",
                                "AREA006_1_Respray",
                                "AREA006_SOUTHPORT_port",
                                "AREA007_0_Respray",
                                "AREA007_SOUTHPORT_OYSTERBAY",
                                "AREA008_1_Fast",
                                "AREA008_SOUTHPORT_port",
                                "AREA009_0_Fast",
                                "AREA009_SOUTHPORT_port",
                                "AREA010_1_Odevy",
                                "AREA011_0_Odevy",
                                "AREA011_SOUTHPORT_TUNEL",
                                "AREA012_1_Respray",
                                "AREA012_SOUTHPORT_WESTSIDE",
                                "AREA013_0_Respray",
                                "AREA013_WESTSIDE_SOUTHPORT",
                                "AREA014_1_Gas",
                                "AREA014_WESTSIDE_uppertown",
                                "AREA015_0_Gas",
                                "AREA016_1_Odevy",
                                "AREA016_SOUTHPORT_MIDTOWN",
                                "AREA017_0_Odevy",
                                "AREA017_SOUTHPORT_midtown",
                                "AREA018_1_Bruno",
                                "AREA018_SOUTHPORT_MIDTOWN",
                                "AREA019_0_Bruno",
                                "AREA019_MIDTOWN_EASTSIDE",
                                "AREA020_SOUTHPORT_oysterbay",
                                "AREA021_SOUTHPORT_OYSTERBAY",
                                "AREA0210-HIGH-GVINTERIER",
                                "AREA022_1_Illia",
                                "AREA022_SOUTHPORT_oysterbay",
                                "AREA023_0_Illia",
                                "AREA023_SOUTHPORT_oysterbay",
                                "AREA024_1_Mona",
                                "AREA024_SOUTHPORT_MIDTOWN",
                                "AREA025_0_Mona",
                                "AREA025_SOUTHPORT_MIDTOWN",
                                "AREA026_1_Odevy",
                                "AREA026_SOUTHPORT_MIDTOWN",
                                "AREA027_0_Odevy",
                                "AREA027_SOUTHPORT_MIDTOWN",
                                "AREA028_1_Subway",
                                "AREA028_SOUTHPORT_MIDTOWN",
                                "AREA028b_1_Subway",
                                "AREA028c_1_Subway",
                                "AREA028d_1_Subway",
                                "AREA028e_1_Subway",
                                "AREA028f_1_Subway",
                                "AREA029_0_Subway",
                                "AREA029_SOUTHPORT_westside",
                                "AREA030_1_Fast",
                                "AREA030_SOUTHPORT_MIDTOWN",
                                "AREA031_0_Fast",
                                "AREA031_MIDTOWN_SOUTHPORT",
                                "AREA032_1_Gas",
                                "AREA032_MIDTOWN_SOUTHPORT",
                                "AREA033_0_Gas",
                                "AREA033_MIDTOWN_SOUTHPORT",
                                "AREA034_1_Fast",
                                "AREA034_MIDTOWN_WESTSIDE",
                                "AREA035_0_Fast",
                                "AREA035_MIDTOWN_SOUTHPORT",
                                "AREA036_1_Odevy",
                                "AREA036_MIDTOWN_OYSTERBAY",
                                "AREA037_0_Odevy",
                                "AREA037_OYSTERBAY_SOUTHPORT",
                                "AREA038_1_Irish",
                                "AREA038_OYSTERBAY_MILLS",
                                "AREA039_0_Irish",
                                "AREA039_MILLS_OYSTERBAY",
                                "AREA040_1_Harry",
                                "AREA041_0_Harry",
                                "AREA041_MIDTOWN_EASTSIDE",
                                "AREA042_1_Odevy",
                                "AREA042_MIDTOWN_OYSTERBAY",
                                "AREA043_0_Odevy",
                                "AREA043_MIDTOWN_EASTSIDE",
                                "AREA044_1_Fast",
                                "AREA044_MIDTOWN_WESTSIDE",
                                "AREA045_0_Fast",
                                "AREA045_MIDTOWN_WESTSIDE",
                                "AREA046_1_Odevy",
                                "AREA046_MIDTOWN_SOUTHPORT",
                                "AREA047_0_Odevy",
                                "AREA047_MIDTOWN_EASTSIDE",
                                "AREA048_EASTSIDE_MIDTOWN",
                                "AREA049_EASTSIDE_UPPERTOWN",
                                "AREA050_1_Respray",
                                "AREA050_EASTSIDE_ITALY",
                                "AREA051_0_Respray",
                                "AREA051_EASTSIDE_chinatown",
                                "AREA052_1_Deli",
                                "AREA052_EASTSIDE_CHINATOWN",
                                "AREA053_0_Deli",
                                "AREA053_EASTSIDE_chinatown",
                                "AREA054_1_VitoA2",
                                "AREA054_EASTSIDE_chinatown",
                                "AREA055_0_VitoA2",
                                "AREA055_EASTSIDE_MIDTOWN",
                                "AREA056_1_Respray",
                                "AREA056_EASTSIDE_MIDTOWN",
                                "AREA057_0_Respray",
                                "AREA057_UPPERTOWN_EASTSIDE",
                                "AREA058_1_Respray",
                                "AREA058_UPPERTOWN_eastside",
                                "AREA058_UPPERTOWN_WESTSIDE",
                                "AREA059_0_Respray",
                                "AREA059_OYSTERBAY_SOUTHPORT",
                                "AREA060_1_Subway",
                                "AREA060b_1_Subway",
                                "AREA061_0_Subway",
                                "AREA061_OYSTERBAY_MIDTOWN",
                                "AREA062_1_04MArcade",
                                "AREA062_SANDISLAND_HUNTERS",
                                "AREA063_0_04MArcade",
                                "AREA063_SANDISLAND_HUNTERS",
                                "AREA064_1_Odevy",
                                "AREA064_HUNTERS_SANDISLAND",
                                "AREA065_0_Odevy",
                                "AREA065_HUNTERS_SANDISLAND",
                                "AREA066_1_Respray",
                                "AREA067_0_Respray",
                                "AREA067_HUNTERS_GREENFIELD",
                                "AREA068_1_OdevyL",
                                "AREA068_HUNTERS_GREENFIELD",
                                "AREA069_0_OdevyL",
                                "AREA069_GREENFIELD_KINGSTONE",
                                "AREA070_1_Lonestar",
                                "AREA070_GREENFIELD_HUNTERS",
                                "AREA071_0_Lonestar",
                                "AREA071_GREENFIELD_KINGSTONE",
                                "AREA072_1_05Byt",
                                "AREA072_GREENFIELD_hunters",
                                "AREA073_0_05Byt",
                                "AREA073_GREENFIELD_KINGSTONE",
                                "AREA074_1_Garage",
                                "AREA074_GREENFIELD_KINGSTONE",
                                "AREA075_0_Garage",
                                "AREA075_KINGSTONE_GREENFIELD",
                                "AREA076_1_Gunshop",
                                "AREA076_KINGSTONE_GREENFIELD",
                                "AREA077_0_Gunshop",
                                "AREA077_KINGSTONE_GREENFIELD",
                                "AREA078_1_Gunshop",
                                "AREA078_KINGSTONE_greenfield",
                                "AREA079_0_Gunshop",
                                "AREA079_KINGSTONE_dipton",
                                "AREA080_1_Respray",
                                "AREA080_KINGSTONE_DIPTON",
                                "AREA081_0_Respray",
                                "AREA081_KINGSTONE_DIPTON",
                                "AREA083_DIPTON_KINGSTONE",
                                "AREA084_1_Gunshop",
                                "AREA084_DIPTON_RIVERSIDE",
                                "AREA085_0_Gunshop",
                                "AREA085_DIPTON_RIVERSIDE",
                                "AREA086_1_Gunshop",
                                "AREA086_RIVERSIDE_DIPTON",
                                "AREA087_0_Gunshop",
                                "AREA087_HUNTERS_greenfield",
                                "AREA088_1_Fredy",
                                "AREA089_0_Fredy",
                                "AREA089_UPPERTOWN_HUNTERS",
                                "AREA090_1_Subway",
                                "AREA090_UPPERTOWN_WESTSIDE",
                                "AREA090b_1_Subway",
                                "AREA090c_1_Subway",
                                "AREA091_0_Subway",
                                "AREA091_UPPERTOWN_westside",
                                "AREA092_1_Giuseppe",
                                "AREA092_UPPERTOWN_hunters",
                                "AREA093_0_Giuseppe",
                                "AREA093_UPPERTOWN_westside",
                                "AREA094_1_Gunshop",
                                "AREA094_UPPERTOWN_WESTSIDE",
                                "AREA095_0_Gunshop",
                                "AREA095_UPPERTOWN_WESTSIDE",
                                "AREA096_1_Gunshop",
                                "AREA096_UPPERTOWN_westside",
                                "AREA097_0_Gunshop",
                                "AREA097_UPPERTOWN_HIGH",
                                "AREA098_1_Gunshop",
                                "AREA098_UPPERTOWN_ITALY",
                                "AREA099_0_Gunshop",
                                "AREA099_UPPERTOWN_ITALY",
                                "AREA100_UPPERTOWN_ITALY",
                                "AREA101_UPPERTOWN_WESTSIDE",
                                "AREA102_1_Respray",
                                "AREA102_WESTSIDE_MIDTOWN",
                                "AREA103_0_Respray",
                                "AREA103_WESTSIDE_EASTSIDE",
                                "AREA104_1_Show",
                                "AREA104_WESTSIDE_EASTSIDE",
                                "AREA105_0_Show",
                                "AREA105_UPPERTOWN_WESTSIDE",
                                "AREA106_1_Odevy",
                                "AREA106_HUNTERS_UPPERTOWN",
                                "AREA107_0_Odevy",
                                "AREA108_1_VitohouseB12",
                                "AREA108_UPPERTOWN_KINGSTONE",
                                "AREA109_0_VitohouseB12",
                                "AREA110_1_Joesflat_chodba",
                                "AREA110_OYSTERBAY_CHINATOWN",
                                "AREA111_0_Joesflat_chodba",
                                "AREA111_OYSTERBAY_MILLS",
                                "AREA112_1_Franhome",
                                "AREA112_OYSTERBAY_MILLS",
                                "AREA113_0_Franhome",
                                "AREA114_OYSTERBAY_MIDTOWN",
                                "AREA115_OYSTERBAY_MILLS",
                                "AREA116_1_Bruski",
                                "AREA116_OYSTERBAY_MILLS",
                                "AREA117_0_Bruski",
                                "AREA117_OYSTERBAY_CHINATOWN",
                                "AREA118_1_Erik_chodba",
                                "AREA118_ITALY_UPPERTOWN",
                                "AREA119_0_Erik_chodba",
                                "AREA119_ITALY_UPPERTOWN",
                                "AREA120_1_Gunshop",
                                "AREA120_ITALY_UPPERTOWN",
                                "AREA121_0_Gunshop",
                                "AREA121_ITALY_uppertown",
                                "AREA122_1_Odevy",
                                "AREA122_ITALY_EASTSIDE",
                                "AREA123_0_Odevy",
                                "AREA123_ITALY_eastside",
                                "AREA124_1_03Price",
                                "AREA124_ITALY_eastside",
                                "AREA125_0_03Price",
                                "AREA125_ITALY_eastside",
                                "AREA126_ITALY_MILLNEW",
                                "AREA127_ITALY_MILLNEW",
                                "AREA128_1_Respray",
                                "AREA129_0_Respray",
                                "AREA129_ITALY_MILLNEW",
                                "AREA130_1_Respray",
                                "AREA130_MILLNEW_ITALY",
                                "AREA131_0_Respray",
                                "AREA131_CHINATOWN_MILLNEW",
                                "AREA132_ITALY_MILLNEW",
                                "AREA133_ITALY_MILLN",
                                "AREA134_ITALY_MILLNEW",
                                "AREA135_ITALY_MILLNEW",
                                "AREA136_1_Gunshop",
                                "AREA136_ITALY_MILLNEW",
                                "AREA137_0_Gunshop",
                                "AREA137_ITALY_UPPERTOWN",
                                "AREA138_ITALY_MILLNEW",
                                "AREA139_ITALY_millnew",
                                "AREA140_1_ElGreco",
                                "AREA140_ITALY_uppertown",
                                "AREA141_0_ElGreco",
                                "AREA141_MILLNEW_ITALY",
                                "AREA142_1_Gvexterier",
                                "AREA143_0_Gvexterier",
                                "AREA143_SANDISLAND_TUNEL",
                                "AREA144_1_SHkanal",
                                "AREA144_SANDISLAND_HUNTERS",
                                "AREA145_0_SHkanal",
                                "AREA145_SANDISLAND_HUNTERS",
                                "AREA146_1_Falcone_satna",
                                "AREA147_0_Falcone_satna",
                                "AREA147_OYSTERBAY_chinatown",
                                "AREA148_1_Psycho",
                                "AREA148_MILLS_OYSTERBAY",
                                "AREA149_0_Psycho",
                                "AREA149_EASTSIDE_CHINATOWN",
                                "AREA150_1_Vitovila",
                                "AREA150_CHINATOWN_OYSTERBAY",
                                "AREA151_0_Vitovila",
                                "AREA151_CHINATOWN_SEAGIFT",
                                "AREA152_CHINATOWN_MILLN",
                                "AREA153_CHINATOWN_OYSTERBAY",
                                "AREA154_1_15Planetex",
                                "AREA155_0_15Planetex",
                                "AREA155_CHINATOWN_OYSTERBAY",
                                "AREA156_1_VitoA1",
                                "AREA156_CHINATOWN_MILLNEW",
                                "AREA157_0_VitoA1",
                                "AREA159_UPPERTOWN_HIGH",
                                "AREA160_HIGH_hill",
                                "AREA161_0_SGoffice",
                                "AREA161_HIGH_UPPERTOWN",
                                "AREA162_1_SGoffice",
                                "AREA162_HIGH_uppertown",
                                "AREA163_0_Argaraz",
                                "AREA163_HIGH_HILL",
                                "AREA164_1_Argaraz",
                                "AREA164_HIGH_GVINTERIER",
                                "AREA165_0_Derek",
                                "AREA165_HIGH_uppertown",
                                "AREA166_1_Derek",
                                "AREA166_HIGH_UPPERTOWN",
                                "AREA167_0_Odevy",
                                "AREA167_HIGH_GVINTERIER",
                                "AREA168_1_Odevy",
                                "AREA168_ITALY_RIVERSIDE",
                                "AREA169_0_Gas",
                                "AREA169_RIVERSIDE_italy",
                                "AREA170_1_Gas",
                                "AREA170_ITALY_MILLN",
                                "AREA171_0_Gas",
                                "AREA171_ITALY_MILLN",
                                "AREA172_1_Gas",
                                "AREA172_MILLNEW_MILLN",
                                "AREA173_0_Odevy",
                                "AREA174_1_Odevy",
                                "AREA175_0_Gunshop",
                                "AREA176_1_Gunshop",
                                "AREA176_MILLN_MILLS",
                                "AREA177_0_Respray",
                                "AREA177_CHINATOWN_mills",
                                "AREA178_1_Respray",
                                "AREA178_CHINATOWN_MILLS",
                                "AREA179_0_Odevy",
                                "AREA179_MILLS_chinatown",
                                "AREA180_1_Odevy",
                                "AREA180_MILLN_MILLS",
                                "AREA181_0_Illia",
                                "AREA181_MILLS_MILLN",
                                "AREA182_1_Illia",
                                "AREA182_MILLS_MILLN",
                                "AREA183_0_Gunshop",
                                "AREA183_MILLS_MILLN",
                                "AREA184_1_Gunshop",
                                "AREA184_MILLS_MILLN",
                                "AREA186_MILLN_CHINATOWN",
                                "AREA187_CHINATOWN_MILLN",
                                "AREA188_MILLN_MILLS",
                                "AREA189_0_Respray",
                                "AREA189_CHINATOWN_milln",
                                "AREA190_1_Respray",
                                "AREA190_CHINATOWN_MILLN",
                                "AREA191_0_Gas",
                                "AREA191_MILLN_CHINATOWN",
                                "AREA192_1_Gas",
                                "AREA192_MILLN_MILLS",
                                "AREA193_MILLNEW_MILLN",
                                "AREA194_MILLN_foundry",
                                "AREA195_0_Illia",
                                "AREA195_MILLN_FOUNDRY",
                                "AREA196_1_Illia",
                                "AREA196_FOUNDRY_MILLN",
                                "AREA197_0_Maria_Agnelo",
                                "AREA197_MILLN_MILLNEW",
                                "AREA198_1_Maria_Agnelo",
                                "AREA198_MILLN_MILLS",
                                "AREA199_0_04Roof_MArcade",
                                "AREA199_MILLN_MILLS",
                                "AREA200_1_04Roof_MArcade",
                                "AREA200_MILLN_mills",
                                "AREA201_0_construction_prizemi",
                                "AREA201_EASTSIDE_ITALY",
                                "AREA202_1_construction_prizemi",
                                "AREA202_EASTSIDE_chinatown",
                                "AREA203_0_cathouse",
                                "AREA203_CHINATOWN_EASTSIDE",
                                "AREA204_1_cathouse",
                                "AREA204_WESTSIDE_UPPERTOWN",
                                "AREA205_0_triads0",
                                "AREA205_HIGH_UPPERTOWN",
                                "AREA206_1_triads0",
                                "AREA206_HIGH_UPPERTOWN",
                                "AREA207_SOUTHPORT_OYSTERBAY",
                                "AREA208_OYSTERBAY_SOUTHPORT",
                                "AREA209_0_Gas",
                                "AREA209_OYSTERBAY_SOUTHPORT",
                                "AREA210_1_Gas",
                                "AREA210_OYSTERBAY_MILLS",
                                "AREA211_MILLS_OYSTERBAY",
                                "AREA212_MILLS_oysterbay",
                                "AREA213_0_vitoa3shop",
                                "AREA213_MILLS_oysterbay",
                                "AREA214_1_vitoa3shop",
                                "AREA214_MILLS_MILLN",
                                "AREA215_0_Gas",
                                "AREA215_MILLS_MILLN",
                                "AREA216_1_Gas",
                                "AREA216_MILLS_MILLN",
                                "AREA217_MILLS_OYSTERBAY",
                                "AREA218_MILLS_OYSTERBAY",
                                "AREA219_0_Fast",
                                "AREA219_MILLS_OYSTERBAY",
                                "AREA220_1_Fast",
                                "AREA220_MILLS_OYSTERBAY",
                                "AREA221_1_04Roof_MArcade",
                                "AREA221_MILLS_OYSTERBAY",
                                "AREA222_1_crazy_horse",
                                "AREA222_MILLS_milln",
                                "AREA223_0_crazy_horse",
                                "AREA223_MILLS_MILLN",
                                "AREA224_1_crazy_horse2",
                                "AREA224_MILLS_MILLN",
                                "AREA225_0_crazy_horse2",
                                "AREA225_MILLS_OYSTERBAY",
                                "AREA226_1_Joesflat_chodba_b",
                                "AREA226_MILLS_OYSTERBAY",
                                "AREA227_0_Joesflat_chodba_b",
                                "AREA228_0_l_garage",
                                "AREA229_1_l_garage",
                                "AREA229_MILLS_MILLN",
                                "AREA230_0_mtrain",
                                "AREA230_MILLS_OYSTERBAY",
                                "AREA231_1_mtrain",
                                "AREA231_MILLS_oysterbay",
                                "AREA232_MILLS_oysterbay",
                                "AREA233_CHINATOWN_MILLNEW",
                                "AREA234_0_SHkanal_z",
                                "AREA234_CHINATOWN_eastside",
                                "AREA235_1_SHkanal_z",
                                "AREA235_MIDTOWN_WESTSIDE",
                                "AREA236_0_dlc_printery_storage",
                                "AREA236_HILL_HIGH",
                                "AREA237_1_dlc_printery_storage",
                                "AREA237_UPPERTOWN_PRICEOFFICE",
                                "AREA238_UPPERTOWN_ITALY",
                                "AREA239_UPPERTOWN_PRICEOFFICE",
                                "AREA240_UPPERTOWN_ITALY",
                                "AREA241_UPPERTOWN_ITALY",
                                "AREA242_1_dlc_cathouse_chodba",
                                "AREA242_UPPERTOWN_ITALY",
                                "AREA243_0_dlc_cathouse_chodba",
                                "AREA243_UPPERTOWN_ITALY",
                                "AREA244_1_dlc_lokace_supermarket",
                                "AREA244_UPPERTOWN_italy",
                                "AREA245_0_dlc_lokace_supermarket",
                                "AREA245_UPPERTOWN_WESTSIDE",
                                "AREA246_UPPERTOWN_WESTSIDE",
                                "AREA247_UPPERTOWN_ITALY",
                                "AREA248_1_dlc_port",
                                "AREA248_UPPERTOWN_ITALY",
                                "AREA249_0_dlc_port",
                                "AREA249_UPPERTOWN_ITALY",
                                "AREA250_ITALY_CHINATOWN",
                                "AREA251_0_dlc_hotel_entrance",
                                "AREA251_ITALY_MILLNEW",
                                "AREA252_ITALY_EASTSIDE",
                                "AREA253_CHINATOWN_ITALY",
                                "AREA254_UPPERTOWN_ITALY",
                                "AREA255_UPPERTOWN_ITALY",
                                "AREA256_ITALY_UPPERTOWN",
                                "AREA257_ITALY_UPPERTOWN",
                                "AREA258_UPPERTOWN_ITALY",
                                "AREA259_DIPTON_UPPERTOWN",
                                "AREA260_CHINATOWN_ITALY",
                                "AREA261_CHINATOWN_eastside",
                                "AREA262_CHINATOWN_EASTSIDE",
                                "AREA263_UPPERTOWN_DIPTON",
                                "AREA264_FOUNDRY_MILLN",
                                "AREA267_FOUNDRY_milln",
                                "AREA269_FOUNDRY_MILLN",
                                "AREA270_FOUNDRY_MILLN",
                                "AREA271_FOUNDRY_FOUNDRYINT",
                                "AREA272_GVINTERIER_high",
                                "AREA273_HIGH_GVINTERIER",
                                "AREA274_RIVERSIDE_SHEXTERIER",
                                "AREA275_RIVERSIDE_SHEXTERIER",
                                "AREA276_RIVERSIDE_SHEXTERIER",
                                "AREA278_RIVERSIDE_DIPTON",
                                "AREA279_ITALY_riverside",
                                "AREA280_ITALY_MILLNEW",
                                "AREA281_ITALY_MILLNEW",
                                "AREA282_JOESFLAT_ITALY",
                                "AREA283_JOESFLAT_ITALY",
                                "AREA284_EASTSIDE_MIDTOWN",
                                "AREA285_EASTSIDE_MIDTOWN",
                                "AREA286_FALCONE_EASTSIDE",
                                "AREA287_KINGSTONE_dipton",
                                "AREA288_KINGSTONE_dipton",
                                "AREA289_KINGSTONE_dipton",
                                "AREA290_KINGSTONE_ERIK",
                                "AREA291_HILL_PLANETIN",
                                "AREA292_HILL_HIGH",
                                "AREA293_HILL_HIGH",
                                "AREA294_TRIADS1_prazdna",
                                "AREA295_TRIADS1_triads2",
                                "AREA296_TRIADS1_TRIADS2",
                                "AREA297_TRIADS1_TRIADS2",
                                "AREA298_TRIADS1_TRIADS2",
                                "AREA299_TRIADS2_TRIADS1",
                                "AREA300_TRIADS2_triads1",
                                "AREA301_TRIADS2_chinatown",
                                "AREA302_TRIADS2_chinatown",
                                "AREA303_TRIADS2_CHINATOWN",
                                "AREA304_TRIADS2_CHINATOWN",
                                "AREA306_CHINATOWN_millnew",
                                "AREA307_TRIADS2_chinatown",
                                "AREA308_TRIADS2_chinatown",
                                "AREA309_TRIADS2_chinatown",
                                "AREA310_CHINATOWN_MILLNEW",
                                "AREA311_CHINATOWN_MILLNEW",
                                "AREA312_CHINATOWN_MILLNEW",
                                "AREA313_MIDTOWN_SOUTHPORT",
                                "AREA314_MIDTOWN_WESTSIDE",
                                "AREA315_MIDTOWN_WESTSIDE",
                                "AREA316_ARPRADELNA_MIDTOWN",
                                "AREA317_ARPRADELNA_arpatro",
                                "AREA318_ARPRADELNA_arpatro",
                                "AREA320_arpatro_arpradelna",
                                "AREA321_ARPATRO_arstrecha",
                                "AREA322_ARPATRO_arstrecha",
                                "AREA324_ARSTRECHA_ARPATRO",
                                "AREA325_ARSTRECHA_arpatro",
                                "AREA326_ARSTRECHA_ARPATRO",
                                "AREA327_ARPATRO_arstrecha",
                                "AREA331_MILLNEW_CHINATOWN",
                                "AREA332_MILLNEW_MILLN",
                                "AREA333_MILLNEW_ITALY",
                                "AREA334_MILLNEW_CHINATOWN",
                                "AREA335_MILLNEW_ITALY",
                                "AREA336_MILLNEW_CHINATOWN",
                                "AREA337_ITALY_eastside",
                                "AREA338_SANDISLAND_TUNEL",
                                "AREA339_SOUTHPORT_TUNEL",
                                "AREA340_GREENFIELD_KINGSTONE",
                                "AREA341_GREENFIELD_KINGSTONE",
                                "AREA342_KINGSTONE_DIPTON",
                                "AREA343_KINGSTONE_DIPTON",
                                "AREA344_KINGSTONE_DIPTON",
                                "AREA345_KINGSTONE_DIPTON",
                                "AREA346_KINGSTONE_GREENFIELD",
                                "AREA347_KINGSTONE_DIPTON",
                                "AREA348_KINGSTONE_DIPTON",
                                "AREA349_KINGSTONE_DIPTON",
                                "AREA350_KINGSTONE_GREENFIELD",
                                "AREA351_HIGH_DIPTON",
                                "AREA352_HIGH_DIPTON",
                                "AREA353_DIPTON_HIGH",
                                "AREA354_DIPTON_RIVERSIDE",
                                "AREA355_DIPTON_KINGSTONE",
                                "AREA356_DIPTON_RIVERSIDE",
                                "AREA357_MIDTOWN_OYSTERBAY",
                                "AREA359_OYSTERBAY_MILLS",
                                "AREA360_EASTSIDE_MIDTOWN",
                                "AREA365_MILLN_ITALY",
                                "AREA366_WESTSIDE_MIDTOWN",
                                "AREA367_WESTSIDE_MIDTOWN",
                                "AREA368_WESTSIDE_MIDTOWN",
                                "AREA369_WESTSIDE_MIDTOWN",
                                "AREA371_WESTSIDE_MIDTOWN",
                                "AREA372_WESTSIDE_MARKETARCADE",
                                "AREA376_WESTSIDE_MIDTOWN",
                                "AREA377_WESTSIDE_MIDTOWN",
                                "AREA379_DISTILLERY_SANDISLAND",
                                "AREA380_UPPERTOWN_PRICEOFFICE",
                                "AREA381_EASTSIDE_FALCONE",
                                "AREA383_HUNTERS_GREENFIELD",
                                "AREA384_HUNTERS_GREENFIELD",
                                "AREA385_GREENFIELD_hunters",
                                "AREA386_FALCONE_EASTSIDE",
                                "AREA389_CHINATOWN_MILLNEW",
                                "AREA390_TRIADS2_chinatown",
                                "AREA391_TRIADS2_chinatown",
                                "AREA393_EASTSIDE_WESTSIDE",
                                "AREA394_EASTSIDE_MIDTOWN",
                                "AREA395_WESTSIDE_MIDTOWN",
                                "AREA396_MIDTOWN_EASTSIDE",
                                "AREA398_OYSTERBAY_MIDTOWN",
                                "AREA399_UPPERTOWN_westside",
                                "AREA400_UPPERTOWN_westside",
                                "AREA401_UPPERTOWN_westside",
                                "AREA402_UPPERTOWN_VITOA3CITY",
                                "AREA403_UPPERTOWN_VITOA3CITY",
                                "AREA407_RIVERSIDE_SHEXTERIER",
                                "AREA408_RIVERSIDE_SHEXTERIER",
                                "AREA409_SHEXTERIER_SHINTERIER",
                                "AREA410_CHINATOWN_SEAGIFT",
                                "AREA411_OYSTERBAY_CHINATOWN",
                                "AREA412_SOUTHPORT_PORT",
                                "AREA413_ITALY_JOESFLAT",
                                "AREA414_ITALY_JOESFLAT",
                                "AREA415_JOESFLAT_ITALY",
                                "AREA417_CHINATOWN_MILLNEW",
                                "AREA418_CHINATOWN_MILLNEW",
                                "AREA419_MIDTOWN_SOUTHPORT",
                                "AREA420_SOUTHPORT_MIDTOWN",
                                "AREA421_SOUTHPORT_MIDTOWN",
                                "AREA422_CONSTRUCTIONSITE_MIDTOWN",
                                "AREA423_SOUTHPORT_MIDTOWN",
                                "AREA425_MIDTOWN_SOUTHPORT",
                                "AREA426_MIDTOWN_SOUTHPORT",
                                "AREA427_MIDTOWN_SOUTHPORT",
                                "AREA428_HILL_HIGH",
                                "AREA429_EASTSIDE_CHINATOWN",
                                "AREA430_EASTSIDE_MIDTOWN",
                                "AREA431_OYSTERBAY_MILLS",
                                "AREA432_OYSTERBAY_chinatown",
                                "AREA433_ITALY_millnew",
                                "AREA434_ITALY_millnew",
                                "AREA435_ITALY_uppertown",
                                "AREA436_UPPERTOWN_DIPTON",
                                "AREA438_WESTSIDE_southport",
                                "AREA439_HIGH_GVINTERIER",
                                "AREA440_HIGH_GVINTERIER",
                                "AREA441_HIGH_GVINTERIER",
                                "AREA442_HIGH_GVINTERIER",
                                "AREA443_GVINTERIER_high",
                                "AREA444_HIGH_GVINTERIER",
                                "AREA445_MIDTOWN_SOUTHPORT",
                                "AREA446_SOUTHPORT_MIDTOWN",
                                "AREA447_MIDTOWN_EASTSIDE",
                                "AREA448_ITALY_UPPERTOWN",
                                "AREA450_SOUTHPORT_WESTSIDE",
                                "AREA451_WESTSIDE_HUNTERS",
                                "AREA452_WESTSIDE_HUNTERS",
                                "AREA455_CONSTRUCTIONSITE_CONSTRSTRECHA",
                                "AREA459_KINGSTONE_dipton",
                                "AREA460_KINGSTONE_dipton",
                                "AREA464_PORT_SOUTHPORT",
                                "AREA465_MIDTOWN_WESTSIDE",
                                "AREA466_ITALY_JOESFLATVARA",
                                "AREA467_JOESFLATVARA_ITALY",
                                "AREA468_JOESFLATVARA_ITALY",
                                "AREA469_JOESFLATVARA_ITALY",
                                "AREA470_ITALY_JOESFLATVARA",
                                "AREA471_ITALY_JOESFLATVARB",
                                "AREA472_JOESFLATVARB_ITALY",
                                "AREA473_JOESFLATVARB_ITALY",
                                "AREA474_JOESFLATVARB_ITALY",
                                "AREA475_ITALY_JOESFLATVARB",
                                "AREA477_WESTSIDE_MIDTOWN",
                                "AREA478_PRISONCELBATH_PRISONI",
                                "AREA479_PRISONCELBATH_PRISONE",
                                "AREA480_PRISONE_PRISONCELBATH",
                                "AREA481_PRISONE_PRISONI",
                                "AREA482_PRISONI_PRISONCELBATH",
                                "AREA483_PRISONI_PRISONE",
                                "AREA484_MIDTOWN_SOUTHPORT",
                                "AREA485_SOUTHPORT_WESTSIDE",
                                "AREA486_CHINATOWN_millnew",
                                "AREA487_ITALY_eastside",
                                "AREA488_SEAGIFT_CHINATOWN",
                                "AREA489_CHINATOWN_SEAGIFT",
                                "AREA490_MILLN_MILLNEW",
                                "AREA491_EASTSIDE_MIDTOWN",
                                "AREA492_EASTSIDE_MIDTOWN",
                                "AREA493_PRAZDNA_PRAZDNA",
                                "AREA494_EASTSIDE_MIDTOWN",
                                "AREA495_MIDTOWN_WESTSIDE",
                                "AREA497_UPPERTOWN_ITALY",
                                "AREA498_EASTSIDE_UPPERTOWN",
                                "AREA499_PORT_SOUTHPORT",
                                "AREA500_PORT_SOUTHPORT",
                                "AREA501_SOUTHPORT_PORT",
                                "AREA502_SOUTHPORT_PORT",
                                "AREA503_SOUTHPORT_PORT",
                                "AREA505_HUNTERS_GREENFIELDF",
                                "AREA506_HUNTERS_GREENFIELDF",
                                "AREA507_GREENFIELDF_KINGSTONE",
                                "AREA509_GREENFIELDF_HUNTERS",
                                "AREA510_GREENFIELDF_KINGSTONE",
                                "AREA511_GREENFIELDF_hunters",
                                "AREA512_GREENFIELDF_KINGSTONE",
                                "AREA513_GREENFIELDF_KINGSTONE",
                                "AREA514_KINGSTONE_GREENFIELDF",
                                "AREA515_KINGSTONE_GREENFIELDF",
                                "AREA516_KINGSTONE_GREENFIELDF",
                                "AREA517_KINGSTONE_greenfieldf",
                                "AREA518_GREENFIELDF_KINGSTONE",
                                "AREA519_GREENFIELDF_KINGSTONE",
                                "AREA520_KINGSTONE_GREENFIELDF",
                                "AREA521_KINGSTONE_GREENFIELDF",
                                "AREA523_HUNTERS_GREENFIELDF",
                                "AREA524_HUNTERS_GREENFIELDF",
                                "AREA525_GREENFIELDF_hunters",
                                "AREA527_SANDISLAND_TUNEL",
                                "AREA528_SOUTHPORT_TUNEL",
                                "AREA529_PRISONE_PRISONPR",
                                "AREA530_PRISONI_PRISONPR",
                                "AREA531_PRISONPR_PRISONE",
                                "AREA533_SOUTHPORT_MIDTOWN",
                                "AREA534_MILLN_MILLNEW",
                                "AREA535_UPPERTOWN_WESTSIDE",
                                "AREA544_CONSTRUCTIONSITE_CONSTRSTRECHA",
                                "AREA546_GREENFIELDF_HUNTERS",
                                "AREA547_GREENFIELD_HUNTERS",
                                "AREA548_DLCPRINTERY_MILLS",
                                "AREA551_DLCPRINTERY_MILLS",
                                "AREA552_DLCPRINTERY_MILLS",
                                "AREA553_DLCPRINTERY_MILLS",
                                "AREA554_DLCPRINTERY_MILLS",
                                "AREA555_MILLS_OYSTERBAY",
                                "AREA557_MILLS_MILLN",
                                "AREA558_KINGSTONE_GREENFIELD",
                                "AREA559_KINGSTONE_GREENFIELDF",
                                "AREA560_GREENFIELD_HUNTERS",
                                "AREA565_OYSTERBAY_DLCCATHOUSE"
                };
                foreach (string name in names)
                {
                    try
                    {
                        uint hash32 = FNV32.Hash(name, encoding);
                        ulong hash64 = FNV64.Hash(name, encoding);
                        _knownHashes32[hash32] = name;
                        _knownHashes64[hash64] = name;
                    }
                    catch (Exception hashEx)
                    {
                        Console.WriteLine($"Error processing name '{name}': {hashEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Localise()
        {
            Text = Language.GetString("$STREAM_EDITOR_TITLE");
            fileToolButton.Text = Language.GetString("$FILE");
            saveToolStripMenuItem.Text = Language.GetString("$SAVE");
            reloadToolStripMenuItem.Text = Language.GetString("$RELOAD");
            exitToolStripMenuItem.Text = Language.GetString("$EXIT");
            AddLineButton.Text = Language.GetString("$ADD_LINE");
            DeleteLineButton.Text = Language.GetString("$DELETE_LINE");
            DuplicateLine.Text = Language.GetString("$DUPLICATE_LINE");
            MoveItemDownButton.Text = Language.GetString("$MOVE_DOWN");
            MoveItemUpButton.Text = Language.GetString("$MOVE_UP");
        }

        private void Sort(List<StreamLoader> loaders)
        {
            for (int i = 0; i < loaders.Count - 1; i++)
            {
                for (int j = i + 1; j < loaders.Count; j++)
                {
                    if (loaders[i].start > loaders[j].start)
                    {
                        StreamLoader temp = loaders[i];
                        loaders[i] = loaders[j];
                        loaders[j] = temp;
                    }
                }
            }
        }

        private void UpdateStream()
        {
            List<StreamLine> lines = new List<StreamLine>();
            List<StreamLoader> loaders = new List<StreamLoader>();
            Dictionary<int, StreamLoader> currentLoaders = new Dictionary<int, StreamLoader>();
            Dictionary<int, bool> temp = new Dictionary<int, bool>();
            foreach (TreeNode node in linesTree.Nodes)
            {
                StreamHeaderGroup HeaderGroup = (StreamHeaderGroup)node.Tag;
                foreach (TreeNode child in node.Nodes)
                {
                    StreamLine line = (child.Tag as StreamLine);
                    line.lineID = lines.Count;
                    line.Group = HeaderGroup.HeaderName;
                    lines.Add(line);
                    temp.Clear();
                    foreach (var loader in currentLoaders)
                    {
                        temp[loader.Key] = false;
                    }
                    foreach (var loader in currentLoaders)
                    {
                        foreach (var load in line.loadList)
                        {
                            if (loader.Key == load.GetHashCode())
                            {
                                temp[loader.Key] = true;
                                break;
                            }
                        }
                    }
                    int i = 0;
                    while (i < temp.Count)
                    {
                        var item = temp.ElementAt(i);
                        if (item.Value == false)
                        {
                            loaders.Add(currentLoaders[item.Key]);
                            currentLoaders.Remove(item.Key);
                            temp.Remove(item.Key);
                        }
                        else
                        {
                            i++;
                        }
                    }
                    foreach (StreamLoader loader in line.loadList)
                    {
                        int hash = loader.GetHashCode();
                        if (!currentLoaders.ContainsKey(hash))
                        {
                            loader.start = line.lineID;
                            loader.end = line.lineID;
                            currentLoaders.Add(hash, loader);
                            temp[hash] = true;
                        }
                        else
                        {
                            currentLoaders[hash].end = line.lineID;
                        }
                    }
                }
            }
            loaders.AddRange(currentLoaders.Values);
            Sort(loaders);
            Dictionary<int, List<StreamLoader>> organised = new Dictionary<int, List<StreamLoader>>();
            List<StreamGroup> groups = new List<StreamGroup>();
            for (int i = 0; i < groupTree.Nodes.Count; i++)
            {
                var group = (groupTree.Nodes[i].Tag as StreamGroup);
                if (group != null)
                {
                    organised.Add(i, new List<StreamLoader>());
                    groups.Add(group);
                }
            }
            foreach (StreamLoader loader in loaders)
            {
                bool assigned = false;
                for (int i = 0; i < groups.Count; i++)
                {
                    var group = groups[i];
                    if (loader.PreferredGroup == group.Name)
                    {
                        loader.AssignedGroup = group.Name;
                        loader.GroupID = i;
                        loader.Type = group.Type;
                        assigned = true;
                        break;
                    }
                }
                if (!assigned)
                {
                    for (int i = 0; i < groups.Count; i++)
                    {
                        var group = groups[i];
                        if (loader.AssignedGroup == group.Name && loader.Type == group.Type)
                        {
                            loader.GroupID = i;
                            assigned = true;
                            break;
                        }
                    }
                }
                if (!assigned && loader.Type != GroupTypes.Null)
                {
                    for (int i = 0; i < groups.Count; i++)
                    {
                        var group = groups[i];
                        if (group.Type == loader.Type)
                        {
                            loader.GroupID = i;
                            loader.AssignedGroup = group.Name;
                            assigned = true;
                            break;
                        }
                    }
                }
                if (!assigned && groups.Count > 0)
                {
                    loader.GroupID = 0;
                    loader.AssignedGroup = groups[0].Name;
                    loader.Type = groups[0].Type;
                }
                if (organised.ContainsKey(loader.GroupID))
                {
                    organised[loader.GroupID].Add(loader);
                }
                else
                {
                    organised.Add(loader.GroupID, new List<StreamLoader> { loader });
                }
            }
            List<StreamLoader> streamLoaders = new List<StreamLoader>();
            foreach (KeyValuePair<int, List<StreamLoader>> pair in organised.OrderBy(x => x.Key))
            {
                if (pair.Key < groups.Count)
                {
                    var group = groups[pair.Key];
                    group.startOffset = streamLoaders.Count;
                    streamLoaders.AddRange(pair.Value);
                    group.endOffset = pair.Value.Count;
                }
            }
            stream.Lines = lines.ToArray();
            stream.Groups = groups.ToArray();
            stream.Loaders = streamLoaders.ToArray();
        }

        private string FindNameByHash(ulong hash64)
        {
            if (areaNameHashes.TryGetValue(hash64, out string name)) return name;
            if (_knownHashes64.TryGetValue(hash64, out name)) return name;
            uint hash32 = (uint)(hash64 & 0xFFFFFFFF);
            if (_knownHashes32.TryGetValue(hash32, out name)) return name;
            return null;
        }

        private void BuildData()
        {
            if (_knownHashes.Count == 0) InitializeHashDictionary();
            linesTree.Nodes.Clear();
            blockView.Nodes.Clear();
            groupTree.Nodes.Clear();
            PropertyGrid_Stream.SelectedObject = null;
            stream = new StreamMapLoader(file);
            for (int i = 0; i < stream.GroupHeaders.Length; i++)
            {
                TreeNode node = new TreeNode("group" + i);
                node.Text = stream.GroupHeaders[i];
                StreamHeaderGroup HeaderGroup = new StreamHeaderGroup();
                HeaderGroup.HeaderName = node.Text;
                node.Tag = HeaderGroup;
                linesTree.Nodes.Add(node);
            }
            for (int i = 0; i < stream.Groups.Length; i++)
            {
                var group = stream.Groups[i];
                TreeNode node = new TreeNode();
                node.Name = "GroupLoader" + i;
                node.Text = $"[{i}] {group.Name} ({(int)group.Type})";
                node.Tag = group;
                for (int x = group.startOffset; x < group.startOffset + group.endOffset; x++)
                {
                    if (x < stream.Loaders.Length)
                    {
                        var loader = stream.Loaders[x];
                        loader.AssignedGroup = group.Name;
                        loader.GroupID = i;
                        TreeNode loaderNode = new TreeNode();
                        loaderNode.Name = $"Loader_{x}";
                        loaderNode.Text = $"[{loader.start}-{loader.end}] {loader.Path}";
                        loaderNode.Tag = loader;
                        node.Nodes.Add(loaderNode);
                    }
                }
                groupTree.Nodes.Add(node);
            }
            for (int i = 0; i != stream.Lines.Length; i++)
            {
                var line = stream.Lines[i];
                TreeNode node = new TreeNode();
                node.Name = line.Name;
                node.Text = $"[{line.lineID}] {line.Name}";
                node.Tag = line;
                List<StreamLoader> list = new List<StreamLoader>();
                for (int x = 0; x < stream.Loaders.Length; x++)
                {
                    var loader = stream.Loaders[x];
                    if (line.lineID >= loader.start && line.lineID <= loader.end)
                    {
                        var newLoader = new StreamLoader(loader);
                        list.Add(newLoader);
                    }
                }
                line.loadList = list.ToArray();
                if (line.groupID >= 0 && line.groupID < linesTree.Nodes.Count)
                {
                    linesTree.Nodes[line.groupID].Nodes.Add(node);
                }
            }
            for (int i = 0; i < stream.Blocks.Length; i++)
            {
                TreeNode node = new TreeNode();
                node.Name = "Block" + i;
                var block = stream.Blocks[i];
                string blockInfo = $"Block: {i}";
                if (block.Hashes != null && block.Hashes.Length > 0)
                {
                    blockInfo += $" - Hashes: {block.Hashes.Length}";
                    for (int j = 0; j < block.Hashes.Length; j++)
                    {
                        var hash = block.Hashes[j];
                        TreeNode hashNode = new TreeNode();
                        hashNode.Name = $"Hash_{i}_{j}";
                        string hashInfo = $"Hash {j}: {hash}"; 
                        string resolvedName = FindNameByHash(hash);
                        if (!string.IsNullOrEmpty(resolvedName))
                        {
                            hashInfo += $" -> {resolvedName}";
                            string source = "Static";
                            if (areaNameHashes.ContainsKey(hash)) source = "CityAreas";
                            hashInfo += $" [{source}]";
                        }
                        else
                        {
                            hashInfo += " [Unknown]";
                            if (hash == 6846398384861516328UL)
                            {
                                hashInfo += " - Scene500";
                            }
                        }
                        hashNode.Text = hashInfo;
                        hashNode.Tag = hash;
                        node.Nodes.Add(hashNode);
                    }
                }
                node.Text = blockInfo;
                node.Tag = stream.Blocks[i];
                blockView.Nodes.Add(node);
            }
            Text = Language.GetString("$STREAM_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void Save()
        {
            UpdateStream();
            stream.WriteToFile();
            Text = Language.GetString("$STREAM_EDITOR_TITLE");
            bIsFileEdited = false;
        }

        private void OnNodeSelectSelect(object sender, TreeViewEventArgs e) => PropertyGrid_Stream.SelectedObject = e.Node.Tag;
        private void ExitButtonPressed(object sender, System.EventArgs e) => Close();
        private void ReloadButtonPressed(object sender, System.EventArgs e) => BuildData();
        private void SaveButtonPressed(object sender, System.EventArgs e) => Save();

        private void OnContextMenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            for (int i = 0; i != LineContextStrip.Items.Count; i++)
            {
                LineContextStrip.Items[i].Visible = false;
            }
            if (linesTree.SelectedNode != null && linesTree.SelectedNode.Tag != null)
            {
                if (linesTree.SelectedNode.Tag.GetType() == typeof(StreamHeaderGroup))
                {
                    AddLineButton.Visible = true;
                }
                else if (linesTree.SelectedNode.Tag.GetType() == typeof(StreamLine))
                {

                    DeleteLineButton.Visible = true;
                    DuplicateLine.Visible = true;
                    MoveItemDownButton.Visible = true;
                    MoveItemUpButton.Visible = true;
                }
            }
        }

        public class ExportedBranch
        {
            public StreamHeaderGroup Header { get; set; }
            public List<ExportedLine> Lines { get; set; }
        }

        public class ExportedLine
        {
            public string Name { get; set; }
            public int LoadType { get; set; }
            public string Flags { get; set; }
            public ulong Unk10 { get; set; }
            public ulong Unk11 { get; set; }
            public int Unk5 { get; set; }
            public int Unk12 { get; set; }
            public int Unk13 { get; set; }
            public int Unk14 { get; set; }
            public int Unk15 { get; set; }
            public List<ExportedLoader> LoadList { get; set; }
        }

        public class ExportedLoader
        {
            public int LoadType { get; set; }
            public string Path { get; set; }
            public string Entity { get; set; }
            public int start { get; set; }
            public int end { get; set; }
            public string Type { get; set; }
            public int LoaderSubID { get; set; }
            public int LoaderID { get; set; }
            public string AssignedGroup { get; set; }
            public string PreferredGroup { get; set; }
        }

        private void ExportBranch(object sender, System.EventArgs e)
        {
            TreeNode selectedNode = linesTree.SelectedNode;
            if (selectedNode?.Tag == null)
            {
                MessageBox.Show("Select a branch or a line to export.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedNode.Tag is StreamLine line)
            {
                var exportedLine = new ExportedLine
                {
                    Name = line.Name,
                    LoadType = line.LoadType,
                    Flags = line.Flags,
                    Unk10 = line.Unk10,
                    Unk11 = line.Unk11,
                    Unk5 = line.Unk5,
                    Unk12 = line.Unk12,
                    Unk13 = line.Unk13,
                    Unk14 = line.Unk14,
                    Unk15 = line.Unk15,
                    LoadList = line.loadList?.Select(l => new ExportedLoader
                    {
                        LoadType = l.LoadType,
                        Path = l.Path,
                        Entity = l.Entity,
                        start = l.start,
                        end = l.end,
                        Type = l.Type.ToString(),
                        LoaderSubID = l.LoaderSubID,
                        LoaderID = l.LoaderID,
                        AssignedGroup = l.AssignedGroup,
                        PreferredGroup = l.PreferredGroup
                    }).ToList() ?? new List<ExportedLoader>()
                };

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "JSON Files (*.json)|*.json";
                    string safeName = string.Join("_", line.Name.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));
                    sfd.FileName = $"Line_{safeName}.json";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string json = JsonConvert.SerializeObject(exportedLine, Formatting.Indented);
                        File.WriteAllText(sfd.FileName, json);
                        MessageBox.Show("The selected line was successfully exported.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                return;
            }

            if (selectedNode.Tag is StreamHeaderGroup headerGroup)
            {
                var branch = new ExportedBranch
                {
                    Header = headerGroup,
                    Lines = selectedNode.Nodes.Cast<TreeNode>().Select(n =>
                    {
                        var ln = n.Tag as StreamLine;
                        return new ExportedLine
                        {
                            Name = ln.Name,
                            LoadType = ln.LoadType,
                            Flags = ln.Flags,
                            Unk10 = ln.Unk10,
                            Unk11 = ln.Unk11,
                            Unk5 = ln.Unk5,
                            Unk12 = ln.Unk12,
                            Unk13 = ln.Unk13,
                            Unk14 = ln.Unk14,
                            Unk15 = ln.Unk15,
                            LoadList = ln.loadList?.Select(l => new ExportedLoader
                            {
                                LoadType = l.LoadType,
                                Path = l.Path,
                                Entity = l.Entity,
                                start = l.start,
                                end = l.end,
                                Type = l.Type.ToString(),
                                LoaderSubID = l.LoaderSubID,
                                LoaderID = l.LoaderID,
                                AssignedGroup = l.AssignedGroup,
                                PreferredGroup = l.PreferredGroup
                            }).ToList() ?? new List<ExportedLoader>()
                        };
                    }).ToList()
                };

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "JSON Files (*.json)|*.json";
                    sfd.FileName = headerGroup.HeaderName + ".json";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string json = JsonConvert.SerializeObject(branch, Formatting.Indented);
                        File.WriteAllText(sfd.FileName, json);
                        MessageBox.Show("The branch was successfully exported.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                return;
            }
            MessageBox.Show("Unsupported selection type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ImportBranch(object sender, System.EventArgs e)
        {
            TreeNode selectedNode = linesTree.SelectedNode;
            if (selectedNode == null || selectedNode.Tag == null)
            {
                MessageBox.Show("Select a branch or a line inside a branch to import into.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON Files (*.json)|*.json";
                if (ofd.ShowDialog() != DialogResult.OK) return;
                string json = File.ReadAllText(ofd.FileName);

                try
                {
                    if (json.TrimStart().StartsWith("{") && json.Contains("\"Header\""))
                    {
                        var imported = JsonConvert.DeserializeObject<ExportedBranch>(json);
                        if (imported?.Header == null)
                        {
                            MessageBox.Show("Invalid branch file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        TreeNode newHeaderNode = new TreeNode(imported.Header.HeaderName);
                        newHeaderNode.Tag = imported.Header;
                        foreach (var importedLine in imported.Lines)
                        {
                            var line = new StreamLine
                            {
                                Name = importedLine.Name,
                                LoadType = importedLine.LoadType,
                                Flags = importedLine.Flags,
                                Unk10 = importedLine.Unk10,
                                Unk11 = importedLine.Unk11,
                                Unk5 = importedLine.Unk5,
                                Unk12 = importedLine.Unk12,
                                Unk13 = importedLine.Unk13,
                                Unk14 = importedLine.Unk14,
                                Unk15 = importedLine.Unk15,
                                loadList = importedLine.LoadList.Select(l => new StreamMapLoader.StreamLoader
                                {
                                    LoadType = l.LoadType,
                                    Path = l.Path,
                                    Entity = l.Entity,
                                    start = l.start,
                                    end = l.end,
                                    Type = Enum.TryParse(l.Type, out GroupTypes type) ? type : GroupTypes.Null,
                                    LoaderSubID = l.LoaderSubID,
                                    LoaderID = l.LoaderID,
                                    AssignedGroup = l.AssignedGroup,
                                    PreferredGroup = l.PreferredGroup
                                }).ToArray()
                            };
                            TreeNode lineNode = new TreeNode(line.Name) { Tag = line };
                            newHeaderNode.Nodes.Add(lineNode);
                        }
                        linesTree.Nodes.Add(newHeaderNode);
                        linesTree.ExpandAll();
                    }
                    else if (json.TrimStart().StartsWith("{") && json.Contains("\"LoadList\"") && !json.Contains("\"Header\""))
                    {
                        var importedLine = JsonConvert.DeserializeObject<ExportedLine>(json);
                        if (importedLine == null)
                        {
                            MessageBox.Show("Invalid line file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        StreamHeaderGroup targetHeader = null;
                        TreeNode targetParentNode = null;

                        if (selectedNode.Tag is StreamHeaderGroup header)
                        {
                            targetHeader = header;
                            targetParentNode = selectedNode;
                        }
                        else if (selectedNode.Tag is StreamLine && selectedNode.Parent?.Tag is StreamHeaderGroup parentHeader)
                        {
                            targetHeader = parentHeader;
                            targetParentNode = selectedNode.Parent;
                        }

                        if (targetHeader == null)
                        {
                            MessageBox.Show("Cannot determine target branch for import.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var line = new StreamLine
                        {
                            Name = importedLine.Name,
                            LoadType = importedLine.LoadType,
                            Flags = importedLine.Flags,
                            Unk10 = importedLine.Unk10,
                            Unk11 = importedLine.Unk11,
                            Unk5 = importedLine.Unk5,
                            Unk12 = importedLine.Unk12,
                            Unk13 = importedLine.Unk13,
                            Unk14 = importedLine.Unk14,
                            Unk15 = importedLine.Unk15,
                            Group = targetHeader.HeaderName,
                            loadList = importedLine.LoadList.Select(l => new StreamMapLoader.StreamLoader
                            {
                                LoadType = l.LoadType,
                                Path = l.Path,
                                Entity = l.Entity,
                                start = l.start,
                                end = l.end,
                                Type = Enum.TryParse(l.Type, out GroupTypes type) ? type : GroupTypes.Null,
                                LoaderSubID = l.LoaderSubID,
                                LoaderID = l.LoaderID,
                                AssignedGroup = l.AssignedGroup,
                                PreferredGroup = l.PreferredGroup
                            }).ToArray()
                        };

                        TreeNode lineNode = new TreeNode(line.Name) { Tag = line };
                        targetParentNode.Nodes.Add(lineNode);
                        linesTree.SelectedNode = lineNode; 
                    }
                    else
                    {
                        MessageBox.Show("Unrecognized JSON format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
                    bIsFileEdited = true;
                    MessageBox.Show("Import completed successfully.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Import error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteLineButtonPressed(object sender, System.EventArgs e)
        {
            linesTree?.Nodes.Remove(linesTree.SelectedNode);
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void AddLineButtonPressed(object sender, System.EventArgs e)
        {
            TreeNode node = linesTree.SelectedNode;
            StreamLine line = new StreamLine();
            line.Group = node.Text;
            line.Flags = "";
            TreeNode child = new TreeNode();
            child.Name = "GroupLoader" + node.Index;
            child.Text = line.Name;
            child.Tag = line;
            node.Nodes.Add(child);
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                foreach (TreeNode node in linesTree.Nodes)
                {
                    if (node.Text.Contains(SearchBox.Text))
                    {
                        linesTree.SelectedNode = node;
                    }
                    foreach (TreeNode child in node.Nodes)
                    {
                        if (child.Text.Contains(SearchBox.Text))
                        {
                            linesTree.SelectedNode = child;
                        }
                    }
                }
            }
        }

        private void MoveItemUp()
        {
            if (linesTree.SelectedNode == null || linesTree.SelectedNode.Tag == null) return;
            TreeNode node = linesTree.SelectedNode;
            if (node.Tag is StreamLine)
            {
                TreeNode parent = node.Parent;
                int index = parent.Nodes.IndexOf(node);
                if (index > 0)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index - 1, node);
                    linesTree.SelectedNode = node;
                }
            }
            else if (node.Tag is StreamHeaderGroup)
            {
                int index = linesTree.Nodes.IndexOf(node);
                if (index > 0)
                {
                    linesTree.Nodes.RemoveAt(index);
                    linesTree.Nodes.Insert(index - 1, node);
                    linesTree.SelectedNode = node;
                }
            }
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void MoveItemUp_Click(object sender, System.EventArgs e)
        {
            MoveItemUp();
        }

        private void MoveItemDown()
        {
            if (linesTree.SelectedNode == null || linesTree.SelectedNode.Tag == null) return;
            TreeNode node = linesTree.SelectedNode;
            if (node.Tag is StreamLine)
            {
                TreeNode parent = node.Parent;
                int index = parent.Nodes.IndexOf(node);
                if (index < parent.Nodes.Count - 1)
                {
                    parent.Nodes.RemoveAt(index);
                    parent.Nodes.Insert(index + 1, node);
                    linesTree.SelectedNode = node;
                }
            }
            else if (node.Tag is StreamHeaderGroup)
            {
                int index = linesTree.Nodes.IndexOf(node);
                if (index < linesTree.Nodes.Count - 1)
                {
                    linesTree.Nodes.RemoveAt(index);
                    linesTree.Nodes.Insert(index + 1, node);
                    linesTree.SelectedNode = node;
                }
            }
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void MoveItemDown_Click(object sender, System.EventArgs e)
        {
            MoveItemDown();
        }

        private void CopyLoadListAbove_Click(object sender, EventArgs e)
        {
            if (linesTree.SelectedNode != null && linesTree.SelectedNode.Tag != null)
            {
                if (linesTree.SelectedNode.Tag.GetType() == typeof(StreamLine))
                {
                    TreeNode node = linesTree.SelectedNode;
                    StreamLine newLine = new StreamLine((node.Tag as StreamLine));
                    TreeNode newNode = new TreeNode();
                    newNode.Name = "GroupLoader" + node.Index;
                    newNode.Text = newLine.Name;
                    newNode.Tag = newLine;
                    node.Parent.Nodes.Insert(node.Index + 1, newNode);

                    Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
                    bIsFileEdited = true;
                }
            }
        }

        private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (e.ChangedItem.Label == "Name")
            {
                if (tabControl.SelectedTab == StreamLinesPage)
                {
                    TreeNode selected = linesTree.SelectedNode;
                    linesTree.SelectedNode.Text = e.ChangedItem.Value.ToString();
                }
                else if (tabControl.SelectedTab == StreamGroupPage)
                {
                    TreeNode selected = groupTree.SelectedNode;
                    groupTree.SelectedNode.Text = e.ChangedItem.Value.ToString();
                }
            }
            else if (e.ChangedItem.Label == "HeaderName")
            {
                if (tabControl.SelectedTab == StreamLinesPage)
                {
                    TreeNode selected = linesTree.SelectedNode;
                    linesTree.SelectedNode.Text = e.ChangedItem.Value.ToString();
                }
            }
            else if (e.ChangedItem.Label == "PreferredGroup")
            {
                UpdateStream();
            }
            PropertyGrid_Stream.Refresh();
            Cursor.Current = Cursors.Default;
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (linesTree.Focused)
            {
                if (e.Control && e.KeyCode == Keys.C)
                {
                    Copy();
                }
                else if (e.Control && e.KeyCode == Keys.V)
                {
                    Paste();
                }
            }
        }

        private void Paste()
        {
            if (clipboard == null)
            {
                return;
            }

            if (linesTree.SelectedNode?.Tag is not StreamLine targetLine)
            {
                return;
            }

            if (clipboard is not StreamLine sourceLine)
            {
                return;
            }
            targetLine.Name = sourceLine.Name;
            targetLine.Flags = sourceLine.Flags;
            targetLine.loadList = sourceLine.loadList?.ToArray();
            linesTree.SelectedNode.Text = targetLine.Name;
            PropertyGrid_Stream.SelectedObject = targetLine;
            if (!bIsFileEdited)
            {
                Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
                bIsFileEdited = true;
            }
        }

        private void Copy()
        {
            if (linesTree.SelectedNode?.Tag is StreamLine or StreamHeaderGroup)
            {
                clipboard = linesTree.SelectedNode.Tag;
            }
        }

        private void Button_CreateLineGroup_Click(object sender, EventArgs e)
        {
            StreamHeaderGroup HeaderGroup = new StreamHeaderGroup();
            HeaderGroup.HeaderName = "New_Line_Group";
            TreeNode NewHeaderNode = new TreeNode();
            NewHeaderNode.Text = "New_Line_Group";
            NewHeaderNode.Tag = HeaderGroup;
            linesTree.Nodes.Add(NewHeaderNode);
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_AddBlock_Click(object sender, EventArgs e)
        {
            if (stream == null)
            {
                MessageBox.Show("Stream not loaded yet.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var newBlock = new StreamMapLoader.StreamBlock();
            newBlock.startOffset = 0;
            newBlock.endOffset = 0;
            newBlock.Hashes = new ulong[0];
            int newIndex = blockView.Nodes.Count;
            TreeNode node = new TreeNode();
            node.Name = "Block" + newIndex;
            node.Text = "Block: " + newIndex;
            node.Tag = newBlock;
            blockView.Nodes.Add(node);
            var blocksList = stream.Blocks?.ToList() ?? new List<StreamMapLoader.StreamBlock>();
            blocksList.Add(newBlock);
            stream.Blocks = blocksList.ToArray();
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_DeleteBlock_Click(object sender, EventArgs e)
        {
            if (stream == null || blockView.SelectedNode == null)
            {
                return;
            }
            TreeNode sel = blockView.SelectedNode;
            int nodeIndex = blockView.Nodes.IndexOf(sel);
            if (nodeIndex < 0) return;
            blockView.Nodes.RemoveAt(nodeIndex);
            var blocksList = stream.Blocks?.ToList() ?? new List<StreamMapLoader.StreamBlock>();
            if (nodeIndex < blocksList.Count)
            {
                blocksList.RemoveAt(nodeIndex);
            }
            else
            {
                var blockToRemove = sel.Tag as StreamMapLoader.StreamBlock;
                if (blockToRemove != null)
                {
                    if (!blocksList.Remove(blockToRemove))
                    {
                        var candidate = blocksList.FirstOrDefault(b =>
                            b.startOffset == blockToRemove.startOffset &&
                            b.endOffset == blockToRemove.endOffset &&
                            ((b.Hashes == null && blockToRemove.Hashes == null) ||
                             (b.Hashes != null && blockToRemove.Hashes != null && Enumerable.SequenceEqual(b.Hashes, blockToRemove.Hashes)))
                        );
                        if (candidate != null) blocksList.Remove(candidate);
                    }
                }
            }
            stream.Blocks = blocksList.ToArray();
            for (int i = 0; i < blockView.Nodes.Count; i++)
            {
                blockView.Nodes[i].Name = "Block" + i;
                blockView.Nodes[i].Text = "Block: " + i;
            }
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void Button_CreateStreamGroup_Click(object Sender, EventArgs Args)
        {
            StreamGroup NewGroup = new StreamGroup();
            NewGroup.Name = "New_Group";
            NewGroup.Type = GroupTypes.Null;
            TreeNode NewGroupNode = new TreeNode();
            NewGroupNode.Text = "New_Group_Node";
            NewGroupNode.Tag = NewGroup;
            groupTree.Nodes.Add(NewGroupNode);
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void StreamEditor_Closing(object sender, FormClosingEventArgs e)
        {
            if (bIsFileEdited)
            {
                System.Windows.MessageBoxResult SaveChanges = System.Windows.MessageBox.Show(Language.GetString("$SAVE_PROMPT"), "Toolkit", System.Windows.MessageBoxButton.YesNoCancel);

                if (SaveChanges == System.Windows.MessageBoxResult.Yes)
                {
                    Save();
                }
                else if (SaveChanges == System.Windows.MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ExportAllStreamLines(object sender, EventArgs e)
        {
            var allBranches = linesTree.Nodes
                .Cast<TreeNode>()
                .Select(node =>
                {
                    var headerGroup = node.Tag as StreamHeaderGroup;
                    return new ExportedBranch
                    {
                        Header = headerGroup,
                        Lines = node.Nodes
                            .Cast<TreeNode>()
                            .Select(childNode =>
                            {
                                var line = childNode.Tag as StreamLine;
                                return new ExportedLine
                                {
                                    Name = line.Name,
                                    LoadType = line.LoadType,
                                    Flags = line.Flags,
                                    Unk10 = line.Unk10,
                                    Unk11 = line.Unk11,
                                    Unk5 = line.Unk5,
                                    Unk12 = line.Unk12,
                                    Unk13 = line.Unk13,
                                    Unk14 = line.Unk14,
                                    Unk15 = line.Unk15,
                                    LoadList = line.loadList?.Select(l => new ExportedLoader
                                    {
                                        LoadType = l.LoadType,
                                        Path = l.Path,
                                        Entity = l.Entity,
                                        start = l.start,
                                        end = l.end,
                                        Type = l.Type.ToString(),
                                        LoaderSubID = l.LoaderSubID,
                                        LoaderID = l.LoaderID,
                                        AssignedGroup = l.AssignedGroup,
                                        PreferredGroup = l.PreferredGroup
                                    }).ToList() ?? new List<ExportedLoader>()
                                };
                            }).ToList()
                    };
                }).ToList();
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "JSON Files (*.json)|*.json";
                sfd.FileName = "AllStreamLines.json";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string json = JsonConvert.SerializeObject(allBranches, Formatting.Indented);
                    File.WriteAllText(sfd.FileName, json);
                    MessageBox.Show("All Stream Lines successfully exported.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ImportAllStreamLines(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON Files (*.json)|*.json";
                if (ofd.ShowDialog() != DialogResult.OK) return;
                string json = File.ReadAllText(ofd.FileName);
                try
                {
                    var importedBranches = JsonConvert.DeserializeObject<List<ExportedBranch>>(json);
                    if (importedBranches == null)
                    {
                        MessageBox.Show("Incorrect file format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    foreach (var branch in importedBranches)
                    {
                        TreeNode newHeaderNode = new TreeNode(branch.Header.HeaderName)
                        {
                            Tag = branch.Header
                        };
                        foreach (var importedLine in branch.Lines)
                        {
                            var line = new StreamLine
                            {
                                Name = importedLine.Name,
                                LoadType = importedLine.LoadType,
                                Flags = importedLine.Flags,
                                Unk10 = importedLine.Unk10,
                                Unk11 = importedLine.Unk11,
                                Unk5 = importedLine.Unk5,
                                Unk12 = importedLine.Unk12,
                                Unk13 = importedLine.Unk13,
                                Unk14 = importedLine.Unk14,
                                Unk15 = importedLine.Unk15,
                                loadList = importedLine.LoadList.Select(l => new StreamMapLoader.StreamLoader
                                {
                                    LoadType = l.LoadType,
                                    Path = l.Path,
                                    Entity = l.Entity,
                                    start = l.start,
                                    end = l.end,
                                    Type = Enum.TryParse(l.Type, out GroupTypes type) ? type : GroupTypes.Null,
                                    LoaderSubID = l.LoaderSubID,
                                    LoaderID = l.LoaderID,
                                    AssignedGroup = l.AssignedGroup,
                                    PreferredGroup = l.PreferredGroup
                                }).ToArray()
                            };
                            TreeNode lineNode = new TreeNode(line.Name) { Tag = line };
                            newHeaderNode.Nodes.Add(lineNode);
                        }
                        linesTree.Nodes.Add(newHeaderNode);
                    }
                    linesTree.ExpandAll();
                    Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
                    bIsFileEdited = true;
                    MessageBox.Show("All Stream Lines successfully imported.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Import: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteStreamGroup_Click(object sender, EventArgs e)
        {
            if (groupTree.SelectedNode == null || groupTree.SelectedNode.Tag == null) return;
            groupTree.Nodes.Remove(groupTree.SelectedNode);
            if (stream != null)
            {
                var groupsList = stream.Groups?.ToList() ?? new List<StreamGroup>();
                var groupToRemove = groupTree.SelectedNode.Tag as StreamGroup;
                if (groupToRemove != null)
                {
                    groupsList.Remove(groupToRemove);
                    stream.Groups = groupsList.ToArray();
                }
            }
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }

        private void DeleteAllStreamLines(object sender, EventArgs e)
        {
            if (linesTree.Nodes.Count == 0) return;
            var result = MessageBox.Show("Are you sure you want to delete ALL Stream Lines?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            foreach (TreeNode headerNode in linesTree.Nodes)
            {
                headerNode.Nodes.Clear();
            }
            if (stream != null)
            {
                stream.Lines = new StreamMapLoader.StreamLine[0];
            }
            Text = Language.GetString("$STREAM_EDITOR_TITLE") + "*";
            bIsFileEdited = true;
        }
    }

    public class StreamHeaderGroup
    {
        public string HeaderName { get; set; }
    }
}