using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CirclesOfHell
{
    class Projectile : GameObject
    {
        
        private float __life = 4.0f;

        ParticleEmitter __particleEmitter;
        ParticleState __startState;
        ParticleState __offsetState;
        ParticleState __endState;


        public Projectile(string _assetPath):
            base(_assetPath,new Vector2(0,0))
        {
            Speed = 500;
            __particleEmitter = new ParticleEmitter(500);

            __startState=new ParticleState();
            __startState.Position = new Vector2(0, 0);
            __startState.Alpha = 1;
            __startState.Angle = 0;
            __startState.Scale = 1;
            __startState.SetColor = Color.Yellow;

            __offsetState = new ParticleState();
            __offsetState.Position = new Vector2(0, -10);
            __offsetState.Alpha = 0.5f;
            __offsetState.Angle = 0;
            __offsetState.Scale = 1;
            __offsetState.SetColor = Color.Yellow;

            __endState=new ParticleState();
            __endState.Position = new Vector2(0, 0);
            __endState.Alpha = 0;
            __endState.Angle = 0;
            __endState.Scale = 0.5f;
            __endState.SetColor = Color.White;

            __particleEmitter.InitEmitter(__startState, __endState, __offsetState, 0.5f);
        }

        public void LoadParticles(ContentManager _content)
        {
            __particleEmitter.LoadContent(_content);
        }

       
        public void Activate(Vector2 _position, Direction _direction)
        {
            __active = true;
            __life = 4.0f;
            __position = _position;
            Speed = 500;

            if (_direction == Direction.Left)
            {
                Speed = -Speed;
            }

            __particleEmitter.StartEmitter(-1, 0.01f);
        }

        public bool CheckCollision(Rectangle _rect)
        {
            if (_rect.X <= __origin.X && (_rect.X + _rect.Width) >= __origin.X &&
                _rect.Y >= __origin.Y && (_rect.Y - _rect.Height) <= __origin.Y)
            {
                return true;
            }
            return false;
        }

        public override void Update(GameTime _gameTime)
        {
            float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            if (Active)
            {
                base.Update(_gameTime);

                __particleEmitter.EndPosition = new Vector2(__position.X - Speed * elapsedTime, __position.Y);

                __position.X += Speed * elapsedTime;

                __particleEmitter.StartPosition = new Vector2(__position.X, __position.Y);

                __life -= elapsedTime;

                __particleEmitter.Update(_gameTime);

                if (__life < 0)
                {
                    __active = false;
                    __life = 4.0f;
                }

                
            }
        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            if(Active)
            {
                _spriteBatch.Draw(__sprite.Texture,
                __position,
                null,
                Color.White,
                0,
                new Vector2(__sprite.Texture.Width / 2, __sprite.Texture.Height / 2),
                1,
                SpriteEffects.None,
                0);

                __particleEmitter.Draw(_spriteBatch);
            }
        }
    }
}
