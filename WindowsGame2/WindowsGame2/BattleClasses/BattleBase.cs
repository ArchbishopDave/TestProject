using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TestGame.BattleClasses;

namespace TestGame
{
    class BattleBase
    {
        List<Unit> unitList;
        public BattleCamera m_camera { get; set; }

        public BattleBase()
        {
            unitList = new List<Unit>();
            m_camera = new BattleCamera(this);
        }

        public void addUnit(Unit u)
        {
            unitList.Add(u);
            if (u.m_important)
            {
                m_camera.addPossibleFocus(u);
            }
        }

        public void drawUnits(SpriteBatch sb)
        {
            List<int> modifier = m_camera.getViewModifier();
            foreach (Unit u in unitList)
            {
                u.DRAW(sb, modifier[0], modifier[1]);
            }
        }
    }
}
