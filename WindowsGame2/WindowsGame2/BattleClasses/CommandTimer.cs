using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGame.BattleClasses
{
    class CommandTimer
    {
        private List<String> commands { get; set; }
        private List<float> times { get; set; }

        private int currentEvent { get; set; }
        private float currentTime { get; set; }

        public static Dictionary<String, CommandTimer> templateTimers;

        public CommandTimer()
        {
            commands = new List<string>();
            times = new List<float>();
            currentEvent = 0;
            currentTime = 0;
        }

        public CommandTimer(CommandTimer timer)
        {
            commands = new List<String>(timer.commands);
            times = new List<float>(timer.times);
            currentEvent = 0;
            currentTime = 0;
        }

        public void addCommand(String command, float time)
        {
            commands.Add(command);
            times.Add(time);
        }

        public String getCommand(float percentSecond)
        {
            currentTime += percentSecond;
            if (times[currentEvent] < currentTime)
            {
                currentEvent++;
                if (currentEvent >= commands.Count)
                {
                    return "NO-OP";
                }
            }
            return commands[currentEvent];
        }

        public static void formTemplates()
        {
            templateTimers = new Dictionary<string, CommandTimer>();
            CommandTimer dodge = new CommandTimer();
            dodge.addCommand("UNIT-DODGE", 0.4f);

            templateTimers.Add("UNIT-DODGE", dodge);
        }

        public static CommandTimer getCommandFromTemplate(String template)
        {
            return new CommandTimer(templateTimers[template]);
        }
    }
}
