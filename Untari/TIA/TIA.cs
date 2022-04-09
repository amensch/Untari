using System;
using KDS.e6502;

public class TIA : IBusDevice
{
    // the TIA only uses six bits A0-A5
    private const ushort ADDRESS_MASK = 0x003f;

    // 68 HBLANK, then 160 active = 228 total
    private int horizontal_position;
    private const int END_OF_HBLANK = 68;
    private const int MAX_HORIZONTAL = 228;

    // 3 VSYNC, 37 VBLANK, 192 active, 30 OVERSCAN = 262 total
    private int vertical_position;
    private const int END_OF_VBLANK = 40;
    private const int MAX_VERTICAL = 262;

    // Pixel action delegates for each x,y position
    private delegate void PixelAction();
    private readonly PixelAction[,] pixelActions = new PixelAction[ MAX_HORIZONTAL, MAX_VERTICAL ];

    // object representing the current visible screen
    private readonly VideoPicture Picture = new VideoPicture();

    // the color palette
    private readonly NTSCColorPalette Palette = new NTSCColorPalette();

    // internal registers
    private byte[] memory;

    private readonly IReadyDevice CPU;

    public TIA(IReadyDevice cpu)
    {
        CPU = cpu;

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
                if( horz < END_OF_HBLANK )
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
        memory = new byte[ 0x3f ];
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
        return memory[ (ushort) (addr & ADDRESS_MASK) ];
    }

    public void Write( ushort addr, byte data )
    {
        ushort tia_addr = (ushort) (addr & ADDRESS_MASK);
        memory[ tia_addr ] = data;

        switch( tia_addr )
        {
            case TIAConstants.WSYNC:
                if(data == 0x00)
                {
                    CPU.ClearReadyLatch();
                }
                break;
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
            CPU.SetReadyLatch();
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
        //Picture.UpdatePixel( horizontal_position - END_OF_HBLANK, 
        //                        vertical_position - END_OF_VBLANK, 
        //                        Palette.GetColor( memory[ TIAConstants.COLUBK ] ) );
    }
}
