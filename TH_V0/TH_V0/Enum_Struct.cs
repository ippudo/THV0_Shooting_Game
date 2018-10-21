namespace TH_V0
{
    /// <summary>
    /// 子弹朝向方式
    /// </summary>
    public enum directionType
    {
        SPEED = 0,
        PLAYER,
        CONST,
        TARGET
    }

    /// <summary>
    /// 事件所对应的属性
    /// </summary>
    enum Attribute
    {
        ID,
        SPEED,
        SPEEDDIRECT,
        ACC,
        ACCDIRECT,
        DIRECTTYPE,
        HEADANGLE,
        RANDOMRADIUS,
        FREQUENCY,
        POSITION,
        WIDTH,
        SELFSNIPE,
        DEFAULT
    }
    /// <summary>
    /// 事件属性更改方式
    /// </summary>
    enum ChangeType
    {
        CONST,
        RATE,
        SIN,
        COS,
        DEFAULT
    }
    /// <summary>
    /// 事件属性变化方式
    /// </summary>
    enum RateType
    {
        CONST,
        PLUS,
        MINUS,
        DEFAULT
    }
    /// <summary>
    /// 精灵绘画特效
    /// </summary>
    enum DrawEffect
    {
        TWINKLE,
        FAINT,
        CONST
    }

    enum SpriteState
    {
        BORN = 0,
        LIVE,
        DEAD,
        DISAPPEAR
    }

    enum ImageName
    {
        RIMON,
        BULLET,
        LIGHT,
        LASER,
        ENEMY,
        POINT,
        BOW,
        DEAD,
        BOSS2,
        BAR,
        MAHO
    }
    /// <summary>
    /// 子弹信息
    /// </summary>
    struct BulletInfo
    {
        public int ID;//子弹类型
        public float speed;//子弹速度
        public float speedDirec;//子弹发射角
        public float acceleration;//子弹加速度
        public float accelerationDegree;//子弹加速度的方向
        public int life;//子弹生命
        public directionType directType;//
        public int bulletHeadAngle;
        public string bulletEvent;
        public int eventLoopFrame;
        public bool isHighLight;
        public bool isFireInCircle;
        public float fireRadius;//子弹的发射半径
        public bool outScreenDisappear;
        public bool maskAvailable;//是否响应遮罩
        public bool reflexAvailable;//是否响应反射板
        public bool forceAvailable;//是否响应力场
        public BulletInfo(int ID, float speed, float speedDirec, float acceleration, float accelerationDegree, int life, 
            directionType directType, int bulletHeadAngle, string eventStr,int loopFrame,bool isHighLight,float fireRadius,
            int isFireInCircle,bool outScreenDisappear,bool maskAvailable,bool reflexAvailable,bool forceAvailable)
        {
            this.ID = ID;
            this.speed = speed;
            this.speedDirec = speedDirec;
            this.acceleration = acceleration;
            this.accelerationDegree = accelerationDegree;
            this.life = life;
            this.directType = directType;
            this.bulletHeadAngle = bulletHeadAngle;
            this.bulletEvent = eventStr;
            this.eventLoopFrame = loopFrame;
            this.isHighLight = isHighLight;
            this.fireRadius = fireRadius;
            this.isFireInCircle = (isFireInCircle == 1);
            this.outScreenDisappear = outScreenDisappear;
            this.maskAvailable = maskAvailable;
            this.reflexAvailable = reflexAvailable;
            this.forceAvailable = forceAvailable;
        }
        public BulletInfo(BulletInfo b)
        {
            this.ID = b.ID;
            this.speed = b.speed;
            this.speedDirec = b.speedDirec;
            this.acceleration = b.acceleration;
            this.accelerationDegree = b.accelerationDegree;
            this.life = b.life;
            this.directType = b.directType;
            this.bulletHeadAngle = b.bulletHeadAngle;
            this.bulletEvent = b.bulletEvent;
            this.eventLoopFrame = b.eventLoopFrame;
            this.isHighLight = b.isHighLight;
            this.fireRadius = b.fireRadius;
            this.isFireInCircle = b.isFireInCircle;
            this.outScreenDisappear = b.outScreenDisappear;
            this.maskAvailable = b.maskAvailable;
            this.reflexAvailable = b.reflexAvailable;
            this.forceAvailable = b.forceAvailable;
        }
    };
    /// <summary>
    /// 激光信息
    /// </summary>
    struct LaserInfo
    {
        public int ID;
        public float hight;
        public float width;
        public float speed;
        public float speedDirec;
        public float acceleration;
        public bool isHighLight;
        public LaserInfo(int ID,float hight,float width,float speed,float speedDirection,float acceleration,bool isHightLight)
        {
            this.ID = ID;
            this.hight = hight;
            this.width = width;
            this.speed = speed;
            this.speedDirec = speedDirection;
            this.acceleration = acceleration;
            this.isHighLight = isHightLight;
        }
    }
    
}