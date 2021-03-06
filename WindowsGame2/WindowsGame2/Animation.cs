﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame
{
    class Animation
    {
        public static Dictionary<String, Animation> s_animations { get; set; }

        public bool m_loop { get; set; }
        public Color m_color { get; set; }

        protected bool m_running { get; set; }

        protected int m_frames { get; set; }
        protected int m_slides { get; set; }

        protected int m_curFrame { get; set; }
        protected int m_curSlide { get; set; }

        public float m_playSpeed { get; set; }

        public String m_animationName { get; set; }

        protected Texture2D m_texture;

        public static void initialize()
        {
            s_animations = new Dictionary<string,Animation>();
        }

        public static Animation getAnimation(String s)
        {
            return new Animation(s_animations[s]);
        }

        public Animation(String name, Texture2D tex, int frames, int slides, float speed)
        {
            m_animationName = name;
            m_texture = tex;
            m_frames = frames;
            m_slides = slides;
            m_curFrame = 0;
            m_curSlide = 0;
            m_loop = false;
            m_color = Color.White;
            m_running = false;
            m_playSpeed = speed;
        }

        protected Animation(Animation a)
        {
            m_animationName = a.m_animationName;
            m_texture = a.m_texture;
            m_frames = a.m_frames;
            m_slides = a.m_slides;
            m_curFrame = 0;
            m_curSlide = 0;
            m_loop = a.m_loop;
            m_color = a.m_color;
            m_running = false;
            m_playSpeed = a.m_playSpeed;
        }

        public void setSlide(int slide)
        {
            m_curSlide = slide;
            m_curFrame = 0;
            m_running = true;
            m_loop = false;
        }

        public bool DRAW(SpriteBatch sb, int x, int y, float facing)
        {
            if (m_running)
            {
                sb.Draw(m_texture, new Vector2(x, y), new Rectangle((int)((int)(m_curFrame/m_playSpeed)) * (m_texture.Width / m_frames), (int)m_curSlide * (m_texture.Height / m_slides),
                    (int)m_texture.Width / m_frames, (int)m_texture.Height / m_slides),
                    m_color, facing, new Vector2(m_texture.Width / (m_frames*2), m_texture.Height / (m_slides*2)), new Vector2(1, 1), SpriteEffects.None, 0.0f);
                m_curFrame++;
                if ((int)(m_curFrame/m_playSpeed) == m_frames)
                {
                    if (!m_loop)
                        m_running = false;
                    else
                        m_curFrame = 0;
                }
            }
            return m_running;
        }
    }
}
