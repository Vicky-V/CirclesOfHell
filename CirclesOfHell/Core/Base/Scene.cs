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
    class Scene
    {
        protected string __sceneID;
        public string SceneID
        {
            get { return __sceneID; }
            set { __sceneID = value; }
        }

        protected bool __active = false;
        public bool Active
        {
            set { __active = value; }
            get { return __active; }
        }

        protected GameEntityManager __sceneObjectsManager;
        public GameEntityManager SceneObjectsManager
        {
            get { return __sceneObjectsManager; }
            set { __sceneObjectsManager = value; }
        }

        protected Camera __camera;

        protected Vector2 __position;

        public Scene(bool _isActive, string _sceneID, GraphicsDevice _gd)
        {
            __active = _isActive;
            __sceneID = _sceneID;

            __camera = new Camera(_gd.Viewport);
            __position = __camera.Origin;

            __sceneObjectsManager = new GameEntityManager();

        }

        virtual public void LoadContent(GraphicsDevice _gd, ContentManager _content)
        {
            __camera.Limits = new Rectangle(0, 0, 128 * 128, 128 * 7);
            __sceneObjectsManager.LoadContent(_gd);

        }

        virtual public void Update(GameTime _gameTime)
        {
            //Update game entity manager
            __sceneObjectsManager.Update(_gameTime);

            __camera.Update(_gameTime);
        }

        virtual public void Draw(GraphicsDevice _gd, SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            //GraphicsDevice.Clear(Color.Purple);

            //Draw objects via game entity manager
            __sceneObjectsManager.Draw(_spriteBatch, _gameTime);
        }

    }
}
