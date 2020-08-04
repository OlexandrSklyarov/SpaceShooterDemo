using UnityEngine;

namespace SA.SpaceShooter
{
    [System.Serializable]
    public class EnemyParameters
    {
        #region Properties

        public float DodgeRange => dodgeRange;
        public float StartManeuverTime => startManeuverTime;
        public float ManeuverTime => maneuverTime;
        public float PauseManeuverTime => pauseManeuverTime;
        public float SpeedManeuver => speedManeuver;

        #endregion


        #region Var

        [SerializeField] [Range(0.1f, 10f)] float dodgeRange = 10f;
        [SerializeField] [Range(1f, 2f)]  float startManeuverTime = 2f;
        [SerializeField] [Range(0.1f, 5f)]  float maneuverTime = 1f;
        [SerializeField] [Range(1f, 3f)]  float pauseManeuverTime = 2f;
        [SerializeField] [Range(0.1f, 10f)] float speedManeuver = 1f;

        #endregion
    }
}
