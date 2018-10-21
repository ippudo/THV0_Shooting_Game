using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;


namespace TH_V0
{
    /// <summary>
    /// 这是一个实现 IUpdateable 的游戏组件。
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Thread mediaSynthThread;
        private string songContentPath="";
        private bool mediaPlayerThreadEnable=true;

        private void mediaPlayThread()
        {
            string nowSongContentPath;
            nowSongContentPath = songContentPath;
            while (mediaPlayerThreadEnable)
            {
                if (nowSongContentPath != "")
                {
                    Song song = Game.Content.Load<Song>(nowSongContentPath);
                    MediaPlayer.Play(song);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = bgmVol;
                }
                while (nowSongContentPath == songContentPath && mediaPlayerThreadEnable)
                {
                }
                nowSongContentPath = songContentPath;
            }
        }


        float bgmVol;
        float seVol;
        Song BGM;
        Song boss1Bgm;
        SoundEffect deadSound;
        SoundEffect tanSound;
        SoundEffect shootSound;
        Random myRan;
        int timeSinceLastShoot = 0;

        const int bossSpellNum = 6;
        int bossSpellID = 0;
        bool isBoss;
        bool pause;
        float fps;
        float updateInterval = 1f;
        float timeSinceLastUpdate = 0.0f;
        float framecount = 0;
        int hitTime = 0;
        int scriptID;
        int playerLife;
        int prePlayerLife;
        const int scriptNum = 7;
        SpriteBatch spriteBatch;
        Shoujo player;
        Boss myBoss;
        List<Enemy> enemy;
        List<BarrageManager> enemyBarrage;
        BarrageHelper barrageHelper;
        BarrageManager shoujoBarrageManager;
        List<LinkedList<Bullet>> shoujoBullet;
        List<LinkedList<Bullet>> highLightBullet;
        List<LinkedList<Bullet>> normalBullet;
        List<List<Enemybatch>> gameScriptData;
        List<DeadAnime> deadAnime;
        SpriteFont gameFont;
        string scriptPath = @"/data/game";
        Rectangle gameBounds;
        public SpriteManager(Game game)
            : base(game)
        {
            mediaSynthThread = new Thread(new ThreadStart(mediaPlayThread));
            mediaSynthThread.Name = "SynthStreamerThread";
            mediaSynthThread.IsBackground = true;
            mediaSynthThread.Start();
            // TODO: 在此处构造任何子组件
        }

        /// <summary>
        /// 允许游戏组件在开始运行之前执行其所需的任何初始化。
        /// 游戏组件能够在此时查询任何所需服务和加载内容。
        /// </summary>
        public override void Initialize()
        {
            // TODO: 在此处添加初始化代码
            base.Initialize();
        }
        protected override void LoadContent()
        {
            LoadImage();
            LoadSound();
            isBoss = false;
            bossSpellID = 0;
            highLightBullet = new List<LinkedList<Bullet>>();
            normalBullet = new List<LinkedList<Bullet>>();
            shoujoBullet = new List<LinkedList<Bullet>>();
            gameScriptData = new List<List<Enemybatch>>();
            barrageHelper = new BarrageHelper(@"data/");
            myRan = new Random();
            deadAnime = new List<DeadAnime>();
            gameFont = Game.Content.Load<SpriteFont>("default");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            player = new Shoujo(ImageName.RIMON, new Point(32, 48), new Point(8, 3), new Vector2(16, 24), 2, 4);
            shoujoBarrageManager = new BarrageManager(barrageHelper.getBarrage("player1"),player,null,null,shoujoBullet);
            enemy = new List<Enemy>();
            enemyBarrage = new List<BarrageManager>();
            scriptID = -1;
            playerLife = 3;
            ReadGameScript();
            gameBounds = new Rectangle(30, 50, 384, 512);
            pause = false;



            base.LoadContent();
        }

        public void Continue()
        {
            pause = false;
        }

        public void Pause()
        {
            pause = true;
        }

        public bool IsPaused
        {
            get { return pause; }
            set { pause = value; }
        }

        public void Reset(int lifeto,float bgmVol,float seVol)
        {
            this.bgmVol = bgmVol;
            this.seVol = seVol;
            highLightBullet = new List<LinkedList<Bullet>>();
            normalBullet = new List<LinkedList<Bullet>>();
            shoujoBullet = new List<LinkedList<Bullet>>();
            deadAnime = new List<DeadAnime>();
            player.setDead();
            isBoss = false;
            bossSpellID = 0;
            shoujoBarrageManager = new BarrageManager(barrageHelper.getBarrage("player1"), player, null, null, shoujoBullet);
            enemy = new List<Enemy>();
            enemyBarrage = new List<BarrageManager>();
            scriptID = -1;
            playerLife = lifeto;
            pause = false;
            timeSinceLastShoot = 0;
            Scorecount = 0;
            /*MediaPlayer.Play(BGM);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = bgmVol;*/
            songContentPath = @"";
            songContentPath = @"Sound/BGM";
        }

