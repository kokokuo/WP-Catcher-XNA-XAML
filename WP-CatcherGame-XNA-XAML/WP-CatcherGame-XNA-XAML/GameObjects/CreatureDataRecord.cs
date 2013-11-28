using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CatcherGame.TextureManager;
namespace CatcherGame.GameObjects
{
    /// <summary>
    /// 紀錄生物的資料,用以產生生物類別時可以填入資訊
    /// </summary>
    public class CreatureDataRecord : DropObjectDataRecord
    {
        public CreatureDataRecord(DropObjectsKeyEnum dropObjKey,float probability, 
            List<TexturesKeyEnum> texturesKey, float fallSpeed,float waveValue,float walkSpeed, int walkOrienation)
            : base(dropObjKey, probability, texturesKey, fallSpeed, waveValue)
        {
            WalkOrienation = walkOrienation;
            WalkSpeed = walkSpeed;
        }
        /// <summary>
        /// 被接到後行走的方向 (左邊是0,右邊是1)
        /// </summary>
        public int WalkOrienation { get; set; }

        /// <summary>
        /// 行走的速度
        /// </summary>
        public float WalkSpeed { get; set; }
    }
}
