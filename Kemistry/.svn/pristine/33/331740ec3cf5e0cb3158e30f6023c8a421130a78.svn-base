using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common
{
    public class UIElement : IUpdateable
    {
        #region From IUpdateable
        public bool Enabled { get; set; }
        public int UpdateOrder { get; set; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        #endregion

        public event EventHandler<EventArgs> Clicked;

        public Texture2D Background { get; set; }
        public Rectangle Bounds { get; set; }
        public List<UIElement> Children { get; set; }
        public Color Color { get; set; }

        public UIElement(Texture2D background = null,
                        Rectangle? bounds = null)
        {
            Background = background;
            Bounds = bounds == null ? Rectangle.Empty : (Rectangle)bounds;
            Children = new List<UIElement>();
            Color = Color.White;
            Clicked += OnClicked;
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (UIElement element in Children)
                element.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (Background != null)
                spriteBatch.Draw(Background,
                    new Rectangle(Bounds.X + (int)position.X,
                        Bounds.Y + (int)position.Y,
                        Bounds.Width,
                        Bounds.Height),
                        Color);
            foreach (UIElement element in Children)
                element.Draw(spriteBatch, position + new Vector2(Bounds.X, Bounds.Y));
        }

        public virtual void OnClicked(object sender, EventArgs args)
        {

        }

        public virtual void Click()
        {
            Clicked(this, new EventArgs());
        }
    }
}
