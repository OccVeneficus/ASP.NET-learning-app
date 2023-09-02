import React, { Component, useState } from 'react';
import Tree from './Tree';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { treeData: [], loading: true };
    this.handleReset = this.handleReset.bind(this);
  }

  async populateTree() {
    const response = await fetch('treeview');
    const data = await response.json();
    this.setState({ treeData: data, loading: false });
  }

  componentDidMount() {
    this.populateTree();
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
        function renderTreeView(treeData) {
            return (
                <>
                  <Tree treeData={treeData} />
                </>
            );
          }

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderTreeView(this.state.treeData);

    return (
      <div>
            <h1>React Tree View</h1>
            {contents}
            <button onClick={this.handleReset}>Reset</button>
      </div>
    );
  }
}
