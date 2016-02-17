using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CirclesOfHell
{
    class ProjectileManager : GameEntityManager
    {
        private float __delay = 0.0f;
        private float __TimeElapsed = 0.0f;

        public ProjectileManager(uint _poolSize, float _delay)
        {
            for(uint i = 0; i < _poolSize; i++)
            {
                AddEntity(new Projectile("Content/projectile.png"));
            }
            __delay = _delay;
        }

        public void Activate(Vector2 _position, GameObject.Direction _direction, ContentManager _content)
        {
            if(__TimeElapsed > __delay)
            {
                __TimeElapsed = 0.0f;

                foreach (Projectile projectile in __entityList)
                {
                    if (!projectile.Active)
                    {
                        projectile.Activate(_position, _direction);
                        projectile.LoadParticles(_content);
                        break;
                    }
                }
            } 
        }

        override public void Update(GameTime _gameTime)
        {
            __TimeElapsed += (float)_gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(_gameTime);
        }

        public void CheckCollision(Enemy[] _demon)
        {
            foreach (Projectile projectile in __entityList)
            {
                for (byte i = 0; i < _demon.Length; i++ )
                {
                    if (projectile.Active && _demon[i].Active)
                    {
                        if (projectile.CheckCollision(_demon[i].Bounds))
                        {
                            projectile.Active = false;
                            _demon[i].Active = false;
                        }
                    }
                }
            }
        }
    }
}
