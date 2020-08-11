
namespace SA.SpaceShooter
{
    [System.Serializable]
    public class PlayerSave
    {
        public Level[] Levels { get; set; }
        public int PointRecord { get; set; }
    }
}