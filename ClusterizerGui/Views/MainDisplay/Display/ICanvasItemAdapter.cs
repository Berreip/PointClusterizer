namespace ClusterizerGui.Views.MainDisplay.Display;

internal interface ICanvasItemAdapter
{
    /// <summary>
    /// The order of display for the items
    /// </summary>
    public int DisplayIndex { get; }

    /// <summary>
    /// Percentage X Position on the canvas 
    /// </summary>
    public double WidthPercentage { get; }

    /// <summary>
    /// Percentage Y Position on the canvas 
    /// </summary>
    public double HeightPercentage { get; }
}