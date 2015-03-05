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
        private int width = 1920;
        private int height = 1080;
        private Interactable[] cards = new Interactable[20];
        private MouseState current;
        private bool gameRunning;
        private static int gameState;
        private const int SPLASH_SCREEN = 0;
        private const int MAIN_MENU = 1;
        private const int CARDS = 2;
        private static int texture;

        private static void Main(string[] args)
        {
            Program p = new Program();
            gameState = SPLASH_SCREEN;
            p.start();
        }

        public void start()
        {
            gameRunning = true;
            GameWindow window = new GameWindow();

            window.Width = width;
            window.Height = height;
            window.Visible = true;

            generatecards();
            moveCardIndex = 0;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, height, 0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Enable(EnableCap.Texture2D);

            texture = Interactable.loadTexture(@"NewMagicTemplate.PNG");

            while (gameRunning)
            {
                if (gameState == SPLASH_SCREEN)
                {
                    gameState = MAIN_MENU;
                }
                else if (gameState == MAIN_MENU)
                {
                    gameState = CARDS;
                }
                else if (gameState == CARDS)
                {
                    //pollInput();
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    GL.Color3(1.0f, 1.0f, 1.0f);

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
                        cards[i].renderCard(texture);
                    }

                    window.SwapBuffers();
                }
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