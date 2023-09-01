import React, { Component, useState } from 'react';
import Tree from './Tree';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { treeData: [], loading: true };
  }

  async populateTreeViewWithInitialData() {
    const response = await fetch('treeview');
    const data = await response.json();
    this.setState({ treeData: data, loading: false });
  }

  componentDidMount() {
    this.populateTreeViewWithInitialData();
  }

    render() {
        function renderTreeView(treeData) {
            return (
                <Tree treeData={treeData} />
            );
          }

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : renderTreeView(this.state.treeData);

    return (
      <div>
            <h1>React Tree View</h1>
            {contents}
      </div>
    );
  }
}
