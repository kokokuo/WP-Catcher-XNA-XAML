﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CatcherGame.GameStates.Dialog
{
    public enum DialogStateEnum
    {
        EMPTY = 0,
        STATE_DICTIONARY ,
        STATE_TOPSCORE,
        STATE_HOW_TO_PLAY,
        STATE_PAUSE,
        STATE_SETTING
        //STATE_EXIT_GAME ->好像沒用到 先註解掉
    }
}
