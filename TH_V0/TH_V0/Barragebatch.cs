using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TH_V0
{
    //弹幕基类，凡未特别说明，时间单位均为帧
    public class Barragebatch
    {
        static public bool sbIsAbsolute;//发射器坐标是否为绝对位置
        static public int siCrossX;//十字光标X
        static public int siCrossY;//十字光标Y

        static public int siCycleStart;//循环起始帧
        static public int siFrameNum;//总帧数

        //int iGunNum;//发射器数
        public int iGunID;//发射器ID
        public enum eGT
        {
            DIRECT = 1,//定向发射器
            MULTI = 2,//多向发射器
            RANDOM = 3,//随机发射器
            LASER = 5,//激光发射器
            MASK = 6,//遮罩
            REFLEX = 7,//反射板
            FORCE = 8,//力场
        };
        public bool bIsAbsolute;//发射器坐标是否为绝对位置
        public int iCrossX;//十字光标X
        public int iCrossY;//十字光标Y

        public int iCycleStart;//循环起始帧
        public int iFrameNum;//总帧数
        public eGT eGunType;//发射器类型
        public int iGunPosX;//发射器坐标X
        public int iGunPosY;//发射器坐标Y
        public int iGunPosX2;//发射器坐标X2（可能用不上）
        public int iGunPosY2;//发射器坐标Y2（可能用不上）
        public string strBulletEvent;//子弹事件描述，需要另加分析
        public string strGunEvent;//发射器事件描述，需要另加分析
        public float fFireAngle;//发射角
        public float fLocationRadius;//位置半径
        public float fLaserWidth;//激光宽度
        public float fRadiusAcc;//半径增速
        public enum eBL
        {
            GUNPOINT = 0,//子弹生成与发射器中心
            GUNCIRCLE = 1//子弹生成与以发射器中心为中心的圆
        };//子弹生成位置
        public int eBulletCreatePos;//子弹生成位置
        public bool bShadow;//是否有拖影效果
        public bool bForceCenter;//是否指向力场中心
        public int iCycleTime;//周期（编辑器误作“频率”）
        public int iGunStartTime;//发射器启动时间
        public int iGunEndTime;//发射器停止时间
        public int iBulletLife;//子弹生命时长
        public bool bKillOutBullet;//是否消除离开屏幕的子弹
        public float fRandomRadius;//随机半径
        public float fLaserLength;//激光长度
        public int iBulletHeadAngle;//子弹头朝向角度
        public directionType eBulletToward;//子弹头朝向方式
        public int iBulletAccAngle;//子弹加速度角度
        public float fLaserFireAngle;//激光发射角度
        public float fBulletAcc;//子弹加速度
        public float fBulletSpeed;//子弹速度
        public float fRandomHalfLength;//随机发射器的随机半长
        public float fRandomHalfHeight;//随机发射器的随机半高
        public int iGunAngleRange;//发射器发射的角度范围，用于多向/激光发射器
        public int iFireNumber;//发射器发射子弹的条数，用于多向/激光发射器
        public float fMaskValue;//遮罩更改值，根据遮罩类型可能为int型
        public int iMaskStartTime;//遮罩起始帧
        public int iMaskHalfHeight;//遮罩半高
        public int iMaskHalfLength;//遮罩半长
        public int iMaskLife;//遮罩生命
        public enum eMT
        {
            SPEED = 1,//更改速度
            ACC = 2,//更改加速度
            TRANSPARENT,//更改透明度
            KILLBULLET//子弹消除
        };
        public eMT eMaskType;//遮罩更改的值类型
        public int iMaskBulletType;//遮罩更改的子弹图案类型，当图案不存在时不改变
        public int iReflexAngle;//发射板倾角，必须为45的整数倍
        public int iReflexStartTime;//反射板起始帧
        public int iReflexLife;//反射板生命
        public int iReflexLength;//反射板长
        public bool bReflexOnce;//是否只反射一次
        public int iReflexBulletType;//反射板反射后的子弹图案类型，当图案不存在时不改变
        public int iForceHalfLength;//力场半长
        public int iForceHalfHeight;//力场半高
        public int iForceStartTime;//力场起始帧
        public int iForceLife;//力场生命
        public float fForceStrength;//力场强度，此参数一般在1.0附近
        public int iForceDirection;//力场方向，即子弹受力的角度
        public bool bSelfSnipe;//是否设定子弹为自机狙
        public int iBulletType;//子弹图案类型，不存在则不显示图案
        public enum eRV
        {
            FIREANGLE,//发射器发射角度
            TYPE,//子弹类型
            LIFE,//子弹生命
            SPEED,//子弹速度
            SPEEDANGLE,//子弹速度角度，不同于发射角度，此参数发射后仍改变
            ACC,//子弹加速度
            ACCANGLE,//子弹加速度角度
            HEADANGLE//子弹头朝向角度
        };
        public eRV eRandomValue;//随机发射器的随机参数类型
        public float fRandomRange;//随机发射器的随机参数范围
        public int iBulletType2;//子弹类型2，该参数可能无效
        public bool bReflexChangeType;//是否改变反射子弹类型，脚本中为0/1，这里改为bool
        public bool bHighLight;//是否赋予子弹高光效果
        public int iGunEventCycleTime;//发射器事件循环帧数
        public int iBindingID;//本发射器绑定的目标发射器ID
        public bool bRelativeBinding;//是否为相对绑定
        public int iBulletEventCycleTime;//子弹事件循环帧
        public bool bReflex;//子弹是否响应反射板，对于一次反射板，反射后应设该值为false
        public bool bMask;//子弹是否响应遮罩
        public bool bForce;//子弹是否响应力场

        ///////////////////////////////////////////////////////////////////
        //方法：_PreTranslate，开发者方法，获得两个分隔符之间的字符串    //
        //输入：储存脚本文件的字符串strFiledata,记录读取位置的iPos       //
        //输出：两个分隔符之间的字符串strTemp,最后读取位置iPos           //
        //返回：读取成功返回true，否则返回false                          //
        ///////////////////////////////////////////////////////////////////
        static private bool _PreTranslate(out string strTemp, ref string strFileData, ref int iPos)
        {
            strTemp = "";
            for(; iPos < strFileData.Length; iPos++)
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

        ////////////////////////////////////////////////////////////////////
        //方法：ReadScript，静态方法，读取弹幕脚本                        //
        //输入：弹幕脚本的相对路径strRelativePath（相对于主程序所处目录） //
        //输出：弹幕类Barragebatch的对象数组                              //
        //返回：成功返回true，否则返回false                               //
        ////////////////////////////////////////////////////////////////////
        static public List<Barragebatch> ReadScript(string strRelativePath)
        {
            List<Barragebatch> bgTarget = new List<Barragebatch>();
            try
            {
                string strFileData;//用于储存文件数据
                int iStrPos = 0;//记录最后读取的字符串的位置
                string strTemp;//文件翻译中间变量
                

                System.IO.StreamReader objReader =
                    new System.IO.StreamReader(strRelativePath);

                //一次性读取弹幕脚本，避免对文件反复操作
                strFileData = objReader.ReadToEnd();
                objReader.Close();

                //发射器坐标是否为绝对位置
                Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                sbIsAbsolute = Convert.ToBoolean(strTemp);

                //十字光标X、Y
                Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                siCrossX = Convert.ToInt32(strTemp);
                Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                siCrossY = Convert.ToInt32(strTemp);

                //设计时弹幕测试当前帧，无需记录
                Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                //未知
                Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                //循环起始帧
                Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                siCycleStart = Convert.ToInt32(strTemp);

                //总帧数
                Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                siFrameNum = Convert.ToInt32(strTemp);

                //发射器数，开始循环读取各发射器参数
                Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                int iGunNum = Convert.ToInt32(strTemp);

                for (int i = 0; i < iGunNum; i++)
                {
                    Barragebatch bgTemp = new Barragebatch();//发射器赋值中间变量
                    bgTemp.bIsAbsolute = Barragebatch.sbIsAbsolute;
                    bgTemp.iCrossX = Barragebatch.siCrossX;
                    bgTemp.iCrossY = Barragebatch.siCrossY;
                    bgTemp.iCycleStart = Barragebatch.siCycleStart;
                    bgTemp.iFrameNum = Barragebatch.siFrameNum;
                    //发射器ID
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunID = Convert.ToInt32(strTemp);
                    
                    //发射器类型
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.eGunType = (eGT)Convert.ToInt32(strTemp);

                    //发射器X、Y
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunPosX = Convert.ToInt32(strTemp);
                    bgTemp.iGunPosX -= bgTemp.iCrossX;
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunPosY = Convert.ToInt32(strTemp);
                    bgTemp.iGunPosY -= bgTemp.iCrossY;

                    //发射器X2、Y2
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunPosX2 = Convert.ToInt32(strTemp) + 30;
                    bgTemp.iGunPosX2 -= 263;
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunPosY2 = Convert.ToInt32(strTemp) + 50;

                    //未知*2
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                    //子弹及发射器事件描述
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.strBulletEvent = strTemp;
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.strGunEvent = strTemp;

                    //（发）射角
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fFireAngle = (float)Convert.ToDouble(strTemp);

                    //未知
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                    //位置半径或激光宽度
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    if (bgTemp.eGunType == eGT.LASER)
                    {
                        bgTemp.fLaserWidth = Convert.ToInt32(strTemp);
                    }
                    else
                    {
                        bgTemp.fLocationRadius = (float)Convert.ToDouble(strTemp);
                    }

                    //半径增速
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fRadiusAcc = (float)Convert.ToDouble(strTemp);

                    //未知
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                    //子弹生成位置
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.eBulletCreatePos = (int)Convert.ToInt32(strTemp);

                    //是否有拖影效果或指向力场中心
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    if (bgTemp.eGunType == eGT.FORCE)
                    {
                        bgTemp.bForceCenter = Convert.ToBoolean(strTemp);
                    }
                    else
                    {
                        bgTemp.bShadow = Convert.ToBoolean(strTemp);
                    }

                    //周期
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iCycleTime = Convert.ToInt32(strTemp);

                    //发射器起始、终止时间
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunStartTime = Convert.ToInt32(strTemp);
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunEndTime = Convert.ToInt32(strTemp);

                    //未知
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                    //子弹生命
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iBulletLife = Convert.ToInt32(strTemp);

                    //是否出屏消除
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bKillOutBullet = Convert.ToBoolean(strTemp);

                    //随机发射器随机半径或激光长度
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    if (bgTemp.eGunType == eGT.RANDOM)
                    {
                        bgTemp.fRandomRadius = (float)Convert.ToDouble(strTemp);
                    }
                    else
                    {
                        bgTemp.fLaserLength = (float)Convert.ToDouble(strTemp);
                    }

                    //子弹头朝向角度
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iBulletHeadAngle = (int)Convert.ToDouble(strTemp);

                    //子弹头朝向方式
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.eBulletToward = (directionType)Convert.ToInt32(strTemp);

                    //未知*2
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                    //加角或激光射角
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    if (bgTemp.eGunType == eGT.LASER)
                    {
                        bgTemp.fLaserFireAngle = (float)Convert.ToDouble(strTemp);
                    }
                    else
                    {
                        bgTemp.iBulletAccAngle = Convert.ToInt32(strTemp);
                    }

                    //子弹加速度
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fBulletAcc = (float)Convert.ToDouble(strTemp);

                    //子弹速度
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fBulletSpeed = (float)Convert.ToDouble(strTemp);

                    //未知
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                    //随机发射器的随机半长
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fRandomHalfLength = (float)Convert.ToDouble(strTemp);

                    //随机发射器的随机半高
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fRandomHalfHeight = (float)Convert.ToDouble(strTemp);

                    //发射器发射的角度范围，用于多向/激光发射器
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunAngleRange = Convert.ToInt32(strTemp);

                    //发射器的发射条数，用于多向/激光发射器
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iFireNumber = Convert.ToInt32(strTemp);

                    //未知*25
                    for (int j = 0; j < 25; j++)
                    {
                        Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    }

                    //遮罩更改值，根据类型可能为int
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fMaskValue = (float)Convert.ToDouble(strTemp);

                    //遮罩起始帧
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iMaskStartTime = Convert.ToInt32(strTemp);

                    //遮罩半高
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iMaskHalfHeight = Convert.ToInt32(strTemp);

                    //遮罩半长
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iMaskHalfLength = Convert.ToInt32(strTemp);

                    //遮罩生命
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iMaskLife = Convert.ToInt32(strTemp);

                    //未知
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                    //遮罩类型
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.eMaskType = (eMT)Convert.ToInt32(strTemp);

                    //遮罩更改后的子弹图案编号
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iMaskBulletType = Convert.ToInt32(strTemp);

                    //反射板倾角
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iReflexAngle = Convert.ToInt32(strTemp) * 45;

                    //反射板起始帧
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iReflexStartTime = Convert.ToInt32(strTemp);

                    //反射板生命
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iReflexLife = Convert.ToInt32(strTemp);

                    //未知
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);

                    //反射板长
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iReflexLength = Convert.ToInt32(strTemp);

                    //子弹是否只碰撞一次
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bReflexOnce = Convert.ToBoolean(strTemp);

                    //反射后子弹类型编号
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iReflexBulletType = Convert.ToInt32(strTemp);

                    //力场半长
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iForceHalfLength = Convert.ToInt32(strTemp);

                    //力场半高
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iForceHalfHeight = Convert.ToInt32(strTemp);

                    //力场起始帧
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iForceStartTime = Convert.ToInt32(strTemp);

                    //力场生命
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iForceLife = Convert.ToInt32(strTemp);

                    //力场大小
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fForceStrength = (float)Convert.ToDouble(strTemp);

                    //力场方向
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iForceDirection = Convert.ToInt32(strTemp);

                    //是否为自机狙
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bSelfSnipe = Convert.ToBoolean(strTemp);

                    //子弹类型
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iBulletType = Convert.ToInt32(strTemp);

                    //随机发射器的随机参数类型
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.eRandomValue = (eRV)Convert.ToInt32(strTemp);

                    //随机发射器的随机范围
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.fRandomRange = (float)Convert.ToDouble(strTemp);

                    //子弹类型2，参数可能无用
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iBulletType2 = Convert.ToInt32(strTemp);

                    //是否改变反射后的子弹类型
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bReflexChangeType = Convert.ToBoolean(Convert.ToInt32(strTemp));

                    //是否有高光效果
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bHighLight = Convert.ToBoolean(strTemp);

                    //发射器事件循环帧数
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iGunEventCycleTime = Convert.ToInt32(strTemp);

                    //绑定发射器ID
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iBindingID = Convert.ToInt32(strTemp);

                    //是否为相对绑定
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bRelativeBinding = Convert.ToBoolean(strTemp);

                    //子弹事件循环帧
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.iBulletEventCycleTime = Convert.ToInt32(strTemp);

                    //是否响应反射板
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bReflex = Convert.ToBoolean(strTemp);

                    //是否响应遮罩
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bMask = Convert.ToBoolean(strTemp);

                    //是否响应力场
                    Barragebatch._PreTranslate(out strTemp, ref strFileData, ref iStrPos);
                    bgTemp.bForce = Convert.ToBoolean(strTemp);
                    
                    //将当前发射器数据载入数组
                    bgTarget.Add(bgTemp);

                }//end of for
                return bgTarget;
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return null;
            }
        }
    }
}
