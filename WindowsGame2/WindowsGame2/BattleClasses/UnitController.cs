using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestGame.BattleClasses;

namespace TestGame.BattleClasses
{
    class UnitController : Controller
    {
        private Unit unit { get; set; }
        private CommandTimer action { get; set; }

        public UnitController(Unit u)
        {
            unit = u;
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
                    unit.turnDirection(true, percentSecond);
                }

                if ( command.Equals("UNIT-TURN-LEFT") )
                {
                    unit.turnDirection(false, percentSecond);
                }

                if (command.Equals("UNIT-MOVE-FORWARD"))
                {
                    unit.moveDirection(true, percentSecond);
                }

                if (command.Equals("UNIT-MOVE-BACKWARD"))
                {
                    unit.moveDirection(false, percentSecond);
                }

                if (command.Equals("UNIT-DODGE"))
                {
                    unit.moveDirection(false, percentSecond);
                    unit.moveDirection(false, percentSecond);
                }

                if (command.Equals("UNIT-COMMAND-START-DODGE"))
                {
                    action = CommandTimer.getCommandFromTemplate("UNIT-DODGE");
                }

                if (command.Equals("UNIT-COMMAND-START-DODGE-FOCUS"))
                {
                    action = CommandTimer.getCommandFromTemplate("UNIT-DODGE");
                }
            }
        }
    }
}
