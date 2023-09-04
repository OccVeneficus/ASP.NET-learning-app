using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TestApp.DTO;
using TestApp.Model;

namespace TestApp.Controllers
{
    /// <summary>
    /// Tree view controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TreeViewController : ControllerBase
    {
        private readonly ILogger<TreeViewController> _logger;
        private readonly TreeDbContext _treeDbContext;

        /// <summary>
        /// Creates instance of <see cref="TreeViewController"/>.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="treeDbContext">Db context with treeview data.</param>
        public TreeViewController(ILogger<TreeViewController> logger, TreeDbContext treeDbContext)
        {
            _logger = logger;
            _treeDbContext = treeDbContext;
        }

        /// <summary>
        /// Http GET request handler. Returns tree data.
        /// </summary>
        /// <returns>Tree data.</returns>
        [HttpGet]
        public IEnumerable<TreeNodeDto> Get()
        {
            var dtoTree = new List<TreeNodeDto>();
            foreach (var treeNode in _treeDbContext.TreeNodes.Where(node => node.ParentId == -1))
            {
                var nodeDto = new TreeNodeDto(treeNode.Id, treeNode.Value);
                SetNodeDtoSubTrees(nodeDto);
                dtoTree.Add(nodeDto);
            }

            return dtoTree;
        }

        /// <summary>
        /// Http DELETE request handler. Deletes node with specified ID.
        /// </summary>
        /// <param name="id">Node to delete id.</param>
        /// <returns>Action result.</returns>
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

        /// <summary>
        /// Http POST request handler for adding child node.
        /// </summary>
        /// <param name="parentId">Id of node, for which the child node will be added.</param>
        /// <returns>Result of action. <see cref="NotFound"/>
        /// if there is no node with specified <see cref="parentId"/>,
        /// or <see cref="Ok"/> if node was successfully added.</returns>
        [HttpPost("{parentId:int}/addChild")]
        public IActionResult AddChild(int parentId)
        {
            var newNodeDto = new TreeNodeDto(-1, "New node");
            var parent = _treeDbContext.TreeNodes.FirstOrDefault(x => x.Id == parentId);
            if (parent == null)
            {
                return NotFound("Parent node not found");
            }

            var newNode = new TreeNode(newNodeDto.Value, parent.Id);
            _treeDbContext.SaveChanges();

            var newNodeDtoWithId = new TreeNodeDto(newNode.Id, newNode.Value);
            return Ok(newNodeDtoWithId);
        }

        /// <summary>
        /// Http PUT request handler, updates specified node value.
        /// </summary>
        /// <param name="nodeId">Id of node, which value will be updated.</param>
        /// <param name="newValue">New value for node.</param>
        /// <returns>Result of action. <see cref="NotFound"/>
        /// if there is no node with specified <see cref="nodeId"/>,
        /// or <see cref="Ok"/> if node was successfully update with <see cref="newValue"/>.</returns>
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

        /// <summary>
        /// Http POST request handler for tree data reset. Resets tree data context to initial state.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost("reset")]
        public IActionResult Reset()
        {
            _treeDbContext.TreeNodes.RemoveRange(_treeDbContext.TreeNodes);
            _treeDbContext.SeedDb();

            return Ok();
        }

        /// <summary>
        /// Recursively searches for all child elements of
        /// specified node and adds them to children collection.
        /// </summary>
        /// <param name="root">Root node.</param>
        private void SetNodeDtoSubTrees(
            TreeNodeDto root)
        {
            var childNodesDtos = _treeDbContext.TreeNodes.Where(node => node.ParentId == root.Id)
                .Select(child => new TreeNodeDto(child.Id, child.Value))
                .ToList();
            foreach (var child in childNodesDtos)
            {
                SetNodeDtoSubTrees(child);
            }
            root.Children = childNodesDtos;
        }

        /// <summary>
        /// Returns all children elements of specified node.
        /// </summary>
        /// <param name="root">Root node.</param>
        /// <returns>All children nodes of root node.</returns>
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
