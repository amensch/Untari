using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class VideoPicture
{

    // Could go easy by thinking of rectangle pixels.
    // Screen is 160 tall and 192 wide with an aspect of 2:1 for each rectangle.
    //
    // Actual NTSC aspect ratio is 12:7


    public Bitmap Picture { get; private set; }

    public VideoPicture()
    {
        Picture = new Bitmap( 160, 192 );
    }

    public void UpdatePixel(int x, int y, Color color)
    {
        Picture.SetPixel( x, y, color );
    }
}
