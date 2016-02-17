using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CirclesOfHell
{
    class Animations
    {
        private struct Film
        {

            public bool doesLoop;
            public byte animSpeed;
            public byte[] animFrames;

            public Film(byte _animSpeed, byte[] _animFrames, bool _doesLoop)
            {
                this.animSpeed = _animSpeed;
                this.animFrames = _animFrames;
                this.doesLoop = _doesLoop;
            }
        }

        //Animation variables
        private int __animPosition = 0;
        private float __animTimeElapsed = 0.0f;
        private string __currentAnimName = "";
        private Dictionary<string, Film> __animations;

        public int FrameWidth=128;

        public bool Finished=false;

        public string CurrentAnimName 
        { 
            set 
            {
                __currentAnimName = value;

                if (__animPosition > __animations[__currentAnimName].animFrames.Length - 1)
                {
                    __animPosition = 0;
                }
            }
        }

        public void AddAnimation(string _animName, byte _animSpeed, byte[] _animFrames,bool _doesLoop)
        {
            __animations[_animName] = new Film(_animSpeed, _animFrames, _doesLoop);
        }
        public ushort GetSourcePosition()
        {
            return (ushort)(__animations[__currentAnimName].animFrames[__animPosition] * FrameWidth);
        }
        public void Reset()
        {
            __animTimeElapsed = 0;
            __animPosition = 0;
        }
        public void Update(GameTime _gameTime)
        {
            if (Finished == false||__animations[__currentAnimName].doesLoop==true)
            {
                __animTimeElapsed += (float)_gameTime.ElapsedGameTime.TotalSeconds;
                if (__animTimeElapsed > 1.0f / __animations[__currentAnimName].animSpeed)
                {
                    __animTimeElapsed = 0;
                    __animPosition++;
                    if (__animPosition == __animations[__currentAnimName].animFrames.Length)
                    {
                        __animPosition = 0;
                        Finished = true;
                    }
                }
            }
        }

        public Animations()
        {
            __animations = new Dictionary<string, Film>();
        }

    }
}
