using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Entities
{
    public abstract class AbstractTeam
    {
        /// <summary>
        /// Name
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Get name
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Base position
        /// </summary>
        private readonly Vector2 _basePosition;

        /// <summary>
        /// Get base position
        /// </summary>
        /// <returns></returns>
        public Vector2 GetBasePosition()
        {
            return _basePosition;
        }

        /// <summary>
        /// Color
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Players
        /// </summary>
        private readonly List<AbstractPlayer> _players;

        /// <summary>
        /// Get closest player
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public AbstractPlayer GetClosestPlayer(Vector2 position)
        {
            // Filter so we are looking at active players only
            AbstractPlayer[] activePlayers = _players.Where(p => p.IsAlive).ToArray();

            if (!activePlayers.Any())
                return null;

            AbstractPlayer result = activePlayers[0];

            float distanceSquaredToClosestPlayer = (position - result.GetBody().GetPosition()).LengthSquared();

            for (int i = 1; i < activePlayers.Length; i++)
            {
                float distanceSquared = (position - activePlayers[i].GetBody().GetPosition()).LengthSquared();

                if (activePlayers[i].IsAlive && (!result.IsAlive || distanceSquared < distanceSquaredToClosestPlayer))
                {
                    result = activePlayers[i];
                    distanceSquaredToClosestPlayer = distanceSquared;
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
            if (!_players.Contains(player))
            {
                _players.Add(player);
            }
        }

        /// <summary>
        /// Respawn points
        /// </summary>
        private readonly List<Vector2> _spawnPoints;

        /// <summary>
        /// Add spawn point
        /// </summary>
        /// <param name="spawnPoint"></param>
        public void AddSpawnPoint(Vector2 spawnPoint)
        {
            _spawnPoints.Add(spawnPoint);
        }

        /// <summary>
        /// Next spawn point index
        /// </summary>
        private int _nextSpawnPointIndex;

        /// <summary>
        /// Get next spawn point
        /// </summary>
        public Vector2 GetNextSpawnPoint()
        {
            Vector2 result = _spawnPoints[_nextSpawnPointIndex];
            _nextSpawnPointIndex++;

            if (_nextSpawnPointIndex > _spawnPoints.Count - 1)
            {
                _nextSpawnPointIndex = 0;
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
        protected AbstractTeam(string name, Vector2 basePosition, Color color)
        {
            _name = name;
            _basePosition = basePosition;
            Color = color;

            _players = new List<AbstractPlayer>();
            _spawnPoints = new List<Vector2>();
            _nextSpawnPointIndex = 0;
        }

        /// <summary>
        /// Update
        /// </summary>
        public virtual void Update()
        {
            // Update all players
            foreach (AbstractPlayer player in _players)
            {
                player.Update();
            }
        }

        /// <summary>
        /// Capture flag
        /// </summary>
        public void CaptureFlag()
        {
            Score++;
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
            foreach (AbstractPlayer player in _players)
            {
                player.Draw(graphicsDevice, basicEffect, spriteBatch);
            }
        }
    }
}
