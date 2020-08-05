using System.Collections;
using System.Collections.Generic;
using SA.SpaceShooter.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SA.SpaceShooter.UI
{
    public class ManagerUI : MonoBehaviour
    {
        #region Var

        [SerializeField] GameObject mobileJoystick;
        [SerializeField] GameObject fireButton;

        DataConfig config;
        SignalBus signalBus;

        UnitManager unitManager;
        AsteroidGenerator asteroidGenerator;

        #endregion


        #region Init

        [Inject]
        public void Construct(DataConfig config, SignalBus signalBus)
        {
            this.config = config;
            this.signalBus = signalBus;

            InitInputButton();
        }

        #endregion


        #region Input panel

        void InitInputButton()
        {

#if UNITY_EDITOR
            mobileJoystick.SetActive(false);
            fireButton.SetActive(false);
#endif

        }

        #endregion
    }
}