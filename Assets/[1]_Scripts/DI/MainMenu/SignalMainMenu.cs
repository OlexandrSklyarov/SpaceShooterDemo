
namespace SA.SpaceShooter
{
    public static class SignalMainMenu
    {
        #region Menu manager

        public class LoadGame
        {
            public Level[] GameLevels { get; set; }
            public int PointRecord { get; set; }
        }

        #endregion


        #region UI

        public class OnClickLevelButton
        {
            public int LevelIndex { get; set; }
        }


        public class OnClickQuitButton { }

        #endregion


        #region Audio

        public class PlaySFX_ClickButton { }

        public class PlayMusicMainMenu { }

        #endregion

    }
}