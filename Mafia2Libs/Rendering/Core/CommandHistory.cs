using System;
using System.Collections.Generic;

namespace Rendering.Core
{
    public class CommandHistory
    {
        private readonly Stack<IEditorCommand> undoStack = new Stack<IEditorCommand>();
        private readonly Stack<IEditorCommand> redoStack = new Stack<IEditorCommand>();
        private const int MAX_HISTORY = 100;

        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;
        public int UndoCount => undoStack.Count;
        public int RedoCount => redoStack.Count;

        public event EventHandler OnHistoryChanged;

        public void ExecuteCommand(IEditorCommand cmd)
        {
            if (cmd == null)
                return;

            cmd.Execute();
            undoStack.Push(cmd);
            redoStack.Clear();

            if (undoStack.Count > MAX_HISTORY)
            {
                TrimHistory();
            }

            OnHistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddExecutedCommand(IEditorCommand cmd)
        {
            if (cmd == null)
                return;

            undoStack.Push(cmd);
            redoStack.Clear();

            if (undoStack.Count > MAX_HISTORY)
            {
                TrimHistory();
            }

            OnHistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool Undo()
        {
            if (!CanUndo)
                return false;

            IEditorCommand cmd = undoStack.Pop();
            cmd.Undo();
            redoStack.Push(cmd);

            OnHistoryChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public bool Redo()
        {
            if (!CanRedo)
                return false;

            IEditorCommand cmd = redoStack.Pop();
            cmd.Execute();
            undoStack.Push(cmd);

            OnHistoryChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
            OnHistoryChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetUndoDescription()
        {
            if (!CanUndo)
                return null;

            return undoStack.Peek().Description;
        }

        public string GetRedoDescription()
        {
            if (!CanRedo)
                return null;

            return redoStack.Peek().Description;
        }

        private void TrimHistory()
        {
            var tempList = new List<IEditorCommand>(undoStack);
            undoStack.Clear();

            for (int i = 0; i < MAX_HISTORY && i < tempList.Count; i++)
            {
                undoStack.Push(tempList[tempList.Count - 1 - i]);
            }
        }
    }
}
