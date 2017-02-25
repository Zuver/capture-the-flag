using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public interface ICollisionBehaviors
    {
        void ReactToCollision(AbstractPlayer player);
        void ReactToCollision(AbstractGun gun);
        void ReactToCollision(AbstractBullet bullet);
        void ReactToCollision(AbstractWall wall);
    }
}