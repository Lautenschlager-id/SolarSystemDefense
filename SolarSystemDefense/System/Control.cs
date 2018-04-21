using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace SolarSystemDefense
{
    class Control
    {
        private static MouseState MouseCurrent, MouseLast;

        public static Vector2 MouseCoordinates
        {
            get
            {
                return new Vector2(MouseCurrent.X, MouseCurrent.Y);
            }
        }
        public static Boolean MouseClicked
        {
            get
            {
                return MouseLast.LeftButton == ButtonState.Pressed && MouseCurrent.LeftButton == ButtonState.Released;
            }
        }
        public static int MouseWheel
        {
            get
            {
                if (MouseCurrent.ScrollWheelValue == MouseLast.ScrollWheelValue)
                    return 0;
                return MouseCurrent.ScrollWheelValue > MouseLast.ScrollWheelValue ? 1 : -1;
            }
        }
        
        public static void Update()
        {
            MouseLast = MouseCurrent;
            MouseCurrent = Mouse.GetState();
        }
    }
}
