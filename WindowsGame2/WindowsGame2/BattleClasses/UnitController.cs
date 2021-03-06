﻿using System;
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
        public UnitController m_target { get; set; }
        private CommandTimer action { get; set; }
        private int m_swingCount { get; set; }

        private float m_fpRestoreTimer { get; set; }
        private float m_fpRestoreTime { get; set; }
        private float m_hpRestoreTimer { get; set; }
        private float m_hpRestoreTime { get; set; }

        private float m_movementModifier { get; set; }
        private float m_turnMoveModifier { get; set; }

        private bool m_swinging { get; set; }
        private bool m_swingAgain { get; set; }

        public BattleGroup m_battleGroup { get; set; }

        public static float DAMAGECONSTANT = 0.75f;

        public UnitController(Unit u)
        {
            m_unit = u;
            m_swingCount = 0;

            m_fpRestoreTimer = 0;
            m_hpRestoreTimer = 0;

            // Calculation for restore time is done here.
            m_fpRestoreTime = 60.0f / u.m_stats["MFP"];
            m_hpRestoreTime = 4.0f;

            m_swingAgain = false;
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
            m_movementModifier = 1.0f;
            m_turnMoveModifier = 1.0f;
            m_swinging = false;

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
                    if (comm.Contains("ATTACK"))
                    {
                        m_swinging = true;
                        m_movementModifier = 0.2f;
                        m_turnMoveModifier = 0.5f;
                    }
                    else
                    {
                        commands = new List<String>();
                        commands.Add(comm);
                    }
                }
                else
                {
                    if (m_swingAgain)
                    {
                        m_swingCount++;
                        action = null;
                        m_swingAgain = false;
                        swingWeapon();
                    }
                    else
                    {
                        foreach (String com in action.commands)
                        {
                            if (com.Contains("ATTACK"))
                            {
                                holsterWeapon(m_swingCount);
                            }
                        }
                        action = null;
                        m_swingCount = 0;
                    }
                }
            }
            handleInnerCommand(commands, percentSecond);
        }

        private void handleInnerCommand(List<String> commands, float percentSecond)
        {
            foreach (String command in commands)
            {
                if (command.Equals("UNIT-TURN-RIGHT"))
                {
                    m_unit.turnDirection(true, percentSecond * m_turnMoveModifier);
                }

                if (command.Equals("UNIT-TURN-LEFT"))
                {
                    m_unit.turnDirection(false, percentSecond * m_turnMoveModifier);
                }

                if (command.Equals("UNIT-MOVE-FORWARD"))
                {
                    m_unit.moveDirection(true, percentSecond * m_movementModifier);
                }

                if (command.Equals("UNIT-MOVE-BACKWARD"))
                {
                    m_unit.moveDirection(false, percentSecond * m_movementModifier);
                }

                if (command.Equals("UNIT-DODGE"))
                {
                    m_unit.moveDirection(false, percentSecond);
                    m_unit.moveDirection(false, percentSecond);
                }

                if (command.Equals("UNIT-COMMAND-START-DODGE"))
                {
                    if (tryDodge(false))
                        action = CommandTimer.getCommandFromTemplate("UNIT-DODGE");
                }

                if (command.Equals("UNIT-COMMAND-START-DODGE-FOCUS"))
                {
                    if (tryDodge(true))
                    {
                        action = CommandTimer.getCommandFromTemplate("UNIT-DODGE");
                        Animation a = Animation.getAnimation("UNIT-CHARGE-EXPLODE");
                        a.m_color = Color.Yellow;
                        a.setSlide(0);
                        m_unit.m_animations.Add(a);
                    }
                }

                if (command.Equals("UNIT-COMMAND-START-ATTACK"))
                {
                    if (m_swinging)
                    {
                        m_swingAgain = true;
                    }

                    if (!m_swinging && tryAttack(false))
                    {
                        swingWeapon();
                        m_swingAgain = false;
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

            return canAttack;
        }

        public void swingWeapon()
        {
            if (m_unit.m_weapon.m_swingCount > m_swingCount)
            {
                Dictionary<String, float> swingData = m_unit.m_weapon.m_swingData[m_swingCount];
                m_unit.m_weapon.swingWeapon(m_swingCount);
                m_battleBase.addDamageArc(new DamageArc(this, swingData["ARC-DIST"], swingData["ARC-START"], swingData["ARC-END"], swingData["ARC-TIME"]));
                action = CommandTimer.getAttackCommand(swingData["ARC-TIME"]);
            }
            else
            {
                holsterWeapon(m_swingCount-1);
                m_swingCount = 0;
            }
        }

        public void holsterWeapon(int count)
        {
            m_unit.m_weapon.holsterWeapon(count);
        }

        public void calculateDamageEffects(UnitController uc)
        {
            Animation a = Animation.getAnimation("UNIT-CHARGE-EXPLODE");
            a.m_color = Color.DarkRed;
            a.setSlide(0);
            m_unit.m_animations.Add(a);

            float power = ((float)uc.m_unit.m_stats["POWER"] / (float)m_unit.m_stats["ENDURE"]);
            power = (float) Math.Pow((double)power, (double)DAMAGECONSTANT);
            power *= uc.m_unit.m_weapon.m_swingData[uc.m_swingCount]["BASE-DAMAGE"];

            m_unit.m_stats["HP"] -= (int)Math.Ceiling(power);
            m_unit.m_stats["CHP"] -= (int)Math.Ceiling(power/4);

            uc.trySetTarget(this);

            if ( m_unit.m_stats["HP"] <= 0 )
            {
                uc.tryRemoveTarget(this);
                if ( m_unit.m_important )
                {
                    Random ran = new Random();
                    if ( ran.Next(100) <= 10 )
                    {
                        m_unit.m_stats["CHP"] = (int)((float)m_unit.m_stats["CHP"] / 1.2);
                        Debug.WriteLine("YOU HAVE BEEN WOUNDED");
                    }
                    else if ( ran.Next(100) >= 95 )
                    {
                        m_unit.m_stats["CHP"] = 0;
                        action = CommandTimer.getCommandFromTemplate("UNIT-DEAD");
                        Debug.WriteLine("YOU HAVE DIED LOL");
                        m_unit.m_alive = false;
                    }
                    else {
                        Debug.WriteLine("YOU HAVE BEEN ROUTED");
                    }
                    attemptHealHealth(9999);
                    attemptHealFocus(9999);
                    m_unit.x_pos = -100; m_unit.y_pos = -100;
                }
                else {
                    m_unit.m_alive = false;
                    m_unit.m_stats["CHP"] = 0;
                    action = CommandTimer.getCommandFromTemplate("UNIT-DEAD");
                }
                uc.m_unit.m_killCount++;
            }


            if (m_unit.m_stats["CHP"] <= 0)
            {
                m_unit.m_alive = false;
                action = CommandTimer.getCommandFromTemplate("UNIT-DEAD");
            }

        }

        #region targeting
        public bool hasTarget()
        {
            return !(m_target == null);
        }

        public void trySetTarget(UnitController uc)
        {
            if (m_target == null || m_target.m_unit.m_stats["HP"] <= 0)
            {
                m_target = uc;
            }

            else
            {
                if (m_target.m_unit.m_stats["LEVEL"] < uc.m_unit.m_stats["LEVEL"])
                {
                    m_target = uc;
                }
            }
        }

        public void tryRemoveTarget(UnitController uc)
        {
            if (uc == m_target)
                m_target = null;
        }
        #endregion
    }
}
