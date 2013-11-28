using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CatcherGame.Sprite
{
    /// <summary>
    /// 動畫物件,可以用來製作動畫效果
    /// </summary>
    public class AnimationSprite : IDisposable
    {
        private List<Texture2D> _texture2DList;
        private Vector2 _position;
        private int _frameCount;
        private int _frameIndex;
        private float _totaleEapsed;
        private float _defaultElapsedTime;
        private int _delayFrameIndex;
        private float _delayFrameTimes;
        private bool isRound; //是否已經播放一輪
        private bool _disposed;
        /// <summary>
        /// 取得現在的Frame編號,第XX張
        /// </summary>
        public int GetCurrentFrameNumber{ get{ return _frameIndex;} }

        public float GetCurrentElapsedTime { get { return _defaultElapsedTime; } }

        /// <summary>
        /// 取得Frame的長度
        /// </summary>
        public int GetFrameSize { get { return _frameCount; } }

        /// <summary>
        /// 改變原來預設或設定的區間時間,以毫秒為單位
        /// </summary>
        public float SetAnimationDelayTime { set { _defaultElapsedTime = value; } }

        /// <summary>
        /// 設定要delay的frame
        /// </summary>
        /// <param name="frameNumer">要delay的frame,以0為起始</param>
        /// <param name="delayTimes">以毫秒為準</param>
        public void  SetCertainFrameDelayTime(int frameNumer,float delayTimes){
            _delayFrameTimes = delayTimes;
            _delayFrameIndex = frameNumer;
        }
        /// <summary>
        /// 取得現在播到的圖片
        /// </summary>
        /// <returns></returns>
        public Texture2D GetCurrentFrameTexture() 
        {
            return _texture2DList[_frameIndex];
        }

        /// <summary>
        /// 動畫圖片組是否已經播一輪
        /// </summary>
        /// <returns></returns>
        public bool GetIsRoundAnimation()
        {
            return isRound;
        }

        /// <summary>
        /// 直接設定下一個要撥放的影格索引
        /// </summary>
        /// <param name="frameIndex"></param>
        public void SetNextWantFrameIndex(int frameIndex) {
            this._frameIndex = frameIndex;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="startPos">初始位置(左上角為準)</param>
        /// <param name="defaultElapsed">切換到下一張,中間所間隔的毫秒數,預設是300毫秒,可以不填</param>
        // error 不能使用選擇性參數
        //public AnimationScript(Vector2 startPos, float defaultElapsed = 300F)
        public AnimationSprite(Vector2 startPos, float defaultElapsed)
        {
            _texture2DList = new List<Texture2D>();
            _position = startPos;
            _defaultElapsedTime = defaultElapsed;
            _frameCount = 0;
            _frameIndex = 0;
            isRound = false;
        }

        /// <summary>
        /// 如果未使用Texture2DManager,則使用此方法:加入圖片(動畫由很多張拼成)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param>
        public void AddPicture(ContentManager content,string path) {
            _texture2DList.Add(content.Load<Texture2D>(path));
            _frameCount++;
        }

        /// <summary>
        /// 設定動畫圖片組
        /// </summary>
        /// <param name="texture2DList"></param>
        public void SetTexture2DList(List<Texture2D> texture2DList)
        {
            _texture2DList = texture2DList;
            _frameCount = texture2DList.Count;
        }

        /// <summary>
        /// 設定圖片左上角的座標
        /// </summary>
        /// <param name="currentPos"></param>
        public void SetToLeftPos(float currentX, float currentY){ 
            _position.X = currentX;
            _position.Y = currentY;
        }

        /// <summary>
        /// 更新動畫
        /// </summary>
        /// <param name="gameTime">取得遊戲時間</param>
        public void UpdateFrame(TimeSpan ElapsedGameTime)
        {
            float elapsed = (float)ElapsedGameTime.Milliseconds;
      
            _totaleEapsed += elapsed;

            if (_delayFrameIndex == _frameIndex) //這邊的判斷是有設定要針對的delayFrame
            {
                if (_totaleEapsed > _defaultElapsedTime + _delayFrameTimes)
                {
                    _frameIndex++;

                    // Keep the Frame between 0 and the total frames, minus one.
                    _frameIndex = _frameIndex % _frameCount;
                    if (_frameIndex == 0)
                    {
                        isRound = true;
                    }
                    else
                        isRound = false;
                    _totaleEapsed = _totaleEapsed - (_defaultElapsedTime + _delayFrameTimes);
                }
            }
            else //正常的frame
            {
                if (_totaleEapsed > _defaultElapsedTime)
                {
                    _frameIndex++;

                    // Keep the Frame between 0 and the total frames, minus one.
                    _frameIndex = _frameIndex % _frameCount;
                    if (_frameIndex == 0)
                    {
                        isRound = true;
                    }
                    else
                        isRound = false;
                    _totaleEapsed -= _defaultElapsedTime;
                }
            }
        }
        /// <summary>
        /// 畫出圖片動畫
        /// </summary>
        /// <param name="SpriteBatch">用來繪畫的工具</param>
        public void Draw(SpriteBatch SpriteBatch) {
            SpriteBatch.Draw(_texture2DList[_frameIndex], _position, Color.White);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);     
        }
        private void Dispose(bool disposing) {
            if (!_disposed){

                if (disposing)
                {
                    if (_texture2DList.Count > 0){
                        _texture2DList = null; //因為是指向記憶體位置 ,所以如果Clear會把Text2DManager中的圖片清掉 =>改成null
                    }
                        
                    Console.WriteLine("Animation disposed.");
                }
            }
            _disposed = true;   
        }
    }
}
