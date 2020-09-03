# ScoutBot

ScoutBot is a discord bot that creates scout sheets, using google sheets, on oppoenents using their previous LoL match histories.

# __Commands__

# Table of Contents

- [Admin Commands](#admin-commands)
- [General Commands](#general-commands)

---

# Admin Commands

## Commands

- [NewSheet](#NewSheet-<TeamName\>)
- [RegisterSheet](#RegisterSheet-<SheetId\>-<TeamName\>)
- [RegisterPlayer](#RegisterPlayer-<DiscordId\>-<TeamName\>)

#

### NewSheet <TeamName\>

Creates a new google sheet and links it to the given RotM team.

#### Parameters

| Param     | Description                                                            |
| --------- | ---------------------------------------------------------------------- |
| TeamName  | The name of the RotM team you want to associate with the google sheet. |

---

### RegisterSheet <SheetId\> <TeamName\>

Links an existing google sheet with a RotM team name.

#### Parameters

| Param    | Description                         |
| -------- | ----------------------------------- |
| SheetId  | The google id of the document.      |
| TeamName | The name of the RotM team.          |

---

### RegisterPlayer <DiscordId\> <TeamName\>

Links a discord account to a RotM team.

#### Parameters

| Param     | Description                         |
| --------- | ----------------------------------- |
| DiscordId | The discord id of the player.       |
| TeamName  | The name of the RotM team.          |

---

# General Commands

## Commands

- [NewScout](#NewScout-<Op.gg\>-<TeamName\>)
- [ScoutGame](#ScoutGame-<SideOrResult\>-<MatchHistory\>)
- [SaveGame](#SaveGame-<MatchHistory\>)
- [Undo](#Undo)

#

### NewScout <Op.gg\> <TeamName\>

Creates a new worksheet for scouting with the enemy team name and op.gg link.

#### Parameters

| Param        | Description                                                           |
| ------------ | --------------------------------------------------------------------- |
| Op.gg        | The op.gg/multi link of the team.                                     |
| TeamName     | The name of the enemy team.                                           |

---

### ScoutGame <SideOrResult\> <MatchHistory\>

Incorperates the game data into the scout spreadsheet for the most recently accessed team.

#### Parameters

| Param        | Description                                                           |
| ------------ | --------------------------------------------------------------------- |
| SideOrResult | The side (red/blue) or the result (win/loss) of the scouted team.     |
| MatchHistory | A match history link.                                                 |

---

### ScoutGame <SideOrResult\> <MatchHistory\> <TeamName\>

Incorperates the game data into the scout spreadsheet for the given team.

#### Parameters

| Param        | Description                                                           |
| ------------ | --------------------------------------------------------------------- |
| SideOrResult | The side (red/blue) or the result (win/loss) of the scouted team.     |
| MatchHistory | A match history link.                                                 |
| TeamName     | The name of the enemy team.                                           |

---

### SaveGame <MatchHistory\>

Adds the match history link to the scout spreadsheet of the most recently accessed team.

#### Parameters

| Param        | Description                 |
| ------------ | --------------------------- |
| MatchHistory | A match history link.       |

---

### SaveGame <MatchHistory\> <TeamName\>

Adds the match history link to the scout spreadsheet of the given team.

#### Parameters

| Param        | Description                 |
| ------------ | --------------------------- |
| MatchHistory | A match history link.       |
| TeamName     | The name of the enemy team. |

---

### Undo

Undoes the most recent change to the spreadsheet.

---