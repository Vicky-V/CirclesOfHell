using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CirclesOfHell
{
    class LevelScene:Scene
    {
        public EventHandler<CollisionArgs> CheckLevelCollision;

        Pickup __heart;
        public Pickup Heart
        {
            get { return __heart; }
            set { __heart = value; }
        }

        Hero __player;
        public Hero Player
        {
            get { return __player; }
            set { __player = value; }
        }

        Enemy __demon;
        public Enemy Demon
        {
            get { return __demon; }
            set { __demon = value; }
        }

        TileMap __level;
        public TileMap Level
        {
            get { return __level; }
            set { __level = value; }
        }

        Bar __energyBar;
        public Bar EnergyBar
        {
            get { return __energyBar; }
            set { __energyBar = value; }
        }

        Bar __healthBar;
        public Bar HealthBar
        {
            get { return __healthBar; }
            set { __healthBar = value; }
        }

        GameEntityManager __gameEntityManager;
        BackgroundManager __backgroundManager;
        //ProjectileManager __projectileManager;

        public LevelScene(bool _isActive, string _sceneID, GraphicsDevice _gd):
            base(_isActive, _sceneID,_gd)
        {
            Player = new Hero("Content/hero.png", new Vector2(600, 230));
            Heart = new Pickup("Content/Hearts.png", new Vector2(1000, 600));
            Demon = new Enemy("Content/Demon.png", new Vector2(300, 650));

            Level = new TileMap("Content/level.txt", "Content/Tiles.png");
            Vector2 position = new Vector2(135, 22);

            __energyBar = new Bar(100, 20, Color.Black, Color.White, Player.GetEnergyValueDel);
            __energyBar.Position = new Vector2(50, 50);
            __energyBar.Active = true;
            __energyBar.ValueRange = new Vector2(0, 100);

            __healthBar = new Bar(100, 20, Color.Coral, Color.White, Player.GetHealthValueDel);
            __healthBar.Position = new Vector2(50, 25);
            __healthBar.Active = true;
            __healthBar.ValueRange = new Vector2(0, 100);

            __camera = new Camera(_gd.Viewport);
            __position = __camera.Origin;
            __camera.Limits = new Rectangle(0, 0, 128 * 128, 128 * 7 + 64);

            __backgroundManager = new BackgroundManager();
            
            __gameEntityManager = new GameEntityManager();
            __gameEntityManager.AddEntity(Heart);
            __gameEntityManager.AddEntity(Demon);
            __gameEntityManager.AddEntity(Player);

            //__projectileManager = new ProjectileManager(25, 0.4f);


            CheckLevelCollision += __gameEntityManager.CheckTileMapCollision;
            CheckLevelCollision += __gameEntityManager.CheckHeroAgainstEntityCollision;

        }

        public override void LoadContent(GraphicsDevice _gd,ContentManager _content)
        {

            Level.LoadContent(_gd);

            Player.LoadContent(_gd);
            Player.LoadParticles(_content);

            Player.WorldBounds = Level.GetMapRectangle();

            Heart.LoadContent(_gd);

            Demon.LoadContent(_gd);

            EnergyBar.LoadContent(_gd);
            HealthBar.LoadContent(_gd);


            SoundManager.Instance.LoadSound("jump", "jump", _content);
            SoundManager.Instance.LoadSound("shooting", "shooting", _content);
            SoundManager.Instance.LoadSong("music", _content);
            //SoundManager.Instance.PlaySong();

            //__projectileManager.LoadContent(_gd);

            BackgroundLayer layer1 = new BackgroundLayer();
            layer1.Parallax = new Vector2(0.5f, 0.5f);
            BackgroundLayer layer2 = new BackgroundLayer();
            BackgroundLayer layer3 = new BackgroundLayer();

            layer1.AddSprite(_content, "Content/bg_layer.png");
            layer1.LoadContent(_gd);
            __backgroundManager.AddLayer(layer1);


            base.LoadContent(_gd, _content);
        }

        public override void Update(GameTime _gameTime)
        {
            __gameEntityManager.Update(_gameTime);
            CheckLevelCollision(this, new CollisionArgs(Level.Tiles));

            __camera.LookAt(new Vector2(__player.Position.X - 100, __player.Position.Y));

            EnergyBar.Update(_gameTime);
            HealthBar.Update(_gameTime);

            base.Update(_gameTime);
        }

        public override void Draw(GraphicsDevice _gd,SpriteBatch _spriteBatch, GameTime _gameTime)
        {

            __backgroundManager.Draw(_gd, _spriteBatch, __camera, _gameTime);

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, __camera.ViewMatrix);
            Level.Draw(_spriteBatch, _gameTime);

            __gameEntityManager.Draw(_spriteBatch, _gameTime);
            //__projectileManager.Draw(_spriteBatch, _gameTime);

            _spriteBatch.End();

            EnergyBar.Draw(_spriteBatch, _gameTime);
            HealthBar.Draw(_spriteBatch, _gameTime);

            base.Draw(_gd,_spriteBatch, _gameTime);
        }
   }
}
