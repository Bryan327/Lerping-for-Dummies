using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Project_Aurora_Stuff
{
    internal class Interactable
    {
        public float x, y, width, height, centerX, centerY, destinationX, destinationY;
        public bool isMoving, hasTarget, hasReachedTarget;
        public static Bitmap bmp = new Bitmap(@"NewMagicTemplate.PNG");

        public Interactable()
        {
            x = 100;
            y = 100;
            destinationX = 0;
            destinationY = 0;
            width = bmp.Width;
            height = bmp.Height;
            centerX = x + (width / 2);
            centerY = y + (height / 2);
            isMoving = false;
            hasReachedTarget = true;
        }

        public Interactable(float setX, float setY, float toX, float toY)
        {
            x = setX;
            y = setY;
            destinationX = toX;
            destinationY = toY;
            width = bmp.Width;
            height = bmp.Height;
            centerX = x + (width / 2);
            centerY = y + (height / 2);
            isMoving = false;
            hasReachedTarget = true;
        }

        public void lerp(float destX, float destY, float percent)
        {
            float vectX, vectY;

            vectX = destX - x;
            vectY = destY - y;

            x += vectX * percent;
            y += vectY * percent;
            centerX = x + (width / 2);
            centerY = y + (height / 2);
        }

        public void setDestination(float destX, float destY)
        {
            destinationX = destX;
            destinationY = destY;
            isMoving = true;
        }

        public void startMovement()
        {
            isMoving = true;
        }

        public void checkIfDestinationReached()
        {
            if (x <= (destinationX + 1f) && x >= (destinationX - 1f) && y <= (destinationY + 1f) && y >= (destinationY - 1f))
            {
                resetMotionFlags();
            }
        }

        public void resetMotionFlags()
        {
            hasReachedTarget = true;
            isMoving = false;
            hasTarget = false;
        }

        public void renderCard(int id)
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0, 0);
            GL.Vertex2(x, y);
            GL.TexCoord2(1, 0);
            GL.Vertex2(x + bmp.Width, y);
            GL.TexCoord2(1, 1);
            GL.Vertex2(x + bmp.Width, y + bmp.Height);
            GL.TexCoord2(0, 1);
            GL.Vertex2(x, y + bmp.Height);
            GL.Flush();
            GL.End();
        }

        public static int loadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
            {
                throw new ArgumentException(filename);
            }

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            bmp = new Bitmap(filename);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);
            bmp.UnlockBits(bmpData);

            return id;
        }
    }
}