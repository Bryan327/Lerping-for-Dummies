using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Project_Aurora_Stuff
{
    internal class Program
    {
        private int mouseX, mouseY;
        private int moveCardTimer;
        private int moveCardIndex;
        private int width = 800;
        private int height = 600;
        private Interactable[] boxes = new Interactable[20];
        private MouseState current;
        private bool gameRunning;

        private static void Main(string[] args)
        {
            Program p = new Program();
            p.start();
        }

        public void start()
        {
            generateBoxes();
            moveCardIndex = 0;

            gameRunning = true;

            VSyncMode VSync = VSyncMode.On;

            GameWindow window = new GameWindow();

            window.Width = width;
            window.Height = height;
            window.Visible = true;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);

            while (gameRunning)
            {
                //pollInput();
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Color3(.5f, .5f, 1.0f);

                for (int i = 0; i < boxes.Length; i++)
                {
                    if (boxes[i].isMoving)
                    {
                        boxes[i].lerp(boxes[i].destinationX, boxes[i].destinationY, .1f);
                        boxes[i].checkIfDestinationReached();
                    }
                }

                if (!(moveCardIndex == boxes.Length))
                {
                    moveCardTimer++;
                    if (moveCardTimer == 30)
                    {
                        boxes[moveCardIndex].startMovement();
                        moveCardTimer = 0;
                        moveCardIndex++;
                    }
                }

                for (int i = 0; i < boxes.Length; i++)
                {
                    GL.Begin(BeginMode.Quads);

                    GL.Vertex2(boxes[i].x, boxes[i].y);
                    GL.Vertex2(boxes[i].x + boxes[i].width, boxes[i].y);
                    GL.Vertex2(boxes[i].x + boxes[i].width, boxes[i].y + boxes[i].height);
                    GL.Vertex2(boxes[i].x, boxes[i].y + boxes[i].height);
                    GL.End();
                }

                window.SwapBuffers();
            }
        }

        public void pollInput()
        {
            current = Mouse.GetState();
            mouseX = current.X;
            mouseY = current.Y;
        }

        public void generateBoxes()
        {
            float x, y, destX, destY;

            x = 100;
            y = 100;
            destX = 50;
            destY = 470;
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i] = new Interactable(x, y, destX, destY);
                destX += boxes[i].width + 20;
                if (destX >= width - boxes[i].width)
                {
                    destX = 50;
                    destY -= boxes[i].height + 20;
                }
            }
        }
    }
}