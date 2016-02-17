using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CirclesOfHell
{
    /// <summary>
    /// Initial code for Player class, everything is subject to change
    /// </summary>
    class Player : GameObject
    {
               
        protected Rectangle __worldBounds;

        protected Direction __characterDirection = Direction.Right;

        public Direction Directions
        {
            get { return __characterDirection; }
        }

        protected Rectangle __sourceRectangle = new Rectangle(0, 0, 48, 128);
        protected Rectangle __destinationRectangle = new Rectangle(0, 0, 128, 128);

        

        public override Rectangle Bounds
        {
            get { return __destinationRectangle; }
        }
        public override Vector2 Position
        {
            get { return __position; }
            set { __position = value; }
        }
        public Rectangle WorldBounds
        {
            set { __worldBounds = value; }
        }

        public Player(string _assetPath, Vector2 _initialPosition)
        {
            __position = _initialPosition;
            __assetPath = _assetPath;
            
        }

        public override void Update(GameTime _gameTime)
        {
            base.Update(_gameTime);
        }

        protected virtual void CheckIfOnGround()
        {
            //Check if the player is on the platform or not
            if (__groundBounds.X != -1 && __groundBounds.Y != -1)
            {
                if (__position.X > __groundBounds.Y || __position.X < __groundBounds.X)
                {
                    Speed = 10;
                    __gravityIsOn = true;//Switch gravity on if the player left the platform
                }
            }
        }

        public override void LoadContent(GraphicsDevice _graphicsDevice)
        {
            foreach(BaseObject child in __children)
            {
                child.LoadContent(_graphicsDevice);
            }
            base.LoadContent(_graphicsDevice);
        }
        
        
        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            //base.Draw(_spriteBatch, _gameTime);

            if (__characterDirection == Direction.Left)
                _spriteBatch.Draw(Sprite.Texture, __destinationRectangle, __sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            else
                _spriteBatch.Draw(Sprite.Texture, __destinationRectangle, __sourceRectangle, Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);


        }
    }
}
