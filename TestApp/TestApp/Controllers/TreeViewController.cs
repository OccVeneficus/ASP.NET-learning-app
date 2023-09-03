using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestApp.DTO;
using TestApp.Model;

namespace TestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TreeViewController : ControllerBase
    {
        private readonly ILogger<TreeViewController> _logger;
        private readonly TreeDbContext _treeDbContext;

        public TreeViewController(ILogger<TreeViewController> logger, TreeDbContext treeDbContext)
        {
            _logger = logger;
            _treeDbContext = treeDbContext;
        }

        [HttpGet]
        public IEnumerable<TreeNodeDTO> Get()
        {
            var dtoTree = new List<TreeNodeDTO>();
            foreach (var treeNode in _treeDbContext.TreeNodes.Where(node => node.ParentId == -1))
            {
                var nodeDto = new TreeNodeDTO(treeNode.Id, treeNode.Value);
                SetNodeDtoSubTrees(nodeDto);
                dtoTree.Add(nodeDto);
            }

            return dtoTree;
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var itemToDelete = _treeDbContext.TreeNodes.FirstOrDefault(x => x.Id == id);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            var children = GetChildren(itemToDelete);
            foreach (var child in children)
            {
                _treeDbContext.TreeNodes.Remove(child);
            }
            _treeDbContext.TreeNodes.Remove(itemToDelete);
            _treeDbContext.SaveChanges();
            return NoContent();
        }

        [HttpPost("{parentId:int}/addChild")]
        public IActionResult AddChild(int parentId)
        {
            var newNodeDto = new TreeNodeDTO(-1, "New node");
            var parent = _treeDbContext.TreeNodes.FirstOrDefault(x => x.Id == parentId);
            if (parent == null)
            {
                return NotFound("Parent node not found");
            }

            var newNode = new TreeNode(newNodeDto.Value, parent.Id);
            var a = _treeDbContext.TreeNodes.Add(newNode);
            _treeDbContext.SaveChanges();

            var newNodeDtoWithId = new TreeNodeDTO(newNode.Id, newNode.Value);
            return Ok(newNodeDtoWithId);
        }

        [HttpPut("{nodeId:int}/{newValue}")]
        public IActionResult UpdateNode(int nodeId, string newValue)
        {
            var nodeToUpdate = _treeDbContext.TreeNodes.FirstOrDefault(node => node.Id == nodeId);
            if (nodeToUpdate == null)
            {
                return NotFound();
            }

            nodeToUpdate.Value = newValue;

            _treeDbContext.SaveChanges();

            return Ok(nodeToUpdate);
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _treeDbContext.TreeNodes.RemoveRange(_treeDbContext.TreeNodes);
            _treeDbContext.SeedDb();

            return Ok();
        }

        private void SetNodeDtoSubTrees(
            TreeNodeDTO root)
        {
            var childNodesDtos = _treeDbContext.TreeNodes.Where(node => node.ParentId == root.Id)
                .Select(child => new TreeNodeDTO(child.Id, child.Value))
                .ToList();
            foreach (var child in childNodesDtos)
            {
                SetNodeDtoSubTrees(child);
            }
            root.Children = childNodesDtos;
        }

        private IEnumerable<TreeNode> GetChildren(TreeNode root)
        {
            var result = new List<TreeNode>();
            var children = _treeDbContext.TreeNodes.Where(node => node.ParentId == root.Id).ToList();
            result.AddRange(children);
            foreach (var child in children)
            {
                result.AddRange(GetChildren(child));
            }

            return result;
        }
    }
}
