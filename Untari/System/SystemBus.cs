using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SystemBus : IBusDevice
{
    private e6502 _cpu;
    private PIA _pia;
    private RAM _ram;
    private Cartridge _cartridge;
    private TIA _tia;

    // A12 = 0, A7 = 1,  A9 = 1
    private const int PIA_SELECT_MASK = 0x1280;
    private const int PIA_CHIP_SELECT = 0x0280;

    // A12 = 0, A7 = 1,  A9 = 0
    private const int RAM_SELECT_MASK = 0x1280;
    private const int RAM_CHIP_SELECT = 0x0080;

    // A12 = 1
    private const int CARTRIDGE_SELECT_MASK = 0x1000;
    private const int CARTRIDGE_CHIP_SELECT = 0x1000;

    // A12 = 0, A7 = 0
    private const int TIA_SELECT_MASK = 0x1080;
    private const int TIA_CHIP_SELECT = 0x0000;

    public SystemBus()
    {
        _pia = new PIA();
        _ram = new RAM();
        _tia = new TIA();
        _cartridge = new Cartridge();
        _cpu = new e6502(this);
    }

    public void Boot()
    {
        _cpu.Boot();
        _pia.Boot();
        _ram.Boot();
        _tia.Boot();
        _cartridge.Boot();
    }

    public byte Read( ushort addr )
    {
        return GetSelectedChip( addr ).Read( addr );
    }

    public void Write( ushort addr, byte data )
    {
        GetSelectedChip( addr ).Write( addr, data );
    }

    private IBusDevice GetSelectedChip(ushort addr)
    {
        // Bits 7 and 12 act as chip select.
        // Bit 9 acts as RAM select when PIA is selected.

        if( (addr & CARTRIDGE_SELECT_MASK) == CARTRIDGE_CHIP_SELECT )
        {
            return _cartridge;
        }
        else if( (addr & PIA_SELECT_MASK) == PIA_CHIP_SELECT )
        {
            return _pia;
        }
        else if( (addr & RAM_SELECT_MASK) == RAM_CHIP_SELECT )
        {
            return _ram;
        }
        else
            return _tia;
    }
}
