using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PRF.Utils.CoreComponents.Extensions;

namespace ClusterizerGui.Views.ImportDatasets.Extraction;

internal static class GuessHelper
{
    public static bool TryGuessPosition(
        string firstLine,
        string separator,
        int nameHeaderPosition,
        int longitudeHeaderPosition,
        int latitudeHeaderPosition,
        int categoryHeaderPosition,
        [NotNullWhen(returnValue: true)] out GuessedPosition? guessedPosition)
    {
        if (string.IsNullOrWhiteSpace(firstLine))
        {
            guessedPosition = null;
            return false;
        }

        var splitLine = firstLine.Split(separator);

        var namePosition = GetPosition(nameHeaderPosition, splitLine, "VesselName", "name", "GEOAPP_ID");
        var longitudePosition = GetPosition(longitudeHeaderPosition, splitLine, "Long", "longitude_DD", "");
        var latitudePosition = GetPosition(latitudeHeaderPosition, splitLine, "Lat", "latitude_DD", "");
        var categoryPosition = GetPosition(categoryHeaderPosition, splitLine, "Type", "cat", "");

        var miscHeaders = ExtractOtherData(
            splitLine,
            namePosition,
            longitudePosition,
            latitudePosition,
            categoryPosition);

        guessedPosition = new GuessedPosition(
            guessedNamePosition: namePosition,
            guessedLongitudePosition: longitudePosition,
            guessedLatitudePosition: latitudePosition,
            guessedCategoryPosition: categoryPosition,
            miscHeaders: miscHeaders);
        return true;
    }

    private static int GetPosition(int nameHeaderPosition, string[] splitLine, params string[] searchHeaders)
    {
        // if it is not GUESS_COLUMN_POSITION (-1), use the provided value directly
        return nameHeaderPosition < 0
            ? SearchAmongSplit(splitLine, searchHeaders)
            : nameHeaderPosition;
    }

    private static int SearchAmongSplit(string[] splitLine, params string[] searchHeaders)
    {
        for (var i = 0; i < splitLine.Length; i++)
        {
            var part = splitLine[i];
            foreach (var headerProposal in searchHeaders)
            {
                if (part.EqualsInsensitive(headerProposal))
                {
                    return i;
                }
            }
        }

        return DatasetLoader.GUESS_COLUMN_POSITION;
    }

    public static string ExtractOtherData(string[] lineSplit, int categoryPosition, int namePosition, int latitudePosition, int longitudePosition)
    {
        var otherData = new List<string>();
        for (var i = 0; i < lineSplit.Length; i++)
        {
            if (i != categoryPosition && i != namePosition && i != latitudePosition && i != longitudePosition)
            {
                otherData.Add(lineSplit[i]);
            }
        }

        return string.Join("|", otherData);
    }
}