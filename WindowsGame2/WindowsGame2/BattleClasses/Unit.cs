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

        protected int x_dim { get; set; }
        protected int y_dim { get; set; }

        Dictionary<String, int> stats;
        private float turnSpeed { get; set; }

        public Unit(float x, float y)
        {
            x_pos = x;
            y_pos = y;

            x_dim = 40;
            y_dim = 40;

            stats = new Dictionary<String, int>();
        }

        public Unit(float x, float y, float face)
            : this(x, y)
        {
            facing = face;
        }

        public void setStats(int hp, int fp, int pow, int ski, int fin, int blo, int end, int spe, int lvl, int exp, int turn)
        {
            stats.Add("HP", hp);
            stats.Add("MHP", hp);
            stats.Add("FP", fp);
            stats.Add("MFP",fp);
            stats.Add("POWER",pow);
            stats.Add("SKILL",ski);
            stats.Add("FINESSE",fin);
            stats.Add("BLOCK",blo);
            stats.Add("ENDURE",end);
            stats.Add("SPEED",spe);
            stats.Add("LEVEL",lvl);
            stats.Add("EXP",exp);
            stats.Add("TURN",turn);

            turnSpeed = ((float) turn) / 1000.0f;
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
        
        public void moveDirection(bool forwards, float percentSecond)
        {
            float BACKWARDS_MODIFIER = -0.7f;
            if ( forwards )
                BACKWARDS_MODIFIER = 1.0f;

            // Movement Calculation is Here, obviously will require tweaking
            y_pos += ((float)Math.Sin((double)facing) * stats["SPEED"]) * percentSecond * BACKWARDS_MODIFIER;
            x_pos += ((float)Math.Cos((double)facing) * stats["SPEED"]) * percentSecond * BACKWARDS_MODIFIER;
        }

        public void DRAW(SpriteBatch sb)
        {
            sb.Draw(texture, new Vector2(x_pos, y_pos), null, alpha, facing, new Vector2(x_dim/2, y_dim/2), new Vector2(1, 1), SpriteEffects.None, 0.0f);
        }

        public int getHP()
        {
            return stats["HP"];
        }

        public int getFP()
        {
            return stats["FP"];
        }

        public void TESTCHANGER()
        {
            Random ran = new Random();
            if (2 > ran.Next(100))
            {
                stats["HP"] = ran.Next(stats["MHP"]);
                stats["FP"] = ran.Next(stats["MFP"]);
            }

        }
    }
}
