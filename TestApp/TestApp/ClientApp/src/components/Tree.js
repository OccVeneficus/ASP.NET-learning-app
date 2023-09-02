import React, { useState, useEffect } from 'react';
import TreeNode from "./TreeNode";

function Tree(props) {

    const [treeData, setTreeData] = useState(props.treeData);

    useEffect(() => {
        setTreeData(props.treeData);
    }, [props.treeData]);
    
    const clickDeleteHandler = async (id) => {
        await props.deleteNode(id);
    }

    const clickAddChildHandler = async (parentId) => {
        await props.addChildNode(parentId);
    }

    return (
        <>
            <ul>
                {treeData.map((node) => (
                    <TreeNode 
                    node={node} 
                    id={node.id} 
                    key={node.id} 
                    deleteHandler={clickDeleteHandler} 
                    addChildHandler={clickAddChildHandler}/>
                ))}
            </ul>
        </>
    )
}

export default Tree;