using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CirclesOfHell
{
    class Menu : Scene
    {
        List<Button> __buttonList;

        public Menu(bool _isActive, string _sceneID, GraphicsDevice _gd):
            base(_isActive,_sceneID, _gd)
        {
            __buttonList = new List<Button>();
        }

        public void AddButton(Button _button)
        {
            __buttonList.Add(_button);
        }

        public void NextScene(object sender, EventArgs e)
        {
            SceneManager.Instance.ScenesToPop = 1;
        }

        public override void LoadContent(GraphicsDevice _gd, ContentManager _content)
        {
            base.LoadContent(_gd, _content);
            foreach(Button button in __buttonList)
            {
                button.LoadContent(_gd);
            }

        }

        public override void Update(GameTime _gameTime)
        {
            base.Update(_gameTime);
            foreach (Button button in __buttonList)
            {
                button.Update(_gameTime);
            }

        }

        public override void Draw(GraphicsDevice _gd, SpriteBatch _spriteBatch, GameTime _gameTime)
        {
            _spriteBatch.Begin();
            base.Draw(_gd, _spriteBatch, _gameTime);
            foreach (Button button in __buttonList)
            {
                button.Draw(_spriteBatch,_gameTime);
            }
            _spriteBatch.End();
        }
    }
}
