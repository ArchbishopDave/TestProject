using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TestGame.BattleClasses
{
    class BattleBase
    {
        List<UnitController> unitList;
        Dictionary<String, BattleGroup> m_unitGroups;
        public BattleCamera m_camera { get; set; }

        public BattleBase()
        {
            unitList = new List<UnitController>();
            m_unitGroups = new Dictionary<string, BattleGroup>();
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

        public void addUnitGroup(String name, BattleGroup bg)
        {
            m_unitGroups.Add(name, bg);
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
            foreach (KeyValuePair<String,BattleGroup> bg in m_unitGroups)
            {
                bg.Value.draw(sb, modifier[0], modifier[1]);
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

        public void setVisible()
        {
            List<int> modifier = m_camera.getViewModifier();
            foreach (KeyValuePair<String, BattleGroup> bg in m_unitGroups)
            {
                bg.Value.checkDisplayAdd(modifier[0], modifier[1]);
            }
        }
    }
}
