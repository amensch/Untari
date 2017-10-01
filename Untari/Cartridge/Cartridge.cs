using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
