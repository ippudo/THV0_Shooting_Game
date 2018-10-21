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

namespace THMenuset.Menu
{
    /// <summary>
    /// 表示MenuItem的状态的枚举
    /// </summary>
    public enum MenuItemState
    {
        None=0,Normal=1,Selected=2,Pressed=3
    }

    /// <summary>
    /// 一个查询每个方向对应整型的类
    /// </summary>
    public static class Arrow
    {
        public static int None
        { get { return 0; } }
        public static int Left
        { get { return 1; } }
        public static int Up
        { get { return 2; } }
        public static int Right
        { get { return 3; } }
        public static int Down
        { get { return 4; } }
        /// <summary>
        /// 输出给定整型数对应的相反方向的整型数
        /// </summary>
        /// <param name="ito">代表方向的整型数</param>
        /// <returns>反方向对应的整型数</returns>
        public static int reverse(int ito)
        {
            switch (ito)
            {
                case 1: return 3; break;
                case 2: return 4; break;
                case 3: return 1; break;
                case 4: return 2; break;
                default: return 0;
            }
        }
    }
}
