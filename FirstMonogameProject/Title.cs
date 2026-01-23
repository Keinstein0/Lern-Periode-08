using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonogameProject
{
    internal class Title
    {
        private int _highScore = 0;
        private int _resetCooldown = 0;

        SpriteFont _font;


        //Buttons
        private Button _startGame;


        public Title(Button startGame, SpriteFont font)
        {
            _startGame = startGame;
            _font = font;
        }


        public void UpdateData(int newScore)
        {
            if (newScore > _highScore)
            {
                _highScore = newScore;
            }
        }

        public void Update(GameTime gameTime, ref bool gameOnTitleScreen)
        {
            bool returns = _startGame.Update(gameTime);

            if (!returns)
            {
                _resetCooldown = 50;
                _startGame.Drop();
            }

            if (_resetCooldown > 0) // if is in the process of ending
            {
                _resetCooldown--;
                if (_resetCooldown == 0)
                {
                    gameOnTitleScreen = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _startGame.Draw(spriteBatch);

            spriteBatch.DrawString(_font, $"High Score: {_highScore.ToString()}", new Vector2(10,450), Color.White);
        }


        public void Reset()
        {
            _startGame.Reset();
        }
    }
}
