using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class Enemy:Sprite
    {
        static int[][] enemyData;//储存敌人数据
        static int enemyDataSize;//储存敌人数据的条数
        static string readedData;
        static int enemyPicturePerFrame = 10;
        static StreamReader myStreamReader;//读取敌人文件流
        protected float speedX;
        protected float speedY;
        protected float speedDirection;//表示敌人的朝向,采用弧度
        public int HP
        {
            set;
            get;
        }
        protected int eventID;
        protected int eventLoopFrame;
        protected List<BarrageEvent> myEvent;
        protected List<BarrageEvent> currentEvent;
        protected EventReader myEventReader;

        public Enemy(ImageName imageName, int ID, Vector2 position, int speed, float speedDirection, int HP, string eventStr)
            : this(imageName, new Point(enemyData[ID][2], enemyData[ID][3]), new Point(enemyData[ID][4], enemyData[ID][5]), new Point(enemyData[ID][0], enemyData[ID][1]),
            position, new Vector2(enemyData[ID][6], enemyData[ID][7]), enemyData[ID][8], speed, speedDirection, HP, eventStr)
        { }
        public Enemy(ImageName imageName, Point frameSize, Point sheetSize,Point framePosition,Vector2 position,Vector2 spriteOrigin, int hitRadius, 
            int speed,float speedDirection,int HP,string eventStr)
            : base(imageName, frameSize, sheetSize,framePosition,position,spriteOrigin, hitRadius, speed,enemyPicturePerFrame)
        {
            rotateDegree = 0;
            this.HP = HP;
            this.speedDirection = speedDirection;
            this.myEventReader = new EventReader();
            if (eventStr.Length != 0)
            {
                myEvent = myEventReader.ReadEvent(eventStr);
            }
            currentEvent = new List<BarrageEvent>();
            this.isOneFrame = false;
            eventID = 0;
            eventLoopFrame = 10000;
            animeFrameNumber = 0;
            myState = SpriteState.BORN;
        }

        static Enemy()
        {
            myStreamReader = new StreamReader(@"data\enset.dat");
            readedData = myStreamReader.ReadLine();
            enemyDataSize = Convert.ToInt32(readedData);
            enemyData = new int[100][];
            for (int i = 0; i < 100; i++)
            {
                enemyData[i] = new int[9];
            }
            int k = 0;
            string[] enemyString = new string[9];
            while ((readedData = myStreamReader.ReadLine()) != null)
            {
                enemyString = readedData.Split("_".ToCharArray());
                for (int i = 0; i < 9; i++)
                {
                    enemyData[k][i] = Convert.ToInt32(enemyString[i]);
                }
                k++;
            }
            myStreamReader.Close();
        }

        public override Vector2 direction
        {
            get {
                speedX = speed * (float)Math.Cos(MathHelper.ToRadians(speedDirection));
                speedY = speed * (float)Math.Sin(MathHelper.ToRadians(speedDirection));
                return new Vector2(speedX, speedY); 
            }
        }
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;
            CheckEvent();
            CheckState(clientBounds);
            base.Update(gameTime, clientBounds);
        }
        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle(framePosition.X + currentFrame.X * frameSize.X,
                           framePosition.Y + currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                           Color.White, rotateDegree, spriteOrigin, 1f, SpriteEffects.None, 0.19f);
        }

        void CheckState(Rectangle clientBounds)
        {
            if (myState == SpriteState.BORN&&!isOutOfBounds(clientBounds))
            {
                myState = SpriteState.LIVE;
            }
            if (myState == SpriteState.LIVE && isOutOfBounds(clientBounds))
            {
                myState = SpriteState.DISAPPEAR;
            }
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        protected void CheckEvent()
        {
            while (eventID < myEvent.Count && spriteFrameNumber % eventLoopFrame >= myEvent[eventID].startFrame)
            {
                if (myEvent[eventID].isLoop || spriteFrameNumber < eventLoopFrame)
                {
                    currentEvent.Add(myEvent[eventID]);
                    eventID++;
                }
            }
            if (spriteFrameNumber % eventLoopFrame == 0)
            {
                eventID = 0;
                currentEvent.RemoveRange(0, currentEvent.Count);
                ExecuteEvent(eventLoopFrame);
            }
            else
            {
                ExecuteEvent(spriteFrameNumber % eventLoopFrame);
            }
        }

        protected void ExecuteEvent(int currentTime)
        {
            for (int i = 0; i < currentEvent.Count; i++)
            {
                BarrageEvent be = currentEvent[i];
                switch (be.eAttribute)
                {
                    case Attribute.SPEED:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(speed);
                        }
                        speed = be.Update(speed, currentTime);
                        break;
                    case Attribute.SPEEDDIRECT:
                        if (be.startFrame == currentTime)
                        {
                            be.setOriginValue(speedDirection);
                        }
                        speedDirection = be.Update(speedDirection, currentTime);
                        break;
                    default: break;
                }
                if (currentTime >= be.spendTime + be.startFrame)
                {
                    currentEvent.RemoveAt(i);
                    i--;
                }
            }//end for
        }
    }
}
