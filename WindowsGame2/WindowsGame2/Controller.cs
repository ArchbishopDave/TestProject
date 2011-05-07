using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame
{
    interface Controller
    {
        void handleCommand(List<String> commands, float precentSecond);
    }
}
