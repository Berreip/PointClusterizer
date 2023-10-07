using System.Collections.Generic;
using ClusterizerGui.Utils;
using ClusterizerGui.Utils.MathMisc;
using ClusterizerGui.Views.MainDisplay.Helpers;
using ClusterizerLib;
using PRF.WPFCore;

namespace ClusterizerGui.Views.MainDisplay.Adapters;

internal sealed class ClusterAdapter : ViewModelBase
{
    /// <summary>
    /// The radius of the cluster (depending on the nb of elements)
    /// </summary>
    public int Radius { get; }

    public double WidthPercentage { get; }
    public double HeightPercentage { get; }
    public IPoint Centroid { get; }

    public ClusterAdapter(int pointsCount, IPoint centroid)
    {
        Radius = RadiusCalculation.ComputeRadiusFromPointCounts(pointsCount);
        Centroid = centroid;
        WidthPercentage = centroid.X / ClusterizerGuiConstants.IMAGE_WIDTH;
        HeightPercentage = centroid.Y / ClusterizerGuiConstants.IMAGE_HEIGHT;
    }


    public IReadOnlyCollection<IPoint> GetCentroidAndPointsAround()
    {
        var pointAround = new List<IPoint>(9);
        for (int i = -Radius; i <= Radius; i++)
        {
            for (int j = -Radius; j <= Radius; j++)
            {
                var centroidX = Centroid.X + i;
                var centroidY = Centroid.Y + j;
                
                if(centroidX is > 0 and < ClusterizerGuiConstants.DATA_MAX_X && centroidY is > 0 and < ClusterizerGuiConstants.DATA_MAX_Y)
                {
                    pointAround.Add(new PointAdapter(centroidX, centroidY, Centroid.Z));
                }
            }
        }

        return pointAround;
    }
}