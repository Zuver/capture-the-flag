using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Content
{
    public class SpriteFontRepository
    {
        /// <summary>
        /// Private data
        /// </summary>
        private Dictionary<string, SpriteFont> SpriteFontDictionary;
        private ContentManager ContentManager;

        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static SpriteFontRepository instance;
        public static SpriteFontRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpriteFontRepository();
                }

                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private SpriteFontRepository()
        {
            this.SpriteFontDictionary = new Dictionary<string, SpriteFont>();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="contentManager">ContentManager instance</param>
        public void Initialize(ContentManager contentManager)
        {
            this.ContentManager = contentManager;
        }

        /// <summary>
        /// Add font by name
        /// </summary>
        /// <param name="name">Name</param>
        public void Add(string name)
        {
            try
            {
                SpriteFont spriteFont = this.ContentManager.Load<SpriteFont>(name);
                this.SpriteFontDictionary.Add(name, spriteFont);
            }
            catch (Exception e)
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
            SpriteFont result = null;

            try
            {
                result = this.SpriteFontDictionary[key];
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