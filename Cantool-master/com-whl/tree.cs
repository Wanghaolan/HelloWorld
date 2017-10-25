using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace sf
{
    class tree
    {
        private static Dictionary<int, List<TreeNode>> dic;

        public static void BuildTree(DataTable dt, TreeView treeView, Boolean expandAll,
            string displayName, string nodeId, string parentId)
        {
            // 如果TreeView中有另一个数据，请清除TreeView
            treeView.Nodes.Clear();

            dic = new Dictionary<int, List<TreeNode>>();

            TreeNode node = null;

            foreach (DataRow row in dt.Rows)
            {
                // 将每个记录恢复为一个TreeNode
                node = new TreeNode(row[displayName].ToString());
                node.Tag = row[nodeId];

                // TreeView的根的parentId是DataTable中的DBNull.Value
                // 所以如果parentId是一个DBNull.Value，那么它是根节点
                // 否则它只是一个常见的TreeNode
                if (row[parentId] != DBNull.Value)
                {
                    int _parentId = Convert.ToInt32(row[parentId]);

                    //如果存在一个列表<TreeNode> 被该_parentId标识
                    // 那么我们需要把这个节点放到这个标识的List <TreeNode>中
                    if (dic.ContainsKey(_parentId))
                    {
                        dic[_parentId].Add(node);
                    }
                    else
                    {
                        // 否则，我们将添加一个新的记录到字典 < int，List < TreeNode >>
                          dic.Add(_parentId, new List<TreeNode>());

                        // 然后将这个节点放入 List<TreeNode>
                        dic[_parentId].Add(node);
                    }
                }
                else
                {
                    // 把这个节点放入treeview的根节点
                    treeView.Nodes.Add(node);
                }
            }

            // 收集并识别每个收藏与他们的parentId
            // 我们将继续使用创建的根节点构建此树
            SearchChildNodes(treeView.Nodes[0]);

            if (expandAll)
            {
                // 展开TreeView
                treeView.ExpandAll();
            }
        }
        private static void SearchChildNodes(TreeNode parentNode)
        {
            if (!dic.ContainsKey(Convert.ToInt32(parentNode.Tag)))
            {
                // 如果此parentId没有标识的集合
                // 什么都不做，返回值
                return;
            }

            // 将此parentId的标识集合作为此节点的子项放入树中
            parentNode.Nodes.AddRange(dic[Convert.ToInt32(parentNode.Tag)].ToArray());

            // 将这些子节点看作父节点
            foreach (TreeNode _parentNode in dic[Convert.ToInt32(parentNode.Tag)].ToArray())
            {
                // 然后通过这些id去找到标识的集合
                SearchChildNodes(_parentNode);
            }
        }
    }
}
