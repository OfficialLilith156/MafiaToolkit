using Rendering.Core;
using System;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using Toolkit.Core;

namespace ResourceTypes.Navigation
{
    public class IType
    {
        protected int RefID;
        protected AIWorld OwnWorld;
        public AIWorld World => OwnWorld;
        public int ID => RefID;

        public bool bIsVisible { get; protected set; }

        public IType(AIWorld InWorld) 
        { 
            RefID = RefManager.GetNewRefID(); 
            OwnWorld = InWorld;
            bIsVisible = true;
        }

        

 
        private TreeNode FindTreeNodeByRefID(TreeNodeCollection nodes, int refID)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is IType t && t.RefID == refID)
                    return node;

                TreeNode child = FindTreeNodeByRefID(node.Nodes, refID);
                if (child != null)
                    return child;
            }
            return null;
        }

        //~ Interface
        public virtual void Read(BinaryReader Reader) { }
        public virtual void Write(BinaryWriter Writer) { }
        public virtual void DebugWrite(StreamWriter Writer) { }
        public virtual void ConstructRenderable(PrimitiveBatch BBoxBatcher) { }
        public virtual TreeNode PopulateTreeNode() { return null; }
        public virtual Vector3 GetPosition() { return Vector3.Zero; }
        //~ Interface

        public void NotifyUpdate() 
        { 
            OwnWorld.RequestPrimitiveBatchUpdate(); 
        }

        public void SetVisiblity(bool bInVisiblity)
        {
            bIsVisible = bInVisiblity;
            NotifyUpdate();
        }
    }
}
