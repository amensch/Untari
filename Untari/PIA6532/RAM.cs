using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RAM : IBusDevice
{
    /*
     * The PIA contains 128 bytes of RAM which is mapped from $0080-$00FF.
     */

    private byte[] _ram;
    private const ushort ADDRESS_MASK = 0x7f;

    public void PowerOn()
    {
        _ram = new byte[ 128 ];

        // Initialize RAM to be random values
        //System.Random rnd = new System.Random();
        //for( int ii = 0; ii < 128; ii++ )
        //{
        //    _ram[ ii ] = (byte)rnd.Next( 0, 0x100 );
        //}
    }

    public byte Read( ushort addr )
    {
        return _ram[ addr & ADDRESS_MASK ];
    }

    public void Write( ushort addr, byte data )
    {
        _ram[ addr & ADDRESS_MASK ] = data;
    }
}
