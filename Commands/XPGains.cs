using DSharpPlus.Commands;
using OSRSXPTracker.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using System.Net.Mail;
namespace OSRSXPTracker.Commands
{
    internal class XPGains
    {
        [Command("xp")]
        public static async ValueTask ExecuteAsync(CommandContext context, string username, int topxshown = 5)
        {
            bool exists = await PlayerDB.active.CheckIfPlayerIn(username);
            if (!exists)
            {
                await context.RespondAsync($"{username} is not being tracked, use /track <username> to start tracking");
                return;
            }
            //This might do somthing /shrug
            PlayerStats? weekStats = await PlayerDB.active.GetClosestEntryWeek(username);
            PlayerStats? ps = await GetStats.getPlayerData(username);
            ps.PlayerId = username;
            ps.Timestamp = DateTime.Now.ToUniversalTime().Ticks;

            List<ChangedSkillStats> s = GetChangedSkills(weekStats, ps);
            CompareSkillStats comparer = new CompareSkillStats();
            s.Sort(comparer);
            string xpString = $"```Skill      Lvl       Xp\n";
            //Skip overall
            for (int i = 0; i < topxshown; i++)
            {
                if (i >= s.Count) { continue; }
                ChangedSkillStats s2 = s[i];
                string NamePart = s2.name;
                string LevelChangePart;
                if (s2.levelChange == 0)
                {
                    LevelChangePart = s2.startingLevel.ToString();
                }
                else
                {
                    LevelChangePart = $"{s2.startingLevel} to {s2.startingLevel + s2.levelChange}";
                }
                string XPpart = $"+{s2.xpChange:N0}";
                NamePart = NamePart.PadRight(11);
                LevelChangePart = LevelChangePart.PadRight(10);


                xpString += NamePart + LevelChangePart + XPpart;
                xpString += "\n";
            }
            xpString += "```";



            //Bosses

            List<ChangedBossStats> b = GetChangedBoss(weekStats, ps);
            string bossString = "```";
            long highest = 0;
            for (int i = 0; i < b.Count; i++)
            {
                ChangedBossStats b2 = b[i];
                if (NumDigits(b2.scoreChange) > highest){ highest = NumDigits(b2.scoreChange); }
            }
            long lengthofHighest = highest;
            for (int i = 0; i < b.Count; i++)
            {
                ChangedBossStats b2 = b[i];
                string bosschange = b2.scoreChange.ToString();
                for (int j = 0; j < lengthofHighest - bosschange.Length; j++)
                {
                    bosschange = " " + bosschange;
                }
                bossString += $"{bosschange}  {b2.name}";
                bossString += "\n";
            }
            bossString += "```";

            List<ChangedBossStats> c = GetChangedClues(weekStats, ps);
            string clueString = "```";
            highest = 0;
            for (int i = 0; i < c.Count; i++)
            {
                ChangedBossStats c2 = c[i];
                if (NumDigits(c2.scoreChange) > highest) { highest = NumDigits(c2.scoreChange); }
            }
            lengthofHighest = highest;
            for (int i = 0; i < c.Count; i++)
            {
                ChangedBossStats c2 = c[i];
                string cluechange = c2.scoreChange.ToString();
                for (int j = 0; j < lengthofHighest - cluechange.Length; j++)
                {
                    cluechange = " " + cluechange;
                }
                clueString += $"{cluechange}  {c2.name}";
                clueString += "\n";
            }
            clueString += "```";
            await PlayerDB.active.AddPlayerStats(ps);

            ChangedSkillStats overS = new ChangedSkillStats() { name = "Overall", xpChange = ps.overallXP - weekStats.overallXP, levelChange = ps.overallLevel - weekStats.overallLevel, startingLevel = weekStats.overallLevel, rankChange = ps.overallRank - weekStats.overallRank };


            string overAll = $"**{ps.overallXP:N0}** XP\n**{ps.overallLevel:N0}** Total\n**{ps.overallRank:N0}** Rank";
            string overAllGains = $"{formatOverallNumber(overS.xpChange)} XP\n{formatOverallNumber(overS.levelChange)} Total\n{formatOverallNumber(overS.rankChange)} Rank";


            DiscordEmbedBuilder deb = new DiscordEmbedBuilder();
            deb.AddField($"**{username.Substring(0, 1).ToUpper() + username.Substring(1)}**", overAll, inline: true);
            deb.AddField($"**Overall**", overAllGains, inline: true);
            if (s.Count > 0) { deb.AddField("XP", xpString); };
            if (b.Count > 0) { deb.AddField("Boss", bossString); }
            if (c.Count > 0) { deb.AddField("Clues", clueString); }
            deb.WithFooter($"Oldest data point: {FormatTimeDifference(new DateTime(weekStats.Timestamp),DateTime.UtcNow)}");
            deb.WithTitle("Weekly activity");
            await context.RespondAsync(deb.Build());
        }

