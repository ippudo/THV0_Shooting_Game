using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
namespace TH_V0
{
    abstract class Sprite
    {
        public static int[][] bulletData;//储存BULLET编码数据
        public static int bulletDataSize;//储存BULLET数据的条数
        static ImageHelper myImageHelper;//我的图片读取帮手
        static StreamReader myStreamReader;//读取子弹文件流
        static string readedData;
        protected static Point lightSize = new Point(100,100);//光晕图片的大小
        protected Texture2D textureImage;//游戏贴图
        protected Point frameSize;//贴图中每一帧的大小
        Point sheetSize;//循环贴图的大小
        protected bool isOneFrame;//是否只有一帧
        protected  Point currentFrame;
        protected Point framePosition;
        protected Vector2 position;
        protected Vector2 spriteOrigin;//精灵的原点
        int hitRadius;
        protected int spriteFrameNumber;//当前对象产生到现在的帧数
        protected int life//精灵的生命
        {
            set;
            get;
        }
        protected SpriteState myState;//子弹的状态
        protected bool disappear;//子弹是否已经完全消失或者离开屏幕
        const int deadSpendTime = 20;//消失需要用的帧数，从消失开始子弹不进行判定
        static Point defaultFramePosition = new Point(0,0);//默认图片中起始帧的位置
        static Point defaultSheetSize = new Point (1,1);
        static Point defaultShoujoSheet = new Point(8, 3);//少女默认sheetSize
        protected static int laserBornTime = 45;
        static int maxFrameNumber = 10000000;//防止帧数过大
        public float speed;//精灵移动的速度
        public Circle hitCircle//判定圆
        {
            get
            {
                return new Circle(position, hitRadius);//子弹的命中点同时也是旋转点
            }
        }
        public float rotateDegree//精灵旋转的弧度
        {
            get;
            set;
        }

        public abstract Vector2 direction//移动的方向
        {
            get;
        }
        protected int animeFrameNumber
        {
            get;
            set;
        }
        protected int picturePerFrame
        {
            get;
            set;
        }

        /// <summary>
        /// 表示绘图所采用的特效
        /// </summary>
        protected DrawEffect drawEffect
        {
            set;
            get;
        }

        //激光用构造函数
        public Sprite(ImageName imageName, Point frameSize, Point framePosition,Vector2 position,float speed)
            : this(imageName, frameSize, framePosition, position, new Vector2(Laser.laserFrameSize.X/2, 0),0,speed)
        {
            this.isOneFrame = true;
        }
        //子弹用构造函数
        public Sprite(ImageName imageName, Point frameSize, Point framePosition, Vector2 position,Vector2 spriteOrigin,int hitRadius, float speed):
            this(imageName, frameSize, defaultSheetSize, framePosition, position, spriteOrigin, hitRadius, speed,10)
        { this.isOneFrame = true; }

        //少女专用构造函数
        public Sprite(ImageName imageName, Point frameSize,Point sheetSize,Vector2 spriteOrigin,int hitRadius ,float speed):
            this(imageName, frameSize,sheetSize ,defaultFramePosition,new Vector2 (180,560), spriteOrigin, hitRadius, speed,5)
        { this.isOneFrame = false; }

        public  Sprite(ImageName imageName,Point frameSize,Point sheetSize,Point framePosition,Vector2 position,Vector2 spriteOrigin,
            int hitRadius,float speed,int picturePerFrame)
        {
            this.textureImage = ImageHelper.getImage(imageName);
            this.frameSize = frameSize;
            this.sheetSize = sheetSize;
            this.framePosition = framePosition;
            this.position = position;
            this.spriteOrigin = spriteOrigin;
            this.hitRadius = hitRadius;
            this.speed = speed;
            this.currentFrame = new Point(0, 0);
            this.animeFrameNumber = 3;//默认移动中以前三张为过渡图片，从第4张开始循环
            this.picturePerFrame = picturePerFrame;
            this.spriteFrameNumber = 0;
            this.life = maxFrameNumber;//默认生命为无限
            this.drawEffect = DrawEffect.CONST;
            this.disappear = false;
        }

        static Sprite()
        {
            //读取子弹信息
            myStreamReader = new StreamReader(@"data\bgset.dat");
            readedData = myStreamReader.ReadLine();
            bulletDataSize = Convert.ToInt32(readedData);
            bulletData = new int[1001][];
            for (int i = 0; i < 1001; i++)
            {
                bulletData[i] = new int[7];
            }
            int k = 0;
            string[] bulletString = new string[9];
            while ((readedData = myStreamReader.ReadLine()) != null)
            {
                bulletString = readedData.Split("_".ToCharArray());

                k = Convert.ToInt32(bulletString[0]);
                for (int i = 1; i < 8; i++)
                {
                    bulletData[k][i - 1] = Convert.ToInt32(bulletString[i]);
                }
            }
            myStreamReader.Close();
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
                spriteFrameNumber++;
                if (spriteFrameNumber > maxFrameNumber)//防止帧数数值过大
                {
                    spriteFrameNumber = 0;
                }
                if (spriteFrameNumber % picturePerFrame == 0)//每i帧一个动画画面
                {
                    if (!isOneFrame)
                        currentFrame.X++;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = animeFrameNumber;
                    }
                }
        }

        public abstract void Draw(GameTime gametime, SpriteBatch spriteBatch);

        public void setID(int ID)
        {
            this.currentFrame.X = 0;
            this.frameSize = new Point(Sprite.bulletData[ID][2], Sprite.bulletData[ID][3]);
            this.framePosition = new Point(Sprite.bulletData[ID][0], Sprite.bulletData[ID][1]);
            this.spriteOrigin = new Vector2(Sprite.bulletData[ID][4], Sprite.bulletData[ID][5]);
            this.hitRadius = Sprite.bulletData[ID][6];
        }


        public virtual bool isOutOfBounds(Rectangle clientRect)
        {
            if (position.X < 0 + clientRect.X || position.X > clientRect.Width + clientRect.X || position.Y < 0 + clientRect.Y || position.Y > clientRect.Height + clientRect.Y)
            {
                return true;
            }
            else
                return false;
        }

        public bool isDead()
        {
            return spriteFrameNumber > life;
        }

      
        public Vector2 getPosition()
        {
            return this.position;
        }

        public static void setImageHelper(ImageHelper ImageHelper)
        {
            myImageHelper = ImageHelper;
        }
        public void setSpriteState(SpriteState state)
        {
            myState = state;
        }
        public SpriteState getSpriteState()
        {
            return myState;
        }

        public int getSpriteFrameNumber()
        {
            return spriteFrameNumber;
        }

        public virtual bool isIntersect(Circle other)
        {
            return hitCircle.IsInterSect(other);
        }
    }
}
