using System.Collections.Generic;
using Engine.Drawing;
using Engine.Entities.Graphs.Internal;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Entities.Graphs
{
    public class NodeGraph
    {
        /// <summary>
        /// This is a singleton
        /// </summary>
        private static NodeGraph _instance;
        public static NodeGraph Instance => _instance ?? (_instance = new NodeGraph());

        /// <summary>
        /// Nodes
        /// </summary>
        private readonly List<Node> _nodes;

        /// <summary>
        /// Get nodes
        /// </summary>
        public List<Node> GetNodes()
        {
            return _nodes;
        }

        /// <summary>
        /// Add nodes
        /// </summary>
        /// <param name="nodes"></param>
        public void AddNodes(List<Node> nodes)
        {
            _nodes.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private NodeGraph()
        {
            _nodes = new List<Node>();
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
            foreach (Node node in _nodes)
            {
                node.Neighbors = new List<Node>();
            }
        }

        /// <summary>
        /// We don't want edges to come "too close" to any wall
        /// </summary>
        /// <param name="walls"></param>
        public void CleanUpEdgesAroundWalls(List<AbstractWall> walls, float minimumAllowedSpaceFromWall)
        {
            foreach (AbstractWall wall in walls)
            {
                foreach (Node node in _nodes)
                {
                    List<Node> newNeighbors = new List<Node>();

                    foreach (Node neighbor in node.Neighbors)
                    {
                        LineBody adHocLineBody = BodyFactory.AdHocLine(node.Position, neighbor.Position);

                        Vector2 closestPointOnLine = adHocLineBody.GetClosestPointOnPerimeter(wall.GetBody().GetPosition());
                        Vector2 closestPointOnWallToLine = wall.GetBody().GetClosestPointOnPerimeter(closestPointOnLine);
                        float distance = Vector2.Distance(closestPointOnLine, closestPointOnWallToLine);

                        // 10f is kind of just a magic number
                        if (distance - minimumAllowedSpaceFromWall > -10f)
                        {
                            newNeighbors.Add(neighbor);
                        }
                    }

                    node.Neighbors = newNeighbors;
                }
            }
        }

        /// <summary>
        /// Build shortest path
        /// </summary>
        public void BuildShortestPathData()
        {
            foreach (Node node in _nodes)
            {
                node.SetShortestPathDictionary(GraphTraversalHelper.BuildShortestPathDictionary(node));
            }
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            foreach (Node node in _nodes)
            {
                foreach (Node neighbor in node.Neighbors)
                {
                    PrimitiveFactory.DottedLine(Color.White, node.Position, neighbor.Position, 2f, 5f)
                        .Draw(graphicsDevice, basicEffect);
                }
            }

            foreach (Node node in _nodes)
            {
                node.Draw(graphicsDevice, basicEffect);
            }
        }
    }
}
