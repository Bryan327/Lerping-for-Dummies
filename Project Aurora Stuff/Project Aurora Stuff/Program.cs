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
        private Interactable[] cards = new Interactable[20];
        private MouseState current;
        private bool gameRunning;

        private static void Main(string[] args)
        {
            Program p = new Program();
            p.start();
        }

        public void start()
        {
            generatecards();
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

                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i].isMoving)
                    {
                        cards[i].lerp(cards[i].destinationX, cards[i].destinationY, .1f);
                        cards[i].checkIfDestinationReached();
                    }
                }

                if (!(moveCardIndex == cards.Length))
                {
                    moveCardTimer++;
                    if (moveCardTimer == 30)
                    {
                        cards[moveCardIndex].startMovement();
                        moveCardTimer = 0;
                        moveCardIndex++;
                    }
                }

                for (int i = 0; i < cards.Length; i++)
                {
                    GL.Begin(BeginMode.Quads);

                    GL.Vertex2(cards[i].x, cards[i].y);
                    GL.Vertex2(cards[i].x + cards[i].width, cards[i].y);
                    GL.Vertex2(cards[i].x + cards[i].width, cards[i].y + cards[i].height);
                    GL.Vertex2(cards[i].x, cards[i].y + cards[i].height);
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

        public void generatecards()
        {
            float x, y, destX, destY;

            x = 100;
            y = 100;
            destX = 50;
            destY = 470;
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = new Interactable(x, y, destX, destY);
                destX += cards[i].width + 20;
                if (destX >= width - cards[i].width)
                {
                    destX = 50;
                    destY -= cards[i].height + 20;
                }
            }
        }
    }
}