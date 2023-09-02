import React, { useState, useEffect } from 'react';
import Tree from './Tree';

function TreeNode(props) {
    const { children, value, id } = props.node;
    
    const [showChildren, setShowChildren] = useState(false);

    const [hovered, setHovered] = useState(false);

    const [expandButtonText, setExpandButtonText] = useState("+");

    useEffect(() => {
        setExpandButtonText(showChildren ? "-" : "+")
    },[showChildren])

    const handleMouseEnter = () => {
        setHovered(true);
    };

    const handleMouseLeave = () => {
        setHovered(false);
    };

    const handleClick = () => {
        if (typeof children === 'undefined' || children.length === 0) {
            return;
        }

        setShowChildren(!showChildren);
    };

    const handleAddChildClick = (id) => {
        props.addChildHandler(id)
        setShowChildren(true);
    }

    return (
        <>
            <div
                onMouseEnter={handleMouseEnter}
                onMouseLeave={handleMouseLeave}
                style={{ 
                    marginBottom: "10px",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "space-between",
                     }}>
                <button onClick={handleClick}>{expandButtonText}</button>
                <span style={{ flex: "1" }}>{value}</span>
                {hovered && (
                    <div style={{ 
                        display: "flex",
                        alignContent: "left",
                         }}>
                        <button onClick={() => props.deleteHandler(id)} className="hover-button">delete</button>
                        <button onClick={() => handleAddChildClick(id)} className="hover-button">add</button>
                    </div>
                )}
            </div>
            <ul style={{ paddingLeft: "10px", borderLeft: "1px solid black" }}>
                {showChildren && <Tree treeData={children} />}
            </ul>
        </>
    );
}

export default TreeNode;