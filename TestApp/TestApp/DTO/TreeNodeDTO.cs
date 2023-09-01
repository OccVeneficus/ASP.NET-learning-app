namespace TestApp.DTO;

public class TreeNodeDTO
{
    public TreeNodeDTO(int id, string value, IEnumerable<TreeNodeDTO> children)
    {
        Id = id;
        Value = value;
        Children = children;
    }

    public TreeNodeDTO(int id, string value)
    {
        Id = id;
        Value = value;
        Children = new List<TreeNodeDTO>();
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
    /// .
    /// </summary>
    public IEnumerable<TreeNodeDTO> Children { get; set; }
}