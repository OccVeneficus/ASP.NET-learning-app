import TreeNode from "./TreeNode";

function Tree(props) {
    return (
        <ul>
            {props.treeData.map((node) => (
                <TreeNode node={node} id={node.id} key={node.id} />
            ))}
        </ul>
    )
}

export default Tree;