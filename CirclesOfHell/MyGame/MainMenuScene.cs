using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CirclesOfHell
{
    class MainMenuScene:Menu
    {
        public MainMenuScene(bool _isActive,string _sceneID,GraphicsDevice _gd):
            base(_isActive,_sceneID,_gd)
        {
            Button newGame = new Button("Content/Start.png");
            newGame.Position = new Vector2(400, 200);
            newGame.ButtonPressed += NextScene;
            AddButton(newGame);
            __camera.LookAt(newGame.Position);
        }


    }
}
