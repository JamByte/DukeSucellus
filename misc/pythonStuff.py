#Misc scripts to generate stuff
import json
j = """{"skills":[{"id":0,"name":"Overall","rank":1272147,"level":1429,"xp":24320820},{"id":1,"name":"Attack","rank":-1,"level":1,"xp":-1},{"id":2,"name":"Defence","rank":-1,"level":1,"xp":-1},{"id":3,"name":"Strength","rank":-1,"level":1,"xp":-1},{"id":4,"name":"Hitpoints","rank":-1,"level":1,"xp":-1},{"id":5,"name":"Ranged","rank":1499932,"level":77,"xp":1480269},{"id":6,"name":"Prayer","rank":-1,"level":1,"xp":-1},{"id":7,"name":"Magic","rank":1506964,"level":76,"xp":1342109},{"id":8,"name":"Cooking","rank":1601469,"level":68,"xp":625054},{"id":9,"name":"Woodcutting","rank":613238,"level":80,"xp":2063083},{"id":10,"name":"Fletching","rank":908365,"level":74,"xp":1097774},{"id":11,"name":"Fishing","rank":470116,"level":83,"xp":2874592},{"id":12,"name":"Firemaking","rank":1073724,"level":75,"xp":1218655},{"id":13,"name":"Crafting","rank":984787,"level":70,"xp":802030},{"id":14,"name":"Smithing","rank":1897326,"level":50,"xp":101911},{"id":15,"name":"Mining","rank":299700,"level":86,"xp":3799319},{"id":16,"name":"Herblore","rank":1830867,"level":36,"xp":27363},{"id":17,"name":"Agility","rank":708455,"level":73,"xp":1072404},{"id":18,"name":"Thieving","rank":953801,"level":64,"xp":416780},{"id":19,"name":"Slayer","rank":1639056,"level":53,"xp":136768},{"id":20,"name":"Farming","rank":1273990,"level":53,"xp":145327},{"id":21,"name":"Runecraft","rank":1885457,"level":26,"xp":9018},{"id":22,"name":"Hunter","rank":158523,"level":91,"xp":5963494},{"id":23,"name":"Construction","rank":1170484,"level":52,"xp":134320}],"activities":[{"id":0,"name":"League Points","rank":-1,"score":-1},{"id":1,"name":"Deadman Points","rank":-1,"score":-1},{"id":2,"name":"Bounty Hunter - Hunter","rank":-1,"score":-1},{"id":3,"name":"Bounty Hunter - Rogue","rank":-1,"score":-1},{"id":4,"name":"Bounty Hunter (Legacy) - Hunter","rank":-1,"score":-1},{"id":5,"name":"Bounty Hunter (Legacy) - Rogue","rank":-1,"score":-1},{"id":6,"name":"Clue Scrolls (all)","rank":681240,"score":42},{"id":7,"name":"Clue Scrolls (beginner)","rank":649882,"score":5},{"id":8,"name":"Clue Scrolls (easy)","rank":379111,"score":14},{"id":9,"name":"Clue Scrolls (medium)","rank":580805,"score":13},{"id":10,"name":"Clue Scrolls (hard)","rank":699734,"score":10},{"id":11,"name":"Clue Scrolls (elite)","rank":-1,"score":-1},{"id":12,"name":"Clue Scrolls (master)","rank":-1,"score":-1},{"id":13,"name":"LMS - Rank","rank":236677,"score":529},{"id":14,"name":"PvP Arena - Rank","rank":-1,"score":-1},{"id":15,"name":"Soul Wars Zeal","rank":-1,"score":-1},{"id":16,"name":"Rifts closed","rank":-1,"score":-1},{"id":17,"name":"Colosseum Glory","rank":-1,"score":-1},{"id":18,"name":"Abyssal Sire","rank":-1,"score":-1},{"id":19,"name":"Alchemical Hydra","rank":-1,"score":-1},{"id":20,"name":"Artio","rank":-1,"score":-1},{"id":21,"name":"Barrows Chests","rank":-1,"score":-1},{"id":22,"name":"Bryophyta","rank":-1,"score":-1},{"id":23,"name":"Callisto","rank":-1,"score":-1},{"id":24,"name":"Calvar'ion","rank":-1,"score":-1},{"id":25,"name":"Cerberus","rank":-1,"score":-1},{"id":26,"name":"Chambers of Xeric","rank":-1,"score":-1},{"id":27,"name":"Chambers of Xeric: Challenge Mode","rank":-1,"score":-1},{"id":28,"name":"Chaos Elemental","rank":-1,"score":-1},{"id":29,"name":"Chaos Fanatic","rank":-1,"score":-1},{"id":30,"name":"Commander Zilyana","rank":-1,"score":-1},{"id":31,"name":"Corporeal Beast","rank":-1,"score":-1},{"id":32,"name":"Crazy Archaeologist","rank":-1,"score":-1},{"id":33,"name":"Dagannoth Prime","rank":-1,"score":-1},{"id":34,"name":"Dagannoth Rex","rank":-1,"score":-1},{"id":35,"name":"Dagannoth Supreme","rank":-1,"score":-1},{"id":36,"name":"Deranged Archaeologist","rank":-1,"score":-1},{"id":37,"name":"Duke Sucellus","rank":-1,"score":-1},{"id":38,"name":"General Graardor","rank":-1,"score":-1},{"id":39,"name":"Giant Mole","rank":-1,"score":-1},{"id":40,"name":"Grotesque Guardians","rank":-1,"score":-1},{"id":41,"name":"Hespori","rank":-1,"score":-1},{"id":42,"name":"Kalphite Queen","rank":-1,"score":-1},{"id":43,"name":"King Black Dragon","rank":-1,"score":-1},{"id":44,"name":"Kraken","rank":-1,"score":-1},{"id":45,"name":"Kree'Arra","rank":-1,"score":-1},{"id":46,"name":"K'ril Tsutsaroth","rank":-1,"score":-1},{"id":47,"name":"Lunar Chests","rank":-1,"score":-1},{"id":48,"name":"Mimic","rank":-1,"score":-1},{"id":49,"name":"Nex","rank":-1,"score":-1},{"id":50,"name":"Nightmare","rank":-1,"score":-1},{"id":51,"name":"Phosani's Nightmare","rank":-1,"score":-1},{"id":52,"name":"Obor","rank":-1,"score":-1},{"id":53,"name":"Phantom Muspah","rank":-1,"score":-1},{"id":54,"name":"Sarachnis","rank":-1,"score":-1},{"id":55,"name":"Scorpia","rank":-1,"score":-1},{"id":56,"name":"Scurrius","rank":178978,"score":22},{"id":57,"name":"Skotizo","rank":-1,"score":-1},{"id":58,"name":"Sol Heredit","rank":-1,"score":-1},{"id":59,"name":"Spindel","rank":-1,"score":-1},{"id":60,"name":"Tempoross","rank":-1,"score":-1},{"id":61,"name":"The Gauntlet","rank":-1,"score":-1},{"id":62,"name":"The Corrupted Gauntlet","rank":-1,"score":-1},{"id":63,"name":"The Leviathan","rank":-1,"score":-1},{"id":64,"name":"The Whisperer","rank":-1,"score":-1},{"id":65,"name":"Theatre of Blood","rank":-1,"score":-1},{"id":66,"name":"Theatre of Blood: Hard Mode","rank":-1,"score":-1},{"id":67,"name":"Thermonuclear Smoke Devil","rank":-1,"score":-1},{"id":68,"name":"Tombs of Amascut","rank":-1,"score":-1},{"id":69,"name":"Tombs of Amascut: Expert Mode","rank":-1,"score":-1},{"id":70,"name":"TzKal-Zuk","rank":-1,"score":-1},{"id":71,"name":"TzTok-Jad","rank":-1,"score":-1},{"id":72,"name":"Vardorvis","rank":-1,"score":-1},{"id":73,"name":"Venenatis","rank":-1,"score":-1},{"id":74,"name":"Vet'ion","rank":-1,"score":-1},{"id":75,"name":"Vorkath","rank":-1,"score":-1},{"id":76,"name":"Wintertodt","rank":1375582,"score":9},{"id":77,"name":"Zalcano","rank":-1,"score":-1},{"id":78,"name":"Zulrah","rank":-1,"score":-1}]}"""
j = json.loads(j)
js =j["skills"]
for s in js:
    name=s["name"].lower()
    #print(f"public int {name}XP "+"{ get; set; }")
    #print(f"public int {name}Rank "+"{ get; set; }")
    #print(f"public int {name}Level "+"{ get; set; }")
    
