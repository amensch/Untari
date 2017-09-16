using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISystemBus
{
    byte Read( ushort addr );
    void Write( ushort addr, byte data );
}
