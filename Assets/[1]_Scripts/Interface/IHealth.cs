namespace SA.SpaceShooter
{
    public interface IHealth
    {
        int HP { get; }

        void Damage();
    }
}