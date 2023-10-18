using PRF.WPFCore;

namespace ClusterizerGui.Views.ImportDatasets.Adapter;

internal sealed class CsvLineAdapter : ViewModelBase
{

    public string FeatureName { get; }
    public string Latitude { get; }
    public string Longitude { get; }
    public string Category { get; }
    public string OtherData { get; }
    
    /// <summary>
    /// Whether the current line is skipped or not 
    /// </summary>
    public bool IsNotSkipped { get; }

    public int LineNumber { get; }

    public CsvLineAdapter(int lineNumber, bool isNotSkipped, string name, string longitude, string latitude, string category, string otherData)
    {
        LineNumber = lineNumber;
        FeatureName = name;
        IsNotSkipped = isNotSkipped;
        Longitude = longitude;
        Latitude = latitude;
        Category = category;
        OtherData = otherData;
    }
}