        //https://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number
        public static int NumDigits(int n) =>
        n == 0 ? 1 : (n > 0 ? 1 : 2) + (int)Math.Log10(Math.Abs((double)n));
        static string FormatTimeDifference(DateTime start, DateTime end)
        {
            TimeSpan diff = end - start;
            int days = (int)diff.TotalDays;
            int hours = diff.Hours;
            return days > 0 ? $"{days}d {hours}h" : $"{hours}h";
        }
        static string formatOverallNumber(int num)
        {
            string s = "";
            if(num > 0) { s += "+"; }
            return s + $"{num:N0}";
        }
        public static List<ChangedSkillStats> GetChangedSkills(PlayerStats ps1, PlayerStats ps2)
        {
            List<ChangedSkillStats> l = new List<ChangedSkillStats>();
/*
            if (ps1.overallXP < ps2.overallXP)
            {
                l.Add(new ChangedSkillStats() { name = "Overall", xpChange = ps2.overallXP - ps1.overallXP, levelChange = ps2.overallLevel - ps1.overallLevel, startingLevel = ps1.overallLevel, rankChange = ps2.overallRank - ps1.overallRank });
            }*/

            if (ps1.attackXP < ps2.attackXP)
            {
                l.Add(new ChangedSkillStats() { name = "Attack", xpChange = ps2.attackXP - ps1.attackXP, levelChange = ps2.attackLevel - ps1.attackLevel, startingLevel = ps1.attackLevel, rankChange = ps2.attackRank - ps1.attackRank });
            }

            if (ps1.defenceXP < ps2.defenceXP)
            {
                l.Add(new ChangedSkillStats() { name = "Defence", xpChange = ps2.defenceXP - ps1.defenceXP, levelChange = ps2.defenceLevel - ps1.defenceLevel, startingLevel = ps1.defenceLevel, rankChange = ps2.defenceRank - ps1.defenceRank });
            }

            if (ps1.strengthXP < ps2.strengthXP)
            {
                l.Add(new ChangedSkillStats() { name = "Strength", xpChange = ps2.strengthXP - ps1.strengthXP, levelChange = ps2.strengthLevel - ps1.strengthLevel, startingLevel = ps1.strengthLevel, rankChange = ps2.strengthRank - ps1.strengthRank });
            }

            if (ps1.hitpointsXP < ps2.hitpointsXP)
            {
                l.Add(new ChangedSkillStats() { name = "Hitpoints", xpChange = ps2.hitpointsXP - ps1.hitpointsXP, levelChange = ps2.hitpointsLevel - ps1.hitpointsLevel, startingLevel = ps1.hitpointsLevel, rankChange = ps2.hitpointsRank - ps1.hitpointsRank });
            }

            if (ps1.rangedXP < ps2.rangedXP)
            {
                l.Add(new ChangedSkillStats() { name = "Ranged", xpChange = ps2.rangedXP - ps1.rangedXP, levelChange = ps2.rangedLevel - ps1.rangedLevel, startingLevel = ps1.rangedLevel, rankChange = ps2.rangedRank - ps1.rangedRank });
            }

            if (ps1.prayerXP < ps2.prayerXP)
            {
                l.Add(new ChangedSkillStats() { name = "Prayer", xpChange = ps2.prayerXP - ps1.prayerXP, levelChange = ps2.prayerLevel - ps1.prayerLevel, startingLevel = ps1.prayerLevel, rankChange = ps2.prayerRank - ps1.prayerRank });
            }

            if (ps1.magicXP < ps2.magicXP)
            {
                l.Add(new ChangedSkillStats() { name = "Magic", xpChange = ps2.magicXP - ps1.magicXP, levelChange = ps2.magicLevel - ps1.magicLevel, startingLevel = ps1.magicLevel, rankChange = ps2.magicRank - ps1.magicRank });
            }

            if (ps1.cookingXP < ps2.cookingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Cooking", xpChange = ps2.cookingXP - ps1.cookingXP, levelChange = ps2.cookingLevel - ps1.cookingLevel, startingLevel = ps1.cookingLevel, rankChange = ps2.cookingRank - ps1.cookingRank });
            }

            if (ps1.woodcuttingXP < ps2.woodcuttingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Woodcutting", xpChange = ps2.woodcuttingXP - ps1.woodcuttingXP, levelChange = ps2.woodcuttingLevel - ps1.woodcuttingLevel, startingLevel = ps1.woodcuttingLevel, rankChange = ps2.woodcuttingRank - ps1.woodcuttingRank });
            }

            if (ps1.fletchingXP < ps2.fletchingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Fletching", xpChange = ps2.fletchingXP - ps1.fletchingXP, levelChange = ps2.fletchingLevel - ps1.fletchingLevel, startingLevel = ps1.fletchingLevel, rankChange = ps2.fletchingRank - ps1.fletchingRank });
            }

            if (ps1.fishingXP < ps2.fishingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Fishing", xpChange = ps2.fishingXP - ps1.fishingXP, levelChange = ps2.fishingLevel - ps1.fishingLevel, startingLevel = ps1.fishingLevel, rankChange = ps2.fishingRank - ps1.fishingRank });
            }

            if (ps1.firemakingXP < ps2.firemakingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Firemaking", xpChange = ps2.firemakingXP - ps1.firemakingXP, levelChange = ps2.firemakingLevel - ps1.firemakingLevel, startingLevel = ps1.firemakingLevel, rankChange = ps2.firemakingRank - ps1.firemakingRank });
            }

            if (ps1.craftingXP < ps2.craftingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Crafting", xpChange = ps2.craftingXP - ps1.craftingXP, levelChange = ps2.craftingLevel - ps1.craftingLevel, startingLevel = ps1.craftingLevel, rankChange = ps2.craftingRank - ps1.craftingRank });
            }

            if (ps1.smithingXP < ps2.smithingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Smithing", xpChange = ps2.smithingXP - ps1.smithingXP, levelChange = ps2.smithingLevel - ps1.smithingLevel, startingLevel = ps1.smithingLevel, rankChange = ps2.smithingRank - ps1.smithingRank });
            }

            if (ps1.miningXP < ps2.miningXP)
            {
                l.Add(new ChangedSkillStats() { name = "Mining", xpChange = ps2.miningXP - ps1.miningXP, levelChange = ps2.miningLevel - ps1.miningLevel, startingLevel = ps1.miningLevel, rankChange = ps2.miningRank - ps1.miningRank });
            }

            if (ps1.herbloreXP < ps2.herbloreXP)
            {
                l.Add(new ChangedSkillStats() { name = "Herblore", xpChange = ps2.herbloreXP - ps1.herbloreXP, levelChange = ps2.herbloreLevel - ps1.herbloreLevel, startingLevel = ps1.herbloreLevel, rankChange = ps2.herbloreRank - ps1.herbloreRank });
            }

            if (ps1.agilityXP < ps2.agilityXP)
            {
                l.Add(new ChangedSkillStats() { name = "Agility", xpChange = ps2.agilityXP - ps1.agilityXP, levelChange = ps2.agilityLevel - ps1.agilityLevel, startingLevel = ps1.agilityLevel, rankChange = ps2.agilityRank - ps1.agilityRank });
            }

            if (ps1.thievingXP < ps2.thievingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Thieving", xpChange = ps2.thievingXP - ps1.thievingXP, levelChange = ps2.thievingLevel - ps1.thievingLevel, startingLevel = ps1.thievingLevel, rankChange = ps2.thievingRank - ps1.thievingRank });
            }

            if (ps1.slayerXP < ps2.slayerXP)
            {
                l.Add(new ChangedSkillStats() { name = "Slayer", xpChange = ps2.slayerXP - ps1.slayerXP, levelChange = ps2.slayerLevel - ps1.slayerLevel, startingLevel = ps1.slayerLevel, rankChange = ps2.slayerRank - ps1.slayerRank });
            }

            if (ps1.farmingXP < ps2.farmingXP)
            {
                l.Add(new ChangedSkillStats() { name = "Farming", xpChange = ps2.farmingXP - ps1.farmingXP, levelChange = ps2.farmingLevel - ps1.farmingLevel, startingLevel = ps1.farmingLevel, rankChange = ps2.farmingRank - ps1.farmingRank });
            }

            if (ps1.runecraftXP < ps2.runecraftXP)
            {
                l.Add(new ChangedSkillStats() { name = "Runecraft", xpChange = ps2.runecraftXP - ps1.runecraftXP, levelChange = ps2.runecraftLevel - ps1.runecraftLevel, startingLevel = ps1.runecraftLevel, rankChange = ps2.runecraftRank - ps1.runecraftRank });
            }

            if (ps1.hunterXP < ps2.hunterXP)
            {
                l.Add(new ChangedSkillStats() { name = "Hunter", xpChange = ps2.hunterXP - ps1.hunterXP, levelChange = ps2.hunterLevel - ps1.hunterLevel, startingLevel = ps1.hunterLevel, rankChange = ps2.hunterRank - ps1.hunterRank });
            }

            if (ps1.constructionXP < ps2.constructionXP)
            {
                l.Add(new ChangedSkillStats() { name = "Construction", xpChange = ps2.constructionXP - ps1.constructionXP, levelChange = ps2.constructionLevel - ps1.constructionLevel, startingLevel = ps1.constructionLevel, rankChange = ps2.constructionRank - ps1.constructionRank });
            }


            return l;
        }

