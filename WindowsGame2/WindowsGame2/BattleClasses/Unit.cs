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
        public float x_pos { get; set; }
        public float y_pos { get; set; }
        public float facing { get; set; }

        private List<Texture2D> m_textures { get; set; }
        public Color alpha { get; set; }

        protected int x_dim { get; set; }
        protected int y_dim { get; set; }

        Dictionary<String, int> m_stats;
        private float turnSpeed { get; set; }

        private String m_name { get; set; }

        public bool m_important { get; set; }

        public Weapon m_weapon { get; set; }

        public Unit(float x, float y)
        {
            x_pos = x;
            y_pos = y;

            x_dim = 32;
            y_dim = 32;

            m_stats = new Dictionary<String, int>();
            m_important = false;

            m_textures = new List<Texture2D>();
        }

        public Unit(float x, float y, float face)
            : this(x, y)
        {
            facing = face;
        }

        public Unit(float x, float y, float face, String name)
            : this(x, y, face)
        {
            m_name = name;
        }

        public void setStats(int hp, int fp, int pow, int ski, int fin, int blo, int end, int spe, int lvl, int exp, int turn)
        {
            m_stats.Add("HP", hp);
            m_stats.Add("MHP", hp);
            m_stats.Add("FP", fp);
            m_stats.Add("MFP",fp);
            m_stats.Add("POWER",pow);
            m_stats.Add("SKILL",ski);
            m_stats.Add("FINESSE",fin);
            m_stats.Add("BLOCK",blo);
            m_stats.Add("ENDURE",end);
            m_stats.Add("SPEED",spe);
            m_stats.Add("LEVEL",lvl);
            m_stats.Add("EXP",exp);
            m_stats.Add("TURN",turn);

            turnSpeed = ((float) turn) / 1000.0f;
        }

        public void setTexture(Texture2D tex, Color alph)
        {
            m_textures.Clear();
            m_textures.Add(tex);
            alpha = alph;
        }

        public void addTexture(Texture2D tex)
        {
            m_textures.Add(tex);
        }

        public void setWeapon(Weapon w)
        {
            m_weapon = w;
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
            y_pos += ((float)Math.Sin((double)facing) * m_stats["SPEED"]) * percentSecond * BACKWARDS_MODIFIER;
            x_pos += ((float)Math.Cos((double)facing) * m_stats["SPEED"]) * percentSecond * BACKWARDS_MODIFIER;
        }

        public void DRAW(SpriteBatch sb, int x, int y)
        {
            foreach (Texture2D tex in m_textures)
            {
                sb.Draw(tex, new Vector2((int)x_pos - x, (int)y_pos - y), null, alpha, facing, new Vector2(tex.Width / 2, tex.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0.0f);
            }
            if ( m_weapon != null )
                m_weapon.DRAW(sb, (int)x_pos - x, (int)y_pos - y, facing);
        }

        public int getHP()
        {
            return m_stats["HP"];
        }

        public int getFP()
        {
            return m_stats["FP"];
        }

        public String getName()
        {
            return m_name;
        }

        public void TESTCHANGER()
        {
            Random ran = new Random();
            if (2 > ran.Next(100))
            {
                m_stats["HP"] = ran.Next(m_stats["MHP"]);
                m_stats["FP"] = ran.Next(m_stats["MFP"]);
            }
        }
        public void TESTHITCHEAT()
        {
            alpha = Color.Red;
        }
    }
}
