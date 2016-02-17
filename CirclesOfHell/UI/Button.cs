using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CirclesOfHell
{
    class Button:BaseObject
    {
        Sprite __buttonImage;

        public event EventHandler ButtonPressed;

        public Rectangle ButtonBox
        {
            get
            {
                return new Rectangle((int)__position.X,(int) __position.Y, 
                    __buttonImage.Width, __buttonImage.Height);
            }
        }

        public Button(string _assetPath)
        {
            __buttonImage = new Sprite(_assetPath, false, false);
        }


        public void OnButtonPressed()
        {
            //
            if (ButtonPressed != null)
                ButtonPressed(this, new EventArgs());
        }

        public override void LoadContent(GraphicsDevice _graphicsDevice)
        {
            __buttonImage.LoadContent(_graphicsDevice);
        }

        public override void Update(GameTime _gameTime)
        {
            __buttonImage.Position = this.Position;
            __buttonImage.Update(_gameTime);

            MouseState mouse = Mouse.GetState();
            CheckIfPressed(mouse);
        }

        private void CheckIfPressed(MouseState mouse)
        {
            if (ButtonBox.Contains(new Point(mouse.X,mouse.Y))&&mouse.LeftButton==ButtonState.Pressed)
                OnButtonPressed();
        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            __buttonImage.Draw(_spriteBatch, _gameTime);
        }
    }
}