        public static List<ChangedBossStats> GetChangedClues(PlayerStats ps1, PlayerStats ps2)
        {
            List<ChangedBossStats> l = new List<ChangedBossStats>();
            if (ps1.clueScrollsBeginnerScore < ps2.clueScrollsBeginnerScore)
            {
                l.Add(new ChangedBossStats() { name = "Clue Scrolls (beginner)", scoreChange = ps2.clueScrollsBeginnerScore - ps1.clueScrollsBeginnerScore, rankChange = ps2.clueScrollsBeginnerRank - ps1.clueScrollsBeginnerRank });
            }
            if (ps1.clueScrollsEasyScore < ps2.clueScrollsEasyScore)
            {
                l.Add(new ChangedBossStats() { name = "Clue Scrolls (easy)", scoreChange = ps2.clueScrollsEasyScore - ps1.clueScrollsEasyScore, rankChange = ps2.clueScrollsEasyRank - ps1.clueScrollsEasyRank });
            }
            if (ps1.clueScrollsMediumScore < ps2.clueScrollsMediumScore)
            {
                l.Add(new ChangedBossStats() { name = "Clue Scrolls (medium)", scoreChange = ps2.clueScrollsMediumScore - ps1.clueScrollsMediumScore, rankChange = ps2.clueScrollsMediumRank - ps1.clueScrollsMediumRank });
            }
            if (ps1.clueScrollsHardScore < ps2.clueScrollsHardScore)
            {
                l.Add(new ChangedBossStats() { name = "Clue Scrolls (hard)", scoreChange = ps2.clueScrollsHardScore - ps1.clueScrollsHardScore, rankChange = ps2.clueScrollsHardRank - ps1.clueScrollsHardRank });
            }
            if (ps1.clueScrollsEliteScore < ps2.clueScrollsEliteScore)
            {
                l.Add(new ChangedBossStats() { name = "Clue Scrolls (elite)", scoreChange = ps2.clueScrollsEliteScore - ps1.clueScrollsEliteScore, rankChange = ps2.clueScrollsEliteRank - ps1.clueScrollsEliteRank });
            }
            if (ps1.clueScrollsMasterScore < ps2.clueScrollsMasterScore)
            {
                l.Add(new ChangedBossStats() { name = "Clue Scrolls (master)", scoreChange = ps2.clueScrollsMasterScore - ps1.clueScrollsMasterScore, rankChange = ps2.clueScrollsMasterRank - ps1.clueScrollsMasterRank });
            }
            return l;
        }

