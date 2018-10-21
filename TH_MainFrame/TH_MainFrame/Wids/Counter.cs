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
    class LongCounterDisplay
    {
        public LongCounterDisplay(Vector2 positionto, CounterDisplayStyle cdsto, long numto)
        {
            position = positionto;
            counterDisplaySetting = cdsto;
            toShowNum = numto;
        }
        CounterDisplayStyle counterDisplaySetting;
        SpriteFont testSpriteFont;
        //float speed;
        protected Vector2 position;
        long toShowNum;
        public void Update(double millito)
        {
            counterDisplaySetting.Update(millito);
            //toShowNum++;
        }
        public void SetNumber(long numto)
        {
            toShowNum = numto;
        }
        public void Draw(GameTime gametimeto, SpriteBatch sbto)
        {
            counterDisplaySetting.Draw(toShowNum, position, gametimeto, sbto);
        }
        public void Draw(Vector2 positionOffsetTo, GameTime gametimeto, SpriteBatch sbto)
        {
            counterDisplaySetting.Draw(toShowNum, position+positionOffsetTo, gametimeto, sbto);
        }
    }

    public interface CounterDisplayStyle
    {
        void Draw(object toShowNum, Vector2 position, GameTime gametimeto, SpriteBatch sbto);
        void Update(double millito);
        bool IsTypeFit(Type typeto);
    }

    public class NumberCounterDisplayStyle:CounterDisplayStyle
    {
        //public static CounterDisplaySetting defaultSetting = new CounterDisplaySetting(Game.Content.Load<Texture2D>(@"images/suuji_default"), new Vector2(20, 25));
        public bool IsTypeFit(Type typeto)
        {
            if (typeto.Equals(longNum.GetType())) return true;
            else return false;
        }
        
        public long longNum;
        public int collapse;
        public Vector2 singlenum;
        public int maxcolumn = 10;
        public long mininumAppear;
        public long maxinumAppear;
        public Texture2D texture;
        public int narebekata;//Left0,Center1,Right2
        public Rectangle singleRect
        {
            get { return new Rectangle(0, 0, (int)singlenum.X, (int)singlenum.Y); }
        }
        public NumberCounterDisplayStyle(Texture2D textureto, Vector2 singlenumto)
        {
            maxcolumn = 10;
            mininumAppear = 0;
            maxinumAppear = 10000000;
            narebekata = 2;
            texture = textureto;
            singlenum = singlenumto;
        }
        protected void DrawNumber(Vector2 positionto, int numto, SpriteBatch sbto)
        {
            sbto.Draw(texture, positionto,
                new Rectangle((int)singlenum.X * (numto), 0, (int)singlenum.X, (int)singlenum.Y),
                Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.8f);
        }


        public void Update(double millito)
        {
            //toShowNum++;
        }

        long tempToShowNum;
        int tempSingleNum;
        Vector2 nowposition;
        public void Draw(object toShowNum, Vector2 position, GameTime gametimeto, SpriteBatch sbto)
        {
            nowposition = position;
            if ((long)toShowNum > 0)
            {
                tempToShowNum = (long)toShowNum;
                if (tempToShowNum >= mininumAppear) while (tempToShowNum > 0)
                    {
                        tempSingleNum = (int)(tempToShowNum % 10);
                        tempToShowNum = tempToShowNum / 10;
                        DrawNumber(nowposition, tempSingleNum, sbto);
                        nowposition.X -= collapse;
                        nowposition.X -= singlenum.X;
                    }
            }
            else
            {
                if (0 >= mininumAppear) DrawNumber(nowposition, 0, sbto);
            }
        }
    }

    public class StarLevelCounterDisplayStyle : CounterDisplayStyle
    {
        //public static CounterDisplaySetting defaultSetting = new CounterDisplaySetting(Game.Content.Load<Texture2D>(@"images/suuji_default"), new Vector2(20, 25));
        public bool IsTypeFit(Type typeto)
        {
            if (typeto.Equals(longNum.GetType())) return true;
            else return false;
        }

        public long longNum;
        public int collapse;
        public Vector2 singlenum;
        public int maxcolumn = 10;
        public long mininumAppear;
        public long maxinumAppear;
        public Texture2D texture;
        public int narebekata;//Left0,Center1,Right2
        public Rectangle halfNumRect;
        public StarLevelCounterDisplayStyle(Texture2D textureto)
        {
            maxcolumn = 10;
            mininumAppear = 0;
            maxinumAppear = 5;
            narebekata = 2;
            collapse = -6;
            texture = textureto;
            singlenum = new Vector2(texture.Width, texture.Height);
            halfNumRect = new Rectangle(0,0,texture.Width/2, texture.Height);
        }

        public void Update(double millito)
        {
            //toShowNum++;
        }

        long tempToShowNum;
        int tempSingleNum;
        Vector2 nowposition;
        public void Draw(object toShowNum, Vector2 position, GameTime gametimeto, SpriteBatch sbto)
        {
            if ((long)toShowNum > maxinumAppear) toShowNum = (object)maxinumAppear;
            nowposition = position;
            if ((long)toShowNum > 0)
            {
                tempToShowNum = (long)toShowNum;
                if (tempToShowNum >= mininumAppear) while (tempToShowNum >= 1)
                {
                    tempToShowNum-=1;
                    sbto.Draw(texture, nowposition, Color.White);
                    nowposition.X -= collapse;
                    nowposition.X -= singlenum.X;
                }
                if (tempToShowNum >=0.5)
                {
                    sbto.Draw(texture,nowposition,halfNumRect,Color.White);
                }
            }
            else
            {
            }
        }
    }

    public delegate long longCounterDelegate();
    public class CounterChangeEventArgs
    {
        public CounterChangeEventArgs(Object displayto)
        {
            toDisplayNum = displayto;
        }
        Object toDisplayNum;
        public Object ToDisplayNum
        { get { return toDisplayNum; } }
    }
}