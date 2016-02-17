using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CirclesOfHell
{
    class Bar : BaseObject
    {
        public delegate float GetValueDel();

        GetValueDel GetCurrentValue;

        private Color __fillColor;
        private Color __baseColor;

        public Color FillColor
        {
            get { return __fillColor; }
        }
        public Color BaseColor
        {
            get { return __baseColor; }
        }

        private float __maxFillRectWidth;

        private Rectangle __fillRect;//size should depend on the current value and value range
        private Rectangle __baseRect;

        private Texture2D __fillRectData;
        private Texture2D __baseRectData;

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                __baseRect.X = (int)value.X;
                __baseRect.Y = (int)value.Y;

                __fillRect.X = (int)value.X + 2;
                __fillRect.Y = (int)value.Y + 2;

                base.Position = value;//CHECK IF THIS WORKS PROPERLY

            }
        }

        private Vector2 __valueRange = new Vector2(0.0f, 1.0f);
        public Vector2 ValueRange
        {
            get { return __valueRange; }
            set
            {
                __valueRange.X = value.X;
                __valueRange.Y = value.Y;
            }
        }

        public float ValueRangeSize
        {
            get { return __valueRange.Y - __valueRange.X; }
        }

        private float __currentValue;
        public float CurrentValue
        {
            get { return __currentValue; }
            set
            {
                if (value > ValueRange.Y)
                    __currentValue = ValueRange.Y;
                else if (__currentValue < ValueRange.X)
                    __currentValue = ValueRange.X;
                else
                    __currentValue = value;

            }
        }


        private SpriteBatch __UISpriteBatch;

        public Bar(int _width, int _height, Color _fillColour, Color _baseColor, GetValueDel getCurrentValueMethod)
        {
            __maxFillRectWidth = _width-5;

            __baseRect.Width = _width;
            __baseRect.Height = _height;

            __fillRect.Width = _width - 5;
            __fillRect.Height = _height - 5;

            __fillRect.X = __baseRect.X + 2;
            __fillRect.Y = __baseRect.Y + 2;

            __fillColor = _fillColour;
            __baseColor = _baseColor;

            this.Active = true;

            GetCurrentValue = new GetValueDel(getCurrentValueMethod);
        }

        public override void LoadContent(GraphicsDevice _graphicsDevice)
        {
            __fillRectData = new Texture2D(_graphicsDevice, __fillRect.Width, __fillRect.Height);

            Color[] data = new Color[__fillRect.Width * __fillRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = FillColor;
            __fillRectData.SetData(data);

            __baseRectData = new Texture2D(_graphicsDevice, __baseRect.Width, __baseRect.Height);

            data = new Color[__baseRect.Width * __baseRect.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = BaseColor;
            __baseRectData.SetData(data);

            __UISpriteBatch = new SpriteBatch(_graphicsDevice);

        }

        public override void Update(GameTime _gameTime)
        {
            //Should update the current value based on the variable it's "connected to" (if possible)
            CurrentValue = GetCurrentValue();
            //Updates the width of the rectangle based on the current value
            __fillRect.Width = (int)(CurrentValue / ValueRangeSize * __maxFillRectWidth);

        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            __UISpriteBatch.Begin();

            __UISpriteBatch.Draw(__baseRectData, __baseRect, Color.White);
            __UISpriteBatch.Draw(__fillRectData, __fillRect, Color.White);

            __UISpriteBatch.End();
        }

    }
}
