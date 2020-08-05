using UnityEngine;

namespace SA.SpaceShooter.Ship
{
    public class ShipMoving
    {
        #region Var

        private Rigidbody rb;
        private ShipParameters prm;

        #endregion


        #region Init

        public ShipMoving(Rigidbody rb, ShipParameters prm)
        {
            this.rb = rb;
            this.prm = prm;
        }

        #endregion


        #region Update

        public void Move(float horizontal, float vertical)
        {
            rb.velocity = new Vector3(horizontal, 0f, vertical) * prm.Speed;
        }


        public void Rotation(float AngleY = 0f)
        {
            rb.rotation = Quaternion.Euler(0f, AngleY, rb.velocity.x * -prm.MaxRotateAngle);
        }


        public float GetManeuverValue(float targetManeuver, float speed)
        {
            return Mathf.MoveTowards(rb.velocity.x, targetManeuver, speed * Time.deltaTime);

        }

        #endregion
    }
}