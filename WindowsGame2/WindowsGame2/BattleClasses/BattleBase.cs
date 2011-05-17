using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace TestGame.BattleClasses
{
    class BattleBase
    {
        List<UnitController> unitList;
        Dictionary<String, BattleGroup> m_unitGroups;
        public BattleCamera m_camera { get; set; }

        private List<DamageArc> m_damageArcs;

        public BattleBase()
        {
            unitList = new List<UnitController>();
            m_unitGroups = new Dictionary<string, BattleGroup>();
            m_camera = new BattleCamera(this);
            m_damageArcs = new List<DamageArc>();
        }

        public void addUnit(UnitController u)
        {
            unitList.Add(u);
            if (u.m_unit.m_important)
            {
                m_camera.addPossibleFocus(u.m_unit);
            }
        }

        public void addUnitGroup(String name, BattleGroup bg)
        {
            foreach (UnitController uc in bg.m_units)
            {
                unitList.Add(uc);
            }
            m_unitGroups.Add(name, bg);
        }

        public void addDamageArc(DamageArc arc)
        {
            m_damageArcs.Add(arc);
        }

        public void updateGameState(float percentSecond)
        {
            foreach (KeyValuePair<String, BattleGroup> bg in m_unitGroups)
            {
                if (bg.Value.m_display)
                {
                    int c = 0;
                    Random ran = new Random();
                    foreach (UnitController uc in bg.Value.m_units)
                    {
                        uc.updateRestore(percentSecond);
                        // ( TEST )
                        TESTFAKEAI(uc, percentSecond, ran);
                        c++;
                    }
                }
            }
        }

        public UnitController getUnitController(Unit u)
        {
            foreach (UnitController uc in unitList)
            {
                if (u == uc.m_unit)
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

        public bool TESTFAKEAI(UnitController uc, float percentSecond, Random ran)
        {
            List<String> blank = new List<string>();
            List<String> attack = new List<string>();
            attack.Add("UNIT-COMMAND-START-ATTACK");
            List<String> forward = new List<String>();
            forward.Add("UNIT-MOVE-FORWARD");

            if (uc.m_unit.m_important != true)
            {
                int x = ran.Next(300);
                if (x == 1)
                {
                    uc.handleCommand(attack, percentSecond);
                    return true;
                }
                else if (x % 2 == 0)
                {
                    if (x % 3 == 1)
                        forward.Add("UNIT-TURN-RIGHT");
                    else if (x % 3 == 2)
                        forward.Add("UNIT-TURN-LEFT");
                    uc.handleCommand(forward, percentSecond);
                }
                else
                {
                    // Debug.WriteLine("NO SWING TIME");
                    uc.handleCommand(blank, percentSecond);
                }
            }
            return false;
        }

        public void setVisible()
        {
            List<int> modifier = m_camera.getViewPosition();
            foreach (KeyValuePair<String, BattleGroup> bg in m_unitGroups)
            {
                bg.Value.checkDisplayAdd(modifier[0], modifier[1]);
            }
        }
        public void checkVisible(float percentSecond)
        {
            List<int> modifier = m_camera.getViewPosition();
            foreach (KeyValuePair<String, BattleGroup> bg in m_unitGroups)
            {
                bg.Value.checkDisplayRemove(modifier[0], modifier[1], percentSecond);
            }
        }

        public void checkArcs(float percentSecond)
        {
            List<DamageArc> removes = new List<DamageArc>();
            foreach (DamageArc arc in m_damageArcs)
            {
                if (arc.readyCheck(percentSecond))
                {
                    foreach (KeyValuePair<String, BattleGroup> bg in m_unitGroups)
                    {
                        if (bg.Value.m_display)
                        {
                            foreach (UnitController uc in bg.Value.m_units)
                            {
                                arc.attemptDamageUnit(uc);
                            }
                        }
                    }
                    if (arc.checkFinished())
                    {
                        removes.Add(arc);
                    }
                }
            }
            foreach (DamageArc arc in removes)
            {
                m_damageArcs.Remove(arc);
            }
        }

        public void checkDiagnostics()
        {
            Debug.WriteLine("Unit Count: " + unitList.Count);
            int DISPTOTAL = 0;
            foreach (KeyValuePair<String, BattleGroup> bg in m_unitGroups)
            {
                if (bg.Value.m_display)
                    DISPTOTAL += bg.Value.m_units.Count;
            }
            Debug.WriteLine("Displayed Units: " + DISPTOTAL);
            Debug.WriteLine("Damage Arc Count: " + m_damageArcs.Count);
        }
    }
}
