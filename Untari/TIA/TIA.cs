using System;

public class TIA : IBusDevice
{
    // 68 HBLANK, then 160 active = 228 total
    private int horizontal_position;
    private const int MAX_HORIZONTAL = 228;

    // 3 VSYNC, 37 VBLANK, 192 active, 30 OVERSCAN = 262 total
    private int vertical_position;
    private const int MAX_VERTICAL = 262;




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
        throw new NotImplementedException();
    }

    private void TIATick()
    {
        UpdateScanPosition();
    }

    private void UpdateScanPosition()
    {
        horizontal_position++;

        if( horizontal_position > MAX_HORIZONTAL )
        {
            horizontal_position = 0;
            vertical_position++;

            if( vertical_position > MAX_VERTICAL )
                vertical_position = 1;
        }
    }
}
