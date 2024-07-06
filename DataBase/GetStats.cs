using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;
using System.ComponentModel;
using static System.Net.WebRequestMethods;
using Quartz;
using System.Data.SqlTypes;

namespace OSRSXPTracker.DataBase
{
    internal class GetStats
    {
        public const string osrsEndpointUrl = "https://secure.runescape.com/m=hiscore_oldschool/index_lite.json?player=";
        public static async Task<JsonClasses.Root?> GetAsync(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<JsonClasses.Root>();
                }
                else
                {
                    return null;
                }
                
            }
        }
        public async static Task<PlayerStats?> getPlayerData(string username)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"{osrsEndpointUrl}{username}");
                if (response.IsSuccessStatusCode)
                {
                    JsonClasses.Root? json = await response.Content.ReadFromJsonAsync<JsonClasses.Root>();
                    if(json == null) { return null; }
                    PlayerStats ps = json.ToPlayerStats();
                    return ps;
                }
                else
                {
                    return null;
                }

            }
        }

    }
    public class UpdateXP : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"XP task executed at: {DateTime.Now}");
            List<Player> players = await PlayerDB.active.GetAllPlayers();
            List<PlayerStats> stats = new List<PlayerStats>();
            foreach (Player p in players)
            {
                PlayerStats? ps = await GetStats.getPlayerData(p.Id);
                if (ps == null)
                {
                    Console.WriteLine($"{p.Id} data returned null idiot");
                    continue;
                }
                stats.Add(ps);

            }
            await PlayerDB.active.AddPlayerStatsList(stats);
            await Task.CompletedTask;
        }
    }
    class JsonClasses
    {
        public class Activity
        {
            [JsonProperty("id")]
            public int id { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("rank")]
            public int rank { get; set; }

            [JsonProperty("score")]
            public int score { get; set; }
        }

        public class Root
        {
            [JsonProperty("skills")]
            public List<Skill> skills { get; set; }

            [JsonProperty("activities")]
            public List<Activity> activities { get; set; }

            public PlayerStats ToPlayerStats()
            {
                PlayerStats ps = new PlayerStats();
                
                //Skills
                for (int i = 0; i < skills.Count; i++)
                {
                    Skill s = skills[i];
                    if (s.xp == -1) { s.xp = 0; }
                    if (s.rank == -1) { s.rank = 0; }
                    if (s.level == -1) { s.level = 1; }

                    switch (s.name)
                    {

                        case "Overall":
                            ps.overallXP = s.xp;
                            ps.overallRank = s.rank;
                            ps.overallLevel = s.level;
                            break;
                        case "Attack":
                            ps.attackXP = s.xp;
                            ps.attackRank = s.rank;
                            ps.attackLevel = s.level;
                            break;
                        case "Defence":
                            ps.defenceXP = s.xp;
                            ps.defenceRank = s.rank;
                            ps.defenceLevel = s.level;
                            break;
                        case "Strength":
                            ps.strengthXP = s.xp;
                            ps.strengthRank = s.rank;
                            ps.strengthLevel = s.level;
                            break;
                        case "Hitpoints":
                            ps.hitpointsXP = s.xp;
                            ps.hitpointsRank = s.rank;
                            ps.hitpointsLevel = s.level;
                            break;
                        case "Ranged":
                            ps.rangedXP = s.xp;
                            ps.rangedRank = s.rank;
                            ps.rangedLevel = s.level;
                            break;
                        case "Prayer":
                            ps.prayerXP = s.xp;
                            ps.prayerRank = s.rank;
                            ps.prayerLevel = s.level;
                            break;
                        case "Magic":
                            ps.magicXP = s.xp;
                            ps.magicRank = s.rank;
                            ps.magicLevel = s.level;
                            break;
                        case "Cooking":
                            ps.cookingXP = s.xp;
                            ps.cookingRank = s.rank;
                            ps.cookingLevel = s.level;
                            break;
                        case "Woodcutting":
                            ps.woodcuttingXP = s.xp;
                            ps.woodcuttingRank = s.rank;
                            ps.woodcuttingLevel = s.level;
                            break;
                        case "Fletching":
                            ps.fletchingXP = s.xp;
                            ps.fletchingRank = s.rank;
                            ps.fletchingLevel = s.level;
                            break;
                        case "Fishing":
                            ps.fishingXP = s.xp;
                            ps.fishingRank = s.rank;
                            ps.fishingLevel = s.level;
                            break;
                        case "Firemaking":
                            ps.firemakingXP = s.xp;
                            ps.firemakingRank = s.rank;
                            ps.firemakingLevel = s.level;
                            break;
                        case "Crafting":
                            ps.craftingXP = s.xp;
                            ps.craftingRank = s.rank;
                            ps.craftingLevel = s.level;
                            break;
                        case "Smithing":
                            ps.smithingXP = s.xp;
                            ps.smithingRank = s.rank;
                            ps.smithingLevel = s.level;
                            break;
                        case "Mining":
                            ps.miningXP = s.xp;
                            ps.miningRank = s.rank;
                            ps.miningLevel = s.level;
                            break;
                        case "Herblore":
                            ps.herbloreXP = s.xp;
                            ps.herbloreRank = s.rank;
                            ps.herbloreLevel = s.level;
                            break;
                        case "Agility":
                            ps.agilityXP = s.xp;
                            ps.agilityRank = s.rank;
                            ps.agilityLevel = s.level;
                            break;
                        case "Thieving":
                            ps.thievingXP = s.xp;
                            ps.thievingRank = s.rank;
                            ps.thievingLevel = s.level;
                            break;
                        case "Slayer":
                            ps.slayerXP = s.xp;
                            ps.slayerRank = s.rank;
                            ps.slayerLevel = s.level;
                            break;
                        case "Farming":
                            ps.farmingXP = s.xp;
                            ps.farmingRank = s.rank;
                            ps.farmingLevel = s.level;
                            break;
                        case "Runecraft":
                            ps.runecraftXP = s.xp;
                            ps.runecraftRank = s.rank;
                            ps.runecraftLevel = s.level;
                            break;
                        case "Hunter":
                            ps.hunterXP = s.xp;
                            ps.hunterRank = s.rank;
                            ps.hunterLevel = s.level;
                            break;
                        case "Construction":
                            ps.constructionXP = s.xp;
                            ps.constructionRank = s.rank;
                            ps.constructionLevel = s.level;
                            break;

                        default:
                            Console.WriteLine("Sailing is out");
                            break;

                    }
                }
                for (int i = 0; i < activities.Count; i++)
                {
                    Activity s = activities[i];
                    if (s.rank == -1) { s.rank = 0; }
                    if (s.score == -1) { s.score = 0; }
                    switch (s.name) {
                        case "League Points":
                            ps.leaguePointsRank = s.rank;
                            ps.leaguePointsScore = s.score;
                            break;
                        case "Deadman Points":
                            ps.deadmanPointsRank = s.rank;
                            ps.deadmanPointsScore = s.score;
                            break;
                        case "Bounty Hunter - Hunter":
                            ps.bountyHunterHunterRank = s.rank;
                            ps.bountyHunterHunterScore = s.score;
                            break;
                        case "Bounty Hunter - Rogue":
                            ps.bountyHunterRogueRank = s.rank;
                            ps.bountyHunterRogueScore = s.score;
                            break;
                        case "Bounty Hunter (Legacy) - Hunter":
                            ps.bountyHunterLegacyHunterRank = s.rank;
                            ps.bountyHunterLegacyHunterScore = s.score;
                            break;
                        case "Bounty Hunter (Legacy) - Rogue":
                            ps.bountyHunterLegacyRogueRank = s.rank;
                            ps.bountyHunterLegacyRogueScore = s.score;
                            break;
                        case "Clue Scrolls (all)":
                            ps.clueScrollsAllRank = s.rank;
                            ps.clueScrollsAllScore = s.score;
                            break;
                        case "Clue Scrolls (beginner)":
                            ps.clueScrollsBeginnerRank = s.rank;
                            ps.clueScrollsBeginnerScore = s.score;
                            break;
                        case "Clue Scrolls (easy)":
                            ps.clueScrollsEasyRank = s.rank;
                            ps.clueScrollsEasyScore = s.score;
                            break;
                        case "Clue Scrolls (medium)":
                            ps.clueScrollsMediumRank = s.rank;
                            ps.clueScrollsMediumScore = s.score;
                            break;
                        case "Clue Scrolls (hard)":
                            ps.clueScrollsHardRank = s.rank;
                            ps.clueScrollsHardScore = s.score;
                            break;
                        case "Clue Scrolls (elite)":
                            ps.clueScrollsEliteRank = s.rank;
                            ps.clueScrollsEliteScore = s.score;
                            break;
                        case "Clue Scrolls (master)":
                            ps.clueScrollsMasterRank = s.rank;
                            ps.clueScrollsMasterScore = s.score;
                            break;
                        case "LMS - Rank":
                            ps.lmsRankRank = s.rank;
                            ps.lmsRankScore = s.score;
                            break;
                        case "PvP Arena - Rank":
                            ps.pvpArenaRankRank = s.rank;
                            ps.pvpArenaRankScore = s.score;
                            break;
                        case "Soul Wars Zeal":
                            ps.soulWarsZealRank = s.rank;
                            ps.soulWarsZealScore = s.score;
                            break;
                        case "Rifts closed":
                            ps.riftsClosedRank = s.rank;
                            ps.riftsClosedScore = s.score;
                            break;
                        case "Colosseum Glory":
                            ps.colosseumGloryRank = s.rank;
                            ps.colosseumGloryScore = s.score;
                            break;
                        case "Abyssal Sire":
                            ps.abyssalSireRank = s.rank;
                            ps.abyssalSireScore = s.score;
                            break;
                        case "Alchemical Hydra":
                            ps.alchemicalHydraRank = s.rank;
                            ps.alchemicalHydraScore = s.score;
                            break;
                        case "Artio":
                            ps.artioRank = s.rank;
                            ps.artioScore = s.score;
                            break;
                        case "Barrows Chests":
                            ps.barrowsChestsRank = s.rank;
                            ps.barrowsChestsScore = s.score;
                            break;
                        case "Bryophyta":
                            ps.bryophytaRank = s.rank;
                            ps.bryophytaScore = s.score;
                            break;
                        case "Callisto":
                            ps.callistoRank = s.rank;
                            ps.callistoScore = s.score;
                            break;
                        case "Calvar'ion":
                            ps.calvarionRank = s.rank;
                            ps.calvarionScore = s.score;
                            break;
                        case "Cerberus":
                            ps.cerberusRank = s.rank;
                            ps.cerberusScore = s.score;
                            break;
                        case "Chambers of Xeric":
                            ps.chambersOfXericRank = s.rank;
                            ps.chambersOfXericScore = s.score;
                            break;
                        case "Chambers of Xeric: Challenge Mode":
                            ps.chambersOfXericChallengeModeRank = s.rank;
                            ps.chambersOfXericChallengeModeScore = s.score;
                            break;
                        case "Chaos Elemental":
                            ps.chaosElementalRank = s.rank;
                            ps.chaosElementalScore = s.score;
                            break;
                        case "Chaos Fanatic":
                            ps.chaosFanaticRank = s.rank;
                            ps.chaosFanaticScore = s.score;
                            break;
                        case "Commander Zilyana":
                            ps.commanderZilyanaRank = s.rank;
                            ps.commanderZilyanaScore = s.score;
                            break;
                        case "Corporeal Beast":
                            ps.corporealBeastRank = s.rank;
                            ps.corporealBeastScore = s.score;
                            break;
                        case "Crazy Archaeologist":
                            ps.crazyArchaeologistRank = s.rank;
                            ps.crazyArchaeologistScore = s.score;
                            break;
                        case "Dagannoth Prime":
                            ps.dagannothPrimeRank = s.rank;
                            ps.dagannothPrimeScore = s.score;
                            break;
                        case "Dagannoth Rex":
                            ps.dagannothRexRank = s.rank;
                            ps.dagannothRexScore = s.score;
                            break;
                        case "Dagannoth Supreme":
                            ps.dagannothSupremeRank = s.rank;
                            ps.dagannothSupremeScore = s.score;
                            break;
                        case "Deranged Archaeologist":
                            ps.derangedArchaeologistRank = s.rank;
                            ps.derangedArchaeologistScore = s.score;
                            break;
                        case "Duke Sucellus":
                            ps.dukeSucellusRank = s.rank;
                            ps.dukeSucellusScore = s.score;
                            break;
                        case "General Graardor":
                            ps.generalGraardorRank = s.rank;
                            ps.generalGraardorScore = s.score;
                            break;
                        case "Giant Mole":
                            ps.giantMoleRank = s.rank;
                            ps.giantMoleScore = s.score;
                            break;
                        case "Grotesque Guardians":
                            ps.grotesqueGuardiansRank = s.rank;
                            ps.grotesqueGuardiansScore = s.score;
                            break;
                        case "Hespori":
                            ps.hesporiRank = s.rank;
                            ps.hesporiScore = s.score;
                            break;
                        case "Kalphite Queen":
                            ps.kalphiteQueenRank = s.rank;
                            ps.kalphiteQueenScore = s.score;
                            break;
                        case "King Black Dragon":
                            ps.kingBlackDragonRank = s.rank;
                            ps.kingBlackDragonScore = s.score;
                            break;
                        case "Kraken":
                            ps.krakenRank = s.rank;
                            ps.krakenScore = s.score;
                            break;
                        case "Kree'Arra":
                            ps.kreeArraRank = s.rank;
                            ps.kreeArraScore = s.score;
                            break;
                        case "K'ril Tsutsaroth":
                            ps.krilTsutsarothRank = s.rank;
                            ps.krilTsutsarothScore = s.score;
                            break;
                        case "Lunar Chests":
                            ps.lunarChestsRank = s.rank;
                            ps.lunarChestsScore = s.score;
                            break;
                        case "Mimic":
                            ps.mimicRank = s.rank;
                            ps.mimicScore = s.score;
                            break;
                        case "Nex":
                            ps.nexRank = s.rank;
                            ps.nexScore = s.score;
                            break;
                        case "Nightmare":
                            ps.nightmareRank = s.rank;
                            ps.nightmareScore = s.score;
                            break;
                        case "Phosani's Nightmare":
                            ps.phosanisNightmareRank = s.rank;
                            ps.phosanisNightmareScore = s.score;
                            break;
                        case "Obor":
                            ps.oborRank = s.rank;
                            ps.oborScore = s.score;
                            break;
                        case "Phantom Muspah":
                            ps.phantomMuspahRank = s.rank;
                            ps.phantomMuspahScore = s.score;
                            break;
                        case "Sarachnis":
                            ps.sarachnisRank = s.rank;
                            ps.sarachnisScore = s.score;
                            break;
                        case "Scorpia":
                            ps.scorpiaRank = s.rank;
                            ps.scorpiaScore = s.score;
                            break;
                        case "Scurrius":
                            ps.scurriusRank = s.rank;
                            ps.scurriusScore = s.score;
                            break;
                        case "Skotizo":
                            ps.skotizoRank = s.rank;
                            ps.skotizoScore = s.score;
                            break;
                        case "Sol Heredit":
                            ps.solHereditRank = s.rank;
                            ps.solHereditScore = s.score;
                            break;
                        case "Spindel":
                            ps.spindelRank = s.rank;
                            ps.spindelScore = s.score;
                            break;
                        case "Tempoross":
                            ps.temporossRank = s.rank;
                            ps.temporossScore = s.score;
                            break;
                        case "The Gauntlet":
                            ps.theGauntletRank = s.rank;
                            ps.theGauntletScore = s.score;
                            break;
                        case "The Corrupted Gauntlet":
                            ps.theCorruptedGauntletRank = s.rank;
                            ps.theCorruptedGauntletScore = s.score;
                            break;
                        case "The Leviathan":
                            ps.theLeviathanRank = s.rank;
                            ps.theLeviathanScore = s.score;
                            break;
                        case "The Whisperer":
                            ps.theWhispererRank = s.rank;
                            ps.theWhispererScore = s.score;
                            break;
                        case "Theatre of Blood":
                            ps.theatreOfBloodRank = s.rank;
                            ps.theatreOfBloodScore = s.score;
                            break;
                        case "Theatre of Blood: Hard Mode":
                            ps.theatreOfBloodHardModeRank = s.rank;
                            ps.theatreOfBloodHardModeScore = s.score;
                            break;
                        case "Thermonuclear Smoke Devil":
                            ps.thermonuclearSmokeDevilRank = s.rank;
                            ps.thermonuclearSmokeDevilScore = s.score;
                            break;
                        case "Tombs of Amascut":
                            ps.tombsOfAmascutRank = s.rank;
                            ps.tombsOfAmascutScore = s.score;
                            break;
                        case "Tombs of Amascut: Expert Mode":
                            ps.tombsOfAmascutExpertModeRank = s.rank;
                            ps.tombsOfAmascutExpertModeScore = s.score;
                            break;
                        case "TzKal-Zuk":
                            ps.tzkalZukRank = s.rank;
                            ps.tzkalZukScore = s.score;
                            break;
                        case "TzTok-Jad":
                            ps.tztokJadRank = s.rank;
                            ps.tztokJadScore = s.score;
                            break;
                        case "Vardorvis":
                            ps.vardorvisRank = s.rank;
                            ps.vardorvisScore = s.score;
                            break;
                        case "Venenatis":
                            ps.venenatisRank = s.rank;
                            ps.venenatisScore = s.score;
                            break;
                        case "Vet'ion":
                            ps.vetionRank = s.rank;
                            ps.vetionScore = s.score;
                            break;
                        case "Vorkath":
                            ps.vorkathRank = s.rank;
                            ps.vorkathScore = s.score;
                            break;
                        case "Wintertodt":
                            ps.wintertodtRank = s.rank;
                            ps.wintertodtScore = s.score;
                            break;
                        case "Zalcano":
                            ps.zalcanoRank = s.rank;
                            ps.zalcanoScore = s.score;
                            break;
                        case "Zulrah":
                            ps.zulrahRank = s.rank;
                            ps.zulrahScore = s.score;
                            break;
                        default:
                            Console.WriteLine("Spider boss is out");
                            break;
                    }

                }

                return ps;
            }
        }

        public class Skill
        {
            [JsonProperty("id")]
            public int id { get; set; }

            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("rank")]
            public int rank { get; set; }

            [JsonProperty("level")]
            public int level { get; set; }

            [JsonProperty("xp")]
            public int xp { get; set; }
        }


    }
}
