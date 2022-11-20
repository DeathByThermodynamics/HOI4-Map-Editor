using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Diagnostics;

public class TransparentImage
    : Image
{
    protected override HitTestResult HitTestCore(
        PointHitTestParameters hitTestParameters)
    {
        // Get value of current pixel
        var source = (BitmapSource)Source;
        var x = (int)(hitTestParameters.HitPoint.X /
            ActualWidth * source.PixelWidth);
        var y = (int)(hitTestParameters.HitPoint.Y /
            ActualHeight * source.PixelHeight);
        var pixels = new byte[4];
        source.CopyPixels(new Int32Rect(x, y, 1, 1), pixels, 4, 0);
        // Check alpha channel
        if (pixels[3] < 10)
        {
            return new PointHitTestResult(this, hitTestParameters.HitPoint);
        }
        else
        {
            return null;
        }
    }

    protected override GeometryHitTestResult HitTestCore(
        GeometryHitTestParameters hitTestParameters)
    {
        // Do something similar here, possibly checking every pixel within
        // the hitTestParameters.HitGeometry.Bounds rectangle
        return base.HitTestCore(hitTestParameters);
    }
}
