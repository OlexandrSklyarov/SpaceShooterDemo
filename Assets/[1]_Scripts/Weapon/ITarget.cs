
namespace SA.SpaceShooter
{
    public interface ITarget
    {
       Target TargetType {get;}
    }
     

    public enum Target { PLAYER, OTHER }

}