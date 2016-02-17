using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CirclesOfHell
{
    class GameEntityManager
    {
        private List<Vector2> __entityPositions = new List<Vector2>();

        protected List<GameObject> __entityList = new List<GameObject>();

        public GameEntityManager()
        {

        }
        virtual public void AddEntity(GameObject _ent)
        {
            __entityList.Add(_ent);
        }
        virtual public void RemoveEntity(GameObject _ent)
        {
            __entityList.Remove(_ent);
        }
        virtual public void LoadContent(GraphicsDevice _gd)
        {
            for (int i = 0; i < __entityList.Count; i++)
            {
                __entityList[i].LoadContent(_gd);
                foreach(BaseObject child in __entityList[i].Children)
                {
                    child.LoadContent(_gd);
                }
            }
        }
        public void CheckTileMapCollision(object _sender, CollisionArgs _args)
        {
            for (int i = 0; i < __entityList.Count; i++)
            {
                if (__entityList[i].Collidable)
                    __entityList[i].CheckTileMapCollision(_sender, _args);

            }
        }
        public void CheckHeroAgainstEntityCollision(object _sender, CollisionArgs _args)
        {
            LevelScene Scene = (LevelScene)_sender;
            Vector2 playerPos = Scene.Player.Position;
            Rectangle collisionCheckRect = new Rectangle((int)(playerPos.X - 256), (int)(playerPos.Y - 256), 128 * 5, 128 * 5);

            for (int i = 0; i < __entityList.Count; i++)
            {
                if (__entityList[i].Collidable)
                {
                    if((int)__entityList[i].EntityType!=1)
                    {
                         //Check if the entity's position is in the player's range
                        Vector2 entityPos = __entityList[i].Position;

                        if(entityPos.X>collisionCheckRect.X && entityPos.X<(collisionCheckRect.X+collisionCheckRect.Width) &&
                            entityPos.Y > collisionCheckRect.Y && entityPos.Y < (collisionCheckRect.Y + collisionCheckRect.Height))
                        {

                            //If it is, get the collision rect and check if it's empty
                            Rectangle intersection=__entityList[i].CollisionRect(Scene.Player);

                            if(intersection!=Rectangle.Empty)
                            {
                                //If the rect is not empty, fire an event for both the entity and the player
                                __entityList[i].HasCollided(__entityList[i].EntityType);
                                Scene.Player.HasCollided(__entityList[i].EntityType);

                                //Handle collision if the object is not a trigger
                                if(__entityList[i].IsTrigger == false)
                                    Scene.Player.HandleCollision(intersection, Scene.Player.Position.X);
                            }

                        }
                    }
                }

            }
        }
        virtual public void Update(GameTime _gameTime)
        {
            for (int i = 0; i < __entityList.Count; i++)
            {
                __entityList[i].Update(_gameTime);

                foreach (BaseObject child in __entityList[i].Children)
                {
                    if (child.Active)
                    {
                        child.Update(_gameTime);
                    }
                }
            }
        }
        virtual public void Draw(SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            for (int i = 0; i < __entityList.Count; i++)
            {
                __entityList[i].Draw(_spriteBatch, _gameTime);

                foreach (BaseObject child in __entityList[i].Children)
                {
                    if (child.Active)
                    {
                        child.Draw(_spriteBatch, _gameTime);
                    }
                }
            }
        }

        virtual public GameObject GetEntityAtIndex(int _index)
        {
            return __entityList[_index];
        }
        virtual public int GetNumberOfObjects()
        {
            return __entityList.Count;
        }

    }
}
