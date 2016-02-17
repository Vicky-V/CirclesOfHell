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
    class Pickup:GameObject
    {
        public Pickup(string _assetPath, Vector2 _initialPosition) :
            base(_assetPath, _initialPosition)
        {
            this.EntityType = Entity.Pickup;

            Collidable = true;
            Interactable = true;
            IsTrigger = true;
        }

        public override void HasCollided(Entity _type)
        {
            base.HasCollided(_type);

            this.Active = false;
            this.Collidable = false;
            this.Interactable = false;
        }
    }
}
