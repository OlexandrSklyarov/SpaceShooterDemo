using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter
{
    [CreateAssetMenu(fileName = "SO_GameInstaller", menuName = "CustomInstallers/SO_GameInstaller")]
    public class SO_GameInstaller : ScriptableObjectInstaller<SO_GameInstaller>
    {
        #region Var

        [SerializeField] DataGame data;
        [SerializeField] DataConfig config;
        [SerializeField] DataBackgraund dataBackgraund;
        [SerializeField] DataAudio dataAudio;

        #endregion


        #region Inject

        public override void InstallBindings()
        {
            Container.BindInstance(data);
            Container.BindInstance(config);
            Container.BindInstance(dataBackgraund);
            Container.BindInstance(dataAudio);
        }

        #endregion
    }
}