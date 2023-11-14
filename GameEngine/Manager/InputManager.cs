using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine.Manager
{
    public static class InputManager
    {
        static KeyboardState PreviousKeyboardState { get; set; }
        static KeyboardState CurrentKeyboardState { get; set; }
        static MouseState PreviousMouseState { get; set; }
        static MouseState CurrentMouseState { get; set; }

        public static void Initialize()
        {
            PreviousKeyboardState = CurrentKeyboardState = Keyboard.GetState();
            PreviousMouseState = CurrentMouseState = Mouse.GetState();
        }

        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public static bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }
        public static bool IsKeyReleased(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key) &&
                    PreviousKeyboardState.IsKeyDown(key);
        }

        // *** Lab08 ***
        public static Vector2 GetMousePosition()
        {
            return new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        }
        // *************
        public static bool LeftButtonPressed()
        {
            if (CurrentMouseState.LeftButton == ButtonState.Pressed
                && PreviousMouseState.LeftButton == ButtonState.Released)
                return true;
            else return false;
        }
        public static bool LeftButtonDown()
        {
            if (CurrentMouseState.LeftButton == ButtonState.Pressed
                && PreviousMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else return false;
        }
        public static bool RightButtonPressed()
        {
            if (CurrentMouseState.RightButton == ButtonState.Pressed
                && PreviousMouseState.RightButton == ButtonState.Released)
                return true;
            else return false;
        }
        public static bool RightButtonDown()
        {
            if (CurrentMouseState.RightButton == ButtonState.Pressed
                && PreviousMouseState.RightButton == ButtonState.Pressed)
                return true;
            else return false;
        }
    }
}
