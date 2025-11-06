using System;

namespace Graphical_2D_Frame_Analysis_CSharp
{
    /// <summary>
    /// Represents a point in 2D space with X and Y coordinates.
    /// </summary>
    public class PointXY
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointXY()
        {
            X = 0;
            Y = 0;
        }

        public PointXY(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
