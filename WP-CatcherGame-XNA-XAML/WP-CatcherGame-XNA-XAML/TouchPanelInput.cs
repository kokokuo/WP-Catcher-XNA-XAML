using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
namespace CatcherGame
{
    public delegate void TouchGestureEventHandler(TouchLocation data);
    public delegate void NoTouchGestureEventHandler();

    /// <summary>
    ///  把手機的輸入方式包裝成類別,並用事件處理
    /// </summary>
    public class TouchPanelInput
    {
        private TouchLocation preTouchLocation; //記錄前一個點擊的位置
        public event TouchGestureEventHandler TouchEvent;
        public bool IsRunningInput;

        public TouchPanelInput()
        {
            //可使用的手勢類型設定
            IsRunningInput = true;
            //設定水平橫向時的座標
            TouchPanel.DisplayOrientation = Microsoft.Xna.Framework.DisplayOrientation.LandscapeLeft;
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.None | GestureType.Hold;
        }
        public void ScanInput()
        {
            while (IsRunningInput)
            {
                TouchCollection tc = TouchPanel.GetState();

                foreach (TouchLocation currentTouch in tc)	//取得所有的觸碰點
                {
                    //前一個碰觸點的狀態與位置都一樣的話,就不要傳送事件(過濾一樣的資料)
                    //if (preTouchLocation.State == currentTouch.State &&
                     //   preTouchLocation.Position.X == currentTouch.Position.X &&
                    //    preTouchLocation.Position.Y == currentTouch.Position.Y)
                    //{
                    //}
                    //else
                    //{
                        //利用 tl.Position 取得觸碰點座標
                        //if(tc.Count>0)Debug.WriteLine("size=" + tc.Count);
                        //Debug.WriteLine("input :id=" + currentTouch.Id + "state=" + currentTouch.State + "," + currentTouch.Position.X + "," + currentTouch.Position.Y);
                        if (TouchEvent != null) TouchEvent(currentTouch);
                    //}
                    //記錄前一筆資料,用來和下次做比較
                    preTouchLocation = currentTouch;
                }
            }
        }
    }
}
