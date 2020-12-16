using System;
using KDS.e6502CPU;

public class Cartridge : IBusDevice
{
    public void Boot()
    {

    }

    public void Tick()
    {

    }

    public byte Read( ushort addr )
    {
        throw new NotImplementedException();
    }

    public void Write( ushort addr, byte data )
    {
        throw new NotImplementedException();
    }
}
