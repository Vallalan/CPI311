using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public class ButtonGroup : UIElement
    {
        public int Current { get; set; }

        public ButtonGroup(Texture2D background = null,
            Rectangle? bounds = null)
            : base(background, bounds)
        {
            Current = 0;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.Down))
                Current = (Current + 1) % Children.Count;
            if (keyboardState.IsKeyDown(Keys.Enter))
                Children[Current].Click();
            foreach (UIElement element in Children)
                element.Color = Color.Gray;
            Children[Current].Color = Color.White;
            base.Update(gameTime);
        }
    }
}
