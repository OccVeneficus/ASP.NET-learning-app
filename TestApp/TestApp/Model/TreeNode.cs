namespace TestApp.Model;

/// <summary>
/// Represents node in treeview.
/// </summary>
public class TreeNode
{
    public TreeNode(string key, string value, List<TreeNode> children)
    {
        Key = key;
        Value = value;
        Children = children;
    }

    public TreeNode(string key, string value)
    {
        Key = key;
        Value = value;
        Children = new List<TreeNode>();
    }

    /// <summary>
    /// Unique value. Identifies a node.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Node value for user data.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Children nodes.
    /// </summary>
    public List<TreeNode> Children { get; set; }
}