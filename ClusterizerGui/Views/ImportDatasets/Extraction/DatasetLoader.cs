using System.Collections.Generic;
using ClusterizerGui.Views.ImportDatasets.Adapter;
using PRF.Utils.CoreComponents.IO;

namespace ClusterizerGui.Views.ImportDatasets.Extraction;

internal static class DatasetLoader
{
    public const int GUESS_COLUMN_POSITION = -1;

    public static DatasetLoadingResult LoadDataset(
        IFileInfo validFile,
        string separator,
        int dataStartingPosition,
        int nameHeaderPosition,
        int longitudeHeaderPosition,
        int latitudeHeaderPosition,
        int categoryHeaderPosition)
    {
        var allLines = validFile.ReadAllLines();
        GuessedPosition guessedPosition;
        // try to infer column positions from first line if possible 
        if (dataStartingPosition != 0 &&
            allLines.Length != 0 &&
            GuessHelper.TryGuessPosition(allLines[0], separator, nameHeaderPosition, longitudeHeaderPosition, latitudeHeaderPosition, categoryHeaderPosition, out var guessedPositionExtracted))
        {
            guessedPosition = guessedPositionExtracted;
        }
        else
        {
            // else use provided one
            guessedPosition = new GuessedPosition(nameHeaderPosition, longitudeHeaderPosition, latitudeHeaderPosition, categoryHeaderPosition, null);
        }

        var linesExtracted = new List<CsvLineAdapter>(allLines.Length);
        for (var i = 0; i < allLines.Length; i++)
        {
            var lineParts = allLines[i].Split(separator);
            var isNotSkipped = i >= dataStartingPosition;
            linesExtracted.Add(new CsvLineAdapter(
                i,
                isNotSkipped,
                guessedPosition.ExtractName(lineParts),
                guessedPosition.ExtractLong(lineParts),
                guessedPosition.ExtractLat(lineParts),
                guessedPosition.ExtractCat(lineParts),
                GuessHelper.ExtractOtherData(lineParts, guessedPosition.GuessedCategoryPosition, guessedPosition.GuessedNamePosition, guessedPosition.GuessedLatitudePosition, guessedPosition.GuessedLongitudePosition)
            ));
        }

        return new DatasetLoadingResult(guessedPosition, linesExtracted);
    }
}

internal sealed class DatasetLoadingResult
{
    public GuessedPosition GuessedPositions { get; }
    public IReadOnlyCollection<CsvLineAdapter> CsvLines { get; }

    public DatasetLoadingResult(GuessedPosition guessedPositions, IReadOnlyCollection<CsvLineAdapter> csvLines)
    {
        GuessedPositions = guessedPositions;
        CsvLines = csvLines;
    }
}