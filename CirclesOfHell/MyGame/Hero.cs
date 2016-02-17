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
    class Hero : Player
    {
        Sprite __fire;
        static float __fireEnergy;
        public Bar.GetValueDel GetEnergyValueDel = new Bar.GetValueDel(GetEnergyValue);

        public static float GetEnergyValue()
        {
            return __fireEnergy;
        }

        static float __healthPoints;
        public Bar.GetValueDel GetHealthValueDel = new Bar.GetValueDel(GetHealthValue);

        public static float GetHealthValue()
        {
            return __healthPoints;
        }

        bool isAttacking = false;

        bool isInvincible = false;
        const float INVINCIBILITY_TIME = 1.0f;
        float invincibilityTimer = INVINCIBILITY_TIME;

        ParticleEmitter __particleEmitter;

        public Hero(string _assetPath, Vector2 _initialPosition) :
            base(_assetPath, _initialPosition)
        {
            EntityType = Entity.Player;

            Collidable = true;
            IsTrigger = false;
            __sourceRectangle = new Rectangle(40, 0, 48, 128);
            __destinationRectangle = new Rectangle(0, 0, 48, 128);

            this.Sprite = new Sprite(_assetPath,true,true);

            this.Sprite.Animation.AddAnimation("idle", 1, new byte[] { 0 },false);
            this.Sprite.Animation.AddAnimation("walk", 8, new byte[] { 0, 1, 2, 3, 4, 5, 6 },true);
            this.Sprite.Animation.AddAnimation("jump_up", 1, new byte[] { 6 },false);
            this.Sprite.Animation.AddAnimation("jump_down", 1, new byte[] { 5 },false);
            this.Sprite.Animation.CurrentAnimName = "idle";

            __fire = new Sprite(_assetPath, true, true);
            __fire.Animation.AddAnimation("melee", 8, new byte[] { 7, 8 },true);
            __fire.Animation.CurrentAnimName = "melee";
            __fire.SetFrameDimensions(128, 128);
            __fire.Active = true;

            AddChild(__fire);

            __healthPoints = 100;

            //this.Children.ElementAt(this.Children.Count - 1).Active = true;

            __particleEmitter = new ParticleEmitter(500);

            ParticleState startState = new ParticleState();
            startState.Position = new Vector2(0, 0);
            startState.Alpha = 1;
            startState.Angle = MathHelperEasing.Pi;
            startState.Scale = 3;
            startState.SetColor = Color.Black;

            ParticleState offsetState = new ParticleState();
            offsetState.Position = new Vector2(0, -10);
            offsetState.Angle = MathHelperEasing.Pi * 45 / 180;
            offsetState.Scale = 1;


            ParticleState endState = new ParticleState();
            endState.Position = new Vector2(0, 0);
            endState.Alpha = 0;
            endState.Angle = 0;
            endState.Scale = 0.1f;
            endState.SetColor = Color.White;

            __particleEmitter.InitEmitter(startState, endState, offsetState, 0.3f);
            __particleEmitter.StartEmitter(-1, 0.01f);
        }

        public override void LoadContent(GraphicsDevice _graphicsDevice)
        {
            base.LoadContent(_graphicsDevice);
            Sprite.SetFrameDimensions(128, 128);
        }

        public void LoadParticles(ContentManager _content)
        {
            __particleEmitter.LoadContent(_content);
        }

        public override void HasCollided(GameObject.Entity _type)
        {
            base.HasCollided(_type);

            switch (_type)
            {
                case Entity.Blank:
                    break;
                case Entity.Player:
                    break;
                case Entity.NPC:
                    break;
                case Entity.Enemy:
                    {
                        if(isInvincible == false)
                        {
                            isInvincible = true;
                            __healthPoints -= 20;
                            __healthPoints = MathHelper.Clamp(__healthPoints, 0, 100);
                        }
                    }
                    break;
                case Entity.Pickup:
                    {
                        __healthPoints = 100;
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Update(GameTime _gameTime)
        {
            //base.Update(_gameTime);
            float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            //Invincibility timer
            if(isInvincible)
            {
                if(invincibilityTimer<0)
                {
                    isInvincible = false;
                    invincibilityTimer = INVINCIBILITY_TIME;
                }
                invincibilityTimer -= elapsedTime;
            }

            //CHECKS FOR BUTTONS PRESSED

            KeyboardState keyState = Keyboard.GetState();
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One);
            MouseState mouse = Mouse.GetState();

            if (keyState.IsKeyDown(Keys.W) || gamePad.DPad.Up == ButtonState.Pressed)
            {
                Jump();
            }

            UpdateJumping(elapsedTime);
            
            if (__gravityIsOn)
            {
                Speed += (float)(PhysicsVars.JUMPING_ACCELERATION_DOWN * elapsedTime);
                __position.Y += (int)(Math.Ceiling(elapsedTime * Speed));

            }


            if ((keyState.IsKeyDown(Keys.Up) || (gamePad.Buttons.A == ButtonState.Pressed)) && !__isJumping && !__isReversing)
            {
                if(__fireEnergy>0)
                {
                    isAttacking = true;
                    __fire.Active = true;
                    __fireEnergy = MathHelper.Clamp(__fireEnergy - 1, 0, 100);

                    this.Sprite.Animation.CurrentAnimName = "idle";
                    if (__characterDirection == Direction.Right)
                        __fire.Position = new Vector2(__position.X + 128 / 2 + __destinationRectangle.Width - 84, __position.Y);
                    else
                        __fire.Position = new Vector2(__position.X - 128 + 20, __position.Y);
                }
                else
                {
                    isAttacking = false;
                    __fire.Active = false;
                }
                

            }
            else if (keyState.IsKeyDown(Keys.D) || gamePad.DPad.Right == ButtonState.Pressed)
            {
                __fireEnergy = MathHelper.Clamp(__fireEnergy + 5, 0, 100);
                __characterDirection = Direction.Right;

                if (__position.X + __sprite.Width < __worldBounds.Width)
                {
                    if (__isJumping || __isReversing || __gravityIsOn)
                    {
                        __position.X += PhysicsVars.HORIZONTAL_SPEED_AIR * elapsedTime;
                    }
                    else
                    {
                        this.Sprite.Animation.CurrentAnimName = "walk";
                        isAttacking = false;
                        __fire.Active = false;
                        __fire.Orientation = Direction.Right;

                        __position.X += PhysicsVars.HORIZONTAL_SPEED_GROUND * elapsedTime;

                        CheckIfOnGround();
                    }
                }

            }
            else if (keyState.IsKeyDown(Keys.A) || gamePad.DPad.Left == ButtonState.Pressed)
            {
                __fireEnergy = MathHelper.Clamp(__fireEnergy + 5, 0, 100);
                __characterDirection = Direction.Left;

                if (__position.X > __worldBounds.X)
                {
                    if (__isJumping || __isReversing || __gravityIsOn)
                    {

                        __position.X -= PhysicsVars.HORIZONTAL_SPEED_AIR * elapsedTime;
                    }
                    else
                    {

                        this.Sprite.Animation.CurrentAnimName = "walk";
                        isAttacking = false;
                        __fire.Active = false;
                        __fire.Orientation = Direction.Left;

                        __position.X -= PhysicsVars.HORIZONTAL_SPEED_GROUND * elapsedTime;

                        CheckIfOnGround();
                    }
                }

            }

            //If no button is pressed, disable animation and leave the function
            else
            {
                __fireEnergy = MathHelper.Clamp(__fireEnergy + 5, 0, 100);
                isAttacking = false;
                __fire.Active = false;

                __destinationRectangle.X = (int)__position.X;
                __destinationRectangle.Y = (int)__position.Y;


                if (!__gravityIsOn && !__isJumping)
                    this.Sprite.Animation.CurrentAnimName = "idle";

                __sourceRectangle.X = this.Sprite.Animation.GetSourcePosition() + 40;
                //__fire.Animation.Update(_gameTime);

                UpdateParticles(_gameTime);

                return;
            }

            //ANIMATION MANAGEMENT
            __destinationRectangle.X = (int)__position.X;
            __destinationRectangle.Y = (int)__position.Y;

            this.Sprite.Animation.Update(_gameTime);
            //__fire.Animation.Update(_gameTime);

            __sourceRectangle.X = this.Sprite.Animation.GetSourcePosition() + 40;

            
            UpdateBounds();
            UpdateParticles(_gameTime);

        }

        private void Jump()
        {
            if (__isJumping == false && __isReversing == false && isAttacking == false)
            {
                this.Sprite.Animation.CurrentAnimName = "jump_up";
                __isJumping = true;
                __initialY = (int)__position.Y;
                if (__gravityIsOn)
                {
                    __gravityIsOn = false;
                }
                Speed = PhysicsVars.JUMPING_INITIAL_VELOCITY;

                //SoundManager.Instance.PlaySound("jump");
            }
        }

        private void UpdateJumping(float deltaTime)
        {
            //If the player is jumping, apply acceleration
            if (__isJumping)
            {


                //If the player is moving up, accelerate
                if (Speed >= 0)
                {

                    Speed -= (float)(PhysicsVars.JUMPING_ACCELERATION_UP * deltaTime);
                    __position.Y -= (int)(Math.Ceiling(deltaTime * Speed));
                }
                //If the player reached the highest point, set reversing mode to true
                else
                {
                    this.Sprite.Animation.CurrentAnimName = "jump_down";
                    __isReversing = true;
                    __gravityIsOn = true;
                    __isJumping = false;
                    Speed = 0;

                }
            }
        }

        

        private void UpdateBounds()
        {
            //UPDATING BOUNDS (DEPENDING ON THE CHARACTER'S ACTUAL WIDTH)
            __boundingBox.X = (int)__position.X + 40;
            __boundingBox.Width = __sourceRectangle.Width;
        }

        private void UpdateParticles(GameTime _gameTime)
        {
            //PARTICLE EFFECT MAMANGEMENT
            __particleEmitter.StartPosition = new Vector2(__position.X + Bounds.Width / 2, __position.Y + Bounds.Height / 2);
            if (__characterDirection == Direction.Right)
                __particleEmitter.EndPosition = new Vector2(__position.X + Bounds.Width / 2 - 20, __position.Y + Bounds.Height / 2);
            else
                __particleEmitter.EndPosition = new Vector2(__position.X + Bounds.Width / 2 + 20, __position.Y + Bounds.Height / 2);
            __particleEmitter.Update(_gameTime);
        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            __particleEmitter.Draw(_spriteBatch);
            if (isAttacking)
            {
               // if (__characterDirection == Direction.Left)
                 //   _spriteBatch.Draw(__fire.Texture, __fire.Position, new Rectangle(__fire.Animation.GetSourcePosition(), 0, 128, 128), Color.White, 0.0f, __fire.Origin, new Vector2(1.0f, 1.0f), SpriteEffects.FlipHorizontally, 0);
                //else
                //    _spriteBatch.Draw(__fire.Texture, __fire.Position, new Rectangle(__fire.Animation.GetSourcePosition(), 0, 128, 128), Color.White, 0.0f, __fire.Origin, new Vector2(1.0f, 1.0f), SpriteEffects.None, 0);
            }
            base.Draw(_spriteBatch, _gameTime);
        }
    }
}
