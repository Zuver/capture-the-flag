using Engine.Content;
using Engine.Drawing;
using Engine.Entities;
using Engine.Entities.Graphs;
using Engine.Physics.Bodies;
using Engine.UI.Labels;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public abstract class AbstractMap
    {
        /// <summary>
        /// Size of the map
        /// </summary>
        protected Vector2 Size;

        /// <summary>
        /// Walls
        /// </summary>
        protected List<AbstractWall> Walls;

        /// <summary>
        /// Get walls
        /// </summary>
        /// <returns></returns>
        public List<AbstractWall> GetWalls()
        {
            return this.Walls;
        }

        /// <summary>
        /// Add wall
        /// </summary>
        public void AddWall(AbstractWall wall)
        {
            this.Walls.Add(wall);
            NodeGraph.Instance.AddNodes(Node.Construct(wall.GetBody(), (int)AppSettingsFacade.PlayerRadius, 50));
            this.ConstructNodeGraphEdges();
            NodeGraph.Instance.BuildShortestPathData();
        }

        /// <summary>
        /// Teams
        /// </summary>
        private List<AbstractTeam> Teams;

        /// <summary>
        /// Add team
        /// </summary>
        /// <param name="team"></param>
        public void AddTeam(AbstractTeam team)
        {
            this.Teams.Add(team);
        }

        /// <summary>
        /// Guns
        /// </summary>
        private List<AbstractGun> Guns;

        /// <summary>
        /// Add gun
        /// </summary>
        /// <param name="gun"></param>
        public void AddGun(AbstractGun gun)
        {
            this.Guns.Add(gun);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size"></param>
        public AbstractMap(Vector2 size)
        {
            this.Size = size;
            this.Walls = new List<AbstractWall>();
            this.Teams = new List<AbstractTeam>();
            this.Guns = new List<AbstractGun>();

            this.ConstructNodeGraphEdges();
        }

        #region Abstract methods

        protected abstract void Initialize();

        #endregion

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            foreach (AbstractWall wall in this.Walls)
            {
                wall.Update();
            }

            foreach (AbstractTeam team in this.Teams)
            {
                team.Update();
            }

            foreach (AbstractGun gun in this.Guns)
            {
                gun.Update();
            }
        }

        /// <summary>
        /// Gets the gun that is closest to the given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public AbstractGun GetClosestGun(Vector2 position)
        {
            AbstractGun result = null;

            // Obtain guns that have no owner
            AbstractGun[] orhpanGuns = this.Guns.Where(n => !n.HasOwner()).ToArray();

            if (orhpanGuns.Length > 0)
            {
                // Set default result
                result = orhpanGuns[0];

                float closestDistanceSquared = (position - result.GetBody().GetPosition()).LengthSquared();

                for (int i = 1; i < orhpanGuns.Length; i++)
                {
                    float distanceSquared = (position - orhpanGuns[i].GetBody().GetPosition()).LengthSquared();

                    if (distanceSquared < closestDistanceSquared)
                    {
                        result = orhpanGuns[i];
                        closestDistanceSquared = distanceSquared;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the closest player to the given location on the given team
        /// </summary>
        /// <param name="team"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public AbstractPlayer GetClosestPlayer(AbstractTeam team, Vector2 position)
        {
            return team.GetClosestPlayer(position);
        }

        /// <summary>
        /// Gets the sequence of Nodes to use as the quickest route from start to goal
        /// </summary>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public List<Node> GetShortestRoute(Vector2 start, Vector2 goal)
        {
            List<Node> result = new List<Node>();

            // Add the first Node in the route
            Node closestNodeToStart = this.GetClosestNode(start);
            result.Add(closestNodeToStart);

            Node closestNodeToGoal = this.GetClosestNode(goal);

            Node currentNode = closestNodeToStart;

            while (currentNode != closestNodeToGoal && !currentNode.Neighbors.Contains(closestNodeToGoal))
            {
                Node nextNode = currentNode.GetShortestPathNode(closestNodeToGoal);
                result.Add(nextNode);
                currentNode = nextNode;
            }

            if (result == null)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Returns a list of visible nodes from the given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public List<Node> GetVisibleNodes(Vector2 position)
        {
            List<Node> result = new List<Node>();

            foreach (Node node in NodeGraph.Instance.GetNodes())
            {
                bool isWallInTheWay = CheckLineCollision(position, node.Position);

                if (!isWallInTheWay)
                {
                    result.Add(node);
                }
            }

            return result;
        }

        /// <summary>
        /// Check whether a given line intersects with any of the walls
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool CheckLineCollision(Vector2 start, Vector2 end)
        {
            bool result = false;

            LineBody lineBody = BodyFactory.Line(true, 0f, 0f, 0f, false, start, end);
            
            foreach (AbstractWall wall in this.Walls)
            {
                result = wall.GetBody().DetectCollision(lineBody);

                if (result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            foreach (AbstractWall wall in this.Walls)
            {
                wall.Draw(graphicsDevice, basicEffect, spriteBatch);
            }

            if (AppSettingsFacade.IsDebugModeOn)
            {
                NodeGraph.Instance.Draw(graphicsDevice, basicEffect, true);
            }

            foreach (AbstractTeam team in this.Teams)
            {
                team.Draw(graphicsDevice, basicEffect, spriteBatch);
            }

            foreach (AbstractGun gun in this.Guns)
            {
                gun.Draw(graphicsDevice, basicEffect, spriteBatch);
            }

            for (int i = 0; i < this.Teams.Count; i++)
            {
                spriteBatch.DrawString(SpriteFontRepository.Instance.Get("test"), this.Teams[i].Score.ToString(), new Vector2(AppSettingsFacade.WindowWidth / 2, 10 + (i * 30)), this.Teams[i].Color);
            }
        }

        /// <summary>
        /// Construct outer walls
        /// </summary>
        protected abstract void ConstructOuterWalls(Vector2 size);

        /// <summary>
        /// Gets the closest "visible" Node to the given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Node GetClosestNode(Vector2 position)
        {
            // Candidates can only be Nodes that are "visible" from the given positon
            List<Node> candidates = this.GetVisibleNodes(position);

            Node result = candidates.FirstOrDefault();

            if (candidates.Count > 1)
            {
                float shortestDistance = (position - result.Position).LengthSquared();

                foreach (Node node in candidates)
                {
                    float distance = (position - node.Position).LengthSquared();

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        result = node;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Construct node graph edges
        /// </summary>
        private void ConstructNodeGraphEdges()
        {
            // Clear
            NodeGraph.Instance.ClearEdges();

            // Rebuild
            foreach (Node node1 in NodeGraph.Instance.GetNodes())
            {
                foreach (Node node2 in NodeGraph.Instance.GetNodes())
                {
                    if (node1 != node2)
                    {
                        if (!this.CheckLineCollision(node1.Position, node2.Position))
                        {
                            NodeGraph.Instance.AddEdge(node1, node2);
                        }
                    }
                }
            }
        }
    }
}