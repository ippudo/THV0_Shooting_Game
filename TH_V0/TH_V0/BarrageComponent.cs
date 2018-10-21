using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace TH_V0
{
    abstract class BarrageComponent
    {
        public int ID;
        protected LinkedList<Bullet> bindingBullet;
        protected int life;
        protected int startFrame;
        protected int frameCount;//产生到现在的帧数
        protected Vector2 position;
        protected Vector2 relativePosition;
        Sprite bindingSprite;
        bool available;
        protected bool isAbsolutePos;
        protected Sprite target;//目标精灵
        public int bindingID;
        protected bool relativeBinding;
        protected bool isDisappear;//表示发射器的生命到期或者拥有发射器的对象死亡
        protected LinkedList<Bullet> localBullet;//由该发射器发射出去的子弹
        public BarrageComponent(int ID,Vector2 relativePosition, int startFrame, int life,int bindingID)
        {
            this.ID = ID;
            this.bindingID = bindingID;
            this.startFrame = startFrame;
            this.life = life;
            this.position = relativePosition;
            this.relativePosition = relativePosition;
            available = false;
            this.frameCount = 0;
            this.relativeBinding = false;
            this.isAbsolutePos = true;
            this.isDisappear = false;
        }
        public void Update()
        {
            if (frameCount >= startFrame)
            {
                available = true;
            }
            if (frameCount > startFrame + life)
            {
                available = false;
            }
            if (available)
            {
                if (bindingSprite != null)
                {
                    this.position = bindingSprite.getPosition()+relativePosition;
                }
                Function();
            }
            frameCount++;
        }
        /// <summary>
        /// 发射器的功能部分在这里实现
        /// </summary>
        public abstract void Function();

        public void BindSprite(Sprite mySprite)
        {
            this.bindingSprite = mySprite;
        }

        public void removeSprite(Sprite mySprite)
        {
            this.bindingSprite = null;
        }

        public Vector2 getSpritePosition()
        {
            if (bindingSprite != null)
            {
                return bindingSprite.getPosition();
            }
            else
            {
                return new Vector2(0, 0);
            }
        }

        public void setFrameCount(int num)
        {
            this.frameCount = num;
        }
        public void setAvailable(bool isAvailable)
        {
            this.available = isAvailable;
        }

        public bool isDead()
        {
            if (available)
            {
                return false;
            }
            else
                return true;
        }
        public virtual void Initialize(int iStartFrame)
        {
            this.frameCount = iStartFrame;
            if (iStartFrame >= startFrame)
            {
                available = true;
            }
            else
                available = false;
        }
        public LinkedList<Bullet> getLocalBullet()
        {
            return localBullet;
        }

        public void setBindingBullet(LinkedList<Bullet> bindingB)
        {
            this.bindingBullet = bindingB;
        }
        public void setDisappear(bool disappear)
        {
            isDisappear = disappear;
        }
        public virtual void setTarget(Sprite target)
        {
            this.target = target;
        }
    }
}
