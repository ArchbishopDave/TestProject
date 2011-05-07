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

        public BattleBase()
        {
            unitList = new List<Unit>();
        }

        public void addUnit(Unit u)
        {
            unitList.Add(u);
        }

        public void drawUnits(SpriteBatch sb)
        {
            foreach (Unit u in unitList)
            {
                u.DRAW(sb);
            }
        }

        public void BADMETHOD_TURNTEST(bool direction, float percentSecond)
        {
            unitList[0].turnDirection(direction, percentSecond);
        }
    }
}
