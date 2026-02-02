using System.Numerics;
using Rendering.Graphics;

namespace Rendering.Core.Commands
{
    public class TransformCommand : IEditorCommand
    {
        private readonly int refID;
        private readonly Matrix4x4 oldTransform;
        private readonly Matrix4x4 newTransform;
        private readonly GraphicsClass graphics;

        public string Description => $"Transform object {refID}";

        public TransformCommand(GraphicsClass graphics, int refID, Matrix4x4 oldTransform, Matrix4x4 newTransform)
        {
            this.graphics = graphics;
            this.refID = refID;
            this.oldTransform = oldTransform;
            this.newTransform = newTransform;
        }

        public void Execute()
        {
            graphics.SetObjectTransform(refID, newTransform);
        }

        public void Undo()
        {
            graphics.SetObjectTransform(refID, oldTransform);
        }

        public int RefID => refID;
        public Matrix4x4 OldTransform => oldTransform;
        public Matrix4x4 NewTransform => newTransform;
    }
}