for s in js:
    name=s["name"].lower()
    name2 = s["name"]
    st = f"""case "{name2}":
    ps.{name}XP = s.xp;
    ps.{name}Rank = s.rank;
    ps.{name}Level = s.level;
    break;"""
#    print(st)
for a in j["activities"]:
    name = a["name"]
    name2=name
    name = name[0].lower() + name[1:]
    st = f"""case "{name2}":
    ps.{name}Rank = s.rank;
    ps.{name}Score = s.score;
    break;"""
    #print(st)


for s in js:
    name=s["name"].lower()
    name2 = s["name"]
    fd=f"""if(ps1.{name}XP < ps2.{name}XP) {{
                l.Add(new ChangedSkillStats() {{ name = "{name2}", xpChange = ps2.{name}XP - ps1.{name}XP, levelChange = ps2.{name}Level - ps1.{name}Level, rankChange = ps2.{name}Rank - ps1.{name}Rank }});
           }}
    """
    print(fd)

bossNames = [ "leaguePoints", "deadmanPoints", "bountyHunterHunter", "bountyHunterRogue", "bountyHunterLegacyHunter", "bountyHunterLegacyRogue", "clueScrollsAll", "clueScrollsBeginner", "clueScrollsEasy", "clueScrollsMedium", "clueScrollsHard", "clueScrollsElite", "clueScrollsMaster", "lmsRank", "pvpArenaRank", "soulWarsZeal", "riftsClosed", "colosseumGlory", "abyssalSire", "alchemicalHydra", "artio", "barrowsChests", "bryophyta", "callisto", "calvarion", "cerberus", "chambersOfXeric", "chambersOfXericChallengeMode", "chaosElemental", "chaosFanatic", "commanderZilyana", "corporealBeast", "crazyArchaeologist", "dagannothPrime", "dagannothRex", "dagannothSupreme", "derangedArchaeologist", "dukeSucellus", "generalGraardor", "giantMole", "grotesqueGuardians", "hespori", "kalphiteQueen", "kingBlackDragon", "kraken", "kreeArra", "krilTsutsaroth", "lunarChests", "mimic", "nex", "nightmare", "phosanisNightmare", "obor", "phantomMuspah", "sarachnis", "scorpia", "scurrius", "skotizo", "solHeredit", "spindel", "tempoross", "theGauntlet", "theCorruptedGauntlet", "theLeviathan", "theWhisperer", "theatreOfBlood", "theatreOfBloodHardMode", "thermonuclearSmokeDevil", "tombsOfAmascut", "tombsOfAmascutExpertMode", "tzkalZuk", "tztokJad", "vardorvis", "venenatis", "vetion", "vorkath", "wintertodt", "zalcano", "zulrah" ]

for i in range(0,len(j["activities"])):
    name = bossNames[i]
    name2 = j["activities"][i]["name"].replace("'","\\'")
    fd=f"""if (ps1.{name}Score < ps2.{name}Score)
            {{
                l.Add(new ChangedBossStats() {{ name = "{name2}", scoreChange = ps2.{name}Score - ps1.{name}Score, rankChange = ps2.{name}Rank - ps1.{name}Rank }});
            }}"""
    #print(fd)