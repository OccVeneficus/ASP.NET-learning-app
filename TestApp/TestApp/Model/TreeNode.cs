namespace TestApp.Model;

/// <summary>
/// Represents node in treeview.
/// </summary>
public class TreeNode
{
    /// <summary>
    /// Creates instance of <see cref="TreeNode"/>.
    /// </summary>
    /// <param name="id">Unique id.</param>
    /// <param name="parentId">Id of parent node.</param>
    /// <param name="value">Value displayed in node.</param>
    public TreeNode(int id, int parentId, string value)
    {
        Id = id;
        ParentId = parentId;
        Value = value;
    }

    /// <summary>
    /// Creates instance of <see cref="TreeNode"/>.
    /// </summary>
    /// <param name="parentId">Id of parent node.</param>
    /// <param name="value">Value displayed in node.</param>
    public TreeNode(string value, int parentId)
    {
        Value = value;
        ParentId = parentId;
    }

    /// <summary>
    /// Unique value. Identifies a node.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Id of a parent node.
    /// </summary>
    public int ParentId { get; set; }

    /// <summary>
    /// Node value for user data.
    /// </summary>
    public string Value { get; set; }
}