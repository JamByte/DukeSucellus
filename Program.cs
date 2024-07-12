using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Commands;
using Microsoft.EntityFrameworkCore;
using OSRSXPTracker.DataBase;
using Quartz.Impl;
using Quartz;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;

namespace MyFirstBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(File.ReadAllText("token.txt"), DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);


            DiscordClient client = builder.Build();

            // Use the commands extension
            CommandsExtension commandsExtension = client.UseCommands(new CommandsConfiguration()
            {
                DebugGuildId =0,
                // The default value, however it's shown here for clarity
                RegisterDefaultCommandProcessors = true
            });
            commandsExtension.AddCommands(typeof(Program).Assembly);


            TextCommandProcessor textCommandProcessor = new(new()
            {
                // The default behavior is that the bot reacts to direct mentions
                // and to the "!" prefix.
                // If you want to change it, you first set if the bot should react to mentions
                // and then you can provide as many prefixes as you want.
                PrefixResolver = new DefaultPrefixResolver(true, "!d", "&d").ResolvePrefixAsync
            });

            // Add text commands with a custom prefix (?ping)
            await commandsExtension.AddProcessorsAsync(textCommandProcessor);



            GetStats x = new GetStats();
 
            PlayerDB db = new PlayerDB();
            db.context = new AppDbContext();
            
            PlayerDB.active = db;
            await db.context.Database.EnsureCreatedAsync();
            
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();
            IJobDetail job = JobBuilder.Create<UpdateXP>()
                .WithIdentity("updateXP", "group1")
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("updateXP", "group1")
                .WithCronSchedule("0 0 0,12 * * ?")
                .ForJob(job)
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
