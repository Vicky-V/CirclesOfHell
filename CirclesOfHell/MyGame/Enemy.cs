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
    class Enemy : Player
    {
        private GameObject __target = null;
        public GameObject Target
        {
            set
            {
                __target = value;
            }
        }

        public Enemy(string _assetPath, Vector2 _initialPosition) :
            base(_assetPath, _initialPosition)
        {
            __active = true;
            EntityType = Entity.Enemy;

            Collidable = true;
            IsTrigger = false;
            __sourceRectangle = new Rectangle(0, 0, 32, 32);
            __destinationRectangle = new Rectangle(0, 0, 128, 128);

            this.Sprite = new Sprite(_assetPath, true, true);

            this.Sprite.Animation.AddAnimation("idle", 1, new byte[] { 0}, true);
            this.Sprite.Animation.AddAnimation("walk", 2, new byte[] { 1,2 }, true);
            this.Sprite.Animation.FrameWidth = 32;
            this.Sprite.Animation.CurrentAnimName = "idle";

            this.Sprite.Position = _initialPosition;
            this.Position = _initialPosition;

        }

        public override void Update(GameTime _gameTime)
        {
            float elapsedTime = (float)_gameTime.ElapsedGameTime.TotalSeconds;

            if (Active)
            {
                base.Update(_gameTime);

                UpdateJumping(elapsedTime);

                ApplyGravity(elapsedTime);

                //If there is
                if(__target == null)
                {
                    __characterDirection = Direction.Right;
                    ;
                    if (__isJumping || __isReversing || __gravityIsOn)
                    {
                        __position.X += PhysicsVars.ENEMY_HORIZONTAL_SPEED_AIR * elapsedTime;
                    }
                    else
                    {
                        this.Sprite.Animation.CurrentAnimName = "walk";
                        __position.X += PhysicsVars.ENEMY_HORIZONTAL_SPEED_GROUND * elapsedTime;

                        CheckIfOnGround();
                    }
                }

                //ANIMATION MANAGEMENT

                __destinationRectangle.X = (int)__position.X;
                __destinationRectangle.Y = (int)__position.Y;

                this.Sprite.Animation.Update(_gameTime);

                __sourceRectangle.X = this.Sprite.Animation.GetSourcePosition();
            }
        }

        private void ApplyGravity(float deltaTime)
        {
            if (__gravityIsOn)
            {
                Speed += (float)(PhysicsVars.ENEMY_JUMPING_ACCELERATION_DOWN * deltaTime);
                __position.Y += (int)(Math.Ceiling(deltaTime * Speed));
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

                    Speed -= (float)(PhysicsVars.ENEMY_JUMPING_ACCELERATION_UP * deltaTime);
                    __position.Y -= (int)(Math.Ceiling(deltaTime * Speed));
                }
                //If the player reached the highest point, set reversing mode to true
                else
                {
                    //this.Sprite.Animation.CurrentAnimName = "jump_down";
                    __isReversing = true;
                    __gravityIsOn = true;
                    __isJumping = false;
                    Speed = 0;

                }
            }

        }

        private void Jump()
        {
            if (__isJumping == false && __isReversing == false)
            {
                //this.Sprite.Animation.CurrentAnimName = "jump_up";
                __isJumping = true;
                __initialY = (int)__position.Y;
                if (__gravityIsOn)
                {
                    __gravityIsOn = false;
                }
                Speed = PhysicsVars.ENEMY_JUMPING_INITIAL_VELOCITY;

                //SoundManager.Instance.PlaySound("jump");
            }
        }

        public override void HasCollided(GameObject.Entity _type)
        {
            switch (_type)
            {
                case Entity.TileLeft:
                    Jump();
                    break;
                case Entity.TileRight:
                    Jump();
                    break;
                default:
                    break;
            }

        }
        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            if (Active)
            {
                this.Sprite.Draw(_spriteBatch, _gameTime);
                base.Draw(_spriteBatch, _gameTime);
            }
        }
    }
}
