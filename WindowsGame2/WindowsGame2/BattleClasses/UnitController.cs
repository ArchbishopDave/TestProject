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

        public UnitController(Unit u)
        {
            unit = u;
        }

        public void handleCommand(List<String> commands, float percentSecond)
        {
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
            }
        }
    }
}
