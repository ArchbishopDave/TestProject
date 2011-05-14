using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TestGame.BattleClasses
{
    class DamageArc
    {
        private UnitController m_sourceUnit;
        private List<UnitController> m_hitUnits;

        private static int m_arcCount = 7;

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
            m_arcStart = arcS + s.unit.facing;
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
            float x = u.unit.x_pos - m_sourceUnit.unit.x_pos;
            float y = u.unit.y_pos - m_sourceUnit.unit.y_pos;
            float rad = 32 + m_arcLength;

            // Intersectionnn!
           // Debug.WriteLine((x * x) + (y * y) + " " + rad);
            if ((x * x) + (y * y) < (rad*rad))
            {
                Debug.WriteLine("Possible Intersection?");
                float nx = m_sourceUnit.unit.x_pos + (float)Math.Cos(m_arcTurn * m_count + m_arcStart) * (m_arcLength+32);
                float ny = m_sourceUnit.unit.y_pos + (float)Math.Sin(m_arcTurn * m_count + m_arcStart) * (m_arcLength+32);
                if (Math.Abs(nx - u.unit.x_pos) <= 16 && Math.Abs(ny - u.unit.y_pos) <= 16)
                {
                    Debug.WriteLine("WOAH FUCK YEAHHHH!");
                    m_hitUnits.Add(u);
                    u.unit.TESTHITCHEAT();
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
