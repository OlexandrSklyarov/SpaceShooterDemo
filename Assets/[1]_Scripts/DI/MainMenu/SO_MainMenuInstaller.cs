using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter
{
    [CreateAssetMenu(fileName = "SO_MainMenuInstaller", menuName = "CustomInstallers/SO_MainMenuInstaller")]
    public class SO_MainMenuInstaller : ScriptableObjectInstaller<SO_MainMenuInstaller>
    {
        #region Var

        [SerializeField] DataLevel dataLevel;
        [SerializeField] DataLevelUI dataLeveUI;
        [SerializeField] DataAudio dataAudio;

        #endregion


        #region Inject

        public override void InstallBindings()
        {
            Container.BindInstance(dataLevel);
            Container.BindInstance(dataLeveUI);
            Container.BindInstance(dataAudio);
        }

        #endregion
    }
}