using Engine.Physics.Bodies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public sealed class EntityTable
    {
        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static EntityTable instance;
        public static EntityTable Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityTable();
                }

                return instance;
            }
        }

        /// <summary>
        /// Lookup table
        /// </summary>
        private static Dictionary<AbstractBody, AbstractEntity> dictionary = new Dictionary<AbstractBody, AbstractEntity>();

        /// <summary>
        /// Add map entry
        /// </summary>
        /// <param name="body"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(AbstractBody body, AbstractEntity entity)
        {
            bool result = true;

            try
            {
                // Enforce one-to-one mapping
                if (!dictionary.ContainsKey(body) && !dictionary.ContainsValue(entity))
                {
                    dictionary.Add(body, entity);
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Look up Entity by Body reference
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public AbstractEntity Get(AbstractBody body)
        {
            AbstractEntity result;

            try
            {
                result = dictionary[body];
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
    }
}