using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CirclesOfHell
{
    sealed class SceneManager
    {
        private static SceneManager __instance = new SceneManager();
        public static SceneManager Instance
        {
            get
            {
                return __instance;
            }
        }
        private SceneManager()
        { }

        List<Scene> __scenesList = new List<Scene>();

        public List<Scene> Scenes
        {
            get { return __scenesList; }
        }
        public int ScenesToPop = 0;

        public void PushScene(Scene _scene, bool _active)
        {
            _scene.Active = _active;
            this.Scenes.Add(_scene);
        }

        public void PopScenes(int _popAmount)
        {
            __scenesList.RemoveRange(__scenesList.Count-_popAmount, _popAmount);
            ScenesToPop = 0;

            Scenes[Scenes.Count - 1].Active = true;
            
        }

        public void SetActiveScene(string _sceneID, GraphicsDevice _gd,ContentManager _content)
        {
            foreach(Scene scene in Scenes)
            {
                scene.Active = false;
                if (scene.SceneID == _sceneID)
                    scene.Active = true;
            }

        }

        public void LoadContent(GraphicsDevice _graphicsDevice, ContentManager _content)
        {
           //Load active scene
            foreach(Scene scene in Scenes)
            {
              scene.LoadContent(_graphicsDevice,_content);
            }
        }

        public void Update(GameTime _gameTime)
        {
           //Update active scenes
            foreach (Scene scene in Scenes)
            {
                if (scene.Active)
                    scene.Update(_gameTime);
            }
            if (ScenesToPop > 0)
                PopScenes(ScenesToPop);
            
        }

        public void Draw(GraphicsDevice _gd,SpriteBatch _spriteBatch, GameTime _gameTime)
        {
           //Draw active scenes
            foreach(Scene scene in Scenes)
            {
                if (scene.Active)
                    scene.Draw(_gd,_spriteBatch, _gameTime);
            }
        }
    }
}
