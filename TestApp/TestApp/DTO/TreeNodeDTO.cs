namespace TestApp.DTO;

/// <summary>
/// Data transfer object for TreeNode.
/// </summary>
public class TreeNodeDto
{
    /// <summary>
    /// Creates instance of <see cref="TreeNodeDto"/>.
    /// </summary>
    /// <param name="id">TreeNode Id.</param>
    /// <param name="value">TreeNode value.</param>
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