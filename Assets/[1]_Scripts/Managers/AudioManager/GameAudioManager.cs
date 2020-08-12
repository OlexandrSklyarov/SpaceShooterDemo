using SA.SpaceShooter.Data;
using Zenject;

namespace SA.SpaceShooter.Audio
{
    public class GameAudioManager : AudioManager
    {
        #region Init

        public GameAudioManager(SignalBus signalBus, DataAudio dataAudio) 
            : base(signalBus, dataAudio)
        {
            SubscriptionMusic();
            SubscriptionSFX();
        }


        protected void SubscriptionSFX()
        {
            signalBus.Subscribe<SignalGame.PlaySFX_BigAsteroidDestroy>(() =>
            {
                PlaySFX(dataAudio.BigAsteroidDestroy);
            });

            signalBus.Subscribe<SignalGame.PlaySFX_SmallAsteroidDestroy>(() =>
            {
                PlaySFX(dataAudio.SmallAsteroidDestroy);
            });

            signalBus.Subscribe<SignalGame.PlaySFX_ShipDestroy>(() =>
            {
                PlaySFX(dataAudio.ShipDestroy);
            });

            signalBus.Subscribe<SignalGame.PlaySFX_BulletShoot>(() =>
            {
                PlaySFX(dataAudio.BulletShoot);
            });

            signalBus.Subscribe<SignalGame.PlaySFX_GameOver>(() =>
            {
                PlaySFX(dataAudio.GameOver);
            });
        }


        protected void SubscriptionMusic()
        {
            signalBus.Subscribe<SignalGame.PlayMusic_Game>(() =>
            {
                PlayMusic(dataAudio.GameMusic);
            });

            signalBus.Subscribe<SignalGame.PlayMusicGameMenu>(() =>
            {
                PlayMusic(dataAudio.GameMenuMusic);
            });

            signalBus.Subscribe<SignalGame.PlayMusic_Win>(() =>
            {
                PlayMusic(dataAudio.WinMusic);
            });
        }

        #endregion 
    }
}