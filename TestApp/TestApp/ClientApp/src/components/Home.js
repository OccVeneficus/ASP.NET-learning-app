import React, { Component, useState } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { treeData: [], loading: true };
  }

  componentDidMount() {
    this.populateTreeViewWithInitialData();
  }

    render() {

        function TreeNode({ node }) {
            const { children, value, key } = node;

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

            
            async function  deleteNode(nodeKey){
    await fetch(`treeview/${nodeKey}`, {method: "DELETE"})
    const response = await fetch('treeview');
    const data = await response.json();
    this.setState({ treeData: data, loading: false });
  }
            const handleRemoveButtonClick = (key) =>{
                deleteNode(key)
                }

            return (
                <>
                    <div
                        onMouseEnter={handleMouseEnter}
                        onMouseLeave={handleMouseLeave}
                        style={{ marginBottom: "10px" }}>
                        <span onClick={handleClick}>{value}</span>
                        {hovered && (
                            
                            <button onClick={() => handleRemoveButtonClick(key)} className="hover-button">x</button>
                        )}
                    </div>
                    <ul style={{ paddingLeft: "10px", borderLeft: "1px solid black" }}>
                        {showChildren && <Tree treeData={children} />}
                    </ul>
                </>
            );
        }

        function renderTreeView(treeData) {
            return (
                <Tree treeData={treeData} />
            );
          }

        let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : renderTreeView(this.state.treeData);

        function Tree({ treeData }) {
            return (
                <ul>
                    {treeData.map((node) => (
                        <TreeNode node={node} key={node.key} />
                    ))}
                </ul>
            );
        }

    return (
      <div>
            <h1>React Tree View</h1>
            {contents}
      </div>
    );
  }

  async populateTreeViewWithInitialData() {
    const response = await fetch('treeview');
    const data = await response.json();
    this.setState({ treeData: data, loading: false });
  }
}
