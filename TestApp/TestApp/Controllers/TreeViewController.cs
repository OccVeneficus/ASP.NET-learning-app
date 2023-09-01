using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
            new TreeNode("0", "Documents", new List<TreeNode>()
            {
                new TreeNode(
                    "0-0",
                    "Document 1-1",
                    new List<TreeNode>
                    {
                        new TreeNode("0-1-1", "Document-0-1.doc"),
                        new TreeNode("0-1-2", "Document-0-2.doc")
                    }),
            }),
            new TreeNode("1", "Desktop")
        };

        public TreeViewController(ILogger<TreeViewController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<TreeNode> Get()
        {
            return _treeViewData;
        }

        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            var itemToDelete = _treeViewData.FirstOrDefault(x => x.Key == key);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            _treeViewData.Remove(itemToDelete);

            return NoContent();
        }
    }
}
