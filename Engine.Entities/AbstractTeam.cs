using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public abstract class AbstractTeam
    {
        /// <summary>
        /// Name
        /// </summary>
        private string Name;

        /// <summary>
        /// Get name
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.Name;
        }

        /// <summary>
        /// Base position
        /// </summary>
        private Vector2 BasePosition;

        /// <summary>
        /// Get base position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetBasePosition()
        {
            return this.BasePosition;
        }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Players
        /// </summary>
        private List<AbstractPlayer> Players;

        /// <summary>
        /// Get closest player
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public AbstractPlayer GetClosestPlayer(Vector2 position)
        {
            if (this.Players.Count == 0)
            {
                return null;
            }

            AbstractPlayer result = this.Players[0];
            float distanceToClosestPlayerSquared = (position - result.GetBody().GetPosition()).LengthSquared();

            for (int i = 1; i < this.Players.Count; i++)
            {
                float distanceSquared = (position - this.Players[i].GetBody().GetPosition()).LengthSquared();

                if (this.Players[i].IsAlive && (!result.IsAlive || distanceSquared < distanceToClosestPlayerSquared))
                {
                    result = this.Players[i];
                    distanceToClosestPlayerSquared = distanceSquared;
                }
            }

            return result;
        }

        /// <summary>
        /// Add player
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(AbstractPlayer player)
        {
            if (!this.Players.Contains(player))
            {
                this.Players.Add(player);
            }
        }

        /// <summary>
        /// Spawn points
        /// </summary>
        private List<Vector2> SpawnPoints;

        /// <summary>
        /// Add spawn point
        /// </summary>
        /// <param name="spawnPoint"></param>
        public void AddSpawnPoint(Vector2 spawnPoint)
        {
            this.SpawnPoints.Add(spawnPoint);
        }

        /// <summary>
        /// Next spawn point index
        /// </summary>
        private int NextSpawnPointIndex;

        /// <summary>
        /// Get next spawn point
        /// </summary>
        public Vector2 GetNextSpawnPoint()
        {
            Vector2 result = this.SpawnPoints[this.NextSpawnPointIndex];
            this.NextSpawnPointIndex++;

            if (this.NextSpawnPointIndex > this.SpawnPoints.Count - 1)
            {
                this.NextSpawnPointIndex = 0;
            }

            return result;
        }

        /// <summary>
        /// Score
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="basePosition"></param>
        /// <param name="color"></param>
        public AbstractTeam(string name, Vector2 basePosition, Color color)
        {
            this.Name = name;
            this.BasePosition = basePosition;
            this.Color = color;

            this.Players = new List<AbstractPlayer>();
            this.SpawnPoints = new List<Vector2>();
            this.NextSpawnPointIndex = 0;
        }

        /// <summary>
        /// Update
        /// </summary>
        public virtual void Update()
        {
            // Update all players
            foreach (AbstractPlayer player in this.Players)
            {
                player.Update();
            }
        }

        /// <summary>
        /// Capture flag
        /// </summary>
        public void CaptureFlag()
        {
            this.Score++;
        }

        /// <summary>
        /// Get flag (abstract method)
        /// </summary>
        /// <returns></returns>
        public abstract AbstractFlag GetFlag();

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            // Draw all players
            foreach (AbstractPlayer player in this.Players)
            {
                player.Draw(graphicsDevice, basicEffect, spriteBatch);
            }
        }
    }
}