using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CirclesOfHell
{
    struct ParticleState
    {
        public Vector2 Position;
        public float Angle;
        public float Scale;        
        public Color SetColor;
        public float Alpha;
        static Random randy = new Random();

        public static ParticleState randomize(ParticleState StateOne, ParticleState OffSet)
        {
            ParticleState result = new ParticleState();
            result.Position.X = MathHelper.Lerp(StateOne.Position.X + OffSet.Position.X, StateOne.Position.X - OffSet.Position.X, (float)randy.NextDouble());
            result.Position.Y = MathHelper.Lerp(StateOne.Position.Y + OffSet.Position.Y, StateOne.Position.Y - OffSet.Position.Y, (float)randy.NextDouble());

            result.SetColor.R = (byte)MathHelper.Lerp(StateOne.SetColor.R + OffSet.SetColor.R, StateOne.SetColor.R - OffSet.SetColor.R, (float)randy.NextDouble());
            result.SetColor.G = (byte)MathHelper.Lerp(StateOne.SetColor.G + OffSet.SetColor.G, StateOne.SetColor.G - OffSet.SetColor.G, (float)randy.NextDouble());
            result.SetColor.B = (byte)MathHelper.Lerp(StateOne.SetColor.B + OffSet.SetColor.B, StateOne.SetColor.B - OffSet.SetColor.B, (float)randy.NextDouble());

            result.SetColor.A = (byte)MathHelper.Lerp(StateOne.Alpha + OffSet.Alpha, StateOne.Alpha - OffSet.Alpha, (float)randy.NextDouble());
            result.Angle = MathHelper.Lerp(StateOne.Angle + OffSet.Angle, StateOne.Angle - OffSet.Angle, (float)randy.NextDouble());
            result.Scale = MathHelper.Lerp(StateOne.Scale + OffSet.Scale, StateOne.Scale - OffSet.Scale, (float)randy.NextDouble());

            return result;          
        }

        public static ParticleState Lerp(ParticleState StateOne, ParticleState stateTwo, float T)
        {
            ParticleState result = new ParticleState();
            result.Position = Vector2.Lerp(StateOne.Position , stateTwo.Position, T);

            result.SetColor.R = (byte)MathHelper.Lerp(StateOne.SetColor.R , stateTwo.SetColor.R, T);
            result.SetColor.G = (byte)MathHelper.Lerp(StateOne.SetColor.G , stateTwo.SetColor.G, T);
            result.SetColor.B = (byte)MathHelper.Lerp(StateOne.SetColor.B , stateTwo.SetColor.B, T);

            result.SetColor.A = (byte)MathHelper.Lerp(StateOne.Alpha, stateTwo.Alpha, T);
            result.Angle = MathHelper.Lerp(StateOne.Angle , stateTwo.Angle, T);
            result.Scale = MathHelper.Lerp(StateOne.Scale , stateTwo.Scale, T);

            return result;
        }
        public static ParticleState operator +(ParticleState StateOne, ParticleState StateTwo)
        {
            ParticleState result = new ParticleState();
            result.Position = StateOne.Position + StateTwo.Position;

            result.SetColor.R = (byte)MathHelper.Clamp((StateOne.SetColor.R + StateTwo.SetColor.R), 0, 255);
            result.SetColor.G = (byte)MathHelper.Clamp((StateOne.SetColor.G + StateTwo.SetColor.G),0,255);
            result.SetColor.B = (byte)MathHelper.Clamp((StateOne.SetColor.B + StateTwo.SetColor.B),0,255);
            result.SetColor.A = (byte)MathHelper.Clamp((StateOne.SetColor.A + StateTwo.SetColor.A), 0, 255);
            result.Alpha = MathHelper.Clamp(StateOne.Alpha + StateTwo.Alpha, 0, 1);

            result.Angle = StateOne.Angle + StateTwo.Angle;
            result.Scale = Math.Max(StateOne.Scale + StateTwo.Scale, 0);

            return result;
        }
        public static ParticleState operator -(ParticleState StateOne, ParticleState StateTwo)
        {
            ParticleState result = new ParticleState();
            result.Position = StateOne.Position - StateTwo.Position;

            result.SetColor.R = (byte)MathHelper.Clamp((StateOne.SetColor.R - StateTwo.SetColor.R), 0, 255);
            result.SetColor.G = (byte)MathHelper.Clamp((StateOne.SetColor.G - StateTwo.SetColor.G), 0, 255);
            result.SetColor.B = (byte)MathHelper.Clamp((StateOne.SetColor.B - StateTwo.SetColor.B), 0, 255);
            result.SetColor.A = (byte)MathHelper.Clamp((StateOne.SetColor.A - StateTwo.SetColor.A), 0, 255);
            result.Alpha = MathHelper.Clamp(StateOne.Alpha - StateTwo.Alpha, 0, 1);

            result.Angle = StateOne.Angle - StateTwo.Angle;
            result.Scale = Math.Max(StateOne.Scale - StateTwo.Scale, 0);

            return result;
        }
    }
}
