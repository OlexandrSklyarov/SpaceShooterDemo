using UnityEngine;

namespace SA.SpaceShooter
{
    [System.Serializable]
    public class ShipParameters
    {
        #region Properties

        public float Speed => speed;
        public float MaxRotateAngle => maxRotateAngle;
        public float Mass => mass;
        public float Drag => drag;
        public float AngularDrag => angularDrag;
        public int MaxHP => maxHP;
        public GameObject BulletPrefab => bulletPrefab;
        public GameObject ShipDestroyVFX => shipDestroyVFX;
        public float FireCooldown => fireCooldown;

        #endregion


        #region Var

        [SerializeField] [Range(0.1f, 200f)] float speed = 1f;
        [SerializeField] [Range(0f, 90f)] float maxRotateAngle = 20f;
        [SerializeField] [Range(0.1f, 200f)] float mass = 1f;
        [SerializeField] [Range(0f, 200f)] float drag;
        [SerializeField] [Range(0f, 200f)] float angularDrag = 0.05f;

        [Space]
        [Header("Health")]
        [SerializeField] [Range(1, 100)] int maxHP;
        [SerializeField] GameObject shipDestroyVFX;

        [Space]
        [Header("Weapon")]
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] [Range(0.1f, 2f)] float fireCooldown = 0.25f;

        #endregion
    }
}