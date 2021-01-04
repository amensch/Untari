using KDS.e6502CPU;

public class PIA : IBusDevice
{
    private int _counterValue;
    private int _timerCountValue;
    private int _currentInterval;

    private byte TIMINT;

    private const ushort ADDRESS_MASK = 0x0007;
    private const byte TIMER_INTERRUPT_FLAG = 0x80;

    // port A is fully software controlled and can be configured for input or output
    // Write at 0x0281 (SWACNT) to set the I/O direction for each pin.
    // "0" is input and "1" is output.
    //
    // The port itself is read and written at 0x0280 (SWCHA)
    private byte SWCHA;
    private byte SWACNT;    // 0x0281 (data direction register)

    // port B is used for the console switches and is read only
    // port is read via 0x0282 (SWCHB)
    private byte SWCHB;
    private byte SWBCNT;    // 0x0283 (data direction register)

    public void Boot()
    {
        _counterValue = 1024;
        _currentInterval = 1024;

        // The timer output starts as a random value. This is important because
        // many games rely on this.
        System.Random rnd = new System.Random();
        _timerCountValue = (byte) rnd.Next( 0, 0x100 );

        SWCHA = 0;
        SWCHB = 0;

        SWACNT = 0xff;    // IO is high (output) by default
        SWBCNT = 0;       // system is hardwired for input only
    }

    public byte Read( ushort addr )
    {
        int register = addr & ADDRESS_MASK;
        byte value = 0;

        switch( register )
        {
            case 0x00:
                value = SWCHA;
                break;
            case 0x01:
                value = SWACNT;
                break;
            case 0x02:
                value = SWCHB;
                break;
            case 0x03:
                value = SWBCNT;
                break;
            case 0x04:  // 0x0284 read value at TIMINT
                value = (byte)_timerCountValue;
                break;
            case 0x05:  // 0x0295 TIM8T
                value = TIMINT;
                break;
            case 0x06:  // 0x0296 TIM64T
                value = (byte) _timerCountValue;
                break;
            case 0x07:  // 0x0297 T1024T
                value = TIMINT;
                break;
        }
        return value;
    }

    public void Write( ushort addr, byte data )
    {
        int register = addr & ADDRESS_MASK;

        switch( register )
        {
            case 0x00:
                SWCHA = data;
                break;
            case 0x04:
                SetInterval( data, 1 );
                break;
            case 0x05:
                SetInterval( data, 8 );
                break;
            case 0x06:
                SetInterval( data, 64 );
                break;
            case 0x07:
                SetInterval( data, 1024 );
                break;
        }
    }

    public void Tick()
    {
        _counterValue--;
        if( _counterValue <= 0 )
        {
            DecrementTimer();
        }
    }


    private void SetInterval( byte data, int interval )
    {
        _timerCountValue = data;
        TIMINT = 0;

        _currentInterval = interval;
        _counterValue = _currentInterval;

        // decrements immediately after setting
        DecrementTimer();
    }

    private void DecrementTimer()
    {
        /* From the Stella docs
         * 
         * When the timer reaches zero:
         * The PIA decrements the value or count loaded into it once each interval until it reaches 0. 
         * It holds that 0 counts for one interval, then the counter flips to FF(HEX) and decrements 
         * once each clock cycle, rather than once per interval. 
         * 
         * The purpose of this feature is to allow the programmer to determine how long ago the timer 
         * zeroed out in the event the timer was read after it passed zero.
         */

        _timerCountValue--;

        if( _timerCountValue < 0 )
        {
            TIMINT = TIMER_INTERRUPT_FLAG;
            _timerCountValue = 0xff;
            _counterValue = 1;
            _currentInterval = 1;
        }
        else
        {
            _counterValue = _currentInterval;
        }
    }

}
