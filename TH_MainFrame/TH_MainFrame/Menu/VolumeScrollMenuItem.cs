﻿using System;
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
    /// 一个拥有简单显示功能的MenuItem，可以根据状态切换MenuItem图案，继承自THMenuset.Menu.MenuItem
    /// </summary>
    public class BasicMenuItem:MenuItem
    {
        /// <summary>
        /// 每个按钮图案的显示范围。总共有４个。
        /// </summary>
        Rectangle[] drawSquare;

        /// <summary>
        /// 创建一个Basic类型的MenuItem实例
        /// </summary>
        /// <param name="patternto">所需显示的Texture2D文件，从上往下依次的４张叠在一起</param>
        /// <param name="eventto">MenuItem确认后进入的事件，null的话使用MenuItem的默认事件</param>
        /// <param name="positionto">MenuItem相对Menu的位置，左上角坐标</param>
        /// <param name="IntroDuctto">MenuItem对的简介，可用于显示输出喜好功能</param>
        public BasicMenuItem(Texture2D patternto, EventHandler eventto, Vector2 positionto, string IntroDuctto)
            : base(patternto, eventto, positionto, new Vector2(patternto.Width,patternto.Height/4), IntroDuctto)
        {
            drawSquare = new Rectangle[4];
            drawSquare[0] = new Rectangle(0, 0, patternto.Width, patternto.Height / 4);
            drawSquare[1] = new Rectangle(0, patternto.Height / 4, patternto.Width, patternto.Height / 4);
            drawSquare[2] = new Rectangle(0, patternto.Height / 2, patternto.Width, patternto.Height / 4);
            drawSquare[3] = new Rectangle(0, patternto.Height / 4 * 3, patternto.Width, patternto.Height / 4);
        }

        /// <summary>
        /// 在指定画布画出该实例
        /// </summary>
        /// <param name="sbto">指定画布</param>
        public override void Draw(SpriteBatch sbto,Vector2 posiOffset,GameTime gameTime)
        {
            switch (State)
            {
                case MenuItemState.Normal:
                    sbto.Draw(basicPattern, Position+posiOffset, drawSquare[0], Color.White,
                        0, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
                    break;
                case MenuItemState.Selected:
                    sbto.Draw(basicPattern, Position + posiOffset, drawSquare[1], Color.White,
                        0, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
                    break;
                case MenuItemState.Pressed:
                    sbto.Draw(basicPattern, Position + posiOffset, drawSquare[2], Color.White,
                        0, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
                    break;
            }
        }
    }
}
