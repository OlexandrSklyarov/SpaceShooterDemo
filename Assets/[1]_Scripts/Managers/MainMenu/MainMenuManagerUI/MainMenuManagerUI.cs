using SA.SpaceShooter.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace SA.SpaceShooter.UI
{
    public class MainMenuManagerUI : MonoBehaviour
    {
        #region Var

        [Header("Top panel")]
        [SerializeField] Button quitButton;
        [SerializeField] TextMeshProUGUI recordText;

        [Space]
        [Header("Levels list")]
        [SerializeField] Transform content;


        SignalBus signalBus;
        DataLevelUI dataLevelUI;

        #endregion


        #region Init

        [Inject]
        public void Construct(SignalBus signalBus, DataLevelUI dataLevelUI)
        {
            this.signalBus = signalBus;
            this.dataLevelUI = dataLevelUI;

            InitTopPanel();

            Subscription();

        }


        void Subscription()
        {
            signalBus.Subscribe((SignalMainMenu.LoadGame s) =>
            {
                ClearLevelList();
                UpdateLevelList(s.GameLevels);
                SetRecordText(s.PointRecord.ToString());
            });
        }

        #endregion


        #region Top panel

        void InitTopPanel()
        {
            quitButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalMainMenu.OnClickQuitButton());
            });

            SetRecordText("0");
        }


        void SetRecordText(string record)
        {
            recordText.text = record;
        }

        #endregion


        #region Level list

        void UpdateLevelList(Level[] levels)
        {     
            for(int i = 0; i < levels.Length; i++)
            {
                var newButton = CreateLevel(dataLevelUI.LevelTemplatePrefab);
                var status = levels[i].status;
                var index = i + 1;

                InitLevelButton(ref newButton, status, index);

                //подписываемся на нажатие по кнопке уровня
                newButton.SelectButton.onClick.AddListener(() =>
                { 
                    //если уровень имеет статус открыт, или пройден
                    if (status == Level.LevelStatus.OPEN ||
                        status == Level.LevelStatus.COMPLETED)
                    {
                        signalBus.Fire(new SignalMainMenu.OnClickLevelButton()
                        {
                            LevelIndex = index
                        });
                    }
                });
            }
        }


        //инициализирует кнопку уровня
        void InitLevelButton(ref LevelButton newButton, Level.LevelStatus status, int index)
        {
            Sprite bkg = dataLevelUI.CloseLevelBGR;
            Sprite icon = dataLevelUI.CloseLevelIcon;

            switch (status)
            {
                case Level.LevelStatus.OPEN:
                    bkg = dataLevelUI.OpenLevelBGR;
                    icon = dataLevelUI.OpenLevelIcon;
                    break;
                case Level.LevelStatus.COMPLETED:
                    bkg = dataLevelUI.CompletedLevelBGR;
                    icon = dataLevelUI.CompletedLevelIcon;
                    break;
            }

            //инициализируем кнопку
            newButton.Init(bkg, icon, index);
        }


        //возвращает объект кнопки уровня
        LevelButton CreateLevel(GameObject prefab)
        {
            var go = BuildManager.GetInstance().Spawn(  Pool.PoolType.UI,
                                                        prefab,
                                                        Vector3.zero,
                                                        Quaternion.identity,
                                                        content);

            return go.GetComponent<LevelButton>();
        }


        //очищает контент от кнопок уровней
        void ClearLevelList()
        {
            var bm = BuildManager.GetInstance();

            for (int i = 0; i < content.childCount; i++)
            {
                var go = content.GetChild(i).gameObject;
                bm.Despawn(Pool.PoolType.UI, go);
            }
        }

        #endregion

    }
}