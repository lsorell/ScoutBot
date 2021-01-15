using Google.Apis.Sheets.v4.Data;

namespace ScoutBot.Services
{
    /// <summary>
    /// Compiles the important data for a spreadsheet cell into one object.
    /// </summary>
    public class GoogleCell
    {
        public string Value;
        public int FontSize = 10;
        public bool Bold = false;
        public Color BackgroundColor = null;
        public TextAlignment TextAlignment;
        // Might need this later.
        // public CellBorder TopBorder;
        // public CellBorder BottomBorder;
        // public CellBorder LeftBorder;
        // public CellBorder RightBorder;

        /// <summary>
        /// Constructor for a basic cell.
        /// </summary>
        /// <param name="value">The string value of the cell.</param>
        /// <param name="bold">If the text should be bold (default: false).</param>
        /// <param name="fontSize">The font size (default: 10).</param>
        public GoogleCell(string value, bool bold = false, int fontSize = 10)
        {
            Value = value;
            Bold = bold;
            FontSize = fontSize;
        }

        /// <summary>
        /// Constructor for cell with change in text alignment.
        /// </summary>
        /// <param name="value">The string value of the cell.</param>
        /// <param name="alignment">The text alignment within the cell.</param>
        /// <param name="bold">If the text should be bold (default: false).</param>
        /// <param name="fontSize">The font size (default: 10).</param>
        public GoogleCell(string value, TextAlignment alignment, bool bold = false, int fontSize = 10)
        {
            Value = value;
            TextAlignment = alignment;
            Bold = bold;
            FontSize = fontSize;
        }
    }

    // public enum CellBorder
    // {
    //     None,
    //     Solid,
    //     SolidMedium,
    //     SolidThick
    // }

    /// <summary>
    /// The text alignment options for a cell.
    /// </summary>
    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }
}