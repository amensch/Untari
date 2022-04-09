using KDS.e6502;
using KDS.Untari;
using System;

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

    private readonly BusDeviceInterface DeviceInterface;

    public SystemBus()
    {
        // create the bus devices
        cartridge = new Cartridge();
        pia = new PIA();
        ram = new RAM();
        cpu = new CPU( this );
        tia = new TIA(cpu);

        // create the chain of command for Read and Write
        DeviceInterface = new BusDeviceInterface( cartridge, CARTRIDGE_SELECT_MASK, CARTRIDGE_CHIP_SELECT );
        BusDeviceInterface PIAInterface = new BusDeviceInterface( pia, PIA_SELECT_MASK, PIA_CHIP_SELECT );
        BusDeviceInterface RAMInterface = new BusDeviceInterface( ram, RAM_SELECT_MASK, RAM_CHIP_SELECT );
        BusDeviceInterface TIAInterface = new BusDeviceInterface( tia, TIA_SELECT_MASK, TIA_CHIP_SELECT );

        DeviceInterface.SetNext( PIAInterface );
        PIAInterface.SetNext( RAMInterface );
        RAMInterface.SetNext( TIAInterface );
    }

    public byte Read( ushort addr )
    {
        return DeviceInterface.Read( addr );
    }

    public void Write( ushort addr, byte value )
    {
        DeviceInterface.Write( addr, value );
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
