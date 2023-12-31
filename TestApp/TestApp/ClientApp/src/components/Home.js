﻿import React, { Component } from 'react';
import Tree from './Tree';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { treeData: [], loading: true };
    this.handleReset = this.handleReset.bind(this);
    this.addChildNode = this.addChildNode.bind(this);
    this.deleteNode = this.deleteNode.bind(this); 
    this.updateNode = this.updateNode.bind(this);
  }

  async populateTree() {
    const response = await fetch('treeview');
    const data = await response.json();
    this.setState({ treeData: data, loading: false });
  }

  componentDidMount() {
    this.populateTree();
  }

  async addChildNode(parentId){
    const respose = await fetch(`treeview/${parentId}/addChild`, {method: "POST"})
    if(respose.ok){
        const newChildNode = await respose.json();

        const updatedTreeData = this.updateTreeData(this.state.treeData, parentId, (node) => {
          if (node.id === parentId) {
            return { ...node, children: [...(node.children || []), newChildNode] };
          }
          return node;
        });
        this.setState({ treeData: updatedTreeData});
    }
}

updateTreeData(tree, targetId, updateFunction) {
  return tree.map((node) => {
    if (node.id === targetId) {
      return updateFunction(node);
    }
    if (node.children && node.children.length > 0) {
      return { ...node, children: this.updateTreeData(node.children, targetId, updateFunction) };
    }
    return node;
  });
}

async updateNode(nodeId, newValue) {
  const response = await fetch(`treeview/${nodeId}/${newValue}`, {
      method: "PUT"
  });

  if(!response.ok){
    return;
  }

  const updatedTreeData = this.updateTreeData(this.state.treeData, nodeId, (node) => {
      if (node.id === nodeId) {
          return { ...node, value: newValue };
      }
      return node;
  });

  this.setState({ treeData: updatedTreeData });
}

async deleteNode(nodeId){
  await fetch(`treeview/${nodeId}`, {method: "DELETE"});
  const updatedTreeData = this.deleteNodeFromTree(this.state.treeData, nodeId);
  this.setState({ treeData: updatedTreeData});
}

deleteNodeFromTree(tree, targetId) {
  return tree.filter((node) => {
    if (node.id === targetId) {
      return false;
    }
    if (node.children && node.children.length > 0) {
      node.children = this.deleteNodeFromTree(node.children, targetId);
      return true;
    }
    return true;
  });
}

  async reset(){
    const response = await fetch('treeview/reset', {method: "POST"});
    if(response.ok){
      await this.populateTree()
    }
  }

  handleReset(){
      this.reset();
  }
    render() {
        function renderTreeView(treeData, addNode, deleteNode, updateNode) {
            return (
                <>
                  <div style={{ height: 500, overflowY: 'scroll' }}>
                    <Tree 
                      treeData={treeData}
                      deleteNode={deleteNode} 
                      addChildNode={addNode} 
                      updateNodeHandler={updateNode}/>
                  </div>
                </>
            );
          }

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderTreeView(this.state.treeData, this.addChildNode, this.deleteNode, this.updateNode);

    return (
      <div>
            <h1>React Tree View</h1>
            {contents}
            <button onClick={this.handleReset}>Reset</button>
      </div>
    );
  }
}
