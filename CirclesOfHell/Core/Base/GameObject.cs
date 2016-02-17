using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace CirclesOfHell
{
    class GameObject:BaseObject
    {
        public enum Entity { Blank=0, Player, NPC, Enemy, Pickup, TileLeft, TileRight, TileTop, TileBottom };

        public Entity EntityType = Entity.Blank;

        protected const float DEFAULT_SPEED_GROUND = 200.0f;
        
        protected Sprite __sprite;
        protected string __assetPath;

        protected bool Interactable = false;
        
        #region Children
        protected List<BaseObject> __children = new List<BaseObject>();

        public void AddChild(BaseObject _child)
        {
            __children.Add(_child);
        }

        public void RemoveChild(BaseObject _child)
        {
            foreach (BaseObject child in __children)
            {
                if (child.Equals(_child))
                {
                    __children.Remove(child);
                }
            }
        }

        public List<BaseObject> Children
        {
            get { return __children; }
        }
        #endregion

        #region Collision Variables
        protected Rectangle __boundingBox;
        public bool Collidable;
        public bool IsTrigger;
        public virtual Rectangle Bounds //returns the bounds of the sprite by default, should be overridden for classes which use spritesheets
        {
            get { return __boundingBox; }
            protected set { __boundingBox = value; __position.X = value.X; __position.Y = value.Y; }
        }
        #endregion

        #region Physics Varibales
        protected bool __isJumping = false;
        protected bool __isReversing = false;
        protected int __initialY = 600;
        protected bool __gravityIsOn = true;
        protected Vector2 __groundBounds = new Vector2(-1, -1);//X is for left corner X position of the current platform, Y is for the right corner X pos
        protected float __speed = DEFAULT_SPEED_GROUND;

        public float Speed
        {
            set { __speed = value; }
            get { return __speed; }
        }
        #endregion

       
        public Sprite Sprite
        {
            get { return __sprite; }
            set { __sprite = value; }
        }

        #region Constructors
        public GameObject()
        {
            EntityType = 0;
        }
        public GameObject(string _assetPath, Vector2 _initialPosition, Entity _type)
        {
            EntityType = _type;
            __sprite = new Sprite(_assetPath,_initialPosition,false,false);
            __sprite.Position = _initialPosition;
            __position = _initialPosition;
            __origin = new Vector2(__position.X + (__boundingBox.Width / 2),
                                   __position.Y + (__boundingBox.Height / 2));
            __boundingBox = new Rectangle((int)_initialPosition.X, (int)_initialPosition.Y, 0, 0);
        }

        public GameObject(string _assetPath, Vector2 _initialPosition)
        {
            
            __sprite = new Sprite(_assetPath, _initialPosition, false, false);
            __sprite.Position = _initialPosition;
            __position = _initialPosition;
            __origin = new Vector2(__position.X + (__boundingBox.Width / 2),
                                   __position.Y + (__boundingBox.Height / 2));
            __boundingBox = new Rectangle((int)_initialPosition.X, (int)_initialPosition.Y,0,0);
        }
        #endregion

        #region Collision
        public virtual void HasCollided(Entity _type)
        {
            //Override this method to add actions that occur after collision
        }
        public void CheckTileMapCollision(object _sender,CollisionArgs _args)
        {
            //ONLY CHECKS A SMALL PORTION OF THE WORLD FOR COLLISION
            //NEEDS TESTING
            if (_sender is LevelScene)
            {
                LevelScene lvl = (LevelScene)_sender;
                Vector2 position = this.Position;
                Rectangle collisionCheckRect = new Rectangle((int)(position.X - 256), (int)(position.Y - 256), 128 * 5, 128 * 5);
               

                for (int i = 0; i < collisionCheckRect.Height; i += 128)
                {
                    for (int j = 0; j < collisionCheckRect.Width; j += 128)
                    {
                        Vector2 tileCoord = lvl.Level.GetTileCoordinatesForXY(j + collisionCheckRect.X, i + collisionCheckRect.Y);

                        //if (tileCoord.X < 0)
                        //    tileCoord.X = 0;
                        //if (tileCoord.X >= )
                        //    tileCoord.X = _args.Tiles.GetLength(1) - 1;
                        
                        //if (tileCoord.Y < 0)
                        //    tileCoord.Y = 0;
                        //if (tileCoord.Y >= _args.Tiles.GetLength(0))
                        //    tileCoord.Y = _args.Tiles.GetLength(0) - 1;

                        tileCoord.X = MathHelper.Clamp(tileCoord.X, 0, _args.Tiles.GetLength(1)-1);
                        tileCoord.Y = MathHelper.Clamp(tileCoord.Y, 0, _args.Tiles.GetLength(0)-1);


                        if (_args.Tiles[(int)tileCoord.Y, (int)tileCoord.X] != 0)
                        {
                            Rectangle intersection = CollisionRect((int)tileCoord.Y, (int)tileCoord.X);
                            if (intersection != Rectangle.Empty)
                            {
                                HandleCollision(intersection, (int)tileCoord.X);
                            }
                        }
                    }
                }
            }
            
            //CHECKS EVERYTHING
            //for (int i = 0; i < _args.Tiles.GetLength(0); i++)
            //{
            //    for (int j = 0; j < _args.Tiles.GetLength(1); j++)
            //    {
            //        if (_args.Tiles[i, j] != 0)
            //        {
            //            Rectangle intersection = CollisionRect(i, j);
            //            if (intersection != Rectangle.Empty)
            //            {
            //                HandleCollision(intersection, j);
            //            }
            //        }

            //    }
            //}
            
        }
        private void HandleCollision(Rectangle intersection, int tileHorizontalCoordinate)
        {
            //Bottom Tile collision
            if ((intersection.Y == Bounds.Y) && (intersection.Height < intersection.Width))
            {
                Position = new Vector2(Position.X, Position.Y + intersection.Height);
                Speed = -1;

                this.HasCollided(Entity.TileBottom);
            }
            //Top Tile collision
            else if ((intersection.Height < intersection.Width))
            {
                Position = new Vector2(Position.X, Position.Y - intersection.Height);
                __gravityIsOn = false;
                __isJumping = false;
                __isReversing = false;
                __groundBounds = new Vector2(tileHorizontalCoordinate * 128, (tileHorizontalCoordinate + 1) * 128);

                this.HasCollided(Entity.TileTop);
            }
            //Right Tile collision
            else if ((intersection.X + intersection.Width) == (Bounds.X + Bounds.Width))
            {
                Position = new Vector2(Position.X - intersection.Width, Position.Y);

                this.HasCollided(Entity.TileRight);
            }
            //Left Tile collision
            else if (intersection.X == Bounds.X)
            {
                Position = new Vector2(Position.X + intersection.Width, Position.Y);

                this.HasCollided(Entity.TileLeft);
            }
        }

        public void HandleCollision(Rectangle intersection, float horizontalCoordinate)
        {
            if ((intersection.Y == Bounds.Y) && (intersection.Height < intersection.Width))
            {
                Position = new Vector2(Position.X, Position.Y + intersection.Height);
                Speed = -1;
            }
            else if ((intersection.Height < intersection.Width))
            {
                Position = new Vector2(Position.X, Position.Y - intersection.Height);
                __gravityIsOn = false;
                __isJumping = false;
                __isReversing = false;
                __groundBounds = new Vector2(horizontalCoordinate, horizontalCoordinate + 128);
            }
            else if ((intersection.X + intersection.Width) == (Bounds.X + Bounds.Width))
            {
                Position = new Vector2(Position.X - intersection.Width, Position.Y);
            }
            else if (intersection.X == Bounds.X)
            {
                Position = new Vector2(Position.X + intersection.Width, Position.Y);
            }
        }
        public Rectangle CollisionRect(int i, int j)//collision check against tilemap
        {

            Rectangle collidableTile = new Rectangle(j * 128, i * 128, 128, 128);
            if (collidableTile.Intersects(this.Bounds))
            {
                //Determine the left side of the rectangles Bounds
                int x1 = Math.Max(collidableTile.X, this.Bounds.X);
                //Determine the right side of the Rectangle bounds
                int x2 = Math.Min(collidableTile.X + collidableTile.Width, this.Bounds.X + this.Bounds.Width);
                //Determine the top of the rectangle bounds
                int y1 = Math.Max(collidableTile.Y, this.Bounds.Y);
                //Determine the bottom of the rectangle bounds
                int y2 = Math.Min(collidableTile.Y + collidableTile.Height, this.Bounds.Y + this.Bounds.Height);
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }

            else
                return new Rectangle();
        }

        public Rectangle CollisionRect(GameObject _otherEntity)//regular collision check
        {
            if (this.Bounds.Intersects(_otherEntity.Bounds))
            {
                //Determine the left side of the rectangles Bounds
                int x1 = Math.Max(this.Bounds.X, (int)_otherEntity.Bounds.X);
                //Determine the right side of the Rectangle bounds
                int x2 = Math.Min(this.Bounds.X + this.Bounds.Width, (int)_otherEntity.Bounds.X + _otherEntity.Bounds.Width);
                //Determine the top of the rectangle bounds
                int y1 = Math.Max(this.Bounds.Y, _otherEntity.Bounds.Y);
                //Determine the bottom of the rectangle bounds
                int y2 = Math.Min(this.Bounds.Y + this.Bounds.Height, (int)_otherEntity.Bounds.Y + _otherEntity.Bounds.Height);
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }
            else
            {
                return new Rectangle();
            }
        }
        #endregion

        #region BasicFunctions

        public override void LoadContent(GraphicsDevice _graphicsDevice)
        {
            this.Sprite.LoadContent(_graphicsDevice);
            this.__boundingBox.Width = this.__sprite.Width;
            this.__boundingBox.Height = this.__sprite.Height;

        }

        public override void Update(GameTime _gameTime)
        {
            if (this.__active)
            {
                this.__boundingBox.X = (int)this.__position.X;
                this.__boundingBox.Y = (int)this.__position.Y;

                this.__origin = new Vector2(this.__position.X + (this.__boundingBox.Width / 2),
                                       this.__position.Y + (this.__boundingBox.Height / 2));
                this.Sprite.Update(_gameTime);
            }
        }
        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            
            if (this.__active)
                this.Sprite.Draw(_spriteBatch, _gameTime);
        }

        #endregion 
    }
}
