import React, { Component, useState } from 'react';

export class Home extends Component {
  static displayName = Home.name;

    render() {
        const treeData = [
            {
                key: "0",
                label: "Documents",
                children: [
                    {
                        key: "0-0",
                        label: "Document 1-1",
                        children: [
                            {
                                key: "0-1-1",
                                label: "Document-0-1.doc",
                            },
                            {
                                key: "0-1-2",
                                label: "Document-0-2.doc",
                            },
                        ],
                    },
                ],
            },
            {
                key: "1",
                label: "Desktop",
                children: [
                    {
                        key: "1-0",
                        label: "document1.doc",
                    },
                    {
                        key: "0-0",
                        label: "documennt-2.doc",
                    },
                ],
            },
            {
                key: "2",
                label: "Downloads",
                children: [],
            },
        ];
        function Tree({ treeData }) {
            return (
                <ul>
                    {treeData.map((node) => (
                        <TreeNode node={node} key={node.key} />
                    ))}
                </ul>
            );
        }

        function TreeNode({ node }) {
            const { children, label } = node;

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
            return (
                <>
                    <div
                        onClick={handleClick}
                        onMouseEnter={handleMouseEnter}
                        onMouseLeave={handleMouseLeave}
                        style={{ marginBottom: "10px" }}>
                        <span>{label}</span>
                    </div>
                    <button>x</button>
                    <ul style={{ paddingLeft: "10px", borderLeft: "1px solid black" }}>
                        {showChildren && <Tree treeData={children} />}
                    </ul>
                </>
            );
        }
    return (
      <div>
            <h1>React Tree View</h1>
            <Tree treeData={treeData} />
      </div>
    );
  }
}
