# ScoutBot

ScoutBot is a discord bot that creates scout sheets, using google sheets, on oppoenents using their previous LoL match histories.

# __Commands__

# Table of Contents

- [Admin Commands](#admin-commands)
- [General Commands](#general-commands)

---

# Admin Commands

## Commands

- [RegisterSheet](#Register-<SheetId\>-<RoleId\>-<TeamName\>)

#

### Register <SheetId\> <RoleId\> <TeamName\>

Links an existing google sheet with a RotM team name.

#### Parameters

| Param    | Description                                                           |
| -------- | --------------------------------------------------------------------- |
| SheetId  | The google id of the document.                                        |
| RoleId   | The id of the role that will be allowed to access the sheet.          |
| TeamName | The name of the team that will be using the scout sheet.              |

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