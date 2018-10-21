using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TH_V0
{
    
    class ImageHelper
    {
        static List <Texture2D> myImage = new List<Texture2D>();
        public static Texture2D getImage(ImageName imageName)
        {
            int i = (int)imageName;
            if (i < myImage.Count)
                return myImage[i];
            else
                return null;
        }
        public static void AddImage(Texture2D image)
        {
            myImage.Add(image);
        }
    }
}
