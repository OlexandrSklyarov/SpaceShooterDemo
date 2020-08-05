
namespace SA.SpaceShooter.Input
{
    public class PlayerInput
    {
        #region Properties

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public bool IsFire { get; private set; }

        #endregion      


        #region Update

        public void Tick()
        {
            GetInput();
        }


        void GetInput()
        {

#if UNITY_EDITOR

            Horizontal = UnityEngine.Input.GetAxis(StaticPrm.Input.HORIZONTAL);
            Vertical = UnityEngine.Input.GetAxis(StaticPrm.Input.VERTICAL);
            IsFire = UnityEngine.Input.GetButton(StaticPrm.Input.FIRE);

#elif UNITY_ANDROID

            Horizontal = SimpleInput.GetAxis(StaticPrm.Input.HORIZONTAL);
            Vertical = SimpleInput.GetAxis(StaticPrm.Input.VERTICAL);
            IsFire = SimpleInput.GetButtonDown(StaticPrm.Input.FIRE);

#endif
        }

        #endregion
    }
}