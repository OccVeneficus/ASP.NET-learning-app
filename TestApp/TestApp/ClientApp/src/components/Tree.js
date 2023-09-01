import React, { useState } from 'react';
import TreeNode from "./TreeNode";

function Tree(props) {

    async function  deleteNode(nodeId){
        await fetch(`treeview/${nodeId}`, {method: "DELETE"});
    }
    
    const clickHandler = async (id) => {
        console.log(`CLICK id = ${id}`);
        await deleteNode(id);
    }

    return (
        <ul>
            {props.treeData.map((node) => (
                <TreeNode node={node} id={node.id} key={node.id} handler={clickHandler}/>
            ))}
        </ul>
    )
}

export default Tree;