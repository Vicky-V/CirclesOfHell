using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CirclesOfHell
{
    class Camera
    {
        Viewport __viewport;

        //Location relative to the game world
        Vector2 __position;
        //Outer boundaries for the camera to pan
        Rectangle __limits;

        public Vector2 Origin;

        public Vector2 Target;

        public float Zoom = 0.7f;

        public float Rotation;

        public Rectangle Limits
        {
            get { return __limits; }
            set
            {
                if (value != null)
                {
                    __limits = new Rectangle();
                    __limits.X = value.X;
                    __limits.Y = value.Y;
                    __limits.Width = Math.Max(__viewport.Width, value.Width);
                    __limits.Height = Math.Max(__viewport.Height, value.Height);
                }
                else
                {
                    __limits = Rectangle.Empty;
                }
            }
        }

        public Vector2 Position
        {
            get { return __position; }
            set
            {
                __position = value;
                if (__limits != Rectangle.Empty)
                {
                    __position.X = MathHelper.Clamp(__position.X, __limits.X, __limits.X + __limits.Width - __viewport.Width);
                    __position.Y = MathHelper.Clamp(__position.Y, __limits.Y, __limits.Y + __limits.Height - __viewport.Height);
                }
            }
        }

        public Matrix ViewMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0)) *
                       Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                       Matrix.CreateRotationZ(Rotation) *
                       Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                       Matrix.CreateTranslation(new Vector3(Origin, 0));
            }

        }

        public Matrix GetParallaxViewMatrix(Vector2 _parallax)
        {

            return Matrix.CreateTranslation(new Vector3(-Position * _parallax, 0)) *
                    Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom, Zoom, 1.0f) *
                    Matrix.CreateTranslation(new Vector3(Origin, 0));


        }

        public Camera(Viewport _vp)
        {
            this.__viewport = _vp;
            //Origin = new Vector2(__viewport.Width / 2, __viewport.Height / 2);
            Origin = new Vector2(0,0);
        }

        public void LookAt(Vector2 lookAtPosition)
        {
            Target = lookAtPosition - Origin;
        }
        public void Update(GameTime _gameTime)
        {
            float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 newPos = new Vector2(MathHelper.Lerp(Position.X, Target.X, 0.5f * elapsedTime),
                                       MathHelper.Lerp(Position.Y, Target.Y, 0.5f * elapsedTime));
            Position = newPos;
        }
    }
}
