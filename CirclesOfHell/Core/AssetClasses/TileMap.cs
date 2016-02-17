using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace CirclesOfHell
{
    class TileMap
    {
        public const int TILE_HEIGHT=128;
        public const int TILE_WIDTH = 128;

        Texture2D __tileSet;
        string __tileFile; // string name for the file that has the level, level.txt
        string __tileSprite; // string name for the file that has the art of the tiles, temptiles.png
        int[,] __tilePosition; // array the hold the information for the level

        //the source rectangle is where the rectangle that we're sampling
        //our drawable image from the source file
        Rectangle __sourceRectangle = new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT);

        //the destination rectangle is the location we will draw our rectangle to.
        Rectangle __destinationRectangle = new Rectangle(0, 0, TILE_WIDTH, TILE_HEIGHT);

        public TileMap()
        {}

        public TileMap(string _tileFile, string _tileSprite)
        {
            __tileFile = _tileFile;
            __tileSprite = _tileSprite;
        }

        public int[,] Tiles
        {
            get { return __tilePosition; }
        }

        public Rectangle GetMapRectangle()
        {
            return new Rectangle(0, 0, __tilePosition.GetLength(1) * TILE_WIDTH, __tilePosition.GetLength(0) * TILE_HEIGHT);
        }

        public int GetIndexForTileXYCoordinates(float x, float y)
        {
            return (int)(Math.Floor(x/128) + Math.Floor(y/128) * __tilePosition.GetLength(1));
        }

        public Vector2 GetTileCoordinatesForXY(float x, float y)
        {
            int index = GetIndexForTileXYCoordinates(x, y);
            return new Vector2(index % __tilePosition.GetLength(1), index / __tilePosition.GetLength(1));
        }

        public void LoadContent(GraphicsDevice _graphics)
        {
            string[] mapData = File.ReadAllLines(__tileFile);
            int mapWidth = mapData[0].Length;
            int mapHeight = mapData.Length;
            __tilePosition = new int[mapHeight, mapWidth];

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    __tilePosition[i, j] = int.Parse(mapData[i][j].ToString());
                }
            }
            FileStream fileStream = new FileStream(__tileSprite, FileMode.Open);
            __tileSet = Texture2D.FromStream(_graphics, fileStream);
            fileStream.Close();
        }

        public void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            for (int i = 0; i < __tilePosition.GetLength(0); i++)
            {
                for (int j = 0; j < __tilePosition.GetLength(1); j++)
                {
                    if (__tilePosition[i, j] != 0)
                    {
                        __sourceRectangle.X = __tilePosition[i, j] * TILE_WIDTH;
                        __destinationRectangle.X = j * TILE_WIDTH;
                        __destinationRectangle.Y = i * TILE_HEIGHT;
                        _spriteBatch.Draw(__tileSet, __destinationRectangle, __sourceRectangle, Color.White);
                    }
                }
            }
        }
    }
}
