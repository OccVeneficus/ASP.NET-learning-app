import React, { useState, useEffect } from 'react';
import TreeNode from "./TreeNode";

function Tree(props) {

    const [treeData, setTreeData] = useState(props.treeData);

    useEffect(() => {
        setTreeData(props.treeData);
    }, [props.treeData]);

    async function  deleteNode(nodeId){
        await fetch(`treeview/${nodeId}`, {method: "DELETE"});
        const updatedTreeData = treeData.filter(node => node.id !== nodeId);
        setTreeData(updatedTreeData);
    }
    
    const clickDeleteHandler = async (id) => {
        await deleteNode(id);
    }
    
    async function addChildNode(parentId){
        const respose = await fetch(`treeview/${parentId}/addChild`, {method: "POST"})
        if(respose.ok){
            const newChildNode = await respose.json();

             // Найдем текущую ноду по parentId в treeData
            const updatedTreeData = treeData.map(node => {
                if (node.id === parentId) {
                    // Если нашли текущую ноду, добавим новую ноду в ее children
                    return {
                        ...node,
                         children: [...(node.children || []), newChildNode]
                    };
                }
                return node;
            });
            setTreeData(updatedTreeData);
        }
    }

    const clickAddChildHandler = async (parentId) => {
        await addChildNode(parentId);
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