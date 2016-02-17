using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CirclesOfHell
{
    class Particle
    {
        bool __active = false;

        float __life = 4.0f;
        float __currentLife;

        ParticleState __currentState;
        ParticleState __startState;
        ParticleState __endState;


        public bool Active
        {
            get { return __active; }
        }

        public Texture2D SpriteTexture { get; set; }

        public void Activate(ParticleState _startState, ParticleState _endState,
                             float _life)
        {
            __active = true;
            __life = _life;
            __currentLife = _life;
            __currentState = _startState;
            __startState = _startState;
            __endState = _endState;
        }

        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            __currentLife -= elapsedTime;


            __currentState.Position = Vector2.Lerp(__startState.Position,
                __endState.Position, Easing.EaseOut(1 - __currentLife / __life, EasingType.Linear));

            __currentState.SetColor = Color.Lerp(__startState.SetColor,
              __endState.SetColor, Easing.EaseIn(1 - __currentLife / __life, EasingType.Quartic));

            __currentState.Angle = MathHelper.Lerp(__startState.Angle,
              __endState.Angle, Easing.EaseOut(1 - __currentLife / __life, EasingType.Linear));

            __currentState.Scale = MathHelper.Lerp(__startState.Scale,
              __endState.Scale, Easing.EaseOut(1 - __currentLife / __life, EasingType.Cubic));

            __currentState.Alpha = MathHelper.Lerp(__startState.Alpha,
                __endState.Alpha, Easing.EaseOut(1 - __currentLife / __life, EasingType.Linear));

            __currentState.SetColor.A = (byte)(__currentState.Alpha * 255);

            if(__currentLife < 0)
            {
                __active = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteTexture,
                __currentState.Position,
                null,
                __currentState.SetColor,
                __currentState.Angle,
                Vector2.One / 2,
                __currentState.Scale,
                SpriteEffects.None,
                0);
        }
    }
}
