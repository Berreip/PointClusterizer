using System.Collections.Generic;

namespace ClusterizerGui.Views.ImportDatasets.Extraction;

/// <summary>
/// Position guessed
/// </summary>
internal class GuessedPosition
{
    public int GuessedNamePosition { get; }
    public int GuessedLongitudePosition { get; }
    public int GuessedLatitudePosition { get; }
    public int GuessedCategoryPosition { get; }
    public string MiscHeaders { get; }

    public GuessedPosition(int guessedNamePosition, int guessedLongitudePosition, int guessedLatitudePosition, int guessedCategoryPosition, string? miscHeaders)
    {
        GuessedNamePosition = guessedNamePosition;
        GuessedLongitudePosition = guessedLongitudePosition;
        GuessedLatitudePosition = guessedLatitudePosition;
        GuessedCategoryPosition = guessedCategoryPosition;
        MiscHeaders = miscHeaders ?? string.Empty;
    }

    public string ExtractName(string[] lineParts) => ExtractShared(lineParts, GuessedNamePosition);
    public string ExtractLong(string[] lineParts) => ExtractShared(lineParts, GuessedLongitudePosition);
    public string ExtractLat(string[] lineParts) => ExtractShared(lineParts, GuessedLatitudePosition);
    public string ExtractCat(string[] lineParts) => ExtractShared(lineParts, GuessedCategoryPosition);

    private static string ExtractShared(string[] lineParts, int position)
    {
        if (position < 0) // DatasetLoader.GUESS_COLUMN_POSITION (-1) or invalid values
        {
            return string.Empty;
        }

        if (lineParts.Length <= position)
        {
            // position not within split array:
            return string.Empty;
        }

        return lineParts[position];
    }

}