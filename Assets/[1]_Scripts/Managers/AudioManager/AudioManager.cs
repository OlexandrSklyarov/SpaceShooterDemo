using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;
using System.Linq;

namespace SA.SpaceShooter.Audio
{
    public abstract class AudioManager
    {

        #region Var       

        protected readonly SignalBus signalBus;
        protected readonly DataAudio dataAudio;

        private AudioSource musicSourceOne;
        private AudioSource musicSourceTwo;
        private AudioSource[] sfxSources;

        bool isGameMusicPlay;       

    #endregion


    #region Init

        [Inject]
        protected AudioManager(SignalBus signalBus, DataAudio dataAudio) 
        {
            this.signalBus = signalBus;
            this.dataAudio = dataAudio;

            CreateAudiosorce();

            Debug.Log("AudioManager Init...");
        }


        //создаёт пустой объект на сцене, на него вешает источники звука, и получает ссылки на них
        void CreateAudiosorce()
        {
            var go = new GameObject("AudioSource");

            //music
            musicSourceOne =  go.AddComponent<AudioSource>();
            musicSourceTwo =  go.AddComponent<AudioSource>();

            musicSourceOne.loop = true;
            musicSourceTwo.loop = true;

            //sfx
            sfxSources = new AudioSource[dataAudio.SFX_AudioSourceAmount];

            for (int i =  0;  i < dataAudio.SFX_AudioSourceAmount; i++)
            {
                var source = go.AddComponent<AudioSource>();
                source.playOnAwake = false;
                sfxSources[i] = source;
            }

            SetSFXVolume(0.3f);
        }


        #endregion


        #region Play music


        protected void PlayMusic(AudioClip musicClip)
        {
            AudioSource activeSource = GetCurrentMusicAudioSource();

            activeSource.clip = musicClip;
            activeSource.volume = 1f;
            activeSource.Play();
        }


        #endregion


        #region Play music with fade

        // public void PlayMusicWithFade(AudioClip newClip, float trasitionTime = 1f)
        // {
        //     AudioSource activeSource = GetCurrentAudioSource();

        //     StartCoroutine( UpdateMusicWithFade(activeSource, newClip, trasitionTime) );
        // }


        // public void PlayMusicWithCrossFade(AudioClip musicClip, float trasitionTime = 1f)
        // {
        //     //получаем текущи источник звука
        //     AudioSource activeSource = GetCurrentAudioSource();

        //     //получаем второй источник в зависимости от того какой уже проигрывается
        //     AudioSource newSource = (activeSource == musicSourceOne) ? musicSourceTwo : musicSourceOne;

        //     //меняем значение переменной на противоболожный 
        //     //(если был [ПЕРВЫЙ], то отметить что играет [ВТОРОЙ], и наоборот)
        //     isFirstSourcePlaying = !isFirstSourcePlaying;

        //     newSource.clip = musicClip;
        //     newSource.Play();            

        //     StartCoroutine( UpdateMusicWithCrossFade(activeSource, newSource, trasitionTime) );
        // }


        // //смена музыки на источнике с затуханием
        // IEnumerator UpdateMusicWithFade(AudioSource activeSource , AudioClip newClip, float trasitionTime)
        // {
        //     if (!activeSource.isPlaying)
        //     {
        //         activeSource.Play();
        //     }

        //     //уменьшение звука
        //     for(float i = 0f; i <= trasitionTime; i += Time.deltaTime)
        //     {
        //         activeSource.volume = (1- (i / trasitionTime));
        //         yield return null;
        //     }

        //     activeSource.Stop();
        //     activeSource.clip = newClip;
        //     activeSource.Play();
        // }


        // //перекресная смена музыки с затуханием (с одного источника на другой)
        // IEnumerator UpdateMusicWithCrossFade(AudioSource originSource , AudioSource newSource, float trasitionTime)
        // {            
        //     //уменьшение звука
        //     for(float i = 0f; i <= trasitionTime; i += Time.deltaTime)
        //     {
        //         originSource.volume = (1 - (i / trasitionTime));
        //         newSource.volume = (i / trasitionTime);
        //         yield return null;
        //     }

        //     originSource.Stop();            
        // }

        #endregion


        #region Play SFX

        protected void PlaySFX(AudioClip clip)
        {
            var sources = GetFreeAudioSource();
            sources.PlayOneShot(clip);
        }


        protected void PlaySFX(AudioClip clip, float volume)
        {
            var sources = GetFreeAudioSource();
            sources.PlayOneShot(clip, volume);
        }


        AudioSource GetFreeAudioSource()
        {
            for(int i = 0; i < sfxSources.Length; i++)
            {
                if (!sfxSources[i].isPlaying)
                {
                    return sfxSources[i];
                }
            }

            return sfxSources.First();
        }

        #endregion


        #region Volume music / SFX

        protected void SetMusicVolume(float volume)
        {
            musicSourceOne.volume = volume;
            musicSourceTwo.volume = volume;
        }


        protected void SetSFXVolume(float volume)
        {
            for (int i = 0; i < sfxSources.Length; i++)
            {
                sfxSources[i].volume = volume;

            }
        }

    #endregion


    #region  Utilyty

        AudioSource GetCurrentMusicAudioSource()
        {
            return (isGameMusicPlay) ? musicSourceOne : musicSourceTwo;
        }

    #endregion

    }
}