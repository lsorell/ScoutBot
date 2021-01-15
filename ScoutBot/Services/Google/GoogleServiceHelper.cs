using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;

namespace ScoutBot.Services
{
    /// <summary>
    /// Methods to streamline code in GoogleService.cs
    /// </summary>
    public static class GoogleServiceHelper
    {
        /// <summary>
        /// Updates cells in a sheet starting at the given indexes.
        /// </summary>
        /// <param name="rows">The cell data organized in rows.</param>
        /// <param name="sheetId">The sheetId in the spreadsheet.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="colIndex">The starting column index.</param>
        /// <returns>The update cells request.</returns>
        public static BatchUpdateSpreadsheetRequest UpdateCellsHelper(List<List<GoogleCell>> rows, int? sheetId, int rowIndex, int colIndex)
        {
            UpdateCellsRequest updateCellsRequest = new UpdateCellsRequest
            {
                Start = new GridCoordinate
                {
                    SheetId = sheetId,
                    ColumnIndex = colIndex,
                    RowIndex = rowIndex
                },
                Fields = "*"
            };
            updateCellsRequest.Rows = new List<RowData>();

            foreach (List<GoogleCell> row in rows)
            {
                RowData rd = new RowData();
                rd.Values = new List<CellData>();
                foreach (GoogleCell gc in row)
                {
                    CellData cd = ConvertToCellData(gc);
                    rd.Values.Add(cd);
                }
                updateCellsRequest.Rows.Add(rd);
            }

            BatchUpdateSpreadsheetRequest batchRequest = new BatchUpdateSpreadsheetRequest();
            batchRequest.Requests = new List<Request> {
                new Request {
                    UpdateCells = updateCellsRequest
                }
            };
            return batchRequest;
        }

        /// <summary>
        /// Converts custom GoogleCell object to Google's CellData object.
        /// </summary>
        /// <param name="cell">The object to be converted.</param>
        /// <returns>The converted object.</returns>
        public static CellData ConvertToCellData(GoogleCell cell)
        {
            CellData cd = new CellData();
            cd.UserEnteredValue = new ExtendedValue { StringValue = cell.Value };
            cd.UserEnteredFormat = new CellFormat
            {
                HorizontalAlignment = cell.TextAlignment.ToString(),
                TextFormat = new TextFormat
                {
                    Bold = cell.Bold,
                    FontSize = cell.FontSize
                }
            };
            if (cell.BackgroundColor != null)
            {
                cd.UserEnteredFormat.BackgroundColor = cell.BackgroundColor;
            }
            return cd;
        }
    }
}