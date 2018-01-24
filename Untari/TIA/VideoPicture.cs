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

    private void UpdatePixelUsingLockbits(int x, int y, Color color)
    {
        BitmapData bitmapData = Picture.LockBits( new Rectangle( 0, 0, Picture.Width, Picture.Height ), ImageLockMode.ReadWrite, Picture.PixelFormat );

        int bytesPerPixel = Bitmap.GetPixelFormatSize( Picture.PixelFormat ) / 8;
        int byteCount = bitmapData.Stride * Picture.Height;
        byte[] pixels = new byte[ byteCount ];
        IntPtr ptrFirstPixel = bitmapData.Scan0;
        Marshal.Copy( ptrFirstPixel, pixels, 0, pixels.Length );
        int heightInPixels = bitmapData.Height;
        int widthInBytes = bitmapData.Width * bytesPerPixel;


        // This is not correct!
        //pixels[ x + y ] = (byte) color.B;
        //pixels[ x + y + 1 ] = (byte) color.G;
        //pixels[ x + y + 2 ] = (byte) color.R;

        // copy modified bytes back
        Marshal.Copy( pixels, 0, ptrFirstPixel, pixels.Length );
        Picture.UnlockBits( bitmapData );
    }

}
