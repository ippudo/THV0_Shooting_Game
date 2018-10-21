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
using System.Threading;

namespace THPages
{
    public static class KeyConfig
    {
        public static Keys[] GameKeys;
        public static Keys[] TestKeys;
        public static Keys[] SystemKeys;
        public static void Init()
        {
            GameKeys = new Keys[10];
            TestKeys = new Keys[10];
            SystemKeys = new Keys[10];
            SetDefaultKey();
        }
        public static void SetDefaultKey()
        {
            GameKeys[0] = Keys.Z;
            GameKeys[1] = Keys.X;
            GameKeys[2] = Keys.LeftShift;
            TestKeys[0] = Keys.D1;
            TestKeys[1] = Keys.D2;
            SystemKeys[0] = Keys.Up;
            SystemKeys[1] = Keys.Right;
            SystemKeys[2] = Keys.Down;
            SystemKeys[3] = Keys.Left;
            SystemKeys[4] = Keys.Z;
            SystemKeys[5] = Keys.Space;
            SystemKeys[6] = Keys.X;
            SystemKeys[7] = Keys.Escape;
        }
    }
    public static class GameConfig
    {
        static int sevol=100,bgmvol=100;
        public static int Sevol { get { return sevol; } }
        public static int Bgmvol { get { return bgmvol; } }
        public static float FSevol { get { return sevol/(float)100; } }
        public static float FBgmvol { get { return bgmvol / (float)100; } }
        public static void setVol(int bgmto, int seto)
        {
            sevol = seto;
            bgmvol = bgmto;
        }
    }

    public class hiscoredata:IComparable<hiscoredata>
    {
        //String sonfirmData;
        //int ID;
        //string chara;
        long score;
        string name;
        DateTime date;
        static public bool operator <(hiscoredata a, hiscoredata b)
        {
            if (a.score < b.score)
            {
                return true;
            }
            if (a.score > b.score)
            {
                return false;
            }
            if (a.date > b.date)
                return false;
            else return true;
        }
        static public bool operator >(hiscoredata a, hiscoredata b)
        {
            if (a.score < b.score)
            {
                return false;
            }
            if (a.score > b.score)
            {
                return true;
            }
            if (a.date > b.date)
                return true;
            else return false;
        }
        public int CompareTo(hiscoredata b)
        {
            if (score < b.score)
            {
                return -1;
            }
            if (score > b.score)
            {
                return 1;
            }
            if (date > b.date)
                return 1;
            else return -1;
        }
        public override string ToString()
        {
            return (name.PadRight(10, ' ') + " " + score.ToString().PadRight(10, ' ') + " " + date.Year + @"/" + date.Month + @"/" + date.Day);
        }
        public hiscoredata(long scoreto, string nameto, DateTime dateto)
        {
            score = scoreto;
            name = nameto;
            date = dateto;
        }
        public hiscoredata(long scoreto, string nameto)
        {
            score = scoreto;
            name = nameto;
            date = DateTime.Now;
        }
    }

    public static class hiscorestorage
    {
        public static List<hiscoredata> hiscorelist;
        public static void Init()
        {
            hiscorelist = new List<hiscoredata>();
            SetDefaultScore();
        }

        public static void SetDefaultScore()
        {
            hiscorelist.Clear();
            /*AddHiScore(new hiscoredata(3000, "MeiLing"));
            AddHiScore(new hiscoredata(2000, "Cirno"));
            AddHiScore(new hiscoredata(6000, "Remilia"));
            AddHiScore(new hiscoredata(5000, "Sakuya"));
            AddHiScore(new hiscoredata(7000, "Flandre"));
            AddHiScore(new hiscoredata(1000, "Rumia"));
            AddHiScore(new hiscoredata(4000, "Pachouli"));*/
            for (int i = 0; i < 12; i++) AddHiScore(new hiscoredata(0, "Koishi"));
        }

        public static void AddHiScore(hiscoredata datato)
        {
            int i=0;
            while (i < hiscorelist.Count&&datato < hiscorelist[i]) i++;
            hiscorelist.Insert(i, datato);
        }
    }
    public static class BGMManager
    {
        private static Thread mediaSynthThread;
        private static string songContentPath = "";
        private static bool mediaPlayerThreadEnable = true;
        static Game game;

        public static void ChangeSong(string pathto)
        {
            songContentPath = pathto;
        }

        private static void mediaPlayThread()
        {
            string nowSongContentPath;
            nowSongContentPath = songContentPath;
            while (mediaPlayerThreadEnable)
            {
                if (nowSongContentPath != "")
                {
                    Song song = game.Content.Load<Song>(nowSongContentPath);
                    MediaPlayer.Play(song);
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = GameConfig.FBgmvol;
                }
                while (nowSongContentPath == songContentPath && mediaPlayerThreadEnable)
                {
                }
                nowSongContentPath = songContentPath;
            }
        }

        public static void CreateThread(Game gameto)
        {
            if (mediaSynthThread != null) mediaSynthThread.Abort();
            game=gameto;
            mediaSynthThread = new Thread(new ThreadStart(mediaPlayThread));
            mediaSynthThread.Name = "SynthStreamerThread";
            mediaSynthThread.IsBackground = true;
            mediaSynthThread.Start();
        }
    }

}
