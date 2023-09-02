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

        private static readonly List<TreeNode> _initialTreeViewData = new List<TreeNode>()
        {
            new TreeNode(0, "Test1", -1),
            new TreeNode(1, "Child1", 0),
            new TreeNode(2, "Child2", 0),
            new TreeNode(3, "SubChild1", 2),
            new TreeNode(4, "SubChild2", 2),
            new TreeNode(5, "Test2", -1),
        };

        private static readonly List<TreeNode> _treeViewData = new List<TreeNode>()
        {
            new TreeNode(0, "Test1", -1),
            new TreeNode(1, "Child1", 0),
            new TreeNode(2, "Child2", 0),
            new TreeNode(3, "SubChild1", 2),
            new TreeNode(4, "SubChild2", 2),
            new TreeNode(5, "Test2", -1),
        };

        public TreeViewController(ILogger<TreeViewController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<TreeNodeDTO> Get()
        {
            var dtoTree = new List<TreeNodeDTO>();
            foreach (var treeNode in _treeViewData.Where(node => node.ParentId == -1))
            {
                var nodeDto = new TreeNodeDTO(treeNode.Id, treeNode.Value);
                GetSubTree(nodeDto);
                dtoTree.Add(nodeDto);
            }

            return dtoTree;
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var itemToDelete = _treeViewData.FirstOrDefault(x => x.Id == id);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            var children = GetChildren(itemToDelete);
            foreach (var child in children)
            {
                _treeViewData.Remove(child);
            }
            _treeViewData.Remove(itemToDelete);

            return NoContent();
        }

        [HttpPost("{parentId:int}/addChild")]
        public IActionResult AddChild(int parentId)
        {
            var newNodeDto = new TreeNodeDTO(-1, "New node");
            var parent = _treeViewData.FirstOrDefault(x => x.Id == parentId);
            if (parent == null)
            {
                return NotFound("Parent node not found");
            }

            var newNode = new TreeNode(_treeViewData.Count, newNodeDto.Value, parent.Id);
            _treeViewData.Add(newNode);

            var newNodeDtoWithId = new TreeNodeDTO(newNode.Id, newNode.Value);
            return Ok(newNodeDtoWithId);
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _treeViewData.Clear();
            _treeViewData.AddRange(new List<TreeNode>(_initialTreeViewData));

            return Ok();
        }

        private void GetSubTree(
            TreeNodeDTO root)
        {
            var childNodesDtos = _treeViewData.Where(node => node.ParentId == root.Id)
                .Select(child => new TreeNodeDTO(child.Id, child.Value))
                .ToList();
            foreach (var child in childNodesDtos)
            {
                GetSubTree(child);
            }
            root.Children = childNodesDtos;
        }

        private IEnumerable<TreeNode> GetChildren(TreeNode root)
        {
            var result = new List<TreeNode>();
            var children = _treeViewData.Where(node => node.ParentId == root.Id).ToList();
            result.AddRange(children);
            foreach (var child in children)
            {
                result.AddRange(GetChildren(child));
            }

            return result;
        }
    }
}
