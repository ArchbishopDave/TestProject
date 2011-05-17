using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TestGame.BattleClasses;
using System.Diagnostics;

namespace TestGame.BattleClasses
{
    class UnitController : Controller
    {
        public static BattleBase m_battleBase { get; set; }
        public Unit m_unit { get; set; }
        private CommandTimer action { get; set; }
        private int m_swingCount { get; set; }

        private float m_fpRestoreTimer { get; set; }
        private float m_fpRestoreTime { get; set; }
        private float m_hpRestoreTimer { get; set; }
        private float m_hpRestoreTime { get; set; }

        public UnitController(Unit u)
        {
            m_unit = u;
            m_swingCount = 0;

            m_fpRestoreTimer = 0;
            m_hpRestoreTimer = 0;

            // Calculation for restore time is done here.
            m_fpRestoreTime = 60.0f / u.m_stats["MFP"];
            m_hpRestoreTime = 4.0f;
        }

        public void updateRestore(float percentSecond)
        {
            m_fpRestoreTimer += percentSecond;
            m_hpRestoreTimer += percentSecond;

            // Tries to restore HP/FP is applicable
            if (m_hpRestoreTimer >= m_hpRestoreTime)
            {
                attemptHealHealth(1);
                m_hpRestoreTimer -= 1.0f;
            }

            while (m_fpRestoreTimer >= m_fpRestoreTime)
            {
                attemptHealFocus(1);
                m_fpRestoreTimer -= m_fpRestoreTime;
            }
               
        }

        public void attemptHealHealth(int hp)
        {
            m_unit.m_stats["HP"] += hp;
            if ( m_unit.m_stats["HP"] + hp > m_unit.m_stats["CHP"] )
                m_unit.m_stats["HP"] = m_unit.m_stats["CHP"];
        }
        public void attemptHealFocus(int fp)
        {
            m_unit.m_stats["FP"] += fp;
            if ( m_unit.m_stats["FP"] + fp > m_unit.m_stats["MFP"] )
                m_unit.m_stats["FP"] = m_unit.m_stats["MFP"];
        }

        public void handleCommand(List<String> commands, float percentSecond)
        {
            foreach ( String command in commands )
            {
                if ( command.Contains("FOCUS"))
                {
                    action = null;
                }
            }

            if (action != null)
            {
                String comm = action.getCommand(percentSecond);
                if (!comm.Equals("NO-OP"))
                {
                    commands = new List<String>();
                    commands.Add(comm);
                }
                else
                {
                    action = null;
                }
            }

            foreach(String command in commands )
            {
                if ( command.Equals("UNIT-TURN-RIGHT") )
                {
                    m_unit.turnDirection(true, percentSecond);
                }

                if ( command.Equals("UNIT-TURN-LEFT") )
                {
                    m_unit.turnDirection(false, percentSecond);
                }

                if (command.Equals("UNIT-MOVE-FORWARD"))
                {
                    m_unit.moveDirection(true, percentSecond);
                }

                if (command.Equals("UNIT-MOVE-BACKWARD"))
                {
                    m_unit.moveDirection(false, percentSecond);
                }

                if (command.Equals("UNIT-DODGE"))
                {
                    m_unit.moveDirection(false, percentSecond);
                    m_unit.moveDirection(false, percentSecond);
                }

                if (command.Equals("UNIT-COMMAND-START-DODGE"))
                {
                    if ( tryDodge(false) )
                        action = CommandTimer.getCommandFromTemplate("UNIT-DODGE");
                }

                if (command.Equals("UNIT-COMMAND-START-DODGE-FOCUS"))
                {
                    if (tryDodge(true))
                    {
                        action = CommandTimer.getCommandFromTemplate("UNIT-DODGE");
                        Animation a = Animation.getAnimation("UNIT-CHARGE-EXPLODE");
                        a.m_color = Color.SandyBrown;
                        a.setSlide(0);
                        m_unit.m_animations.Add(a);
                    }
                }

                if (command.Equals("UNIT-COMMAND-START-ATTACK"))
                {
                    if (tryAttack(false))
                    {
                        action = CommandTimer.getCommandFromTemplate("UNIT-ATTACK");
                        swingWeapon();
                    }
                }
            }
        }

        public bool tryDodge(bool focus)
        {
            bool canDodge = false;
            int fp = m_unit.m_stats["FP"];

            if (fp >= 15 && !focus )
            {
                m_unit.m_stats["FP"] -= 15;
                canDodge = true;
            }
            else if (fp >= 30 && focus)
            {
                m_unit.m_stats["FP"] -= 35;
                canDodge = true;
            }

            return canDodge;
        }

        public bool tryAttack(bool focus)
        {
            bool canAttack = false;
            int fp = m_unit.m_stats["FP"];

            if (fp >= 5 && !focus)
            {
                m_unit.m_stats["FP"] -= 5;
                canAttack = true;
            }
            else if (fp >= 40 && focus)
            {
                m_unit.m_stats["FP"] -= 40;
                canAttack = true;
            }

            if (canAttack)
            {
                m_unit.alpha = Color.Yellow;
            }
            else
            {
                m_unit.alpha = Color.Blue;
            }

            return canAttack;
        }

        public void swingWeapon()
        {
            Dictionary<String, float> swingData = m_unit.m_weapon.m_swingData[m_swingCount];
            m_battleBase.addDamageArc(new DamageArc(this, swingData["ARC-DIST"], swingData["ARC-START"], swingData["ARC-END"], swingData["ARC-TIME"]));
        }

        public void calculateDamageEffects(UnitController uc)
        {

        }
    }
}
