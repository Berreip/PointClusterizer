﻿namespace ClusterizerGui.Utils;

internal static class ClusterizerGuiConstants
{
    public const int IMAGE_WIDTH = 600;
    public const int IMAGE_HEIGHT = 300;
    public const int IMAGE_ALTITUDE = 10; // use a much lower altitude to 'simulate' data that are close of the ground
    public const string ALL_REMAINING_ALIAS_CATEGORY = "ALL_REMAINING";
    
    // for point values, cap to max less 1 as position are zero based : 0-1999 if 2000 max value
    public const int DATA_MAX_X = IMAGE_WIDTH -1;
    public const int DATA_MAX_Y = IMAGE_HEIGHT -1;
    public const int DATA_MAX_Z = IMAGE_ALTITUDE -1 ;
}