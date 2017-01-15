using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordTest
{
    class Magic8 : Module
    {
        static private Random random;
        static private string answerString = "It is certain&It is decidedly so&Without a doubt&Yes, definitely&You may rely on it&As I see it, yes&Most likely&Outlook good&Yes&Signs point to yes&Reply hazy try again&Ask again later&Better not tell you now&Cannot predict now&Concentrate and ask again&Don't count on it&My reply is no&My sources say no&Outlook not so good&Very doubtful";
        static private string[] answers;
        public Magic8()
        {
            command = "magic8";
            if (answers == null)
                answers = answerString.Split('&');
            if (random == null)
                random = new Random();
            methods = new Dictionary<string, Func<CommandEventArgs, Task>>();
            methods.Add("ask", async (command) =>
            {
                await command.Channel.SendMessage(command.Message.User.NicknameMention + " " + answers[random.Next(answers.Length)]);
            });
        }
        public override string getHelp()
        {
            throw new NotImplementedException();
        }
        
    }
}
