using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.BattleClasses
{
    class BattleGroup
    {

        public bool m_display { get; set; }

        public List<UnitController> m_units { get; set; }
        UnitController m_leader;

        static int XDRAWDISTANCE;
        static int YDRAWDISTANCE;
        static int CHECKREMOVETIME;

        private float m_timeDisplayed;

        public BattleGroup(UnitController uc)
        {
            m_display = false;

            m_leader = uc;

            m_units = new List<UnitController>();
            m_units.Add(uc);
            m_timeDisplayed = 0.0f;
        }

        public static void setConstants(int X, int Y, int CR)
        {
            XDRAWDISTANCE = X;
            YDRAWDISTANCE = Y;
            CHECKREMOVETIME = CR;
        }

        public void addUnit(UnitController u)
        {
            m_units.Add(u);
        }

        public bool checkDisplayAdd(int x, int y)
        {
            if (Math.Abs(x - m_leader.unit.x_pos) <= XDRAWDISTANCE && Math.Abs(y - m_leader.unit.y_pos) <= YDRAWDISTANCE)
            {
                m_display = true;
            }
            return m_display;
        }

        public void checkDisplayRemove(int x, int y, float percentTime)
        {
            m_timeDisplayed += percentTime;
            if (m_display)
            {
                m_timeDisplayed += percentTime;
                if (m_timeDisplayed >= CHECKREMOVETIME)
                {
                    foreach (UnitController u in m_units)
                    {
                        if (Math.Abs(x - u.unit.x_pos) <= XDRAWDISTANCE && Math.Abs(y - u.unit.y_pos) <= YDRAWDISTANCE)
                        {
                            m_timeDisplayed = 0;
                            return;
                        }
                    }
                    m_timeDisplayed = 0;
                    m_display = false;
                }
            }
        }

        public void draw(SpriteBatch sb, int modx, int mody)
        {
            if (m_display)
            {
                foreach (UnitController u in m_units)
                {
                    u.unit.DRAW(sb, modx, mody);
                }
            }
        }
    }
}
