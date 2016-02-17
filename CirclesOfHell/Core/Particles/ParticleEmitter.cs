using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace CirclesOfHell
{
    class ParticleEmitter
    {
        Particle[] __particleArray;

        //Amount of time since our last particle
        // was emitted
        float __particleTimer;

        //frequency of particle emition
        //(in seconds per particle)
        float __emitRate;

        //total number of particles to emit
        // in each set
        int __particleNumber;

        //Number of Particles emitted since
        // the start call.
        int __particleCount;

        ParticleState __startState;
        ParticleState __endState;
        ParticleState __offsetState;

        float __life;

       

        public Vector2 StartPosition
        {
            set { __startState.Position = value; }
        }
        public Vector2 EndPosition
        {
            set { __endState.Position = value; }
        }


        public ParticleEmitter(int _maxParticles)
        {
            __particleArray = new Particle[_maxParticles];
            for (int i = 0; i < __particleArray.Length; i++)
            {
                __particleArray[i] = new Particle();
            }
        }
        public void LoadContent(ContentManager Content)
        {
            Texture2D particleTexture = Content.Load<Texture2D>("Bullet");
            foreach (Particle particle in __particleArray)
            {
                particle.SpriteTexture = particleTexture;
            }
        }

        //public void LoadContent(GraphicsDevice _graphics)
        //{
        //    FileStream fileStream = new FileStream("Content/Bullet.xnb", FileMode.Open);
        //    Texture2D particleTexture = Texture2D.FromStream(_graphics, fileStream);
        //    fileStream.Close();

        //    foreach (Particle particle in __particleArray)
        //    {
        //        particle.SpriteTexture = particleTexture;
        //    }
        //}
        //Initializes the variables that this emitter emits
        public void InitEmitter(ParticleState _startState, 
                                ParticleState _endState, float _life)
        {
            __endState = _endState;
            __startState = _startState;
            __life = _life;
        }
        public void InitEmitter(ParticleState _startState,
                        ParticleState _endState, ParticleState _offsetFactor, float _life)
        {
            __endState = _endState;
            __offsetState = _offsetFactor;
            __startState = _startState;
            __life = _life;
        }

        public void StartEmitter(int _particleNumer, float _emitRate)
        {
            __particleCount = 0;
            __particleNumber = _particleNumer;
            __emitRate = _emitRate;

            if(__emitRate == -1)
            {
                foreach(Particle particle in __particleArray)
                {
                    if (!particle.Active)
                    {
                        __particleCount++;
                        
                        ParticleState tempState;
                        tempState = ParticleState.randomize(__endState, __offsetState);

                        particle.Activate(__startState, tempState, __life);
                    }
                    if (__particleCount == __particleNumber|| __particleNumber == -1)
                        break;
                }
            }

        }
        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (__emitRate != -1 && (__particleCount < __particleNumber || __particleNumber == -1))
            {
                __particleTimer += elapsedTime;
                if (__particleTimer > __emitRate)
                {
                    __particleTimer = 0;
                    foreach (Particle particle in __particleArray)
                    {
                        if (!particle.Active)
                        {
                            __particleCount++;
                            ParticleState tempState;
                            tempState = ParticleState.randomize(__endState, __offsetState);

                            particle.Activate(__startState, tempState, __life);
                            break;
                        }
                    }
                }
            }
            foreach (Particle particle in __particleArray)
            {
                if (particle.Active)
                    particle.Update(gameTime);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in __particleArray)
            {
                if (particle.Active)
                    particle.Draw(spriteBatch);
            }
        }
    }
}
