using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataAudio", menuName = "Data/DataAudio")]
    public class DataAudio : ScriptableObject
    {
        #region Properties

        public AudioClip BigAsteroidDestroy => bigAsteroidDestroy;
        public AudioClip SmallAsteroidDestroy => smallAsteroidDestroy;
        public AudioClip ShipDestroy => shipDestroy;       
        public AudioClip BulletShoot => bulletShoot;

        public int SFX_AudioSourceAmount => sfx_AudioSourceAmount;

        public AudioClip GameMusic => gameMusic;
        public AudioClip MainMenuMusic => mainMenuMusic;
        public AudioClip GameMenuMusic => gameMenuMusic;

        #endregion


        #region Var

        [Header("SFX")]
        [SerializeField] AudioClip bigAsteroidDestroy;
        [SerializeField] AudioClip smallAsteroidDestroy;
        [SerializeField] AudioClip shipDestroy;
        [SerializeField] AudioClip bulletShoot;
        [SerializeField] [Range(1, 50)] int sfx_AudioSourceAmount;

        [Space]
        [Header("Music")]
        [SerializeField] AudioClip gameMusic;
        [SerializeField] AudioClip mainMenuMusic;
        [SerializeField] AudioClip gameMenuMusic;

        #endregion
    }
}