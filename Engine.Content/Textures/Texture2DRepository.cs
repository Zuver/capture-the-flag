using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Content.Textures
{
    public class Texture2DRepository
    {
        /// <summary>
        /// Private data
        /// </summary>
        private readonly Dictionary<string, Texture2D> _texture2DDictionary;

        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static Texture2DRepository _instance;
        public static Texture2DRepository Instance => _instance ?? (_instance = new Texture2DRepository());

        /// <summary>
        /// Constructor
        /// </summary>
        private Texture2DRepository()
        {
            _texture2DDictionary = new Dictionary<string, Texture2D>();
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="texture">Texture2D</param>
        public void Add(string key, Texture2D texture)
        {
            try
            {
                _texture2DDictionary.Add(key, texture);
            }
            catch (Exception)
            {
                // TODO: Log error
            }
        }

        /// <summary>
        /// Get Texture2D instance by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Texture2D instance</returns>
        public Texture2D Get(string key)
        {
            Texture2D result;

            try
            {
                result = _texture2DDictionary[key];
            }
            catch (Exception)
            {
                result = null;
                // TODO: Log error
            }

            return result;
        }
    }
}