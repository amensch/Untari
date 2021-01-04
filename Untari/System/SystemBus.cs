using KDS.e6502CPU;
using KDS.Untari;
using System;
using System.IO;

public class SystemBus : IBusDevice
{
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

    private readonly Cartridge cartridge;
    private readonly PIA pia;
    private readonly RAM ram;
    private readonly TIA tia;
    private readonly CPU cpu;

    public SystemBus()
    {
        // create the bus devices
        cartridge = new Cartridge();
        pia = new PIA();
        ram = new RAM();
        cpu = new CPU( this );
        tia = new TIA(cpu);
    }

    public byte Read( ushort addr )
    {
        // read order: cartridge, PIA, RAM, TIA
        if((addr & CARTRIDGE_SELECT_MASK) == CARTRIDGE_CHIP_SELECT)
        {
            return cartridge.Read(addr);
        }
        else if ((addr & PIA_SELECT_MASK) == PIA_CHIP_SELECT)
        {
            return pia.Read(addr);
        }
        else if((addr & RAM_SELECT_MASK) == RAM_CHIP_SELECT)
        {
            return ram.Read(addr);
        }
        else if((addr & TIA_SELECT_MASK) == TIA_CHIP_SELECT)
        {
            return tia.Read(addr);  
        }
        else
        {
            throw new InvalidOperationException("No device mask matched for read of address " + addr.ToString("X4"));
        }
    }

    public void Write( ushort addr, byte value )
    {
        // write order: cartridge, PIA, RAM, TIA
        if ((addr & CARTRIDGE_SELECT_MASK) == CARTRIDGE_CHIP_SELECT)
        {
            cartridge.Write(addr, value);
        }
        else if ((addr & PIA_SELECT_MASK) == PIA_CHIP_SELECT)
        {
            pia.Write(addr, value); 
        }
        else if ((addr & RAM_SELECT_MASK) == RAM_CHIP_SELECT)
        {
            ram.Write(addr, value); 
        }
        else if ((addr & TIA_SELECT_MASK) == TIA_CHIP_SELECT)
        {
            tia.Write(addr, value); 
        }
        else
        {
            throw new InvalidOperationException("No device mask matched for write to address " + addr.ToString("X4"));
        }
    }

    public void Tick()
    {
        cpu.Tick();
        cartridge.Tick();
        pia.Tick();
        tia.Tick();
    }

    public void Boot()
    {
        cpu.Boot();
        ram.Boot();
        pia.Boot();
        tia.Boot();
        cartridge.Boot();
    }
}
