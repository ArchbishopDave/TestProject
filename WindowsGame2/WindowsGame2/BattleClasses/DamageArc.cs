using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace TestGame.BattleClasses
{
    class DamageArc
    {
        public UnitController m_sourceUnit { get; set; }
        private List<UnitController> m_hitUnits;

        private static int m_arcCount = 4;

        private float m_arcLength { get; set; }
        private float m_arcStart { get; set; }
        private float m_arcTurn { get; set; }
        private float m_turnSpeed { get; set; }

        private float m_time { get; set; }
        private float m_count { get; set; }

        public DamageArc(UnitController s, float arcLen, float arcS, float arcE, float ts)
        {
            m_sourceUnit = s;
            m_hitUnits = new List<UnitController>();
            m_arcLength = arcLen;
            m_arcStart = arcS + s.m_unit.facing;
            m_arcTurn = (arcE - arcS) / ((float)m_arcCount-1);
            m_turnSpeed = ts;

            m_time = 0;
            m_count = -1;
        }

        public bool readyCheck(float percentSecond)
        {
            m_time += percentSecond;
            if (m_time >= m_turnSpeed / 7.0f)
            {
                m_time = 0;
                m_count++;
                return true;
            }
            return false;
        }

        public void attemptDamageUnit(UnitController u)
        {
            if (u == m_sourceUnit)
                return;

            if (m_hitUnits.Contains(u))
                return;

            // Need a get radius method for unit controllers to make sure this actually works.
            float x = u.m_unit.x_pos - m_sourceUnit.m_unit.x_pos;
            float y = u.m_unit.y_pos - m_sourceUnit.m_unit.y_pos;
            float rad = 32 + m_arcLength;

            // Intersectionnn!
           // Debug.WriteLine((x * x) + (y * y) + " " + rad);
            if ((x * x) + (y * y) < (rad*rad))
            {
                float nx = m_sourceUnit.m_unit.x_pos + (float)Math.Cos(m_arcTurn * m_count + m_arcStart) * (m_arcLength+16);
                float ny = m_sourceUnit.m_unit.y_pos + (float)Math.Sin(m_arcTurn * m_count + m_arcStart) * (m_arcLength+16);

                float n2x = m_sourceUnit.m_unit.x_pos + (float)Math.Cos(m_arcTurn * m_count + m_arcStart) * (m_arcLength/2 + 16);
                float n2y = m_sourceUnit.m_unit.y_pos + (float)Math.Sin(m_arcTurn * m_count + m_arcStart) * (m_arcLength/2 + 16);

                if( (Math.Abs(nx - u.m_unit.x_pos) <= 16 && Math.Abs(ny - u.m_unit.y_pos) <= 16) || (Math.Abs(n2x - u.m_unit.x_pos) <= 16 && Math.Abs(n2y - u.m_unit.y_pos) <= 16) )
                {
                    m_hitUnits.Add(u);
                    u.calculateDamageEffects(m_sourceUnit);
                }
            }

        }

        public bool checkFinished()
        {
            if (m_arcCount == m_count + 1)
                return true;
            return false;
        }
    }
}
