using System;
using System.Collections.Generic;
using Engine.Drawing;
using Engine.Physics.Bodies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Entities.Graphs
{
    public class Node
    {
        /// <summary>
        /// Position
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Neighbors
        /// </summary>
        public List<Node> Neighbors;

        /// <summary>
        /// Shortest path dictionary
        /// </summary>
        private Dictionary<Node, Node> _shortestPathDictionary;

        /// <summary>
        /// Gets the best Node to take on the way to the goal Node
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        public Node GetShortestPathNode(Node goal)
        {
            Node result = null;
            _shortestPathDictionary.TryGetValue(goal, out result);

            return result;
        }

        /// <summary>
        /// Set shortest path dictionary
        /// </summary>
        /// <param name="shortestPathDictionary"></param>
        public void SetShortestPathDictionary(Dictionary<Node, Node> shortestPathDictionary)
        {
            _shortestPathDictionary = shortestPathDictionary;
        }

        /// <summary>
        /// Model for displaying/debugging
        /// </summary>
        private readonly CirclePrimitive _model;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"></param>
        public Node(Vector2 position)
        {
            Position = position;
            Neighbors = new List<Node>();

            // For debugging
            _model = PrimitiveFactory.Circle(Color.LightBlue, 3f, 4);
            _model.SetPosition(position);
        }

        /// <summary>
        /// Returns the best node to travserse next in order to eventually reach the given goal Node
        /// </summary>
        /// <param name="goalNode"></param>
        /// <returns></returns>
        public Node GetNextNode(Node goalNode)
        {
            if (goalNode == this)
                return null;

            // The shortest path information is assumed to be in the ShortestPathDictionary
            Node result = _shortestPathDictionary[goalNode];

            return result;
        }

        /// <summary>
        /// Constructs a list of nodes based on the geometry of the given body
        /// </summary>
        /// <param name="body"></param>
        /// <param name="distanceFromBody"></param>
        /// <param name="maxDistanceBetweenEachNode"></param>
        /// <returns></returns>
        public static List<Node> Construct(AbstractBody body, int distanceFromBody, int maxDistanceBetweenEachNode)
        {
            List<Node> result;

            Type bodyType = body.GetType();

            if (bodyType == typeof(CircleBody))
            {
                result = Construct((CircleBody)body, distanceFromBody, maxDistanceBetweenEachNode);
            }
            else
            {
                result = Construct((RectangleBody)body, distanceFromBody, maxDistanceBetweenEachNode);
            }

            return result;
        }

        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="body"></param>
        /// <param name="distanceFromBody"></param>
        /// <param name="maxDistanceBetweenEachNode"></param>
        /// <returns></returns>
        private static List<Node> Construct(CircleBody body, int distanceFromBody, int maxDistanceBetweenEachNode)
        {
            List<Node> result = new List<Node>();

            int numNodes = (int)(2 * MathHelper.Pi * body.GetRadius()) / maxDistanceBetweenEachNode;

            for (int i = 0; i < numNodes; i++)
            {
                float angle = (i / (float)numNodes) * MathHelper.TwoPi;
                Vector2 angleUnitVector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                result.Add(new Node(body.GetPosition() + (body.GetRadius() + distanceFromBody) * angleUnitVector));
            }

            return result;
        }

        /// <summary>
        /// Construct
        /// </summary>
        /// <param name="body"></param>
        /// <param name="distanceFromBody"></param>
        /// <param name="maxDistanceBetweenEachNode"></param>
        /// <returns></returns>
        private static List<Node> Construct(RectangleBody body, int distanceFromBody, int maxDistanceBetweenEachNode)
        {
            List<Node> result = new List<Node>();

            Vector2 bodyPosition = body.GetPosition();
            Vector2 bodySize = body.GetSize();

            // Top-left corner
            Vector2 northWestUnit = new Vector2(-1f, -1f);
            northWestUnit.Normalize();
            Vector2 topLeftNodePosition = bodyPosition - (bodySize / 2) + distanceFromBody * northWestUnit;
            result.Add(new Node(topLeftNodePosition));

            // Top-right corner
            Vector2 northEastUnit = new Vector2(1f, -1f);
            northEastUnit.Normalize();
            Vector2 topRightNodePosition = bodyPosition + new Vector2(bodySize.X, -bodySize.Y) / 2 + distanceFromBody * northEastUnit;
            result.Add(new Node(topRightNodePosition));

            // Bottom-right corner
            Vector2 southEastUnit = new Vector2(1f, 1f);
            southEastUnit.Normalize();
            Vector2 bottomRightNodePosition = bodyPosition + (bodySize / 2) + distanceFromBody * southEastUnit;
            result.Add(new Node(bottomRightNodePosition));

            // Bottom-left corner
            Vector2 southWestUnit = new Vector2(-1f, 1f);
            southWestUnit.Normalize();
            Vector2 bottomLeftNodePosition = bodyPosition + new Vector2(-bodySize.X, bodySize.Y) / 2 + distanceFromBody * southWestUnit;
            result.Add(new Node(bottomLeftNodePosition));

            // Top-left corner to top-right corner
            result.AddRange(FillLine(topLeftNodePosition, topRightNodePosition, maxDistanceBetweenEachNode, false));

            // Top-right corner to bottom-right corner
            result.AddRange(FillLine(topRightNodePosition, bottomRightNodePosition, maxDistanceBetweenEachNode, false));

            // Bottom-right corner to bottom-left corner
            result.AddRange(FillLine(bottomRightNodePosition, bottomLeftNodePosition, maxDistanceBetweenEachNode, false));

            // Bottom-left corner to top-left corner
            result.AddRange(FillLine(bottomLeftNodePosition, topLeftNodePosition, maxDistanceBetweenEachNode, false));

            return result;
        }

        /// <summary>
        /// Fill the line between start and end with nodes
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="maxDistanceBetweenEachNode"></param>
        /// <param name="includeEndpoints"></param>
        /// <returns></returns>
        private static List<Node> FillLine(Vector2 start, Vector2 end, int maxDistanceBetweenEachNode, bool includeEndpoints)
        {
            List<Node> result = new List<Node>();

            if (includeEndpoints)
            {
                result.Add(new Node(start));
                result.Add(new Node(end));
            }

            Vector2 startToEnd = end - start;
            float startToEndLength = startToEnd.Length();

            int numNodes = (int)(startToEndLength / maxDistanceBetweenEachNode);
            int step = (int)(startToEndLength / (numNodes + 1));

            Vector2 startToEndUnit = end - start;
            startToEndUnit.Normalize();

            for (int i = 0; i < numNodes; i++)
            {
                start += step * startToEndUnit;
                result.Add(new Node(start));
            }

            return result;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            _model.Draw(graphicsDevice, basicEffect);
        }
    }
}
