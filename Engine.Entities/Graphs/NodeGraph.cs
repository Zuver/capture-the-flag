using Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities.Graphs
{
    public class NodeGraph
    {
        /// <summary>
        /// This is a singleton
        /// </summary>
        private static NodeGraph _instance;
        public static NodeGraph Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NodeGraph();

                return _instance;
            }
        }

        /// <summary>
        /// Nodes
        /// </summary>
        private List<Node> Nodes;

        /// <summary>
        /// Get nodes
        /// </summary>
        public List<Node> GetNodes()
        {
            return this.Nodes;
        }

        /// <summary>
        /// Add nodes
        /// </summary>
        /// <param name="nodes"></param>
        public void AddNodes(List<Node> nodes)
        {
            this.Nodes.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private NodeGraph()
        {
            this.Nodes = new List<Node>();
        }

        /// <summary>
        /// Add edge
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        public void AddEdge(Node node1, Node node2)
        {
            node1.Neighbors.Add(node2);
            node2.Neighbors.Add(node1);
        }

        /// <summary>
        /// Clear edges
        /// </summary>
        public void ClearEdges()
        {
            foreach (Node node in this.Nodes)
            {
                node.Neighbors = new List<Node>();
            }
        }

        /// <summary>
        /// Build shortest path
        /// </summary>
        public void BuildShortestPathData()
        {
            foreach (Node node in this.Nodes)
            {
                node.SetShortestPathDictionary(GraphTraversalHelper.BuildShortestPathDictionary(node));
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="drawEdges"></param>
        public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, bool drawEdges)
        {
            if (drawEdges)
            {
                foreach (Node node in this.Nodes)
                {
                    foreach (Node neighbor in node.Neighbors)
                    {
                        PrimitiveFactory.DottedLine(Color.White, node.Position, neighbor.Position, 1f, 5f)
                            .Draw(graphicsDevice, basicEffect);
                    }
                }
            }

            foreach (Node node in this.Nodes)
            {
                node.Draw(graphicsDevice, basicEffect);
            }
        }
    }
}