using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Content.Fonts
{
    public class SpriteFontRepository
    {
        private readonly Dictionary<string, SpriteFont> _spriteFontDictionary;

        private ContentManager _contentManager;

        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static SpriteFontRepository _instance;
        public static SpriteFontRepository Instance => _instance ?? (_instance = new SpriteFontRepository());

        /// <summary>
        /// Constructor
        /// </summary>
        private SpriteFontRepository()
        {
            _spriteFontDictionary = new Dictionary<string, SpriteFont>();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="contentManager">ContentManager instance</param>
        public void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        /// <summary>
        /// Add font by name
        /// </summary>
        /// <param name="name">Name</param>
        public void Add(string name)
        {
            try
            {
                SpriteFont spriteFont = _contentManager.Load<SpriteFont>(name);
                _spriteFontDictionary.Add(name, spriteFont);
            }
            catch (Exception)
            {
                // TODO: Log error
            }
        }

        /// <summary>
        /// Get SpriteFont instance by key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>SpriteFont instance</returns>
        public SpriteFont Get(string key)
        {
            SpriteFont result;

            try
            {
                result = _spriteFontDictionary[key];
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