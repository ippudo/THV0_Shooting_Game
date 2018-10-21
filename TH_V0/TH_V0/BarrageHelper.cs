using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TH_V0
{
    class BarrageHelper
    {
        String[] barrageName;
        List<List<Barragebatch>> myBarrageData;
        public BarrageHelper(string barrageScriptMenu)
        {
            myBarrageData = new List<List<Barragebatch>>();
            StreamReader myReader = new StreamReader(barrageScriptMenu + "barraset.dat");
            string readString = myReader.ReadToEnd();
            barrageName = readString.Split(",".ToCharArray());
            foreach (string bname in barrageName)
            {
                myBarrageData.Add(Barragebatch.ReadScript(barrageScriptMenu + bname + ".mbg"));
            }
            myReader.Close();
        }
        public List<Barragebatch> getBarrage(string name)
        {
            List<Barragebatch> barrage = null;
            try
            {
                for (int i = 0; i < barrageName.Length; i++)
                {
                    if (barrageName[i] == name)
                        return myBarrageData[i];
                }
                Exception e = new Exception("读取的弹幕文件不存在", null);
                throw e;
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return barrage;
            }
        }
    }
}
