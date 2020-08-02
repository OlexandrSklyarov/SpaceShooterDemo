namespace SA.Pool
{
    public interface IPoolable
    {
        int PoolID {get; set;}
        void OnSpawn();
        void OnDespawn();
    }
}