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
            List<List<GoogleCell>> rows = new List<List<GoogleCell>>();

            // Row 1
            List<GoogleCell> row1 = new List<GoogleCell>();
            GoogleCell a1 = new GoogleCell(teamName.ToUpper(), true, 18);
            row1.Add(a1);
            rows.Add(row1);

            List<GoogleCell> row2 = new List<GoogleCell>();
            GoogleCell a2 = new GoogleCell("OP.GG:", TextAlignment.Right);
            GoogleCell b2 = new GoogleCell(opgg);
            row2.Add(a2);
            row2.Add(b2);
            rows.Add(row2);

            BatchUpdateSpreadsheetRequest batchRequest = GoogleServiceHelper.UpdateCellsHelper(rows, sheetId, 0, 0);
            SpreadsheetsResource.BatchUpdateRequest requests = _service.Spreadsheets.BatchUpdate(batchRequest, spreadsheetId);
            await requests.ExecuteAsync();
        }
    }
}