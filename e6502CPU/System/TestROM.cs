using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e6502CPU
{
    public class TestROM : ISystemBus
    {
        private byte[] ROM;

        public TestROM( int size )
        {
            ROM = new byte[ size ];
        }

        public TestROM( int size, ushort loadAddress, byte[] data ) : this( size )
        {
            Load( loadAddress, data );
        }

        public void Load( ushort loadAddress, byte[] data )
        {
            data.CopyTo( ROM, loadAddress );
        }

        public byte Read( ushort addr )
        {
            return ROM[ addr ];
        }

        public void Write( ushort addr, byte data )
        {
            ROM[ addr ] = data;
        }
    }

}
