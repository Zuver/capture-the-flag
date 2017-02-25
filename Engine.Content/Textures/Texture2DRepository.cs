using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Content.Textures
{
    public class Texture2DRepository
    {
        /// <summary>
        /// Private data
        /// </summary>
        private Dictionary<string, Texture2D> Texture2DDictionary;

        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static Texture2DRepository instance;
        public static Texture2DRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Texture2DRepository();
                }

                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Texture2DRepository()
        {
            this.Texture2DDictionary = new Dictionary<string, Texture2D>();
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
                this.Texture2DDictionary.Add(key, texture);
            }
            catch (Exception e)
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
            Texture2D result = null;

            try
            {
                result = this.Texture2DDictionary[key];
            }
            catch (Exception e)
            {
                result = null;
                // TODO: Log error
            }

            return result;
        }
    }
}