using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScoutBot.Services
{
    /// <summary>
    /// Handles methods that deal with google sheets api calls.
    /// </summary>
    public static class GoogleService
    {
        /// <summary>
        /// The access rights of the service (read/write)
        /// </summary>
        private static string[] _scopes = { SheetsService.Scope.Spreadsheets };
        /// <summary>
        /// The name of the application registered with google developer portal.
        /// </summary>
        private static string _applicationName = "ScoutBot";
        /// <summary>
        /// The google sheet api service.
        /// </summary>
        private static SheetsService _service;

        private const string CredentialPath = @"../credentials.json";

        /// <summary>
        /// Creates the google sheets api service.
        /// </summary>
        public static void Initialize()
        {
            GoogleCredential credential;
            using (FileStream stream = new FileStream(CredentialPath, FileMode.Open, FileAccess.ReadWrite))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }

            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }

        /// <summary>
        /// Adds a new scout sheet to a spreadsheet.
        /// </summary>
        /// <param name="spreadsheetId">The id of the spreadsheet to add to.</param>
        /// <param name="opgg">The op.gg link of the team.</param>
        /// <param name="teamName">The team's name.</param>
        public static async Task<string> AddNewScoutSheetAsync(string spreadsheetId, string opgg, string teamName)
        {
            string error = null;

            // Add new spreadsheet
            string sheetName = string.Format("Scout: {0}", teamName);
            int? sheetId = null;
            try
            {
                sheetId = await AddSheetRequestAsync(spreadsheetId, sheetName);
            }
            catch
            {
                error = "Something went wrong while adding a sheet.";
            }

            // Add team name and opgg to spreadsheet
            try
            {
                await AddNewScoutSheetDataAsync(spreadsheetId, sheetId, opgg, teamName);
            }
            catch
            {
                error = "Something went wrong while adding data to the sheet";
            }

            return error;
        }

        /// <summary>
        /// Adds a new sheet to the spreadsheet.
        /// </summary>
        /// <param name="spreadsheetId">The id of the spreadsheet to add to.</param>
        /// <param name="sheetName">The title of the new sheet.</param>
        /// <returns></returns>
        private static async Task<int?> AddSheetRequestAsync(string spreadsheetId, string sheetName)
        {
            AddSheetRequest addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties
            {
                Title = sheetName
            };

            BatchUpdateSpreadsheetRequest batchRequest = new BatchUpdateSpreadsheetRequest();
            batchRequest.Requests = new List<Request> {
                new Request {
                    AddSheet = addSheetRequest
                }
            };
            SpreadsheetsResource.BatchUpdateRequest requests = _service.Spreadsheets.BatchUpdate(batchRequest, spreadsheetId);
            BatchUpdateSpreadsheetResponse response = await requests.ExecuteAsync();

            return response.Replies[0].AddSheet.Properties.SheetId;
        }

        /// <summary>
        /// Adds the base data to the new scout sheet.
        /// </summary>
        /// <param name="spreadsheetId">The id of the spreadsheet to add data to.</param>
        /// <param name="sheetId">The id of the sheet to add data to.</param>
        /// <param name="opgg">The op.gg link of the team.</param>
        /// <param name="teamName">The team's name.</param>
        private static async Task AddNewScoutSheetDataAsync(string spreadsheetId, int? sheetId, string opgg, string teamName)
        {
            UpdateCellsRequest updateCellsRequest = new UpdateCellsRequest
            {
                Start = new GridCoordinate
                {
                    SheetId = sheetId,
                    ColumnIndex = 0,
                    RowIndex = 0
                },
                Fields = "*"
            };
            updateCellsRequest.Rows = new List<RowData>();
            // Row 1
            RowData row1 = new RowData();
            row1.Values = new List<CellData>();
            CellData a1 = new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = teamName.ToUpper()
                },
                UserEnteredFormat = new CellFormat
                {
                    TextFormat = new TextFormat
                    {
                        Bold = true,
                        FontSize = 18
                    }
                }
            };
            row1.Values.Add(a1);
            updateCellsRequest.Rows.Add(row1);
            // Row 2
            RowData row2 = new RowData();
            row2.Values = new List<CellData>();
            CellData a2 = new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = "OP.GG:"
                },
                UserEnteredFormat = new CellFormat
                {
                    HorizontalAlignment = "Right"
                }
            };
            row2.Values.Add(a2);
            CellData b2 = new CellData
            {
                UserEnteredValue = new ExtendedValue
                {
                    StringValue = opgg
                }
            };
            row2.Values.Add(b2);
            updateCellsRequest.Rows.Add(row2);

            BatchUpdateSpreadsheetRequest batchRequest = new BatchUpdateSpreadsheetRequest();
            batchRequest.Requests = new List<Request> {
                new Request {
                    UpdateCells = updateCellsRequest
                }
            };
            SpreadsheetsResource.BatchUpdateRequest requests = _service.Spreadsheets.BatchUpdate(batchRequest, spreadsheetId);
            await requests.ExecuteAsync();
        }
    }
}