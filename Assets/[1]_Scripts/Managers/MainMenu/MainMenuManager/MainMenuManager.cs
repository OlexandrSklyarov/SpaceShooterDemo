using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SA.SpaceShooter
{
    public class MainMenuManager : MonoBehaviour
    {
        #region Var

        SignalBus signalBus;
        DataLevel dataLevel;
        GameSettings settings;

        #endregion


        #region Init

        [Inject]
        public void Construct(SignalBus signalBus, DataLevel dataLevel)
        {
            this.signalBus = signalBus;
            this.dataLevel = dataLevel;

            settings = GameSettings.GetInstance();

            Subscription();
            LoadGame();

            StartCoroutine( StartGame(0.05f) );
        }

        IEnumerator StartGame(float time)
        {
            yield return new WaitForEndOfFrame();

            SiganlLoadGame();
            SignalPlayMusic();
        }


        void Subscription()
        {
            //нажатие на кнопку с уровнем в списке
            signalBus.Subscribe((SignalMainMenu.OnClickLevelButton s) =>
            {
                settings.CurrentLevelIndex = s.LevelIndex;
                Debug.Log("Select level");
                LoadScene(StaticPrm.Scene.GAME_LEVEL);
            });

            //нажатие на кнопку выхода
            signalBus.Subscribe<SignalMainMenu.OnClickQuitButton>(() =>
            {
                GameQuit();
            });
        }


        void LoadScene(string sceneName)
        {
            Debug.Log("Load game scene");
            SceneManager.LoadSceneAsync(sceneName);
        }

        #endregion


        #region Save / Load

        void LoadGame()
        {
            if (!settings.IsGameStarted)
            {
                if (SaveLoadManager.GetInstance().LoadGame(out PlayerSave save))
                {
                    settings.Levels = save.Levels;
                    settings.PointRecord = save.PointRecord;
                }
                else
                {
                    settings.Levels = dataLevel.Levels;
                    settings.PointRecord = 0;
                }
            }

            settings.IsGameStarted = true;
        }


        void SaveGame()
        {
            SaveLoadManager.GetInstance().SaveGame(new PlayerSave()
            { 
                Levels = settings.Levels,
                PointRecord = settings.PointRecord
            });
        }

        #endregion


        #region Siganls

        void SiganlLoadGame()
        {
            signalBus.Fire(new SignalMainMenu.LoadGame()
            {
                GameLevels = settings.Levels,
                PointRecord = settings.PointRecord
            });
        }


        void SignalPlayMusic()
        {
            signalBus.Fire(new SignalMainMenu.PlayMusicMainMenu());           
        }

        #endregion


        #region Quit

        void GameQuit()
        {
            Application.Quit();
        }


        void OnApplicationQuit()
        {
            SaveGame();
        }

        #endregion
    }
}