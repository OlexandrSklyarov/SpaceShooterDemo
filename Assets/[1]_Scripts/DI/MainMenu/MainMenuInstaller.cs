using SA.SpaceShooter.Audio;
using Zenject;

namespace SA.SpaceShooter
{
    public class MainMenuInstaller : MonoInstaller
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
            Container.Bind<AudioManager>().To<MainMenuAudioManager>().AsSingle().NonLazy();
        }

        #endregion


        #region Signals

        private void InstallSignalGame()
        {
            SignalBusInstaller.Install(Container);

            //Menu manger
            Container.DeclareSignal<SignalMainMenu.LoadGame>();

            //UI
            Container.DeclareSignal<SignalMainMenu.OnClickLevelButton>();
            Container.DeclareSignal<SignalMainMenu.OnClickQuitButton>();

            //Audio
            Container.DeclareSignal<SignalMainMenu.PlaySFX_ClickButton>();
            Container.DeclareSignal<SignalMainMenu.PlayMusicMainMenu>();

        }

        #endregion
    }
}