using SA.SpaceShooter.Audio;
using Zenject;

namespace SA.SpaceShooter
{
    public class GameInstaller : MonoInstaller
    {
        #region Init

        public override void InstallBindings()
        {
            InstallManagers();
            InstallSignalGame();
        }

        #endregion


        #region Managers

        private void InstallManagers()
        {
            Container.Bind<AudioManager>().AsSingle().NonLazy();
        }

        #endregion


        #region Signals

        private void InstallSignalGame()
        {
            SignalBusInstaller.Install(Container);

            //UI
            Container.DeclareSignal<SignalGame.OnClickPauseButton>();
            Container.DeclareSignal<SignalGame.OnClickRestartButton>();
            Container.DeclareSignal<SignalGame.OnClickContinueGameButton>();
            Container.DeclareSignal<SignalGame.OnClickMainMenuButton>();

            //GameManager
            Container.DeclareSignal<SignalGame.ChangeGameMode>();

            //player 
            Container.DeclareSignal<SignalGame.AddPoints>();
            Container.DeclareSignal<SignalGame.UpdatePointSum>();
            Container.DeclareSignal<SignalGame.ChangePlayerHP>();
            Container.DeclareSignal<SignalGame.PlayerDestroy>();

            //Audio 
            Container.DeclareSignal<SignalGame.PlaySFX_BigAsteroidDestroy>();
            Container.DeclareSignal<SignalGame.PlaySFX_SmallAsteroidDestroy>();
            Container.DeclareSignal<SignalGame.PlaySFX_ShipDestroy>();
            Container.DeclareSignal<SignalGame.PlaySFX_BulletShoot>();
            Container.DeclareSignal<SignalGame.PlayMusic_Game>();
            Container.DeclareSignal<SignalGame.PlayMusicMainMenu>();
            Container.DeclareSignal<SignalGame.PlayMusicGameMenu>();

        }

        #endregion
    }
}