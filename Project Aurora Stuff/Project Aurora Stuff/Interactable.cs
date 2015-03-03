using System;

namespace Project_Aurora_Stuff
{
    internal class Interactable
    {
        public float x, y, width, height, centerX, centerY, destinationX, destinationY;
        public Boolean isMoving, hasTarget, hasReachedTarget;

        public Interactable()
        {
            x = 100;
            y = 100;
            destinationX = 0;
            destinationY = 0;
            width = 64;
            height = 96;
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
            width = 64;
            height = 96;
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
    }
}