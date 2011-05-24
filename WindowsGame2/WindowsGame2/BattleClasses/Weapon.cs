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

        private Color color;
        public Color m_color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                this.m_animation.m_color = value;
            }
        }

        private WeaponAnimation m_animation { get; set; }

        public Dictionary<String, int> m_stats { get; set; }

        public int m_swingCount { get; set; }
        public List<Dictionary<String,float>> m_swingData { get; set; }

        public static Dictionary<String, Weapon> s_weaponTemplates { get; set; }

        public Weapon(String name, WeaponAnimation animation)
        {
            m_name = name;
            m_animation = animation;
        }

        public Weapon(Weapon w)
        {
            m_name = w.m_name;
            m_animation = WeaponAnimation.getAnimation(w.m_animation.m_animationName);
            m_color = w.m_color;
            m_stats = new Dictionary<string,int>(w.m_stats);
            m_swingCount = w.m_swingCount;
            m_swingData = new List<Dictionary<string,float>>(w.m_swingData);
        }

        public void DRAW(SpriteBatch sb, int x, int y, float facing)
        {
            if (!m_animation.DRAW(sb, x, y, facing))
            {

                m_animation.setSlide(0);
                m_animation.DRAW(sb, x, y, facing);
            }
            //sb.Draw(m_animation, new Vector2(x, y), null, m_color, facing, new Vector2(m_animation.Width / 2, m_animation.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0.0f);
        }

        public void swingWeapon(int swing)
        {
            m_animation.setSlide(swing+1, true);
        }

        public void holsterWeapon(int swing)
        {
            m_animation.setSlide(swing+1, false);
        }

        public Dictionary<String, float> getSwingData(int swing)
        {
            return m_swingData[swing];
        }

        public static Weapon getWeaponFromTemplate(String name)
        {
            return new Weapon(s_weaponTemplates[name]);
        }

        public static void addNewWeaponTemplate(String name, Color color, WeaponAnimation tex, Dictionary<String, int> stats)
        {
            if (s_weaponTemplates == null)
                s_weaponTemplates = new Dictionary<string, Weapon>();
            Weapon w = new Weapon(name, tex);
            w.m_color = color;
            w.m_stats = stats;
            w.m_swingData = new List<Dictionary<String,float>>();
            s_weaponTemplates.Add(name, w);
        }

        public static void addSwingToWeapon(String name, float arcDist, float arcStart, float arcEnd, float swingTime, float baseDamage)
        {
            Weapon w = s_weaponTemplates[name];
            Dictionary<String,float> swingData = new Dictionary<string,float>();

            swingData.Add("ARC-DIST", arcDist);
            swingData.Add("ARC-START", arcStart);
            swingData.Add("ARC-END", arcEnd);
            swingData.Add("ARC-TIME", swingTime);
            swingData.Add("BASE-DAMAGE", baseDamage);

            w.m_swingData.Add(swingData);
            w.m_swingCount++;
            s_weaponTemplates[name] = w;
        }
    }
}
