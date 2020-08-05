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

        }

        #endregion


        #region Signals

        private void InstallSignalGame()
        {
            SignalBusInstaller.Install(Container);

            //UI
            Container.DeclareSignal<SignalGame.OnPressedPauseButton>();

        }

        #endregion
    }
}