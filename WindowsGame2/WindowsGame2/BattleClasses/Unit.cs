using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.BattleClasses
{
    class Unit
    {
        private float x_pos { get; set; }
        private float y_pos { get; set; }
        private float facing { get; set; }
        private Texture2D texture { get; set; }
        private Color alpha { get; set; }
        private float turnSpeed { get; set; }

        public Unit(float x, float y)
        {
            x_pos = x;
            y_pos = y;

            turnSpeed = (float) Math.PI/2;
        }

        public Unit(float x, float y, float face)
            : this(x, y)
        {
            facing = face;
        }

        public void setTexture(Texture2D tex, Color alph)
        {
            texture = tex;
            alpha = alph;
        }

        public void turnDirection(bool right, float percentSecond)
        {
            float turn = turnSpeed * percentSecond;
            if (right)
            {
                turn *= -1;
            }

            facing += turn;

            if (facing > Math.PI*2)
            {
                facing -= (float)Math.PI*2;
            }

            else if (facing < 0)
            {
                facing += (float)Math.PI*2;
            }
        }

        public void DRAW(SpriteBatch sb)
        {
            sb.Draw(texture, new Rectangle((int)x_pos, (int)y_pos,20,20), null, alpha, facing, 
                new Vector2(x_pos,y_pos), SpriteEffects.None, 0f);
        }
    }
}
