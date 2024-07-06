using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSRSXPTracker.DataBase
{
    class Player
    {
        [Key]
        public string Id { get; set; }
    }
    class PlayerStats
    {
        public int Id {  get; set; }
        public string PlayerId { get; set; }
        public long Timestamp { get; set; }

        public int overallXP { get; set; }
        public int overallRank { get; set; }
        public int overallLevel { get; set; }

        public int attackXP { get; set; }
        public int attackRank { get; set; }
        public int attackLevel { get; set; }

        public int defenceXP { get; set; }
        public int defenceRank { get; set; }
        public int defenceLevel { get; set; }

        public int strengthXP { get; set; }
        public int strengthRank { get; set; }
        public int strengthLevel { get; set; }

        public int hitpointsXP { get; set; }
        public int hitpointsRank { get; set; }
        public int hitpointsLevel { get; set; }

        public int rangedXP { get; set; }
        public int rangedRank { get; set; }
        public int rangedLevel { get; set; }

        public int prayerXP { get; set; }
        public int prayerRank { get; set; }
        public int prayerLevel { get; set; }

        public int magicXP { get; set; }
        public int magicRank { get; set; }
        public int magicLevel { get; set; }

        public int cookingXP { get; set; }
        public int cookingRank { get; set; }
        public int cookingLevel { get; set; }

        public int woodcuttingXP { get; set; }
        public int woodcuttingRank { get; set; }
        public int woodcuttingLevel { get; set; }

        public int fletchingXP { get; set; }
        public int fletchingRank { get; set; }
        public int fletchingLevel { get; set; }

        public int fishingXP { get; set; }
        public int fishingRank { get; set; }
        public int fishingLevel { get; set; }

        public int firemakingXP { get; set; }
        public int firemakingRank { get; set; }
        public int firemakingLevel { get; set; }

        public int craftingXP { get; set; }
        public int craftingRank { get; set; }
        public int craftingLevel { get; set; }

        public int smithingXP { get; set; }
        public int smithingRank { get; set; }
        public int smithingLevel { get; set; }

        public int miningXP { get; set; }
        public int miningRank { get; set; }
        public int miningLevel { get; set; }

        public int herbloreXP { get; set; }
        public int herbloreRank { get; set; }
        public int herbloreLevel { get; set; }

        public int agilityXP { get; set; }
        public int agilityRank { get; set; }
        public int agilityLevel { get; set; }

        public int thievingXP { get; set; }
        public int thievingRank { get; set; }
        public int thievingLevel { get; set; }

        public int slayerXP { get; set; }
        public int slayerRank { get; set; }
        public int slayerLevel { get; set; }

        public int farmingXP { get; set; }
        public int farmingRank { get; set; }
        public int farmingLevel { get; set; }

        public int runecraftXP { get; set; }
        public int runecraftRank { get; set; }
        public int runecraftLevel { get; set; }

        public int hunterXP { get; set; }
        public int hunterRank { get; set; }
        public int hunterLevel { get; set; }

        public int constructionXP { get; set; }
        public int constructionRank { get; set; }
        public int constructionLevel { get; set; }

        
        public int leaguePointsRank { get; set; }
        public int leaguePointsScore { get; set; }

        public int deadmanPointsRank { get; set; }
        public int deadmanPointsScore { get; set; }

        public int bountyHunterHunterRank { get; set; }
        public int bountyHunterHunterScore { get; set; }

        public int bountyHunterRogueRank { get; set; }
        public int bountyHunterRogueScore { get; set; }

        public int bountyHunterLegacyHunterRank { get; set; }
        public int bountyHunterLegacyHunterScore { get; set; }

        public int bountyHunterLegacyRogueRank { get; set; }
        public int bountyHunterLegacyRogueScore { get; set; }

        public int clueScrollsAllRank { get; set; }
        public int clueScrollsAllScore { get; set; }

        public int clueScrollsBeginnerRank { get; set; }
        public int clueScrollsBeginnerScore { get; set; }

        public int clueScrollsEasyRank { get; set; }
        public int clueScrollsEasyScore { get; set; }

        public int clueScrollsMediumRank { get; set; }
        public int clueScrollsMediumScore { get; set; }

        public int clueScrollsHardRank { get; set; }
        public int clueScrollsHardScore { get; set; }

        public int clueScrollsEliteRank { get; set; }
        public int clueScrollsEliteScore { get; set; }

        public int clueScrollsMasterRank { get; set; }
        public int clueScrollsMasterScore { get; set; }

        public int lmsRankRank { get; set; }
        public int lmsRankScore { get; set; }

        public int pvpArenaRankRank { get; set; }
        public int pvpArenaRankScore { get; set; }

        public int soulWarsZealRank { get; set; }
        public int soulWarsZealScore { get; set; }

        public int riftsClosedRank { get; set; }
        public int riftsClosedScore { get; set; }

        public int colosseumGloryRank { get; set; }
        public int colosseumGloryScore { get; set; }

        public int abyssalSireRank { get; set; }
        public int abyssalSireScore { get; set; }

        public int alchemicalHydraRank { get; set; }
        public int alchemicalHydraScore { get; set; }

        public int artioRank { get; set; }
        public int artioScore { get; set; }

        public int barrowsChestsRank { get; set; }
        public int barrowsChestsScore { get; set; }

        public int bryophytaRank { get; set; }
        public int bryophytaScore { get; set; }

        public int callistoRank { get; set; }
        public int callistoScore { get; set; }

        public int calvarionRank { get; set; }
        public int calvarionScore { get; set; }

        public int cerberusRank { get; set; }
        public int cerberusScore { get; set; }

        public int chambersOfXericRank { get; set; }
        public int chambersOfXericScore { get; set; }

        public int chambersOfXericChallengeModeRank { get; set; }
        public int chambersOfXericChallengeModeScore { get; set; }

        public int chaosElementalRank { get; set; }
        public int chaosElementalScore { get; set; }

        public int chaosFanaticRank { get; set; }
        public int chaosFanaticScore { get; set; }

        public int commanderZilyanaRank { get; set; }
        public int commanderZilyanaScore { get; set; }

        public int corporealBeastRank { get; set; }
        public int corporealBeastScore { get; set; }

        public int crazyArchaeologistRank { get; set; }
        public int crazyArchaeologistScore { get; set; }

        public int dagannothPrimeRank { get; set; }
        public int dagannothPrimeScore { get; set; }

        public int dagannothRexRank { get; set; }
        public int dagannothRexScore { get; set; }

        public int dagannothSupremeRank { get; set; }
        public int dagannothSupremeScore { get; set; }

        public int derangedArchaeologistRank { get; set; }
        public int derangedArchaeologistScore { get; set; }

        public int dukeSucellusRank { get; set; }
        public int dukeSucellusScore { get; set; }

        public int generalGraardorRank { get; set; }
        public int generalGraardorScore { get; set; }

        public int giantMoleRank { get; set; }
        public int giantMoleScore { get; set; }

        public int grotesqueGuardiansRank { get; set; }
        public int grotesqueGuardiansScore { get; set; }

        public int hesporiRank { get; set; }
        public int hesporiScore { get; set; }

        public int kalphiteQueenRank { get; set; }
        public int kalphiteQueenScore { get; set; }

        public int kingBlackDragonRank { get; set; }
        public int kingBlackDragonScore { get; set; }

        public int krakenRank { get; set; }
        public int krakenScore { get; set; }

        public int kreeArraRank { get; set; }
        public int kreeArraScore { get; set; }

        public int krilTsutsarothRank { get; set; }
        public int krilTsutsarothScore { get; set; }

        public int lunarChestsRank { get; set; }
        public int lunarChestsScore { get; set; }

        public int mimicRank { get; set; }
        public int mimicScore { get; set; }

        public int nexRank { get; set; }
        public int nexScore { get; set; }

        public int nightmareRank { get; set; }
        public int nightmareScore { get; set; }

        public int phosanisNightmareRank { get; set; }
        public int phosanisNightmareScore { get; set; }

        public int oborRank { get; set; }
        public int oborScore { get; set; }

        public int phantomMuspahRank { get; set; }
        public int phantomMuspahScore { get; set; }

        public int sarachnisRank { get; set; }
        public int sarachnisScore { get; set; }

        public int scorpiaRank { get; set; }
        public int scorpiaScore { get; set; }

        public int scurriusRank { get; set; }
        public int scurriusScore { get; set; }

        public int skotizoRank { get; set; }
        public int skotizoScore { get; set; }

        public int solHereditRank { get; set; }
        public int solHereditScore { get; set; }

        public int spindelRank { get; set; }
        public int spindelScore { get; set; }

        public int temporossRank { get; set; }
        public int temporossScore { get; set; }

        public int theGauntletRank { get; set; }
        public int theGauntletScore { get; set; }

        public int theCorruptedGauntletRank { get; set; }
        public int theCorruptedGauntletScore { get; set; }

        public int theLeviathanRank { get; set; }
        public int theLeviathanScore { get; set; }

        public int theWhispererRank { get; set; }
        public int theWhispererScore { get; set; }

        public int theatreOfBloodRank { get; set; }
        public int theatreOfBloodScore { get; set; }

        public int theatreOfBloodHardModeRank { get; set; }
        public int theatreOfBloodHardModeScore { get; set; }

        public int thermonuclearSmokeDevilRank { get; set; }
        public int thermonuclearSmokeDevilScore { get; set; }

        public int tombsOfAmascutRank { get; set; }
        public int tombsOfAmascutScore { get; set; }

        public int tombsOfAmascutExpertModeRank { get; set; }
        public int tombsOfAmascutExpertModeScore { get; set; }

        public int tzkalZukRank { get; set; }
        public int tzkalZukScore { get; set; }

        public int tztokJadRank { get; set; }
        public int tztokJadScore { get; set; }

        public int vardorvisRank { get; set; }
        public int vardorvisScore { get; set; }

        public int venenatisRank { get; set; }
        public int venenatisScore { get; set; }

        public int vetionRank { get; set; }
        public int vetionScore { get; set; }

        public int vorkathRank { get; set; }
        public int vorkathScore { get; set; }

        public int wintertodtRank { get; set; }
        public int wintertodtScore { get; set; }

        public int zalcanoRank { get; set; }
        public int zalcanoScore { get; set; }

        public int zulrahRank { get; set; }
        public int zulrahScore { get; set; }
        



    }
}
