using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CirclesOfHell
{
    class BackgroundManager
    {
        List<BackgroundLayer> backgroundLayers = new List<BackgroundLayer>();

        public void AddLayer(BackgroundLayer _layer)
        {
            backgroundLayers.Add(_layer);
        }

        public void Draw(GraphicsDevice _gd,SpriteBatch _spriteBatch, Camera _camera, GameTime _gameTime)
        {
            for (int i = 0; i < backgroundLayers.Count; i++)
            {
                backgroundLayers[i].Draw(_gd, _spriteBatch, _camera, _gameTime);
            }
        }   
    }
}
