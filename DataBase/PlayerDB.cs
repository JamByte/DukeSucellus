using Microsoft.EntityFrameworkCore;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRSXPTracker.DataBase
{
    internal class PlayerDB
    {
        public static PlayerDB active;
        public AppDbContext context;
        public async Task AddPlayer(Player p)
        {
            context.Players.Add(p);
            await context.SaveChangesAsync();
        }
        public async Task<List<Player>> GetAllPlayers()
        {
            return await context.Players.ToListAsync();
        }
        public async Task<bool> CheckIfPlayerIn(string playerId)
        {
            bool exists =  await context.Players.AnyAsync(ps => ps.Id == playerId);
            return exists;
        }

        public async Task AddPlayerStats(PlayerStats ps) 
        {
            context.PlayerStats.Add(ps);
            await context.SaveChangesAsync();
        }
        public async Task AddPlayerStatsList(List<PlayerStats> ps)
        {
            context.PlayerStats.AddRange(ps);
            await context.SaveChangesAsync();
        }
        public async Task<PlayerStats?> GetClosestEntryTime(long timeStampOffset, string name)
        {
            PlayerStats? ps = await context.PlayerStats.Where(ps => ps.PlayerId == name).OrderByDescending(ps => Math.Abs(ps.Timestamp - DateTime.UtcNow.Ticks- timeStampOffset)).FirstOrDefaultAsync();
            return ps;
            
        }
        public async Task<PlayerStats?> GetClosestEntryWeek(string name)
        {
            return await GetClosestEntryTime(new DateTime(1970,1,7).Ticks,name);
        }
        public async Task<PlayerStats?> GetNewestEntry(string name)
        {
            return await GetClosestEntryTime(0, name);
        }
        public class UpdateXPKJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine($"Hourly task executed at: {DateTime.Now}");
                
                await Task.CompletedTask;
            }
        }

    }
}
