﻿namespace TestApp.Model;

/// <summary>
/// Represents node in treeview.
/// </summary>
public class TreeNode
{
    public TreeNode(string value, int parentId)
    {
        Id = IdGenerator.GetNextId();
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