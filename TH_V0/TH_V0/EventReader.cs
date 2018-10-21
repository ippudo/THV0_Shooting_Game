using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TH_V0
{
    class EventReader
    {
        List<BarrageEvent> myEvent;
        public EventReader()
        { }
        public List<BarrageEvent> ReadEvent(string eventStr)
        {
            myEvent = new List<BarrageEvent>();
            string oneEvent;
            string tempString;
            BarrageEvent newEvent;
            while (eventStr.Length != 0)
            {
                oneEvent = ReadOneEvent(ref eventStr);
                if (oneEvent != null)
                {
                    newEvent = new BarrageEvent();
                    SplitEvent(out tempString, ref oneEvent, '.', '.');
                    newEvent.startFrame = Convert.ToInt32(tempString);
                    int index = oneEvent.IndexOf('+');
                    newEvent.rateType = RateType.CONST;
                    if (index != -1)
                    {
                        if(oneEvent[index+1]=='=')
                        newEvent.rateType = RateType.PLUS;
                    }
                    index = oneEvent.IndexOf('-');
                    if (index != -1)
                    {
                        if(oneEvent[index+1]=='=')
                        newEvent.rateType = RateType.MINUS;
                    }
                    index = oneEvent.IndexOf('!');
                    if (index != -1)
                    {
                        int temp = index;
                        newEvent.isRandom = true;
                        index++;
                        string num = "";
                        while (index < oneEvent.Length && oneEvent[index] != '~')
                        {
                            num += oneEvent[index];
                            index++;
                        }
                        newEvent.randomStart = (float)Convert.ToDouble(num);
                        index++;
                        num = "";
                        while (index < oneEvent.Length)
                        {
                            num += oneEvent[index];
                            index++;
                        }
                        newEvent.randomEnd = (float)Convert.ToDouble(num);
                        oneEvent = oneEvent.Remove(temp);
                    }
                    while (oneEvent.Length != 0)
                    {
                        string attribute = "";
                        string result = "";
                        int ipos = 0;
                        SplitEvent(out tempString, ref oneEvent, '[', ']');
                        for (ipos = 0; ipos < tempString.Length; ipos++)
                        {
                            if (tempString[ipos] == '+' || tempString[ipos] == '-')
                            {
                                ipos += 2;
                                break;
                            }
                            else
                                if (tempString[ipos] == '=')
                                {
                                    ipos += 1;
                                    break;
                                }
                            attribute += tempString[ipos];
                        }
                        result = tempString.Substring(ipos, tempString.Length - ipos);
                        switch (attribute)
                        {
                            case "速度":
                                newEvent.eAttribute = Attribute.SPEED;
                                newEvent.result = result;
                                newEvent.isContinuous = true;
                                break;
                            case "速度角":
                                newEvent.eAttribute = Attribute.SPEEDDIRECT;
                                newEvent.result = result;
                                break;
                            case "加速度":
                                newEvent.eAttribute = Attribute.ACC;
                                newEvent.result = result;
                                newEvent.isContinuous = true;
                                break;
                            case "加速角":
                                newEvent.eAttribute = Attribute.ACCDIRECT;
                                newEvent.result = result;
                                newEvent.isContinuous = true;
                                break;
                            case "方式":
                                newEvent.eChangeType = StringToChangeType(result);
                                break;
                            case "耗时":
                                newEvent.spendTime = Convert.ToInt32(result);
                                break;
                            case "朝向方式"://处理为两个子弹事件
                                newEvent.eAttribute = Attribute.DIRECTTYPE;
                                newEvent.result = result;
                                newEvent.isContinuous = false;
                                break;
                            case "朝向角度":
                                myEvent.Add(newEvent);//将前面朝向方式事件加入列表
                                newEvent = new BarrageEvent(newEvent.startFrame, newEvent.eAttribute, newEvent.result,
                                    newEvent.eChangeType, newEvent.rateType, newEvent.spendTime);
                                newEvent.eAttribute = Attribute.HEADANGLE;
                                newEvent.result = result;
                                newEvent.isContinuous = true;
                                break;
                            case "类型":
                                newEvent.eAttribute = Attribute.ID;
                                newEvent.result = result;
                                newEvent.isContinuous = false;
                                break;
                            case "射角":
                                newEvent.eAttribute = Attribute.SPEEDDIRECT;
                                newEvent.result = result;
                                break;
                            case "频率":
                                newEvent.eAttribute = Attribute.FREQUENCY;
                                newEvent.result = result;
                                break;
                            case "随机半径":
                                newEvent.eAttribute = Attribute.RANDOMRADIUS;
                                newEvent.result = result;
                                break;
                            case "位置":newEvent.eAttribute =Attribute.POSITION;
                                newEvent.result = result;
                                break;
                            case "宽度":
                                newEvent.eAttribute = Attribute.WIDTH;
                                newEvent.result = result;
                                break;
                            case "自机狙":
                                newEvent.eAttribute = Attribute.SELFSNIPE;
                                newEvent.result = result;
                                break;
                            default:
                                newEvent.eAttribute = Attribute.DEFAULT;
                                break;
                        }//end switch
                    }
                    myEvent.Add(newEvent);//添加事件
                }
            }
            myEvent.Sort();//按启动时间排序
            return myEvent;
        }

        public ChangeType StringToChangeType(string str)
        {
            ChangeType eChangeType = ChangeType.DEFAULT;
            switch (str)
            {
                case "固定":
                    eChangeType = ChangeType.CONST;
                    break;
                case "正比":
                    eChangeType = ChangeType.RATE;
                    break;
                case "sin":
                    eChangeType = ChangeType.SIN;
                    break;
                case "cos":
                    eChangeType = ChangeType.COS;
                    break;
                default:
                    eChangeType = ChangeType.DEFAULT;
                    break;
            }
            return eChangeType;
        }

        public string ReadOneEvent(ref string eventStr)
        {
            string oneEvent;
            int istart = 0;
            while (eventStr[istart] == '#')
            { istart++; }
            int ifinish = eventStr.IndexOf('#', istart);
            if (ifinish != -1)
            {
                oneEvent = eventStr.Substring(istart + 1, ifinish - istart - 1);
                eventStr = eventStr.Remove(0, ifinish + 1);
            }
            else
            {
                oneEvent = eventStr.Substring(istart, eventStr.Length - istart);
                eventStr = eventStr.Remove(0);
            }
            return oneEvent;
        }

        /// <summary>
        /// 分割单个属性
        /// </summary>
        /// <param name="strTemp">
        /// 分割出来的临时字符串
        /// </param>
        /// <param name="oneEvent">
        /// 输入的待分割的字符串
        /// </param>
        /// <param name="startMark">
        /// 分割标记
        /// </param>
        public void SplitEvent(out string strTemp, ref string oneEvent, char startMark, char finishMark)
        {
            strTemp = "";
            if (oneEvent.Length != 0)
            {
                int istart = oneEvent.IndexOf(startMark);
                int ifinish = oneEvent.IndexOf(finishMark, istart + 1);
                strTemp = oneEvent.Substring(istart + 1, ifinish - istart - 1);
                if (startMark == '.')
                {
                    ifinish = oneEvent.IndexOf('：');
                    oneEvent = oneEvent.Remove(0, ifinish + 1);
                }
                else
                    oneEvent = oneEvent.Remove(istart, ifinish - istart + 1);
            }
        }
    }
}
