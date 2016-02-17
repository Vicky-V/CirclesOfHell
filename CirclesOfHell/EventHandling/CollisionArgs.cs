using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CirclesOfHell
{
    public class CollisionArgs:EventArgs
    {
        public int[,] Tiles;
        public Rectangle EntityBounds;

        //for tilemap collision
        public CollisionArgs(int[,] _levelTiles)
        {
            Tiles = _levelTiles;
        }

        //for collision against another entity
        public CollisionArgs(Rectangle _otherEntityBounds)
        {
            EntityBounds = _otherEntityBounds;
        }
    }
}
