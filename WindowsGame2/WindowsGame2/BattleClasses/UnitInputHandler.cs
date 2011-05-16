using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestGame.BattleClasses
{
    class UnitInputHandler
    {
        private PlayerIndex m_index;
        private UnitController m_controller;
        List<Buttons> m_heldButtons;

        public UnitInputHandler(PlayerIndex index, UnitController controller)
        {
            m_index = index;
            m_controller = controller;
            m_heldButtons = new List<Buttons>();
        }

        public void handleInputs(float percentSecond)
        {
            List<String> temp = new List<String>();

            if (GamePad.GetState(m_index).ThumbSticks.Left.X >= 0.2)
                temp.Add("UNIT-TURN-LEFT");

            if (GamePad.GetState(m_index).ThumbSticks.Left.X <= -0.2)
                temp.Add("UNIT-TURN-RIGHT");

            if (GamePad.GetState(m_index).ThumbSticks.Left.Y >= 0.2)
                temp.Add("UNIT-MOVE-FORWARD");

            if (GamePad.GetState(m_index).ThumbSticks.Left.Y <= -0.2)
                temp.Add("UNIT-MOVE-BACKWARD");

            // Focus Moves
            if (GamePad.GetState(m_index).Triggers.Right >= 0.5)
            {
                if (GamePad.GetState(m_index).IsButtonDown(Buttons.B) && !m_heldButtons.Contains(Buttons.B))
                    temp.Add("UNIT-COMMAND-START-DODGE-FOCUS");
            }
            // Non Focus Equivilants
            else
            {
                if (GamePad.GetState(m_index).IsButtonDown(Buttons.B) && !m_heldButtons.Contains(Buttons.B))
                    temp.Add("UNIT-COMMAND-START-DODGE");
                if (GamePad.GetState(m_index).IsButtonDown(Buttons.X) && !m_heldButtons.Contains(Buttons.X))
                    temp.Add("UNIT-COMMAND-START-ATTACK");
            }

            m_heldButtons.Clear();
            checkHeldButtons(Buttons.A); checkHeldButtons(Buttons.B); checkHeldButtons(Buttons.X); checkHeldButtons(Buttons.Y);

            m_controller.handleCommand(temp, percentSecond);
        }

        public void checkHeldButtons(Buttons button)
        {
            if (GamePad.GetState(m_index).IsButtonDown(button))
                m_heldButtons.Add(button);
        }
    }
}
