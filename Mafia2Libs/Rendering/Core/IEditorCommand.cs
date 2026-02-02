namespace Rendering.Core
{
    public interface IEditorCommand
    {
        string Description { get; }
        void Execute();
        void Undo();
    }
}
