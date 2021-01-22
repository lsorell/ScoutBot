# ScoutBot

ScoutBot is a discord bot that creates scout sheets, using google sheets, on oppoenents using their previous LoL match histories.

# __Commands__

# Table of Contents

- [Admin Commands](#admin-commands)
- [General Commands](#general-commands)

---

# Admin Commands

## Commands

- [RegisterSheet](#Register-<SheetUrl\>-<Name\>)
- [GiveAccess](#GiveAccess)

#

### AddSheet <SheetUrl\> <Name\>

Saves a google sheet to the database.

#### Parameters

| Param    | Description                                          |
| -------- | -----------------------------------------------------|
| SheetUrl | The url of the google sheet.                         |
| Name     | A common name for the sheet. (i.e. Gold Fall 2020)   |

### GiveAccess

Prompts user for what sheet to give what role(s) access to.

---

# General Commands

## Commands

- [NewScout](#NewScout-<Op.gg\>-<TeamName\>)
- [ScoutGame](#ScoutGame-<Result\>-<MatchHistory\>)
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

### ScoutGame <Result\> <MatchHistory\>

Incorperates the game data into the scout spreadsheet for the most recently accessed team.

#### Parameters

| Param        | Description                                                           |
| ------------ | --------------------------------------------------------------------- |
| Result       | The result (win/loss) of the scouted team.                            |
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