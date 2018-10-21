using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna;

namespace TH_V0
{
    class BarrageEvent : IComparable<BarrageEvent>
    {
        public int startFrame;
        public Attribute eAttribute;
        public ChangeType eChangeType;
        public string result;
        public int spendTime;
        public RateType rateType;
        public bool isLoop;
        public float originValue;
        public float resultValue;
        public bool isContinuous;
        public bool isRandom;
        public float randomStart;
        public float randomEnd; 

        public BarrageEvent()
            : this(999999, Attribute.DEFAULT, null, ChangeType.DEFAULT, RateType.DEFAULT, 1)
        { }
        public BarrageEvent(int startFrame, Attribute attr, string result, ChangeType type, RateType rate, int time, bool isLoop)
            : this(startFrame, attr, result, type, rate, time)
        {
            this.isLoop = isLoop;
        }
        public BarrageEvent(int startFrame, Attribute attr, string result, ChangeType type, RateType rate, int time)
        {
            this.startFrame = startFrame;
            this.spendTime = time;
            this.eAttribute = attr;
            this.result = result;
            this.eChangeType = type;
            this.rateType = rate;
            this.isLoop = true;
            this.originValue = 0;
            this.isContinuous = true;
            this.randomEnd = 0;
            this.randomStart = 0;
            this.isRandom = false;
        }

        public BarrageEvent(BarrageEvent copy)
        {
            this.startFrame = copy.startFrame;
            this.spendTime = copy.spendTime;
            this.eAttribute = copy.eAttribute;
            this.eChangeType = copy.eChangeType;
            this.rateType = copy.rateType;
            this.result = copy.result;
            this.isLoop = copy.isLoop;
            this.originValue = copy.originValue;
            this.isContinuous = copy.isContinuous;
            this.randomEnd = copy.randomEnd;
            this.randomStart = copy.randomStart;
            this.isRandom = copy.isRandom;
        }

        public void setLoop(bool isLoop)
        {
            this.isLoop = isLoop;
        }

        public void setOriginValue(float inputValue)
        {
            this.originValue = inputValue;
            setChangeValue();
        }

        protected void setChangeValue()
        {
            switch (this.rateType)
            {
                case RateType.CONST:
                    resultValue = (float)Convert.ToDouble(this.result);
                    break;
                case RateType.MINUS:
                    resultValue = originValue - (float)Convert.ToDouble(this.result);
                    break;
                case RateType.PLUS:
                    resultValue = originValue + (float)Convert.ToDouble(this.result);
                    break;
                case RateType.DEFAULT:
                    resultValue = (float)Convert.ToDouble(this.result);
                    break;
            }
        }

        public float Update(float inputAttribute, int currentTime)
        {
            switch (this.eChangeType)
            {
                case ChangeType.CONST:
                    inputAttribute = resultValue;
                    break;
                case ChangeType.RATE:
                    inputAttribute = originValue + (resultValue - originValue) * (currentTime - this.startFrame) / this.spendTime;
                    break;
                case ChangeType.SIN:
                    inputAttribute = originValue + (resultValue - originValue) *
                        (float)Math.Sin(MathHelper.Pi * (currentTime - this.startFrame) / this.spendTime);
                    break;
                case ChangeType.COS:
                    inputAttribute = originValue + (resultValue - originValue) *
                        (float)Math.Cos(MathHelper.Pi * (currentTime - this.startFrame) / this.spendTime);
                    break;
                case ChangeType.DEFAULT:
                    inputAttribute = resultValue;
                    break;
            }
            return inputAttribute;
        }
        public int CompareTo(BarrageEvent x)
        {
            return this.startFrame - x.startFrame;
        }
    }
}
