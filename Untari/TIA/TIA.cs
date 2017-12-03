using System;

public class TIA : IBusDevice
{
    // 68 HBLANK, then 160 active = 228 total
    private int horizontal_position;
    private const int MAX_HORIZONTAL = 228;

    // 3 VSYNC, 37 VBLANK, 192 active, 30 OVERSCAN = 262 total
    private int vertical_position;
    private const int MAX_VERTICAL = 262;

    // Pixel action delegates for each x,y position
    private delegate void PixelAction();
    private PixelAction[,] pixelActions = new PixelAction[ MAX_HORIZONTAL, MAX_VERTICAL ];

    // object representing the current visible screen
    private VideoPicture Picture = new VideoPicture();

    public TIA()
    {
        // create an array to call the proper delegate based on the current active pixel
        for( int horz = 0; horz < MAX_HORIZONTAL; horz++)
        {
            // add VSYNC
            for( int vert = 0; vert < 3; vert++ )
            {
                pixelActions[ horz, vert ] = new PixelAction( VerticalSync );
            }

            // add VBLANK
            for( int vert = 3; vert < 40; vert++ )
            {
                pixelActions[ horz, vert ] = new PixelAction( VerticalBlank );
            }

            for( int vert = 40; vert < 232; vert++ )
            {
                // add HBLANK
                if( horz < 68 )
                {
                    pixelActions[ horz, vert ] = new PixelAction( HorizontalBlank );
                }
                // viewable area
                else
                {
                    pixelActions[ horz, vert ] = new PixelAction( ViewableArea );
                }
            }

            // add OVERSCAN
            for( int vert = 232; vert < MAX_VERTICAL; vert++ )
            {
                pixelActions[ horz, vert ] = new PixelAction( Overscan );
            }
        }
    }

    public void Boot()
    {
        horizontal_position = 1;
        vertical_position = 1;
    }

    public void Tick()
    {
        // Each system clock tick is 3 cycles for the TIA
        TIATick();
        TIATick();
        TIATick();
    }

    public byte Read( ushort addr )
    {
        throw new NotImplementedException();
    }

    public void Write( ushort addr, byte data )
    {
        // WSYNC
        if((addr & 0xff02) == 0x02)
        {

        }
    }

    private void TIATick()
    {
        UpdateScanPosition();
        CallPixelAction();
    }

    private void UpdateScanPosition()
    {
        horizontal_position++;

        if( horizontal_position > MAX_HORIZONTAL )
        {
            horizontal_position = 1;
            vertical_position++;

            if( vertical_position > MAX_VERTICAL )
                vertical_position = 1;
        }
    }

    private void CallPixelAction()
    {
        pixelActions[ horizontal_position - 1, vertical_position - 1 ]();
    }

    private void UpdatePixel()
    {

    }

    private void VerticalSync()
    {

    }

    private void VerticalBlank()
    {

    }

    private void HorizontalBlank()
    {

    }

    private void Overscan()
    {

    }

    private void ViewableArea()
    {

    }
}
