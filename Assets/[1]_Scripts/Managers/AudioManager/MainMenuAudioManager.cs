using SA.SpaceShooter.Data;
using Zenject;

namespace SA.SpaceShooter.Audio
{
    public class MainMenuAudioManager : AudioManager
    {
        #region Init

        public MainMenuAudioManager(SignalBus signalBus, DataAudio dataAudio) 
            : base(signalBus, dataAudio)
        {
            SubscriptionMusic();
            SubscriptionSFX();
        }


        protected void SubscriptionSFX()
        {
            signalBus.Subscribe<SignalMainMenu.PlaySFX_ClickButton>(() =>
            {
                //PlaySFX(dataAudio.BigAsteroidDestroy);
            });
        }


        protected void SubscriptionMusic()
        {

            signalBus.Subscribe<SignalMainMenu.PlayMusicMainMenu>(() =>
            {
                PlayMusic(dataAudio.MainMenuMusic);
            });
        }

        #endregion 
    }
}