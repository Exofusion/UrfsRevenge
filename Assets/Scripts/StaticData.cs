﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StaticData {
	public static  List<int> ChampionAvatarFlipLookup = new List<int>()
	{
		266,
		67,
		103,
		12,
		32,
		34,
		1,
		22,
		268,
		201,
		51,
		31,
		131,
		119,
		60,
		28,
		81,
		105,
		3,
		41,
		86,
		79,
		104,
		120,
		74,
		40,
		59,
		126,
		222,
		429,
		43,
		30,
		38,
		55,
		10,
		96,
		64,
		89,
		127,
		117,
		99,
		57,
		11,
		21,
		62,
		267,
		75,
		111,
		76,
		56,
		20,
		61,
		80,
		133,
		421,
		58,
		107,
		92,
		68,
		13,
		35,
		98,
		14,
		15,
		72,
		37,
		16,
		134,
		91,
		44,
		17,
		412,
		18,
		48,
		4,
		77,
		6,
		67,
		161,
		254,
		112,
		106,
		19,
		101,
		5,
		83,
		154,
		238,
		115,
		26,
		143

		/*,
		{412, "Thresh"},
		{23, "Tryndamere"},
		{79, "Gragas"},
		{69, "Cassiopeia"},
		{13, "Ryze"},
		{78, "Poppy"},
		{14, "Sion"},
		{1, "Annie"},
		{111, "Nautilus"},
		{43, "Karma"},
		{99, "Lux"},
		{103, "Ahri"},
		{2, "Olaf"},
		{112, "Viktor"},
		{34, "Anivia"},
		{86, "Garen"},
		{27, "Singed"},
		{127, "Lissandra"},
		{57, "Maokai"},
		{25, "Morgana"},
		{28, "Evelynn"},
		{105, "Fizz"},
		{74, "Heimerdinger"},
		{238, "Zed"},
		{68, "Rumble"},
		{37, "Sona"},
		{82, "Mordekaiser"},
		{96, "KogMaw"},
		{55, "Katarina"},
		{117, "Lulu"},
		{22, "Ashe"},
		{30, "Karthus"},
		{12, "Alistar"},
		{122, "Darius"},
		{67, "Vayne"},
		{77, "Udyr"},
		{110, "Varus"},
		{89, "Leona"},
		{126, "Jayce"},
		{134, "Syndra"},
		{80, "Pantheon"},
		{92, "Riven"},
		{121, "Khazix"},
		{42, "Corki"},
		{51, "Caitlyn"},
		{268, "Azir"},
		{76, "Nidalee"},
		{3, "Galio"},
		{85, "Kennen"},
		{45, "Veigar"},
		{432, "Bard"},
		{150, "Gnar"},
		{104, "Graves"},
		{90, "Malzahar"},
		{254, "Vi"},
		{10, "Kayle"},
		{39, "Irelia"},
		{64, "LeeSin"},
		{60, "Elise"},
		{106, "Volibear"},
		{20, "Nunu"},
		{4, "TwistedFate"},
		{24, "Jax"},
		{102, "Shyvana"},
		{429, "Kalista"},
		{36, "DrMundo"},
		{63, "Brand"},
		{131, "Diana"},
		{113, "Sejuani"},
		{8, "Vladimir"},
		{154, "Zac"},
		{421, "RekSai"},
		{133, "Quinn"},
		{84, "Akali"},
		{18, "Tristana"},
		{120, "Hecarim"},
		{15, "Sivir"},
		{236, "Lucian"},
		{107, "Rengar"},
		{19, "Warwick"},
		{72, "Skarner"},
		{54, "Malphite"},
		{157, "Yasuo"},
		{101, "Xerath"},
		{17, "Teemo"},
		{75, "Nasus"},
		{58, "Renekton"},
		{119, "Draven"},
		{35, "Shaco"},
		{50, "Swain"},
		{115, "Ziggs"},
		{40, "Janna"},
		{91, "Talon"},
		{61, "Orianna"},
		{114, "Fiora"},
		{9, "FiddleSticks"},
		{33, "Rammus"},
		{31, "Chogath"},
		{7, "Leblanc"},
		{16, "Soraka"},
		{26, "Zilean"},
		{56, "Nocturne"},
		{222, "Jinx"},
		{83, "Yorick"},
		{6, "Urgot"},
		{21, "MissFortune"},
		{62, "MonkeyKing"},
		{53, "Blitzcrank"},
		{98, "Shen"},
		{201, "Braum"},
		{5, "XinZhao"},
		{29, "Twitch"},
		{11, "MasterYi"},
		{44, "Taric"},
		{32, "Amumu"},
		{41, "Gangplank"},
		{48, "Trundle"},
		{38, "Kassadin"},
		{161, "Velkoz"},
		{143, "Zyra"},
		{267, "Nami"},
		{59, "JarvanIV"},
		{81, "Ezreal"}*/
	};

	public static Dictionary<int,string> ChampionIdDictionary = new Dictionary<int,string>()
	{
		{266, "Aatrox"},
		{412, "Thresh"},
		{23, "Tryndamere"},
		{79, "Gragas"},
		{69, "Cassiopeia"},
		{13, "Ryze"},
		{78, "Poppy"},
		{14, "Sion"},
		{1, "Annie"},
		{111, "Nautilus"},
		{43, "Karma"},
		{99, "Lux"},
		{103, "Ahri"},
		{2, "Olaf"},
		{112, "Viktor"},
		{34, "Anivia"},
		{86, "Garen"},
		{27, "Singed"},
		{127, "Lissandra"},
		{57, "Maokai"},
		{25, "Morgana"},
		{28, "Evelynn"},
		{105, "Fizz"},
		{74, "Heimerdinger"},
		{238, "Zed"},
		{68, "Rumble"},
		{37, "Sona"},
		{82, "Mordekaiser"},
		{96, "KogMaw"},
		{55, "Katarina"},
		{117, "Lulu"},
		{22, "Ashe"},
		{30, "Karthus"},
		{12, "Alistar"},
		{122, "Darius"},
		{67, "Vayne"},
		{77, "Udyr"},
		{110, "Varus"},
		{89, "Leona"},
		{126, "Jayce"},
		{134, "Syndra"},
		{80, "Pantheon"},
		{92, "Riven"},
		{121, "Khazix"},
		{42, "Corki"},
		{51, "Caitlyn"},
		{268, "Azir"},
		{76, "Nidalee"},
		{3, "Galio"},
		{85, "Kennen"},
		{45, "Veigar"},
		{432, "Bard"},
		{150, "Gnar"},
		{104, "Graves"},
		{90, "Malzahar"},
		{254, "Vi"},
		{10, "Kayle"},
		{39, "Irelia"},
		{64, "LeeSin"},
		{60, "Elise"},
		{106, "Volibear"},
		{20, "Nunu"},
		{4, "TwistedFate"},
		{24, "Jax"},
		{102, "Shyvana"},
		{429, "Kalista"},
		{36, "DrMundo"},
		{63, "Brand"},
		{131, "Diana"},
		{113, "Sejuani"},
		{8, "Vladimir"},
		{154, "Zac"},
		{421, "RekSai"},
		{133, "Quinn"},
		{84, "Akali"},
		{18, "Tristana"},
		{120, "Hecarim"},
		{15, "Sivir"},
		{236, "Lucian"},
		{107, "Rengar"},
		{19, "Warwick"},
		{72, "Skarner"},
		{54, "Malphite"},
		{157, "Yasuo"},
		{101, "Xerath"},
		{17, "Teemo"},
		{75, "Nasus"},
		{58, "Renekton"},
		{119, "Draven"},
		{35, "Shaco"},
		{50, "Swain"},
		{115, "Ziggs"},
		{40, "Janna"},
		{91, "Talon"},
		{61, "Orianna"},
		{114, "Fiora"},
		{9, "FiddleSticks"},
		{33, "Rammus"},
		{31, "Chogath"},
		{7, "Leblanc"},
		{16, "Soraka"},
		{26, "Zilean"},
		{56, "Nocturne"},
		{222, "Jinx"},
		{83, "Yorick"},
		{6, "Urgot"},
		{21, "MissFortune"},
		{62, "MonkeyKing"},
		{53, "Blitzcrank"},
		{98, "Shen"},
		{201, "Braum"},
		{5, "XinZhao"},
		{29, "Twitch"},
		{11, "MasterYi"},
		{44, "Taric"},
		{32, "Amumu"},
		{41, "Gangplank"},
		{48, "Trundle"},
		{38, "Kassadin"},
		{161, "Velkoz"},
		{143, "Zyra"},
		{267, "Nami"},
		{59, "JarvanIV"},
		{81, "Ezreal"}
	};
}
