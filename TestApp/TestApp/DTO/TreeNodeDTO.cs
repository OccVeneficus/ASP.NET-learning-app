namespace TestApp.DTO;

/// <summary>
/// Data transfer object for TreeNode.
/// </summary>
public class TreeNodeDto
{
    public TreeNodeDto(int id, string value, IEnumerable<TreeNodeDto> children)
    {
        Id = id;
        Value = value;
        Children = children;
    }

    public TreeNodeDto(int id, string value)
    {
        Id = id;
        Value = value;
        Children = new List<TreeNodeDto>();
    }

    /// <summary>
    /// Unique value. Identifies a node.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Node value for user data.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Collection of child elements.
    /// </summary>
    public IEnumerable<TreeNodeDto> Children { get; set; }
}