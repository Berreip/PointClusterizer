using System.Globalization;
using PRF.WPFCore;

namespace ClusterizerGui.Views.ImportDatasets.Adapter;

internal sealed class CsvLineAdapter : ViewModelBase
{
    private readonly double _parsedlatitude;
    private readonly double _parsedlongitude;

    public int LineNumber { get; }
    public string FeatureName { get; }
    public string Latitude { get; }
    public string Longitude { get; }
    public string Category { get; }
    public string OtherData { get; }
    public bool IsValid { get; }
    
    /// <summary>
    /// Whether the current line is skipped or not 
    /// </summary>
    public bool IsNotSkipped { get; }

    public double LatitudeParsed => _parsedlatitude;
    public double LongitudeParsed => _parsedlongitude;

    public CsvLineAdapter(int lineNumber, bool isNotSkipped, string name, string longitude, string latitude, string category, string otherData)
    {
        LineNumber = lineNumber;
        FeatureName = name;
        IsNotSkipped = isNotSkipped;
        Longitude = longitude;
        Latitude = latitude;
        Category = category;
        OtherData = otherData;
        
        var latParseSucceed = double.TryParse(latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out _parsedlatitude);
        var longParseSucceed = double.TryParse(longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out _parsedlongitude);
        IsValid = latParseSucceed && longParseSucceed;
    }
}