using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame.BattleClasses
{
    class BattleCamera
    {

        private bool m_unitFocus;
        private int m_xFocus; 
        private int m_yFocus;

        private int m_xSize;
        private int m_ySize;

        private String m_currentFocus;
        private Dictionary<String,Unit> m_possibleUnitFocuses;

        private BattleBase m_base;

        public BattleCamera(BattleBase basex)
        {
            m_base = basex;
            m_possibleUnitFocuses = new Dictionary<String,Unit>();
        }

        public void setScreenSize(int x, int y )
        {
            m_xSize = x;
            m_ySize = y;
        }

        public void addPossibleFocus(Unit u)
        {
            m_possibleUnitFocuses.Add(u.getName(), u);
        }

        public void setUnitFocus(String name)
        {
            m_currentFocus = name;
            m_unitFocus = true;
        }

        public void setStaticFocus(int x, int y)
        {
            m_xFocus = x;
            m_yFocus = y;
            m_unitFocus = false;
        }

        public List<int> getViewModifier()
        {
            List<int> returnValue = new List<int>();
            if (m_unitFocus)
            {
                returnValue.Add((int)m_possibleUnitFocuses[m_currentFocus].x_pos - (m_xSize/2));
                returnValue.Add((int)m_possibleUnitFocuses[m_currentFocus].y_pos - (m_ySize/2));
            }
            else
            {
                returnValue.Add(m_xFocus);
                returnValue.Add(m_yFocus);
            }
            return returnValue;
        }
    }
}
