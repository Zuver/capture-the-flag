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
