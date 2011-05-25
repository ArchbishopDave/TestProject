using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.BattleClasses.Display
{
    class Minimap
    {
        private Texture2D m_mapEdgeTex { get; set; }
        private Texture2D m_officerTex { get; set; }

        private int m_mapX { get; set; }
        private int m_mapY { get; set; }
        private int m_mapWidth { get; set; }
        private int m_mapHeight { get; set; }

        public static BattleBase m_battleBase { get; set; }

        public List<UnitController> m_watchList { get; set; }

        private bool m_display { get; set; }
        private int m_opacity { get; set; }

        private int m_displayX { get; set; }
        private int m_displayY { get; set; }
        private float m_displayWidth { get; set; }
        private float m_displayHeight { get; set; }

        public static void initialize(BattleBase bb)
        {
            m_battleBase = bb;
        }

        public Minimap(int dx, int dy, int dw, int dh, int opac, Texture2D mapEdge, Texture2D officer)
        {
            m_mapEdgeTex = mapEdge;
            m_officerTex = officer;

            m_watchList = new List<UnitController>();

            m_mapX = 0;
            m_mapY = 0;
            m_mapHeight = m_battleBase.m_mapHeight;
            m_mapWidth = m_battleBase.m_mapWidth;

            m_display = false;
            m_opacity = opac;

            m_displayX = dx;
            m_displayY = dy;
            m_displayHeight = dh;
            m_displayWidth = dw;
        }

        public Minimap(int mx, int my, int mh, int mw, int dx, int dy, int dw, int dh, int opac, Texture2D mapEdge, Texture2D officer)
            : this ( dx, dy, dw, dh, opac, mapEdge, officer )
        {
            m_mapX = mx;
            m_mapY = my;
            m_mapHeight = mh;
            m_mapWidth = mw;
        }

        public void setDisplay(bool display)
        {
            m_display = display;
        }

        public void addWatchUnit(UnitController uc)
        {
            m_watchList.Add(uc);
        }

        public void DRAW(SpriteBatch sb)
        {
            if (m_display)
            {
                sb.Draw(m_mapEdgeTex, new Vector2(m_displayX, m_displayY), null, new Color(255, 255, 255, m_opacity), 0.0f,
                    new Vector2(0, 0), new Vector2(m_displayWidth / 32, m_displayHeight / 32), SpriteEffects.None, 0.0f);

                foreach (UnitController uc in m_watchList)
                {
                    if (uc.m_unit.x_pos >= m_mapX && uc.m_unit.x_pos <= m_mapX + m_mapWidth && uc.m_unit.y_pos >= m_mapY && uc.m_unit.y_pos <= m_mapY + m_mapHeight)
                    {
                        float xdraw = ((float)(uc.m_unit.x_pos - m_mapX)) / ((float)(m_mapWidth));
                        float ydraw = ((float)(uc.m_unit.y_pos - m_mapY)) / ((float)(m_mapHeight));
                        Color c = uc.m_unit.m_color;
                        c.A = (byte)m_opacity;
                        sb.Draw(m_officerTex, new Vector2(m_displayX + m_displayWidth * xdraw, m_displayY + m_displayHeight * ydraw), null,
                            c, 0.0f, new Vector2(0, 0), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0.0f);
                    }
                }
            }
        }
    }
}
