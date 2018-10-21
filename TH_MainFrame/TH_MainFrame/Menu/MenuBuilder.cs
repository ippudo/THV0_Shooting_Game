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
    /// 菜单项的生成器
    /// </summary>
    public class MenuBuilder
    {
        Game game;
        Menu menu;

        /// <summary>
        /// 创建一个菜单项生成器
        /// </summary>
        /// <param name="gameto">所在游戏</param>
        public MenuBuilder(Game gameto)
        {
            game = gameto;
            NewMenu();
        }

        /// <summary>
        /// 在生成器中生成一个新的menu实例
        /// </summary>
        public void NewMenu()
        {
            menu = new Menu(game);
        }

        /// <summary>
        /// 获取生成器种组装的menu实例，菜单项至少1个否则抛出一个异常
        /// </summary>
        /// <returns>组装的menu实例</returns>
        public Menu getMenu()
        {
            if (menu.MenuItems.Count <= 0)
                throw new Exception("MenuBuilder的目录尚未完成");
            menu.SelectedMenuItem = menu.MenuItems[0];
            menu.MenuItems[0].State = MenuItemState.Selected;
            return menu;
        }

        /// <summary>
        /// 单向连接两个菜单项之间的方向信息
        /// </summary>
        /// <param name="id1">菜单项1的id</param>
        /// <param name="id2">菜单项2的id</param>
        /// <param name="arrowto">方向，为菜单项1指向菜单项2</param>
        public void SingleLinkItem(int id1, int id2, int arrowto)
        {
            menu.MenuItems[id1].neighbourItem[arrowto] = menu.MenuItems[id2];
        }

        /// <summary>
        /// 双向连接两个菜单项之间的方向信息
        /// </summary>
        /// <param name="id1">菜单项1的id</param>
        /// <param name="id2">菜单项2的id</param>
        /// <param name="arrowto">方向，为菜单项1指向菜单项2的方向信息</param>
        public void DoubleLinkItem(int id1, int id2, int arrowto)
        {
            menu.MenuItems[id1].neighbourItem[arrowto] = menu.MenuItems[id2];
            menu.MenuItems[id2].neighbourItem[Arrow.reverse(arrowto)] = menu.MenuItems[id1];
        }

        /// <summary>
        /// 为内部菜单实例添加一个菜单项
        /// </summary>
        /// <param name="itemTo">要添加的菜单项</param>
        /// <returns>菜单项的ID</returns>
        public int AddItem(MenuItem itemTo)
        {
            menu.MenuItems.Add(itemTo);
            return menu.MenuItems.Count - 1;
        }

        /// <summary>
        /// 清除内部菜单实例的菜单项
        /// </summary>
        public void ClearItem()
        {
            menu.MenuItems.Clear();
        }

        /// <summary>
        /// 设置菜单的位置
        /// </summary>
        /// <param name="poto">菜单的位置矢量</param>
        public void SetPosition(Vector2 poto)
        {
            menu.Position = poto;
        }

        /// <summary>
        /// 设置菜单项被选择时的默认的音效
        /// </summary>
        /// <param name="seto">要使用的音效</param>
        public void SetButtonSelectSE(SoundEffect seto)
        {
            menu.ButtonSelectSE = seto;
        }
    }
}
