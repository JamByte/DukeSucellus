using DSharpPlus.Commands;
using OSRSXPTracker.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRSXPTracker.Commands
{
    internal class Track
    {
        [Command("track")]
        public static async ValueTask ExecuteAsync(CommandContext context, string username)
        {
            username = username.ToLower();
            //Check we are not tracking it already
            bool exists = await PlayerDB.active.CheckIfPlayerIn(username);
            if (exists)
            {
                await context.RespondAsync($"{username} is already being tracked");
                return;
            }
            PlayerStats? ps = await GetStats.getPlayerData(username);
            if (ps == null)
            {
                await context.RespondAsync($"{username} not found on highscores");
                return;
            }
            await PlayerDB.active.AddPlayer(new Player { Id = username });
            ps.PlayerId = username;
            ps.Timestamp = DateTime.Now.ToUniversalTime().Ticks;
            await PlayerDB.active.AddPlayerStats(ps);
            await context.RespondAsync($"Started tracking {username}");
            return;

        }
    }
}
