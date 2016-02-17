using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace CirclesOfHell
{
    abstract class BaseObject
    {
        public enum Direction { Left, Right };

        protected Vector2 __position;
        public virtual Vector2 Position
        {
            get { return __position; }
            set { __position = value;}
        }
        

        protected Vector2 __origin;
        public Vector2 Origin
        {
            get { return __origin; }
        }


        protected bool __active=true;
        public bool Active
        {
            get
            {
                return __active;
            }
            set
            {
               __active = value;
            }
        }

        abstract public void LoadContent(GraphicsDevice _graphicsDevice);

        abstract public void Update(GameTime _gameTime);

        abstract public void Draw(SpriteBatch _spriteBatch, GameTime _gameTime);
    }
}
