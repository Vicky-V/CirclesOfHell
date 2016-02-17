using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CirclesOfHell
{
    class MessageBox:BaseObject
    {
        string __message;
        string Message
        {
            get
            {
                return __message;
            }
            set 
            {
                __message = value; 
            }
        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(GraphicsDevice _graphicsDevice)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime _gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
