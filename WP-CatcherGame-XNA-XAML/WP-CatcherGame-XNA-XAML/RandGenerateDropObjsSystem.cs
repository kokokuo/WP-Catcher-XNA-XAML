using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CatcherGame.GameObjects;
using CatcherGame.TextureManager;
using System.Diagnostics;
using CatcherGame.GameStates;
namespace CatcherGame
{
    public delegate void DropObjsTimeUpEventHandler(List<DropObjects> objs);
    public class RandGenerateDropObjsSystem : IDisposable
    {
        /// <summary>
        /// 產生角色的事件
        /// </summary>
        public event DropObjsTimeUpEventHandler GenerateDropObjs;

        Dictionary<DropObjectsKeyEnum, DropObjectDataRecord> generaterCreatureReocrd;
        Dictionary<DropObjectsKeyEnum, DropObjectDataRecord> generaterEffectItemReocrd; 
        List<TexturesKeyEnum> loadTexureKeys;
        List<DropObjects> generatedCreatures;
        List<DropObjects> generatedEffectItems;
        //道具
        int nextGenerateEffectItemTimes;
        int nextGenerateEffectItemAmount;
        int maxEffectItemsGenerateTime;
        int minEffectItemsGenerateTime;
        int effectItemMaxGenerateAmount;
        float effectItemTotalEapsed;
        
        //生物
        int nextGenerateCreatureTime;
        int nextGenerateCreatureAmount;
        int maxCreatureGenerateTime;
        int minCreatureGenerateTime;
        int creatureMaxGenerateAmount;
        float creatureTotalEapsed;
        
        float leftBorder, rightBorder;
        const int TEXTURE_MAX_WIDTH = 100; //要調整
        const int MAX_HIGH_POS_Y = 70;
        GameState currentState;
        private bool disposed;

        
        /// 初始化參數,載入所有掉落角色的資料
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="generateCreatureTimeMinRange">允許最短可以產生角色的時間(秒)</param>
        /// <param name="generateCreatureTimeMaxRange">允許最久可以產生角色的時間(秒)</param>
        /// <param name="maxCreatureAmount">最大可以產生的生物角色數量</param>
        /// <param name="generateEffectItemsTimeMinRange">允許最短可以產生道具的時間(秒)</param>
        /// <param name="generateEffectItemsTimeMaxRange">允許最久可以產生道具的時間(秒</param>
        /// <param name="maxEffectItemAmount">最大可以產生的道具數量</param>
        public RandGenerateDropObjsSystem(GameState currentState, int generateCreatureTimeMinRange, int generateCreatureTimeMaxRange, int maxCreatureAmount, int generateEffectItemsTimeMinRange, int generateEffectItemsTimeMaxRange,int maxEffectItemAmount)
        {
            this.minCreatureGenerateTime = generateCreatureTimeMinRange;
            this.maxCreatureGenerateTime = generateCreatureTimeMaxRange;
            this.creatureMaxGenerateAmount = maxCreatureAmount;

            this.minEffectItemsGenerateTime = generateEffectItemsTimeMinRange;
            this.maxEffectItemsGenerateTime = generateEffectItemsTimeMaxRange;
            this.effectItemMaxGenerateAmount = maxEffectItemAmount;

            generatedCreatures = new List<DropObjects>();
            generatedEffectItems = new List<DropObjects>();
            LoadDropObjsGenerateRecord();
            this.currentState = currentState;
        }
        /// <summary>
        /// 設定掉落物的X座標邊界
        /// </summary>
        /// <param name="leftScreenBorder"></param>
        /// <param name="rightScreenBorder"></param>
        public void SetBorder(float leftScreenBorder,float rightScreenBorder){
            leftBorder = leftScreenBorder;
            rightBorder = rightScreenBorder;
        }

