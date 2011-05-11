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
        List<UnitController> unitList;
        public BattleCamera m_camera { get; set; }

        public BattleBase()
        {
            unitList = new List<UnitController>();
            m_camera = new BattleCamera(this);
        }

        public void addUnit(Unit u)
        {
            unitList.Add(new UnitController(u));
            if (u.m_important)
            {
                m_camera.addPossibleFocus(u);
            }
        }

        public UnitController getUnitController(Unit u)
        {
            foreach (UnitController uc in unitList)
            {
                if (u == uc.unit)
                {
                    return uc;
                }
            }
            return null;
        }

        public void drawUnits(SpriteBatch sb)
        {
            List<int> modifier = m_camera.getViewModifier();
            foreach (UnitController u in unitList)
            {
                u.unit.DRAW(sb, modifier[0], modifier[1]);
            }
        }

        public void TESTMETHODCHARGE(float percentSecond)
        {
            List<String> commands = new List<string>();
            commands.Add("UNIT-MOVE-FORWARD");
            foreach (UnitController u in unitList)
            {
                if (u.unit.m_important == false)
                    u.handleCommand(commands, percentSecond);
            }
        }
    }
}
