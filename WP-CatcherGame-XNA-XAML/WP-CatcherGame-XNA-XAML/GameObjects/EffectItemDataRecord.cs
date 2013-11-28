using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatcherGame.TextureManager;
namespace CatcherGame.GameObjects
{
    /// <summary>
    /// 紀錄效果道具的資料,用以產生道具類別時可以填入資訊
    /// </summary>
    public class EffectItemDataRecord : DropObjectDataRecord
    {
        
         public EffectItemDataRecord(DropObjectsKeyEnum dropObjKey,float probability, 
            List<TexturesKeyEnum> texturesKey, float fallSpeed,float waveValue,float effectTimer)
            : base(dropObjKey, probability, texturesKey, fallSpeed, waveValue)
        {
            EffectTimer = effectTimer;
        }
        /// <summary>
        /// 被接到後行走的方向 (左邊是0,右邊是1)
        /// </summary>
        public float EffectTimer { get; set; }

    }
}
