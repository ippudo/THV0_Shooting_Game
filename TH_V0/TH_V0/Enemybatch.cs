using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    class Enemybatch
    {
        public int ID;
        public int positionX;
        public int positionY;
        public int speed;
        public float speedDirect;
        public int HP;
        public string eventStr;
        public string barrageName;
        ///////////////////////////////////////////////////////////////////
        //方法：_PreTranslate，开发者方法，获得两个分隔符之间的字符串    //
        //输入：储存脚本文件的字符串strFiledata,记录读取位置的iPos       //
        //输出：两个分隔符之间的字符串strTemp,最后读取位置iPos           //
        //返回：读取成功返回true，否则返回false                          //
        ///////////////////////////////////////////////////////////////////
        static private bool _PreTranslate(out string strTemp, ref string strFileData, ref int iPos)
        {
            strTemp = "";
            for (; iPos < strFileData.Length; iPos++)
            {
                if (strFileData[iPos] == ',' ||
                    strFileData[iPos] == '|')
                {
                    iPos++;
                    break;
                }//检测到分隔符","或"|"时中断
                strTemp += strFileData[iPos];
            }
            if (iPos == strFileData.Length)
            {
                return false;
            }//读取完毕，返回false
            else
            {
                return true;
            }
        }

        static public List<Enemybatch> ReadScript(string strRelativePath)
        {
            List<Enemybatch> ebTest = new List<Enemybatch>();
            string strFileData;//用于储存文件数据
            int iStrPos = 0;//记录最后读取的字符串的位置
            string strTemp;//文件翻译中间变量

            //获取exe文件绝对路径，不包括文件本身
            string strCurrentPath = System.Environment.CurrentDirectory;
            System.IO.StreamReader objReader =
                new System.IO.StreamReader(strCurrentPath + strRelativePath, System.Text.Encoding.Default);

            //一次性读取弹幕脚本，避免对文件反复操作
            strFileData = objReader.ReadToEnd();
            objReader.Close();
            //发射器数，开始循环读取各发射器参数
            Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
            int enemyNum = Convert.ToInt32(strTemp);
            for (int i = 0; i < enemyNum; i++)
            {
                Enemybatch ebTemp = new Enemybatch();
                Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                ebTemp.ID = Convert.ToInt32(strTemp);

                Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                ebTemp.positionX = Convert.ToInt32(strTemp);

                Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                ebTemp.positionY = Convert.ToInt32(strTemp);

                Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                ebTemp.speed = Convert.ToInt32(strTemp);

                Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                ebTemp.speedDirect = (float)Convert.ToDouble(strTemp);

                Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                ebTemp.HP = Convert.ToInt32(strTemp);

                Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                ebTemp.eventStr = strTemp;

                Enemybatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                ebTemp.barrageName = strTemp;

                ebTest.Add(ebTemp);
            }
            return ebTest;
        }

    }
}
