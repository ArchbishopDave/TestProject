using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.BattleClasses
{
    class Weapon
    {
        private String m_name { get; set; }
        public UnitController m_unitHeld { get; set; }

        public Color m_color { get; set; }

        private Texture2D m_texture { get; set; }

        public Dictionary<String, int> m_stats { get; set; }
        public List<List<float>> m_swingData { get; set; }

        public Weapon(String name, Texture2D tex)
        {
            m_name = name;
            m_texture = tex;
        }

        public void setColor(Color c)
        {
            m_color = c;
        }

        public void DRAW(SpriteBatch sb, int x, int y, float facing)
        {
            sb.Draw(m_texture, new Vector2(x, y), null, m_color, facing, new Vector2(m_texture.Width / 2, m_texture.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0.0f);
        }

    }
}
