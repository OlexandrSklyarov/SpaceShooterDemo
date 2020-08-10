﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.SpaceShooter
{
    public class SignalGame
    {
        #region UI

        public class OnClickPauseButton { }

        public class OnClickRestartButton { }

        public class OnClickContinueGameButton { }

        public class OnClickMainMenuButton { }
        #endregion


        #region Game manager

        public class ChangeGameMode 
        { 
            public GameMode Mode { get; set; }
        }

        #endregion


        #region Player 

        public class AddPoints
        {
            public int PointSum { get; set; }
        }


        public class UpdatePointSum
        {
            public int Sum { get; set; }
        }


        public class ChangePlayerHP
        {
            public int AmountHP { get; set; }
        }


        public class PlayerDestroy { }

        #endregion


        #region Audio

        public class PlaySFX_BigAsteroidDestroy { }

        public class PlaySFX_SmallAsteroidDestroy { }

        public class PlaySFX_ShipDestroy { }

        public class PlaySFX_BulletShoot { }

        public class PlayMusic_Game { }

        public class PlayMusicMainMenu { }

        public class PlayMusicGameMenu { }

        #endregion

    }
}