using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Common
{
    public class Button : UIElement
    {
        public Texture2D Texture { get; set; }
        public String Text { get; set; }

        public SpriteFont SpriteFont { get; set; }

        public Button(Texture2D background = null,
                        Rectangle? bounds = null,
                        Texture2D texture = null,
                        String text = null,
                        SpriteFont font = null)
            :base(background,bounds)
        {
            Texture = texture;
            Text = text;
            SpriteFont = font;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Background != null)
                spriteBatch.Draw(Background,
                    new Rectangle(Bounds.X + (int)position.X,
                        Bounds.Y + (int)position.Y,
                        Bounds.Width,
                        Bounds.Height),
                        Color);
            if(Texture != null)
                spriteBatch.Draw(Texture,
                    new Vector2(Bounds.X + position.X,
                                Bounds.Y + position.Y),
                        Color);
            if (SpriteFont != null)
                spriteBatch.DrawString(SpriteFont, Text,
                    new Vector2(Bounds.X + position.X + Texture.Width,
                                Bounds.Y + position.Y),
                        Color.Red);
            foreach (UIElement element in Children)
                element.Draw(spriteBatch, position + new Vector2(Bounds.X, Bounds.Y));
        }
    }
}
