using UnityEngine;

namespace SA.SpaceShooter
{
    [System.Serializable]
    public class EnemyManeuverParameters
    {
        #region Properties

        public Vector2 StartManeuverTime => startManeuverTime;
        public Vector2 ManeuverTime => maneuverTime;
        public Vector2 PauseManeuverTime => pauseManeuverTime;
        public float DodgeRange => dodgeRange;
        public float SpeedManeuver => speedManeuver;

        #endregion


        #region Var

        [SerializeField] Vector2 startManeuverTime;
        [SerializeField] Vector2 maneuverTime;
        [SerializeField] Vector2 pauseManeuverTime;
        [SerializeField] [Range(0.1f, 10f)] float dodgeRange = 10f;
        [SerializeField] [Range(0.1f, 10f)] float speedManeuver = 1f;

        #endregion
    }
}
