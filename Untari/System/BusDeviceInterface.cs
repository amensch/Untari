using System;
using KDS.e6502;

public class BusDeviceInterface
{
    protected BusDeviceInterface NextBusDevice;
    private readonly IBusDevice BusDevice;
    private readonly int DeviceSelectMask;
    private readonly int DeviceChipSelect;

    public BusDeviceInterface( IBusDevice device, int selectMask, int chipSelect )
    {
        BusDevice = device;
        DeviceSelectMask = selectMask;
        DeviceChipSelect = chipSelect;
    }

    public void SetNext( BusDeviceInterface device )
    {
        NextBusDevice = device;
    }

    public virtual byte Read( ushort addr )
    {
        if( (addr & DeviceSelectMask) == DeviceChipSelect )
        {
            return BusDevice.Read( addr );
        }
        else
        {
            if( NextBusDevice == null )
            {
                throw new InvalidOperationException( "No device mask matched for read of " + addr.ToString( "X4" ) );
            }
            return NextBusDevice.Read( addr );
        }
    }

    public virtual void Write( ushort addr, byte value )
    {
        if( (addr & DeviceSelectMask) == DeviceChipSelect )
        {
            BusDevice.Write( addr, value );
        }
        else
        {
            if( NextBusDevice == null )
            {
                throw new InvalidOperationException( "No device mask matched for write to " + addr.ToString( "X4" ) );
            }
            NextBusDevice.Write( addr, value );
        }
    }
}