            public static List<ChangedBossStats> GetChangedBoss(PlayerStats ps1, PlayerStats ps2)
        {
            List<ChangedBossStats> l = new List<ChangedBossStats>();
            /*if (ps1.leaguePointsScore < ps2.leaguePointsScore)
            {
                l.Add(new ChangedBossStats() { name = "League Points", scoreChange = ps2.leaguePointsScore - ps1.leaguePointsScore, rankChange = ps2.leaguePointsRank - ps1.leaguePointsRank });
            }
            if (ps1.deadmanPointsScore < ps2.deadmanPointsScore)
            {
                l.Add(new ChangedBossStats() { name = "Deadman Points", scoreChange = ps2.deadmanPointsScore - ps1.deadmanPointsScore, rankChange = ps2.deadmanPointsRank - ps1.deadmanPointsRank });
            }
            if (ps1.bountyHunterHunterScore < ps2.bountyHunterHunterScore)
            {
                l.Add(new ChangedBossStats() { name = "Bounty Hunter - Hunter", scoreChange = ps2.bountyHunterHunterScore - ps1.bountyHunterHunterScore, rankChange = ps2.bountyHunterHunterRank - ps1.bountyHunterHunterRank });
            }
            if (ps1.bountyHunterRogueScore < ps2.bountyHunterRogueScore)
            {
                l.Add(new ChangedBossStats() { name = "Bounty Hunter - Rogue", scoreChange = ps2.bountyHunterRogueScore - ps1.bountyHunterRogueScore, rankChange = ps2.bountyHunterRogueRank - ps1.bountyHunterRogueRank });
            }
            if (ps1.bountyHunterLegacyHunterScore < ps2.bountyHunterLegacyHunterScore)
            {
                l.Add(new ChangedBossStats() { name = "Bounty Hunter (Legacy) - Hunter", scoreChange = ps2.bountyHunterLegacyHunterScore - ps1.bountyHunterLegacyHunterScore, rankChange = ps2.bountyHunterLegacyHunterRank - ps1.bountyHunterLegacyHunterRank });
            }
            if (ps1.bountyHunterLegacyRogueScore < ps2.bountyHunterLegacyRogueScore)
            {
                l.Add(new ChangedBossStats() { name = "Bounty Hunter (Legacy) - Rogue", scoreChange = ps2.bountyHunterLegacyRogueScore - ps1.bountyHunterLegacyRogueScore, rankChange = ps2.bountyHunterLegacyRogueRank - ps1.bountyHunterLegacyRogueRank });
            }
            if (ps1.clueScrollsAllScore < ps2.clueScrollsAllScore)
            {
                l.Add(new ChangedBossStats() { name = "Clue Scrolls (all)", scoreChange = ps2.clueScrollsAllScore - ps1.clueScrollsAllScore, rankChange = ps2.clueScrollsAllRank - ps1.clueScrollsAllRank });
            }*/
            
            /* if (ps1.lmsRankScore < ps2.lmsRankScore)
             {
                 l.Add(new ChangedBossStats() { name = "LMS - Rank", scoreChange = ps2.lmsRankScore - ps1.lmsRankScore, rankChange = ps2.lmsRankRank - ps1.lmsRankRank });
             }
             if (ps1.pvpArenaRankScore < ps2.pvpArenaRankScore)
             {
                 l.Add(new ChangedBossStats() { name = "PvP Arena - Rank", scoreChange = ps2.pvpArenaRankScore - ps1.pvpArenaRankScore, rankChange = ps2.pvpArenaRankRank - ps1.pvpArenaRankRank });
             }
             if (ps1.soulWarsZealScore < ps2.soulWarsZealScore)
             {
                 l.Add(new ChangedBossStats() { name = "Soul Wars Zeal", scoreChange = ps2.soulWarsZealScore - ps1.soulWarsZealScore, rankChange = ps2.soulWarsZealRank - ps1.soulWarsZealRank });
             }

             if (ps1.colosseumGloryScore < ps2.colosseumGloryScore)
             {
                 l.Add(new ChangedBossStats() { name = "Colosseum Glory", scoreChange = ps2.colosseumGloryScore - ps1.colosseumGloryScore, rankChange = ps2.colosseumGloryRank - ps1.colosseumGloryRank });
             }*/
            if (ps1.riftsClosedScore < ps2.riftsClosedScore)
            {
                l.Add(new ChangedBossStats() { name = "Rifts closed", scoreChange = ps2.riftsClosedScore - ps1.riftsClosedScore, rankChange = ps2.riftsClosedRank - ps1.riftsClosedRank });
            }
            if (ps1.abyssalSireScore < ps2.abyssalSireScore)
            {
                l.Add(new ChangedBossStats() { name = "Abyssal Sire", scoreChange = ps2.abyssalSireScore - ps1.abyssalSireScore, rankChange = ps2.abyssalSireRank - ps1.abyssalSireRank });
            }
            if (ps1.alchemicalHydraScore < ps2.alchemicalHydraScore)
            {
                l.Add(new ChangedBossStats() { name = "Alchemical Hydra", scoreChange = ps2.alchemicalHydraScore - ps1.alchemicalHydraScore, rankChange = ps2.alchemicalHydraRank - ps1.alchemicalHydraRank });
            }
            if (ps1.artioScore < ps2.artioScore)
            {
                l.Add(new ChangedBossStats() { name = "Artio", scoreChange = ps2.artioScore - ps1.artioScore, rankChange = ps2.artioRank - ps1.artioRank });
            }
            if (ps1.barrowsChestsScore < ps2.barrowsChestsScore)
            {
                l.Add(new ChangedBossStats() { name = "Barrows Chests", scoreChange = ps2.barrowsChestsScore - ps1.barrowsChestsScore, rankChange = ps2.barrowsChestsRank - ps1.barrowsChestsRank });
            }
            if (ps1.bryophytaScore < ps2.bryophytaScore)
            {
                l.Add(new ChangedBossStats() { name = "Bryophyta", scoreChange = ps2.bryophytaScore - ps1.bryophytaScore, rankChange = ps2.bryophytaRank - ps1.bryophytaRank });
            }
            if (ps1.callistoScore < ps2.callistoScore)
            {
                l.Add(new ChangedBossStats() { name = "Callisto", scoreChange = ps2.callistoScore - ps1.callistoScore, rankChange = ps2.callistoRank - ps1.callistoRank });
            }
            if (ps1.calvarionScore < ps2.calvarionScore)
            {
                l.Add(new ChangedBossStats() { name = "Calvar\'ion", scoreChange = ps2.calvarionScore - ps1.calvarionScore, rankChange = ps2.calvarionRank - ps1.calvarionRank });
            }
            if (ps1.cerberusScore < ps2.cerberusScore)
            {
                l.Add(new ChangedBossStats() { name = "Cerberus", scoreChange = ps2.cerberusScore - ps1.cerberusScore, rankChange = ps2.cerberusRank - ps1.cerberusRank });
            }
            if (ps1.chambersOfXericScore < ps2.chambersOfXericScore)
            {
                l.Add(new ChangedBossStats() { name = "Chambers of Xeric", scoreChange = ps2.chambersOfXericScore - ps1.chambersOfXericScore, rankChange = ps2.chambersOfXericRank - ps1.chambersOfXericRank });
            }
            if (ps1.chambersOfXericChallengeModeScore < ps2.chambersOfXericChallengeModeScore)
            {
                l.Add(new ChangedBossStats() { name = "Chambers of Xeric: Challenge Mode", scoreChange = ps2.chambersOfXericChallengeModeScore - ps1.chambersOfXericChallengeModeScore, rankChange = ps2.chambersOfXericChallengeModeRank - ps1.chambersOfXericChallengeModeRank });
            }
            if (ps1.chaosElementalScore < ps2.chaosElementalScore)
            {
                l.Add(new ChangedBossStats() { name = "Chaos Elemental", scoreChange = ps2.chaosElementalScore - ps1.chaosElementalScore, rankChange = ps2.chaosElementalRank - ps1.chaosElementalRank });
            }
            if (ps1.chaosFanaticScore < ps2.chaosFanaticScore)
            {
                l.Add(new ChangedBossStats() { name = "Chaos Fanatic", scoreChange = ps2.chaosFanaticScore - ps1.chaosFanaticScore, rankChange = ps2.chaosFanaticRank - ps1.chaosFanaticRank });
            }
            if (ps1.commanderZilyanaScore < ps2.commanderZilyanaScore)
            {
                l.Add(new ChangedBossStats() { name = "Commander Zilyana", scoreChange = ps2.commanderZilyanaScore - ps1.commanderZilyanaScore, rankChange = ps2.commanderZilyanaRank - ps1.commanderZilyanaRank });
            }
            if (ps1.corporealBeastScore < ps2.corporealBeastScore)
            {
                l.Add(new ChangedBossStats() { name = "Corporeal Beast", scoreChange = ps2.corporealBeastScore - ps1.corporealBeastScore, rankChange = ps2.corporealBeastRank - ps1.corporealBeastRank });
            }
            if (ps1.crazyArchaeologistScore < ps2.crazyArchaeologistScore)
            {
                l.Add(new ChangedBossStats() { name = "Crazy Archaeologist", scoreChange = ps2.crazyArchaeologistScore - ps1.crazyArchaeologistScore, rankChange = ps2.crazyArchaeologistRank - ps1.crazyArchaeologistRank });
            }
            if (ps1.dagannothPrimeScore < ps2.dagannothPrimeScore)
            {
                l.Add(new ChangedBossStats() { name = "Dagannoth Prime", scoreChange = ps2.dagannothPrimeScore - ps1.dagannothPrimeScore, rankChange = ps2.dagannothPrimeRank - ps1.dagannothPrimeRank });
            }
            if (ps1.dagannothRexScore < ps2.dagannothRexScore)
            {
                l.Add(new ChangedBossStats() { name = "Dagannoth Rex", scoreChange = ps2.dagannothRexScore - ps1.dagannothRexScore, rankChange = ps2.dagannothRexRank - ps1.dagannothRexRank });
            }
            if (ps1.dagannothSupremeScore < ps2.dagannothSupremeScore)
            {
                l.Add(new ChangedBossStats() { name = "Dagannoth Supreme", scoreChange = ps2.dagannothSupremeScore - ps1.dagannothSupremeScore, rankChange = ps2.dagannothSupremeRank - ps1.dagannothSupremeRank });
            }
            if (ps1.derangedArchaeologistScore < ps2.derangedArchaeologistScore)
            {
                l.Add(new ChangedBossStats() { name = "Deranged Archaeologist", scoreChange = ps2.derangedArchaeologistScore - ps1.derangedArchaeologistScore, rankChange = ps2.derangedArchaeologistRank - ps1.derangedArchaeologistRank });
            }
            if (ps1.dukeSucellusScore < ps2.dukeSucellusScore)
            {
                l.Add(new ChangedBossStats() { name = "Duke Sucellus", scoreChange = ps2.dukeSucellusScore - ps1.dukeSucellusScore, rankChange = ps2.dukeSucellusRank - ps1.dukeSucellusRank });
            }
            if (ps1.generalGraardorScore < ps2.generalGraardorScore)
            {
                l.Add(new ChangedBossStats() { name = "General Graardor", scoreChange = ps2.generalGraardorScore - ps1.generalGraardorScore, rankChange = ps2.generalGraardorRank - ps1.generalGraardorRank });
            }
            if (ps1.giantMoleScore < ps2.giantMoleScore)
            {
                l.Add(new ChangedBossStats() { name = "Giant Mole", scoreChange = ps2.giantMoleScore - ps1.giantMoleScore, rankChange = ps2.giantMoleRank - ps1.giantMoleRank });
            }
            if (ps1.grotesqueGuardiansScore < ps2.grotesqueGuardiansScore)
            {
                l.Add(new ChangedBossStats() { name = "Grotesque Guardians", scoreChange = ps2.grotesqueGuardiansScore - ps1.grotesqueGuardiansScore, rankChange = ps2.grotesqueGuardiansRank - ps1.grotesqueGuardiansRank });
            }
            if (ps1.hesporiScore < ps2.hesporiScore)
            {
                l.Add(new ChangedBossStats() { name = "Hespori", scoreChange = ps2.hesporiScore - ps1.hesporiScore, rankChange = ps2.hesporiRank - ps1.hesporiRank });
            }
            if (ps1.kalphiteQueenScore < ps2.kalphiteQueenScore)
            {
                l.Add(new ChangedBossStats() { name = "Kalphite Queen", scoreChange = ps2.kalphiteQueenScore - ps1.kalphiteQueenScore, rankChange = ps2.kalphiteQueenRank - ps1.kalphiteQueenRank });
            }
            if (ps1.kingBlackDragonScore < ps2.kingBlackDragonScore)
            {
                l.Add(new ChangedBossStats() { name = "King Black Dragon", scoreChange = ps2.kingBlackDragonScore - ps1.kingBlackDragonScore, rankChange = ps2.kingBlackDragonRank - ps1.kingBlackDragonRank });
            }
            if (ps1.krakenScore < ps2.krakenScore)
            {
                l.Add(new ChangedBossStats() { name = "Kraken", scoreChange = ps2.krakenScore - ps1.krakenScore, rankChange = ps2.krakenRank - ps1.krakenRank });
            }
            if (ps1.kreeArraScore < ps2.kreeArraScore)
            {
                l.Add(new ChangedBossStats() { name = "Kree\'Arra", scoreChange = ps2.kreeArraScore - ps1.kreeArraScore, rankChange = ps2.kreeArraRank - ps1.kreeArraRank });
            }
            if (ps1.krilTsutsarothScore < ps2.krilTsutsarothScore)
            {
                l.Add(new ChangedBossStats() { name = "K\'ril Tsutsaroth", scoreChange = ps2.krilTsutsarothScore - ps1.krilTsutsarothScore, rankChange = ps2.krilTsutsarothRank - ps1.krilTsutsarothRank });
            }
            if (ps1.lunarChestsScore < ps2.lunarChestsScore)
            {
                l.Add(new ChangedBossStats() { name = "Lunar Chests", scoreChange = ps2.lunarChestsScore - ps1.lunarChestsScore, rankChange = ps2.lunarChestsRank - ps1.lunarChestsRank });
            }
            if (ps1.mimicScore < ps2.mimicScore)
            {
                l.Add(new ChangedBossStats() { name = "Mimic", scoreChange = ps2.mimicScore - ps1.mimicScore, rankChange = ps2.mimicRank - ps1.mimicRank });
            }
            if (ps1.nexScore < ps2.nexScore)
            {
                l.Add(new ChangedBossStats() { name = "Nex", scoreChange = ps2.nexScore - ps1.nexScore, rankChange = ps2.nexRank - ps1.nexRank });
            }
            if (ps1.nightmareScore < ps2.nightmareScore)
            {
                l.Add(new ChangedBossStats() { name = "Nightmare", scoreChange = ps2.nightmareScore - ps1.nightmareScore, rankChange = ps2.nightmareRank - ps1.nightmareRank });
            }
            if (ps1.phosanisNightmareScore < ps2.phosanisNightmareScore)
            {
                l.Add(new ChangedBossStats() { name = "Phosani\'s Nightmare", scoreChange = ps2.phosanisNightmareScore - ps1.phosanisNightmareScore, rankChange = ps2.phosanisNightmareRank - ps1.phosanisNightmareRank });
            }
            if (ps1.oborScore < ps2.oborScore)
            {
                l.Add(new ChangedBossStats() { name = "Obor", scoreChange = ps2.oborScore - ps1.oborScore, rankChange = ps2.oborRank - ps1.oborRank });
            }
            if (ps1.phantomMuspahScore < ps2.phantomMuspahScore)
            {
                l.Add(new ChangedBossStats() { name = "Phantom Muspah", scoreChange = ps2.phantomMuspahScore - ps1.phantomMuspahScore, rankChange = ps2.phantomMuspahRank - ps1.phantomMuspahRank });
            }
            if (ps1.sarachnisScore < ps2.sarachnisScore)
            {
                l.Add(new ChangedBossStats() { name = "Sarachnis", scoreChange = ps2.sarachnisScore - ps1.sarachnisScore, rankChange = ps2.sarachnisRank - ps1.sarachnisRank });
            }
            if (ps1.scorpiaScore < ps2.scorpiaScore)
            {
                l.Add(new ChangedBossStats() { name = "Scorpia", scoreChange = ps2.scorpiaScore - ps1.scorpiaScore, rankChange = ps2.scorpiaRank - ps1.scorpiaRank });
            }
            if (ps1.scurriusScore < ps2.scurriusScore)
            {
                l.Add(new ChangedBossStats() { name = "Scurrius", scoreChange = ps2.scurriusScore - ps1.scurriusScore, rankChange = ps2.scurriusRank - ps1.scurriusRank });
            }
            if (ps1.skotizoScore < ps2.skotizoScore)
            {
                l.Add(new ChangedBossStats() { name = "Skotizo", scoreChange = ps2.skotizoScore - ps1.skotizoScore, rankChange = ps2.skotizoRank - ps1.skotizoRank });
            }
            if (ps1.solHereditScore < ps2.solHereditScore)
            {
                l.Add(new ChangedBossStats() { name = "Sol Heredit", scoreChange = ps2.solHereditScore - ps1.solHereditScore, rankChange = ps2.solHereditRank - ps1.solHereditRank });
            }
            if (ps1.spindelScore < ps2.spindelScore)
            {
                l.Add(new ChangedBossStats() { name = "Spindel", scoreChange = ps2.spindelScore - ps1.spindelScore, rankChange = ps2.spindelRank - ps1.spindelRank });
            }
            if (ps1.temporossScore < ps2.temporossScore)
            {
                l.Add(new ChangedBossStats() { name = "Tempoross", scoreChange = ps2.temporossScore - ps1.temporossScore, rankChange = ps2.temporossRank - ps1.temporossRank });
            }
            if (ps1.theGauntletScore < ps2.theGauntletScore)
            {
                l.Add(new ChangedBossStats() { name = "The Gauntlet", scoreChange = ps2.theGauntletScore - ps1.theGauntletScore, rankChange = ps2.theGauntletRank - ps1.theGauntletRank });
            }
            if (ps1.theCorruptedGauntletScore < ps2.theCorruptedGauntletScore)
            {
                l.Add(new ChangedBossStats() { name = "The Corrupted Gauntlet", scoreChange = ps2.theCorruptedGauntletScore - ps1.theCorruptedGauntletScore, rankChange = ps2.theCorruptedGauntletRank - ps1.theCorruptedGauntletRank });
            }
            if (ps1.theLeviathanScore < ps2.theLeviathanScore)
            {
                l.Add(new ChangedBossStats() { name = "The Leviathan", scoreChange = ps2.theLeviathanScore - ps1.theLeviathanScore, rankChange = ps2.theLeviathanRank - ps1.theLeviathanRank });
            }
            if (ps1.theWhispererScore < ps2.theWhispererScore)
            {
                l.Add(new ChangedBossStats() { name = "The Whisperer", scoreChange = ps2.theWhispererScore - ps1.theWhispererScore, rankChange = ps2.theWhispererRank - ps1.theWhispererRank });
            }
            if (ps1.theatreOfBloodScore < ps2.theatreOfBloodScore)
            {
                l.Add(new ChangedBossStats() { name = "Theatre of Blood", scoreChange = ps2.theatreOfBloodScore - ps1.theatreOfBloodScore, rankChange = ps2.theatreOfBloodRank - ps1.theatreOfBloodRank });
            }
            if (ps1.theatreOfBloodHardModeScore < ps2.theatreOfBloodHardModeScore)
            {
                l.Add(new ChangedBossStats() { name = "Theatre of Blood: Hard Mode", scoreChange = ps2.theatreOfBloodHardModeScore - ps1.theatreOfBloodHardModeScore, rankChange = ps2.theatreOfBloodHardModeRank - ps1.theatreOfBloodHardModeRank });
            }
            if (ps1.thermonuclearSmokeDevilScore < ps2.thermonuclearSmokeDevilScore)
            {
                l.Add(new ChangedBossStats() { name = "Thermonuclear Smoke Devil", scoreChange = ps2.thermonuclearSmokeDevilScore - ps1.thermonuclearSmokeDevilScore, rankChange = ps2.thermonuclearSmokeDevilRank - ps1.thermonuclearSmokeDevilRank });
            }
            if (ps1.tombsOfAmascutScore < ps2.tombsOfAmascutScore)
            {
                l.Add(new ChangedBossStats() { name = "Tombs of Amascut", scoreChange = ps2.tombsOfAmascutScore - ps1.tombsOfAmascutScore, rankChange = ps2.tombsOfAmascutRank - ps1.tombsOfAmascutRank });
            }
            if (ps1.tombsOfAmascutExpertModeScore < ps2.tombsOfAmascutExpertModeScore)
            {
                l.Add(new ChangedBossStats() { name = "Tombs of Amascut: Expert Mode", scoreChange = ps2.tombsOfAmascutExpertModeScore - ps1.tombsOfAmascutExpertModeScore, rankChange = ps2.tombsOfAmascutExpertModeRank - ps1.tombsOfAmascutExpertModeRank });
            }
            if (ps1.tzkalZukScore < ps2.tzkalZukScore)
            {
                l.Add(new ChangedBossStats() { name = "TzKal-Zuk", scoreChange = ps2.tzkalZukScore - ps1.tzkalZukScore, rankChange = ps2.tzkalZukRank - ps1.tzkalZukRank });
            }
            if (ps1.tztokJadScore < ps2.tztokJadScore)
            {
                l.Add(new ChangedBossStats() { name = "TzTok-Jad", scoreChange = ps2.tztokJadScore - ps1.tztokJadScore, rankChange = ps2.tztokJadRank - ps1.tztokJadRank });
            }
            if (ps1.vardorvisScore < ps2.vardorvisScore)
            {
                l.Add(new ChangedBossStats() { name = "Vardorvis", scoreChange = ps2.vardorvisScore - ps1.vardorvisScore, rankChange = ps2.vardorvisRank - ps1.vardorvisRank });
            }
            if (ps1.venenatisScore < ps2.venenatisScore)
            {
                l.Add(new ChangedBossStats() { name = "Venenatis", scoreChange = ps2.venenatisScore - ps1.venenatisScore, rankChange = ps2.venenatisRank - ps1.venenatisRank });
            }
            if (ps1.vetionScore < ps2.vetionScore)
            {
                l.Add(new ChangedBossStats() { name = "Vet\'ion", scoreChange = ps2.vetionScore - ps1.vetionScore, rankChange = ps2.vetionRank - ps1.vetionRank });
            }
            if (ps1.vorkathScore < ps2.vorkathScore)
            {
                l.Add(new ChangedBossStats() { name = "Vorkath", scoreChange = ps2.vorkathScore - ps1.vorkathScore, rankChange = ps2.vorkathRank - ps1.vorkathRank });
            }
            if (ps1.wintertodtScore < ps2.wintertodtScore)
            {
                l.Add(new ChangedBossStats() { name = "Wintertodt", scoreChange = ps2.wintertodtScore - ps1.wintertodtScore, rankChange = ps2.wintertodtRank - ps1.wintertodtRank });
            }
            if (ps1.zalcanoScore < ps2.zalcanoScore)
            {
                l.Add(new ChangedBossStats() { name = "Zalcano", scoreChange = ps2.zalcanoScore - ps1.zalcanoScore, rankChange = ps2.zalcanoRank - ps1.zalcanoRank });
            }
            if (ps1.zulrahScore < ps2.zulrahScore)
            {
                l.Add(new ChangedBossStats() { name = "Zulrah", scoreChange = ps2.zulrahScore - ps1.zulrahScore, rankChange = ps2.zulrahRank - ps1.zulrahRank });
            }

            return l;
        }
    }
    internal class ChangedSkillStats
    {
        public string name;
        public int xpChange;
        public int levelChange;
        public int startingLevel;
        public int rankChange;

    }
    internal class ChangedBossStats
    {
        public string name;
        public int scoreChange;
        public int rankChange;

    }
    internal class CompareSkillStats : IComparer<ChangedSkillStats>
    {
        public int Compare(ChangedSkillStats x, ChangedSkillStats y)
        {
            return x.xpChange - y.xpChange;
        }
    }
}
