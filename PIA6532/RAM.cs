using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KDS.e6502;

public class RAM : ISystemBus
{
    /*
     * The PIA contains 128 bytes of RAM which is mapped from $0080-$00FF.
     */

    private byte[] _ram = new byte[ 128 ];

    public byte Read( ushort addr )
    {
        throw new NotImplementedException();
    }

    public void Write( ushort addr, byte data )
    {
        throw new NotImplementedException();
    }
}
