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
    /// MenuItem，用于Menu的菜单项，是其他扩展的基类，拥有菜单最基本的功能
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// MenuItem用于绘画的Texture2D实例
        /// </summary>
        public Texture2D basicPattern;
        /// <summary>
        /// 用于记录不同方向的周围的菜单项，方括号内为代表方向的整型数
        /// </summary>
        public MenuItem[] neighbourItem;
        /// <summary>
        /// MenuItem的位置。在Menu中为Menu左上角坐标的相对位置。
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// MenuItem的大小。可以和鼠标识别联系。
        /// </summary>
        public Vector2 Size;
        /// <summary>
        /// 画图的叠底颜色，默认白色
        /// </summary>
        public Color DrawColor=Color.White;
        /// <summary>
        /// 该MenuItem的状态
        /// </summary>
        public MenuItemState State;
        /// <summary>
        /// 该MenuItem的文字介绍，可根据自己喜好填写，可用于显示详细信息。
        /// </summary>
        public String intro;
        /// <summary>
        /// 是否可用。可用的话可以和用户互动，不可用则反之。
        /// </summary>
        public bool IsAvaliable = true;
        /// <summary>
        /// 是否被选中。应当和Menu结合更紧密。考虑和其他选项整合。
        /// </summary>
        public bool IsSelected = true;

        public virtual MenuItem OnArrowKey(int arrowto)
        {
            if (neighbourItem[arrowto] != null&&neighbourItem[arrowto].IsAvaliable)
            {
                return (neighbourItem[arrowto]);
            }
            else return this;
        }


        /// <summary>
        /// 生成一个MenuItem实例
        /// </summary>
        /// <param name="patternto">图像需要的基本Texture2D实例</param>
        /// <param name="eventto">所对应的事件</param>
        /// <param name="positionto">MenuItem的位置。在Menu中为Menu左上角坐标的相对位置。</param>
        /// <param name="Sizeto">MenuItem的大小。</param>
        /// <param name="IntroDuctto">该MenuItem的文字介绍，可根据自己喜好填写，可用于显示详细信息。</param>
        public MenuItem(Texture2D patternto, EventHandler eventto, Vector2 positionto, Vector2 Sizeto, string IntroDuctto)
        {
            basicPattern = patternto;
            Position = positionto;
            Size = Sizeto;
            IsAvaliable = true;
            IsSelected = false;
            State = MenuItemState.Normal;
            neighbourItem = new MenuItem[5];
            //下面这条应该移动到更合适的地方
            DefaultOperateEvent = new EventHandler(nullEvent);

            if (eventto != null) OperateEvent = new EventHandler(eventto);
            else OperateEvent = new EventHandler(DefaultOperateEvent);
            intro = IntroDuctto;
        }

        //Event Here
        /// <summary>
        /// 所对应的事件
        /// </summary>
        public event EventHandler OperateEvent;
        /// <summary>
        /// 默认所对应的事件，如果构造函数事件为空由本事件代替
        /// </summary>
        public static event EventHandler DefaultOperateEvent;

        /// <summary>
        /// 一个空事件
        /// </summary>
        /// <param name="sender">事件的发送者</param>
        /// <param name="e">没用</param>
        void nullEvent(object sender,EventArgs e)
        { }

        /// <summary>
        /// 执行对应的事件
        /// </summary>
        public void Operate()
        {
            State = MenuItemState.Pressed;
            if (OperateEvent!=null) OperateEvent(this, null);
        }

        /// <summary>
        /// 根据状态在指定画布上画出自己
        /// </summary>
        /// <param name="sbto">指定画布</param>
        /// <param name="posiOffset">相对位置坐标</param>
        /// <param name="timeto">时间</param>
        public virtual void Draw(SpriteBatch sbto,Vector2 posiOffset,GameTime timeto)
        {
            if (basicPattern!=null) sbto.Draw(basicPattern, Position+posiOffset, Color.White);
        }

        /// <summary>
        /// 刷新自己
        /// </summary>
        /// <param name="gameTime">游戏现在的时间</param>
        public virtual void Update(GameTime gameTime)
        {
            if (IsSelected)
            {
                this.State = MenuItemState.Selected;
            }
            else
            {
                this.State = MenuItemState.Normal;
            }
        }

        /// <summary>
        /// 获取判断边界的矩形
        /// </summary>
        /// <returns>记录判断边界的矩形，Menu中相对Menu左上角</returns>
        public Rectangle GetHandanRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }
}