        private void LoadDropObjsGenerateRecord() {
            generaterCreatureReocrd = new Dictionary<DropObjectsKeyEnum, DropObjectDataRecord>();
            generaterEffectItemReocrd = new Dictionary<DropObjectsKeyEnum, DropObjectDataRecord>();
            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_FLYOLDELADY_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_FLYOLDELADY_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_FLYOLDELADY_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_FLY_OLDLADY, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_FLY_OLDLADY, 0.6f, loadTexureKeys, 3f, 0f, 3f, 1));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_FATDANCE_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_FATDANCE_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_FATDANCE_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_FAT_DANCE, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_FAT_DANCE, 0.5f, loadTexureKeys, 6f, 0f, 3f, 0));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_LITTLEGIRL_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_LITTLEGIRL_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_LITTLEGIRL_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_LITTLE_GIRL, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_LITTLE_GIRL, 0.45f, loadTexureKeys, 5f, 0f, 3f, 1));


            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_MANSTUBBLE_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_MANSTUBBLE_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_MANSTUBBLE_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_MAN_STUBBLE, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_MAN_STUBBLE, 0.6f, loadTexureKeys, 3.5f, 0f, 3f, 0));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_NAUGHTYBOY_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_NAUGHTYBOY_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_NAUGHTYBOY_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_NAUGHTY_BOY, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_NAUGHTY_BOY, 0.65f, loadTexureKeys, 4f, 0f, 3f, 1));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_OLDMAN_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_OLDMAN_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_OLDMAN_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_OLD_MAN, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_OLD_MAN, 0.5f, loadTexureKeys, 2f, 0f, 3f, 1));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_ROXANNE_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_ROXANNE_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_ROXANNE_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_ROXANNE, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_ROXANNE, 0.3f, loadTexureKeys, 7f, 0f, 6f, 1));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_NICOLE_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_NICOLE_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_NICOLE_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_NICOLE, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_NICOLE, 0.5f, loadTexureKeys, 5f, 0f, 3f, 0));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_JASON_FALL);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_JASON_CAUGHT);
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_JASON_WALK);
            generaterCreatureReocrd.Add(DropObjectsKeyEnum.PERSON_JASON, new CreatureDataRecord(DropObjectsKeyEnum.PERSON_JASON, 0.3f, loadTexureKeys, 6f, 0f, 3f, 1));

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //道具 
            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_ITEM_BOOSTING_SHOES);
            generaterEffectItemReocrd.Add(DropObjectsKeyEnum.ITEM_BOOSTING_SHOES, new EffectItemDataRecord(DropObjectsKeyEnum.ITEM_BOOSTING_SHOES, 0.3f, loadTexureKeys, 3f, 0f, 8f));
            
            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_ITEM_SLOW_SHOES);
            generaterEffectItemReocrd.Add(DropObjectsKeyEnum.ITEM_SLOW_SHOES, new EffectItemDataRecord(DropObjectsKeyEnum.ITEM_SLOW_SHOES, 0.3f, loadTexureKeys, 3f, 0f, 8f));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_ITEM_HEART);
            generaterEffectItemReocrd.Add(DropObjectsKeyEnum.ITEM_HEART, new EffectItemDataRecord(DropObjectsKeyEnum.ITEM_HEART, 0.15f, loadTexureKeys, 3f, 0f, 1f));
            //網子
            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_ITEM_NET_EXPANDER);
            generaterEffectItemReocrd.Add(DropObjectsKeyEnum.ITEM_NET_EXPANDER, new EffectItemDataRecord(DropObjectsKeyEnum.ITEM_NET_EXPANDER, 0.2f, loadTexureKeys, 3f, 0f, 8f));

            loadTexureKeys = new List<TexturesKeyEnum>();
            loadTexureKeys.Add(TexturesKeyEnum.PLAY_ITEM_NET_SHRINKER);
            generaterEffectItemReocrd.Add(DropObjectsKeyEnum.ITEM_NET_SHRINKER, new EffectItemDataRecord(DropObjectsKeyEnum.ITEM_NET_SHRINKER, 0.2f, loadTexureKeys, 3f, 0f, 8f));
        }

        //計算掉落物產生的機率值 並且是否有滿足條件設定的機率(產生的機率值小於設定的機率)
        private Dictionary<DropObjectsKeyEnum, float> CalculateDropObjProbability(Dictionary<DropObjectsKeyEnum, DropObjectDataRecord> generateRecord,out float x,out float y)
        {
            Dictionary<DropObjectsKeyEnum, float> probabilities;
            Random fallPositionX = new Random(Guid.NewGuid().GetHashCode());
            Random fallPositionY = new Random(Guid.NewGuid().GetHashCode());
            x = fallPositionX.Next((int)leftBorder, (int)rightBorder - TEXTURE_MAX_WIDTH);
            y = fallPositionY.Next(MAX_HIGH_POS_Y);
            if(y != 0)
                y = -y;
            //產生亂數位置位置
            Vector2 startFallPos = new Vector2(x, y);
            //紀錄有在符合機率的所有Creature
            probabilities = new Dictionary<DropObjectsKeyEnum,float>();
            //計算每個角色遊戲產生出的機率
            foreach (KeyValuePair<DropObjectsKeyEnum, DropObjectDataRecord> record in generateRecord){
                
                    Random generateProbability = new Random(Guid.NewGuid().GetHashCode());
                    float p =  generateProbability.Next(100)/100f;
                    //如果有在設定的機率值以下(表示有擲出)
                    if (p <= record.Value.Probability){
                        //從滿足產生機率的角色中找出最優先可以產生(實際產生的機率 離設定的機率值差距最大)
                        float probabiltyDiff = record.Value.Probability - p;
                        probabilities.Add(record.Key, probabiltyDiff);
                    }
                
            }
            return probabilities;
        }
        //找到所有符合產生的機率值中與原機率差植最大的
        private DropObjectsKeyEnum FindHightestPriorityKey(Dictionary<DropObjectsKeyEnum, float> creaturesProbability) {
            float priorityP = -1;
            DropObjectsKeyEnum priorityDropObjKey = DropObjectsKeyEnum.EMPTY;
            //從滿足產生機率的角色中找出最優先可以產生(實際產生的機率 離設定的機率值差距最大)
            foreach (KeyValuePair<DropObjectsKeyEnum, float> creature in creaturesProbability)
            {
                if (priorityDropObjKey == DropObjectsKeyEnum.EMPTY && priorityP == -1)
                {
                    priorityP = creature.Value;
                    priorityDropObjKey = creature.Key;
                }
                else
                {
                    if (priorityP < creature.Value)
                    {
                        priorityP = creature.Value;
                        priorityDropObjKey = creature.Key;
                    }
                }
            }
            return priorityDropObjKey;
        }

        /// <summary>
        /// 產生生物
        /// </summary>
        /// <returns></returns>
        public List<DropObjects> WorkCreatureRandom(){ 
            Random nextDropTimes = new Random(Guid.NewGuid().GetHashCode());
            Random creatureAmount = new Random(Guid.NewGuid().GetHashCode());
            nextGenerateCreatureTime =  nextDropTimes.Next(minCreatureGenerateTime*1000, maxCreatureGenerateTime*1000);
            nextGenerateCreatureAmount =  creatureAmount.Next(1, creatureMaxGenerateAmount);
            //清除之前殘留指向的位置
            generatedCreatures.Clear();
            

            //紀錄每個Creature實際產生的機率值
            while (generatedCreatures.Count != nextGenerateCreatureAmount) {
                float x,y;
                Dictionary<DropObjectsKeyEnum, float> creaturesProbability = CalculateDropObjProbability(generaterCreatureReocrd,out x, out y);

                DropObjectsKeyEnum priorityDropObjKey = FindHightestPriorityKey(creaturesProbability);
                if (priorityDropObjKey != DropObjectsKeyEnum.EMPTY) //如果有符合的機率產生角色
                {
                    //加入至陣列
                    Creature c = new Creature(this.currentState, priorityDropObjKey, this.currentState.GetObjId(), x, y,
                        generaterCreatureReocrd[priorityDropObjKey].FallSpeed,
                        generaterCreatureReocrd[priorityDropObjKey].WaveDisplacement,
                        ((CreatureDataRecord)generaterCreatureReocrd[priorityDropObjKey]).WalkSpeed,
                        ((CreatureDataRecord)generaterCreatureReocrd[priorityDropObjKey]).WalkOrienation);

                    //增加ID
                    currentState.AddObjId();
                    //載入圖片檔
                    foreach (TexturesKeyEnum keys in generaterCreatureReocrd[priorityDropObjKey].TexturesKey)
                    {
                        c.LoadResource(keys);
                    }
                    generatedCreatures.Add(c);
                }
            }
            return generatedCreatures;
        }

        /// <summary>
        /// 產生道具
        /// </summary>
        /// <returns></returns>
        public List<DropObjects> WorkEffectItemRandom()
        {
            Random nextDropTimes = new Random(Guid.NewGuid().GetHashCode());
            Random effectItemAmount = new Random(Guid.NewGuid().GetHashCode());
            nextGenerateEffectItemTimes = nextDropTimes.Next(minEffectItemsGenerateTime *1000, maxEffectItemsGenerateTime * 1000);
            nextGenerateEffectItemAmount = effectItemAmount.Next(0, creatureMaxGenerateAmount);
            //清除之前殘留指向的位置
            generatedEffectItems.Clear();
            

            //紀錄每個Creature實際產生的機率值
            while (generatedEffectItems.Count != nextGenerateEffectItemAmount)
            {
                float x, y;
                Dictionary<DropObjectsKeyEnum, float> effectItemsProbability = CalculateDropObjProbability(generaterEffectItemReocrd,out x, out y);

                DropObjectsKeyEnum priorityDropObjKey = FindHightestPriorityKey(effectItemsProbability);
                if (priorityDropObjKey != DropObjectsKeyEnum.EMPTY) //如果有符合的機率產生角色
                {
                    //加入至陣列
                    EffectItem item = new EffectItem(this.currentState, priorityDropObjKey, this.currentState.GetObjId(), x, y,
                        generaterEffectItemReocrd[priorityDropObjKey].FallSpeed,
                        generaterEffectItemReocrd[priorityDropObjKey].WaveDisplacement,
                        ((EffectItemDataRecord)generaterEffectItemReocrd[priorityDropObjKey]).EffectTimer);

                    //增加ID
                    currentState.AddObjId();
                    //載入圖片檔
                    foreach (TexturesKeyEnum keys in generaterEffectItemReocrd[priorityDropObjKey].TexturesKey)
                    {
                        item.LoadResource(keys);
                    }
                    generatedEffectItems.Add(item);
                }
            }
            return generatedEffectItems;
        }
        /// <summary>
        /// 更新現在的時間(需要)
        /// </summary>
        /// <param name="gameTimeSpan"></param>
        public void UpdateTime(TimeSpan gameTimeSpan) {
            creatureTotalEapsed += gameTimeSpan.Milliseconds;
            effectItemTotalEapsed += gameTimeSpan.Milliseconds;
            if (creatureTotalEapsed > nextGenerateCreatureTime)
            {
                creatureTotalEapsed -= nextGenerateCreatureTime;
                Debug.WriteLine("Creature Time Up!");
                WorkCreatureRandom(); //隨機產生
                if (GenerateDropObjs != null) {
                    GenerateDropObjs.Invoke(generatedCreatures);
                }
            }
            if (effectItemTotalEapsed > nextGenerateEffectItemTimes) {
                effectItemTotalEapsed -= nextGenerateEffectItemTimes;
                Debug.WriteLine("EffectItem Time Up!");
                WorkEffectItemRandom(); //隨機產生
                if (GenerateDropObjs != null)
                {
                    GenerateDropObjs.Invoke(generatedEffectItems);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);     
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {

                if (disposing)
                {
                    if (generaterCreatureReocrd != null) {
                        generaterCreatureReocrd.Clear();
                    }
                    if (generatedCreatures != null) {
                        generatedCreatures.Clear();
                    }
                    if (generatedEffectItems != null)
                    {
                        generatedEffectItems.Clear();
                    }
                    creatureTotalEapsed = 0;
                    effectItemTotalEapsed = 0;
                    currentState = null;
                }
            }
            disposed = true;
        }
    }
}
