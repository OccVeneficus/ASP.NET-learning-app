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

            _treeViewData.Remove(itemToDelete);

            return NoContent();
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
    }
}