        public int Lifecount
        {
            get { return playerLife; }
        }
        public int Bombcount = 1;
        public int Scorecount;

        public void ReSet()
        {
            highLightBullet = new List<LinkedList<Bullet>>();
            normalBullet = new List<LinkedList<Bullet>>();
            shoujoBullet = new List<LinkedList<Bullet>>();
            deadAnime = new List<DeadAnime>();
            player.setDead();
            shoujoBarrageManager = new BarrageManager(barrageHelper.getBarrage("player1"), player, null, null, shoujoBullet);
            enemy = new List<Enemy>();
            enemyBarrage = new List<BarrageManager>();
            scriptID = -1;
            playerLife = 3;
            pause = false;
            isBoss = false;
            bossSpellID = 0; 
            Scorecount = 0;
        }


        /// <summary>
        /// 允许游戏组件进行自我更新。
        /// </summary>
        /// <param name="gameTime">提供计时值的快照。</param>
        public override void Update(GameTime gameTime)
        {
            if (!pause)
            {
                
                // TODO: 在此处添加更新代码
                player.Update(gameTime, gameBounds);//更新玩家
                foreach (BarrageManager barrage in enemyBarrage)
                {
                    barrage.Update(gameTime, gameBounds);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                {
                    shoujoBarrageManager.Update(gameTime, gameBounds);
                    timeSinceLastShoot += gameTime.ElapsedGameTime.Milliseconds;
                    if (timeSinceLastShoot > 80)
                    {
                        timeSinceLastShoot = 0;
                        shootSound.Play(0.6f*seVol,0,0);
                    }
                    
                }
                Enemy nearlyEnemy = null;
                float min = 2000;
                foreach (Enemy e in enemy)
                {
                    e.Update(gameTime, gameBounds);
                    Vector2 distance = e.getPosition() - player.getPosition();
                    if (distance.Length() < min)
                    {
                        nearlyEnemy = e;
                        min = distance.Length();
                    }
                }
                for (int i = 0; i < deadAnime.Count; i++)
                {
                    DeadAnime da = deadAnime[i];
                    da.Update(gameTime,gameBounds);
                    if (da.getSpriteState() == SpriteState.DISAPPEAR)
                    {
                        deadAnime.Remove(da);
                        i--;
                    }
                }
                    shoujoBarrageManager.setTarget(nearlyEnemy);
                UpdateBullet(gameTime);
                CheckInterSect();

                if (enemy.Count == 0 && scriptID < scriptNum)
                {
                    scriptID++;
                    if (scriptID < scriptNum)
                        LoadEnemy(scriptID);
                    else
                    {
                        player.setClear();
                        ClearAllBullet();
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //绘制FPS
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            framecount++;
            timeSinceLastUpdate += elapsed;
            if (timeSinceLastUpdate > updateInterval)
            {
                fps = framecount;
                Game.Window.Title = "FPS: " + fps.ToString() + "||BulletCount: " + CountBulletNum().ToString();
                framecount = 0;
                timeSinceLastUpdate -= updateInterval;
            }

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            foreach (Enemy e in enemy)
            {
                e.Draw(gameTime, spriteBatch);
            }
            foreach (DeadAnime da in deadAnime)
            {
                da.Draw(gameTime,spriteBatch);
            }
            DrawBullet(gameTime, spriteBatch, normalBullet);
            DrawBullet(gameTime, spriteBatch, shoujoBullet);
            spriteBatch.DrawString(gameFont, "HIT TIME:" + hitTime.ToString(), Vector2.Zero, Color.White);
            player.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Additive);
            DrawBullet(gameTime, spriteBatch, highLightBullet);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        /// <summary>
        /// 给ImageHelper添加图片
        /// </summary>
        public void LoadImage()
        {
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/rimon"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/barrages"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/light"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/lazer"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/enemy"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/point"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/bow"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/dead"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/boss3"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/red"));
            ImageHelper.AddImage(Game.Content.Load<Texture2D>(@"Image/maho"));
        }

        public void LoadSound()
        {
            deadSound = Game.Content.Load<SoundEffect>(@"Sound/pldead");
            tanSound = Game.Content.Load<SoundEffect>(@"Sound/tan");
            shootSound = Game.Content.Load<SoundEffect>(@"Sound/plst");
            BGM = Game.Content.Load<Song>(@"Sound/BGM");
            boss1Bgm = Game.Content.Load<Song>(@"Sound/boss1");
        }


        void UpdateBulletList(LinkedList<Bullet> bulletList,GameTime gameTime, Rectangle clientBounds)
        {
            LinkedListNode<Bullet> b = bulletList.First;
            while (b != null)
            {
                b.Value.Update(gameTime, clientBounds);
                if (b.Value.isOutOfBounds(clientBounds)&&b.Value.outDisappear)
                {
                    b.Value.setSpriteState(SpriteState.DISAPPEAR);//移除出屏子弹
                }
                if (b.Value.getSpriteState() == SpriteState.DISAPPEAR)
                {
                    LinkedListNode<Bullet> temp = b;
                    b = b.Next;
                    bulletList.Remove(temp);
                }
                else
                {
                    b = b.Next;
                }
            }
        }

        bool CheckInterSectList(LinkedList<Bullet> bulletList,Circle targetCircle)
        {
            LinkedListNode<Bullet> b = bulletList.First;
            while (b != null)
            {
                if ((int)b.Value.getSpriteState()<=1&&b.Value.isIntersect(targetCircle))
                {
                    b.Value.setSpriteState(SpriteState.DEAD);
                    return true;
                }
                b = b.Next;
            }
            return false;
        }

        void DrawBullet(GameTime gameTime, SpriteBatch spriteBatch, List<LinkedList<Bullet>> myBullet)
        {
            foreach (LinkedList<Bullet> bulletList in myBullet)
            {
                LinkedListNode<Bullet> b = bulletList.First;
                while (b != null)
                {
                    b.Value.Draw(gameTime,spriteBatch);
                    b = b.Next;
                }
            }
        }

        void UpdateBullet(GameTime gameTime)
        {
            foreach(LinkedList<Bullet> bulletList in highLightBullet)
            {
                UpdateBulletList(bulletList, gameTime, gameBounds);
            }
            foreach (LinkedList<Bullet> bulletList in normalBullet)
            {
                UpdateBulletList(bulletList, gameTime, gameBounds);
            }
            foreach (LinkedList<Bullet> bulletList in shoujoBullet)
            {
                UpdateBulletList(bulletList, gameTime, gameBounds);
            }
        }

        void ClearAllBullet()
        {
            foreach (LinkedList<Bullet> bulletList in highLightBullet)
            {
                ClearBulletList(bulletList);
            }
            foreach (LinkedList<Bullet> bulletList in normalBullet)
            {
                ClearBulletList(bulletList);
            }
        }

        void ClearBulletList(LinkedList<Bullet> bulletList)
        {
            LinkedListNode<Bullet> b = bulletList.First;
            while (b != null)
            {
                b.Value.setSpriteState(SpriteState.DEAD);
                b = b.Next;
            }
        }

        int CountBulletNum()
        {
            int count = 0;
            foreach (LinkedList<Bullet> bulletList in highLightBullet)
            {
                count += bulletList.Count;
            }
            foreach (LinkedList<Bullet> bulletList in normalBullet)
            {
                count += bulletList.Count;
            }
            return count;
        }


        void removeBarrageManager(Sprite owner)
        {
            BarrageManager bm;
            for(int i = 0;i<enemyBarrage.Count;i++)
            {
                bm = enemyBarrage[i];
                if (bm.isMyOwner(owner))
                {
                    enemyBarrage.Remove(bm);
                    i--;
                }
            }
        }

        void pauseBarrageManager(int pauseTime)
        {
            for (int i = 0; i < enemyBarrage.Count; i++)
            {
                enemyBarrage[i].Pause(pauseTime);
            }
        }


        public void CheckInterSect()
        {
            if (!player.isUnbeatable)
            {
                foreach (LinkedList<Bullet> bulletList in highLightBullet)
                {
                    if (CheckInterSectList(bulletList, player.hitCircle))
                    {
                        DeadAnime da = new DeadAnime(ImageName.DEAD, player.getPosition(), Color.White, myRan);
                        deadAnime.Add(da);
                        player.setDead();
                        ClearAllBullet();
                        pauseBarrageManager(120);
                        playerLife--;
                        deadSound.Play(0.8f * seVol,0,0);
                    }
                }
                foreach (LinkedList<Bullet> bulletList in normalBullet)
                {
                    if (CheckInterSectList(bulletList, player.hitCircle))
                    {
                        DeadAnime da = new DeadAnime(ImageName.DEAD, player.getPosition(), Color.White, myRan);
                        deadAnime.Add(da);
                        player.setDead();
                        ClearAllBullet();
                        pauseBarrageManager(120);
                        playerLife--;
                        deadSound.Play(0.8f * seVol,0,0);
                    }
                }
            }
            Enemy e;
            for (int i = 0; i < enemy.Count;i++ )
            {
                e = enemy[i];
                foreach (LinkedList<Bullet> bulletList in shoujoBullet)
                {
                    if (CheckInterSectList(bulletList, e.hitCircle)&&e.getSpriteState()==SpriteState.LIVE)
                    {
                        e.HP--;
                    }
                }
                if (e.HP <= 0)
                {
                    if (!isBoss)
                    {
                        e.setSpriteState(SpriteState.DISAPPEAR);
                        DeadAnime da = new DeadAnime(ImageName.DEAD, e.getPosition(), Color.White, myRan);
                        tanSound.Play(0.6f*seVol,0,0);
                        deadAnime.Add(da);
                        Scorecount += 200;
                        if (e.getSpriteFrameNumber() < 200)
                        {
                            Scorecount += 200 - e.getSpriteFrameNumber();
                        }
                    }
                    else
                    {
                        ClearAllBullet();
                        bossSpellID++;
                        if (bossSpellID >= bossSpellNum)
                        {
                            Scorecount += 4000;
                            myBoss.setSpriteState(SpriteState.DISAPPEAR);
                            DeadAnime da = new DeadAnime(ImageName.DEAD, e.getPosition(), Color.White, myRan);
                            tanSound.Play(0.8f * seVol,0,0);
                            deadAnime.Add(da);
                            if (prePlayerLife == playerLife)
                            {
                                foreach (BarrageManager bm in enemyBarrage)
                                {
                                    if (bm.isMyOwner(e) && bm.getFrameCount() < 5940)
                                        Scorecount += 5940 - bm.getFrameCount();
                                }
                            }
                        }
                        else
                        {
                            Scorecount += 2000;
                            tanSound.Play(0.8f * seVol, 0, 0);
                            removeBarrageManager(e);
                            if (prePlayerLife == playerLife)
                            {
                                foreach (BarrageManager bm in enemyBarrage)
                                {
                                    if (bm.isMyOwner(e) && bm.getFrameCount() < 5940)
                                        Scorecount += 5940 - bm.getFrameCount();
                                }
                            }
                            LoadBossSpell(bossSpellID);

                        }
                    }
                }
                if (e.getSpriteState() == SpriteState.DISAPPEAR)
                {
                    enemy.Remove(e);
                    removeBarrageManager(e);
                    i--;
                }
            }
            
        }

        void LoadBossSpell(int k)
        {
            prePlayerLife = playerLife;
            List<Enemybatch> bossData = gameScriptData[scriptNum];
            enemyBarrage.Add(new BarrageManager(barrageHelper.getBarrage(bossData[k].barrageName), enemy[0], player, highLightBullet, normalBullet));
            myBoss.nextSpell(bossData[k].HP,bossData[k].eventStr);
            pauseBarrageManager(60);
        }

        void LoadEnemy(int k)
        {
            List<Enemybatch> enemyData = gameScriptData[k];
            if (enemyData[0].ID >= 6)
            {
                isBoss = true;
                /*MediaPlayer.Stop();
                MediaPlayer.Play(boss1Bgm);
                MediaPlayer.Volume = bgmVol;*/
                songContentPath = @"Sound/boss1";

                Enemy boss = new Boss(ImageName.BOSS2, new Vector2(enemyData[0].positionX, enemyData[0].positionY),
                    new Vector2(30, 28), 60, enemyData[0].speed, enemyData[0].speedDirect, enemyData[0].HP, enemyData[0].eventStr);
                enemy.Add(boss);
                myBoss = (Boss)boss;
                LoadBossSpell(bossSpellID);
            }
            else
            {
                for (int i = 0; i < enemyData.Count; i++)
                {
                    Enemy e = new Enemy(ImageName.ENEMY, enemyData[i].ID, new Vector2(enemyData[i].positionX, enemyData[i].positionY),
                        enemyData[i].speed, enemyData[i].speedDirect, enemyData[i].HP, enemyData[i].eventStr);
                    enemyBarrage.Add(new BarrageManager(barrageHelper.getBarrage(enemyData[i].barrageName), e, player, highLightBullet, normalBullet));
                    enemy.Add(e);
                }
            }
        }
        void ReadGameScript()
        {
            for (int i = 1; i <= scriptNum; i++)
            {
                List<Enemybatch> enemyData = Enemybatch.ReadScript(scriptPath + i.ToString() + ".dat");
                gameScriptData.Add(enemyData);
            }
            List<Enemybatch> enemyData1 = Enemybatch.ReadScript(scriptPath + "BossSpell" + ".dat");
            gameScriptData.Add(enemyData1);
        }


        /// <summary>
        /// 是否游戏结束
        /// </summary>
        /// <returns>返回值为0表示游戏没有结束，返回值为1表示少女生命用尽，游戏结束，返回值为2表示通关</returns>
        public int isGameOver()
        {
            if (scriptID == scriptNum&&player.isOutOfBounds(gameBounds))
            {
                //MediaPlayer.Stop();
                return 2;
            }
            if (playerLife <= 0)
            {
                return 1;
            }
            return 0;
        }
    }
}
