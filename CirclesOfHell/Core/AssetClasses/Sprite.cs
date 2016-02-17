using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace CirclesOfHell
{
    class Sprite : BaseObject
    {
        public bool IsUsingSpriteSheet = false;
        public bool IsAnimated = false;

        public Direction Orientation = Direction.Right;

        public Sprite(string _path, bool _isAnimated, bool _isUsingSpriteSheet)
        {
            ImagePath = _path;
            __position = new Vector2(0, 0);
            IsAnimated = _isAnimated;
            IsUsingSpriteSheet = _isUsingSpriteSheet;

            if (IsAnimated)
                Animation = new Animations();
        }

        public Sprite(string _path, Vector2 _initialPosition, bool _isAnimated,bool _isUsingSpriteSheet)
        {
            ImagePath = _path;
            __position = _initialPosition;
            IsAnimated = _isAnimated;
            IsUsingSpriteSheet = _isUsingSpriteSheet;

            if (IsAnimated)
                Animation = new Animations();
        }

        protected string __assetPath;
        public string ImagePath
        {
            set { __assetPath = value; }
        }

        protected Texture2D __texture;
        public Texture2D Texture
        {
            get { return __texture; }
            set { __texture = value; }
        }

        protected Animations __animations;
        public Animations Animation
        {
            get { return __animations; }
            set { __animations = value; }
        }


        protected int __frameWidth;
        protected int __width=-1;
        public int Width
        {
            get
            {
                if (IsAnimated || IsUsingSpriteSheet)
                    return __frameWidth;
                else if (__width == -1)
                    return __texture.Width;
                else
                    return __width;
            }
            set
            {
                if (!IsAnimated || !IsUsingSpriteSheet)
                    __width = value;
            }
        }

        protected int __frameHeight;
        protected int __height = -1;
        public int Height
        {
            get
            {
                if (IsAnimated || IsUsingSpriteSheet)
                    return __frameHeight;
                else if (__height == -1)
                    return __texture.Height;
                else
                    return __height;
            }
            set
            {
                if (!IsAnimated || !IsUsingSpriteSheet)
                    __height = value;
            }
        }

        public void SetFrameDimensions(int _width, int _height)
        {
            __frameWidth = _width;
            __frameHeight = _height;
        }

        protected int __sourceOffset;
        public int SourceHorizontalOffset
        {
            get { return __sourceOffset / __frameWidth; }
            set { __sourceOffset = value * __frameWidth; }
        }

        public override void LoadContent(GraphicsDevice _graphicsDevice)
        {
            FileStream fileStream = new FileStream(__assetPath, FileMode.Open);
            __texture = Texture2D.FromStream(_graphicsDevice, fileStream);
            fileStream.Close();
        }

        public override void Update(GameTime _gameTime)
        {
            if (this.IsAnimated)
                this.__animations.Update(_gameTime);
        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            SpriteEffects flipEffect;

            if (Orientation == Direction.Right)
                flipEffect = SpriteEffects.None;
            else
                flipEffect = SpriteEffects.FlipHorizontally;

            if (this.IsAnimated == false && this.IsUsingSpriteSheet == false)
                _spriteBatch.Draw(this.Texture, this.Position, new Rectangle(0, 0, this.Width, this.Height), Color.White, 0.0f, this.Origin, new Vector2(1.0f, 1.0f), flipEffect, 0);
            else if (IsAnimated && IsUsingSpriteSheet)
                _spriteBatch.Draw(this.Texture, this.Position, new Rectangle(this.Animation.GetSourcePosition(), 0, this.Width, this.Height), Color.White, 0.0f, this.Origin, new Vector2(1.0f, 1.0f), flipEffect, 0);
            else if (IsAnimated == false)
                _spriteBatch.Draw(this.Texture, this.Position, new Rectangle(this.__sourceOffset, 0, this.Width, this.Height), Color.White, 0.0f, this.Origin, new Vector2(1.0f, 1.0f), flipEffect, 0);

        }
    }
}
