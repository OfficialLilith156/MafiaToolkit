namespace Toolkit.Core
{
    public static class RefManager
    {
        //set to 10 because the first 10 are placeholders for render assets.
        private static int currentRefID = 10;
        private static int currentNamespaceOffset = 0;

        public static void SetNamespaceOffset(int offset)
        {
            currentNamespaceOffset = offset;
        }

        public static int GetNewRefID()
        {
            currentRefID++;
            return currentNamespaceOffset + currentRefID;
        }

        public static void ResetForNamespace(int offset)
        {
            currentNamespaceOffset = offset;
            currentRefID = 10;
        }
    }
}
