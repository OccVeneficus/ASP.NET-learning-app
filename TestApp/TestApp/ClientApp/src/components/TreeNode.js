import React, { useState, useEffect } from 'react';
import Tree from './Tree';
import '../styles/TreeNode.css'

function TreeNode(props) {
    const { children, value, id } = props.node;

    const [showChildren, setShowChildren] = useState(false);
    const [expandButtonText, setExpandButtonText] = useState("+");
    const [isEditing, setIsEditing] = useState(false);
    const [editedValue, setEditedValue] = useState(value);

    useEffect(() => {
        setExpandButtonText(showChildren ? "-" : "+")
    },[showChildren])

    const handleDoubleClick = () => {
        setIsEditing(true);
    };
    
    const handleApply = () => {
        props.updateNodeHandler(id, editedValue);
        setIsEditing(false);
        setEditedValue(value);
    };
    
    const handleCancel = () => {
        setIsEditing(false);
        setEditedValue(value);
    };
    
    const handleInputChange = (event) => {
        setEditedValue(event.target.value);
    };


    const handleClick = () => {
        if (!children || children.length === 0) {
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
                className='tree-node'>
                <button onClick={handleClick} className='expand-button'>{expandButtonText}</button>
                {isEditing ? (
                        <input
                            type="text"
                            value={editedValue}
                            onChange={handleInputChange}
                        />) 
                        : (
                        <span onDoubleClick={handleDoubleClick}>{value}</span>)}
                {isEditing ? (
                <div>
                    <button onClick={handleApply} className='edit-button'>Apply</button>
                    <button onClick={handleCancel} className='edit-button'>Cancel</button>
                </div>
                ) : (
                
                    <div>
                        <button onClick={() => handleAddChildClick(id)} className="hover-button">Add</button>
                        <button onClick={() => props.deleteHandler(id)} className="hover-button">Delete</button>
                    </div>
                    
                )}
            </div>
            <ul 
                style={{ paddingLeft: "10px"}}>
                {showChildren && 
                <Tree
                treeData={children} 
                deleteNode={props.deleteHandler} 
                addChildNode={props.addChildHandler} 
                updateNodeHandler={props.updateNodeHandler}
                />}
            </ul>
        </>
    );
}

export default TreeNode;