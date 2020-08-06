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

        }

        #endregion
    }
}