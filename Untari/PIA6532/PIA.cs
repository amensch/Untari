using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PIA : IBusDevice
{
    private int _counterValue;
    private int _currentInterval;
    private int _lastSetInterval;

    // 8 bit registers
    private int INTIM;
    private int INSTAT;
    private int TIM1T;
    private int TIM8T;
    private int TIM64T;
    private int T1024T;

    public void PowerOn()
    {
        _counterValue = 1024;
        _currentInterval = 1024;
        _lastSetInterval = 1024;

        // The timer output starts as a random value. This is important as
        // many games rely on this.
        System.Random rnd = new System.Random();
        INTIM = rnd.Next( 0, 0x100 );
    }

    public byte Read( ushort addr )
    {
        throw new NotImplementedException();
    }

    public void Write( ushort addr, byte data )
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        _counterValue--;
        if(_counterValue <= 0)
        {
            Decrement();
        }
    }

    private void Decrement()
    {

    }

}
