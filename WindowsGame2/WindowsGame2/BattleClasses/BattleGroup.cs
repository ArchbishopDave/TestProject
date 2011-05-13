using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.BattleClasses
{
    class BattleGroup
    {

        public bool m_display { get; set; }

        List<Unit> m_units;
        Unit m_leader;

        const int XDRAWDISTANCE = 600;
        const int YDRAWDISTANCE = 600;

        float m_timeDisplayed;

        public BattleGroup(Unit u)
        {
            m_display = false;

            m_leader = u;

            m_units = new List<Unit>();
            m_units.Add(u);
            m_timeDisplayed = 0.0f;
        }

        public void addUnit(Unit u)
        {
            m_units.Add(u);
        }

        public bool checkDisplayAdd(int x, int y)
        {
            if (Math.Abs(x - m_leader.x_pos) <= XDRAWDISTANCE && Math.Abs(y - m_leader.y_pos) <= YDRAWDISTANCE)
            {
                m_display = true;
            }
            return m_display;
        }

        public void checkDisplayRemove(int x, int y)
        {
            foreach (Unit u in m_units)
            {
                if (Math.Abs(x - m_leader.x_pos) <= XDRAWDISTANCE / 2 && Math.Abs(y - m_leader.y_pos) <= YDRAWDISTANCE / 2)
                {
                    return;
                }
            }
            m_display = false;
        }

        public void draw(SpriteBatch sb, int modx, int mody)
        {
            if (m_display)
            {
                foreach (Unit u in m_units)
                {
                    u.DRAW(sb, modx, mody);
                }
            }
        }
    }
}
