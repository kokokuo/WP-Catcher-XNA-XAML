using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CatcherGame.TextureManager;


namespace CatcherGame.GameObjects
{
    /// <summary>
    /// 記錄所有掉落物遊戲元件的參數,基類別
    /// </summary>
    public class DropObjectDataRecord
    {
        public DropObjectDataRecord(DropObjectsKeyEnum dropObjKey,float probability, 
            List<TexturesKeyEnum> texturesKey, float fallSpeed,float waveValue)
        {
            this.DropObjectKey = dropObjKey;
            this.Probability = probability;
            this.TexturesKey = texturesKey;
            this.FallSpeed = fallSpeed;
            this.WaveDisplacement = waveValue;
        }

        /// <summary>
        /// 掉落時左右搖擺的位移
        /// </summary>
        public float WaveDisplacement { get; set; }

        /// <summary>
        /// 出線的機率
        /// </summary>
        public DropObjectsKeyEnum DropObjectKey { get; set; }

        /// <summary>
        /// 出線的機率
        /// </summary>
        public float Probability { get; set; }
        
        /// <summary>
        /// 載入圖片的Key
        /// </summary>
        public List<TexturesKeyEnum> TexturesKey { get; set; }
        
        /// <summary>
        /// 掉落的速度
        /// </summary>
        public float FallSpeed { get; set; }

       


    }
}
