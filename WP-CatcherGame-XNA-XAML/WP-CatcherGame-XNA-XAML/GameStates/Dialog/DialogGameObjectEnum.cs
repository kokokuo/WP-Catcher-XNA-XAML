using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace CatcherGame.GameStates.Dialog
{
    public enum DialogGameObjectEnum
    {
        EMPTY = 0,
        //DICTIONARY-----------------------------
        DICTIONARY_FATDANCER,
        DICTIONARY_LITTLEGIRL,
        DICTIONARY_FLYOLDLADY,
        DICTIONARY_MANSTUBBLE,
        DICTIONARY_NAUGHTYBOY,
        DICTIONARY_OLDMAN,
        DICTIONARY_ROXANNE,
        DICTIONARY_NICOLE,

        //HOWTOPLAY-----------------------------
        HOWTOPLAY_PAGE1,
        HOWTOPLAY_PAGE2,
        HOWTOPLAY_PAGE3
    }
}
