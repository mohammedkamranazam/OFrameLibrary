using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace OFrameLibrary.Helpers
{
    public static class TreeViewHelper
    {
        public static void BuildTree(TreeView tv, string path, bool skipFiles)
        {
            var rootDir = new DirectoryInfo(HttpRuntime.AppDomainAppPath + path);

            tv.Nodes.Clear();

            var rootNode = new TreeNode(rootDir.Name, rootDir.FullName);
            tv.Nodes.Add(rootNode);

            TraverseTree(rootDir, rootNode, skipFiles);
        }

        public static void BuildTree(TreeView tv, string path, string[] patterns)
        {
            var rootDir = new DirectoryInfo(HttpRuntime.AppDomainAppPath + path);

            tv.Nodes.Clear();

            var rootNode = new TreeNode(rootDir.Name, rootDir.FullName);
            tv.Nodes.Add(rootNode);

            TraverseFiles(rootDir, rootNode, patterns);

            TraverseTree(rootDir, rootNode, patterns);
        }

        static void TraverseFiles(DirectoryInfo currentDir, TreeNode currentNode)
        {
            foreach (var file in currentDir.GetFiles())
            {
                var node = new TreeNode(file.Name, file.FullName);
                currentNode.ChildNodes.Add(node);
            }
        }

        static void TraverseFiles(DirectoryInfo currentDir, TreeNode currentNode, string[] patterns)
        {
            foreach (var pattern in patterns)
            {
                foreach (var file in currentDir.GetFiles(pattern))
                {
                    var node = new TreeNode(file.Name, file.FullName);
                    currentNode.ChildNodes.Add(node);
                }
            }
        }

        static void TraverseTree(DirectoryInfo currentDir, TreeNode currentNode, bool skipFiles)
        {
            foreach (var dir in currentDir.GetDirectories())
            {
                var node = new TreeNode(dir.Name, dir.FullName);
                currentNode.ChildNodes.Add(node);
                if (!skipFiles)
                {
                    TraverseFiles(dir, node);
                }
                TraverseTree(dir, node, skipFiles);
            }
        }

        static void TraverseTree(DirectoryInfo currentDir, TreeNode currentNode, string[] patterns)
        {
            foreach (var dir in currentDir.GetDirectories())
            {
                var add = false;

                foreach (var pattern in patterns)
                {
                    add |= dir.GetFiles(pattern, SearchOption.AllDirectories).Length > 0;
                }

                if (add)
                {
                    var node = new TreeNode(dir.Name, dir.FullName);
                    currentNode.ChildNodes.Add(node);
                    TraverseFiles(dir, node, patterns);
                    TraverseTree(dir, node, patterns);
                }
            }
        }
    }
}
