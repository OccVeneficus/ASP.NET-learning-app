import React, { useState } from 'react';
import Tree from './Tree';

function TreeNode(props) {
    const { children, value, id } = props.node;
    
    const [showChildren, setShowChildren] = useState(false);

    const [hovered, setHovered] = useState(false);

    const handleMouseEnter = () => {
        setHovered(true);
    };

    const handleMouseLeave = () => {
        setHovered(false);
    };

    const handleClick = () => {
        if (typeof children === 'undefined') {
            return;
        }

        setShowChildren(!showChildren);
    };

    async function  deleteNode(nodeId){
            await fetch(`treeview/${nodeId}`, {method: "DELETE"});
        }

    const handleRemoveButtonClick = async (id) =>{
        await deleteNode(id);
        }

    return (
        <>
            <div
                onMouseEnter={handleMouseEnter}
                onMouseLeave={handleMouseLeave}
                style={{ marginBottom: "10px" }}>
                <span onClick={handleClick}>{value}</span>
                {hovered && (
                    <button onClick={() => handleRemoveButtonClick(id)} className="hover-button">x</button>
                )}
            </div>
            <ul style={{ paddingLeft: "10px", borderLeft: "1px solid black" }}>
                {showChildren && <Tree treeData={children} />}
            </ul>
        </>
    );
}

export default TreeNode;