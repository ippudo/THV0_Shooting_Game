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

namespace THPages
{
    interface IPage
    {
        /// <summary>
        /// 初始化分页所需组件。
        /// </summary>
        void InitPage();

        /// <summary>
        /// 重置分页状态到初始状态。
        /// </summary>
        void Reset();

        /// <summary>
        /// 重新载入分页所需要的资源。这个里面的东西应该比reset占用的资源多。
        /// </summary>
        void ReloadPage();

        /// <summary>
        /// 分页暂停响应(冻结)。
        /// </summary>
        void PausePage();

        /// <summary>
        /// 分页恢复响应。
        /// </summary>
        void ContinuePage();

        /// <summary>
        /// 卸载分页所需要的资源。
        /// </summary>
        void UnloadPage();

        /// <summary>
        /// 获取该分页自用游戏组件列表。
        /// </summary>
        /// <returns>一个列表。里面是该分页自用游戏组件列表。</returns>
        List<GameComponent> GetGeneratedComponents();

        /// <summary>
        /// 认识上属pagemanager 其实这个本来可以用singleton的pageManager解决
        /// </summary>
        /// <param name="pgto">要认识的上属pagemanager</param>
        //void knowPageManager(PageManager pgto);

        /// <summary>
        /// 获取该分页昵称
        /// </summary>
        /// <returns>分页昵称</returns>
        string getName();

        /// <summary>
        /// 获取该分页代号(英文)
        /// </summary>
        /// <returns>分页代号</returns>
        string getSID();
    }
}
