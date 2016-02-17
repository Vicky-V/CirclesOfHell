using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CirclesOfHell
{
    class BackgroundLayer : BaseObject
    {
        public Vector2 Parallax { get; set; }
        public List<Sprite> Sprites { get; private set; }

        public BackgroundLayer()
        {
            Parallax = Vector2.One;
            Sprites = new List<Sprite>();
        }

        public void AddSprite(ContentManager _content, string _assetPath)
        {
            Sprite sprite = new Sprite(_assetPath,false,false);
            sprite.Position = new Vector2(0.0f, 0.0f);
            sprite.Width = 128*128;
            Sprites.Add(sprite);
        }

        public override void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            
        }
        public void Draw(GraphicsDevice _gd, SpriteBatch _spriteBatch, Camera _camera, GameTime _gameTime)
        {
            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null,_camera.GetParallaxViewMatrix(Parallax));

            _gd.SamplerStates[0].AddressU = TextureAddressMode.Wrap;
            _gd.SamplerStates[0].AddressV = TextureAddressMode.Wrap;

            foreach (Sprite sprite in Sprites)
            {
                sprite.Draw(_spriteBatch,_gameTime);
            }

            _spriteBatch.End();

        }

        public override void LoadContent(GraphicsDevice _graphicsDevice)
        {
            foreach (Sprite sprite in Sprites)
            {
                sprite.LoadContent(_graphicsDevice);
            }
        }

        public override void Update(GameTime _gameTime)
        {

        }

    }
}
