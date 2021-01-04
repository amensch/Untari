using KDS.e6502CPU;

public class PIA : IBusDevice
{
    /*
     * The PIA chip is an off-the-shelf 6532 Peripheral Interface Adaptor which has three functions: 
     * a programmable timer, 128 bytes of RAM, and two 8 bit parallel I/O ports.
     * 
     * The PIA uses the same clock as the microprocessor so that one PIA cycle occurs for each machine cycle. 
     * The PIA can be set for one of four different "intervals", where each interval is some multiple of the 
     * clock (and therefore machine cycles). A value from 1 to 255 is loaded into the PIA which will 
     * be decremented by one at each interval. The timer can now be read by the microprocessor to determine 
     * elapsed time for timing various software operations and keep them synchronized with the hardware (TIA chip). 
     * 
     *  0x0280 SWCHA Port A (read/write)
     *      Joysticks Bit Direction Player (when configured as all input)
     *                  7     right P0      A zero is written when the player moves the joystick
     *                  6      left P0      All 1's means the joystick is idle
     *                  5      down P0
     *                  4        up P0
     *                  3     right P1
     *                  2      left P1
     *                  1      down P1
     *                  0        up P1     
     *      Paddle Triggers (values are read in the TIA INP0-INPT3)
     *                  7           P0
     *                  6           P1
     *                  3           P2
     *                  2           P3
     *      Keyboard Controllers (when row is triggered, check TIA INPT0,INPT1,INPT4 for column trigger)
     *                  7   bottom  P0
     *                  6    third  P0
     *                  5   second  P0
     *                  4      top  P0
     *                  3   bottom  P1
     *                  2    third  P1
     *                  1   second  P1
     *                  0      top  P1
     *                  
     *  0x0281 SWACNT Port A data direction register (0 = input, 1 = output)
     *  
     *  0x0282 SWCHB Port B Console Switches (read only - hard wired as input)
     *      Bit Switch          Meaning
     *      7   P1 difficulty   0 = normal 1 = pro
     *      6   P0 difficult    0 = normal 1 = pro
     *      5/4 not used
     *      3   color           0 = B/W 1 = color
     *      2   not used
     *      1   game select     0 = switch pressed
     *      0   game reset      0 = switch pressed
     *  
     *  0x0283 SWBCNT Port B data direction register (hard wired as input)
     *  
     *  0x0284 INTIM Timer Value (read only)
     *  0x0294 TIM1T 1 clock interval timer (write only)
     *  0x0295 TIM8T 8 clock interval timer (write only)
     *  0x0296 TIM64T 64 clock interval timer (write only)
     *  0x0297 T1024T 1024 clock interval timer (write only)
     */

    private int intervalCounter;
    private int currentInterval;

    private byte TIMINT;
    private byte INTIM; // timer value at 0x0284
    private byte TIM1T;
    private byte TIM8T;
    private byte TIM64T;
    private byte T1024T;

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
    private byte SWBCNT;    // 0x0283 (data direction register - hard wired as input)

    public void Boot()
    {
        intervalCounter = 1024;
        currentInterval = 1024;

        // The timer output starts as a random value. This is important because
        // many games rely on this.
        System.Random rnd = new System.Random();
        INTIM = (byte) rnd.Next( 0, 0x100 );

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
            case 0x00:  // 0x0280
                value = (byte)(SWCHA & ~SWACNT);
                break;
            case 0x01:
                value = SWACNT;
                break;
            case 0x02:
                value = (byte)(SWCHB & ~SWBCNT);
                break;
            case 0x03:
                value = SWBCNT;
                break;
            case 0x04:  // 0x0284 read value at INTIM
                value = INTIM;
                break;
            case 0x05:  // 0x0295 TIM8T
                value = TIMINT;
                break;
            case 0x06:  // 0x0296 TIM64T
                value = INTIM;
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
                SWCHA = (byte)(SWCHA & SWACNT);
                break;
            case 0x01:
                SWACNT = data;
                break;
            case 0x02:
                SWCHB = (byte)(SWCHB & SWBCNT);
                break;
            case 0x03:
                SWBCNT = 0;  // hard wired for input only
                break;
            case 0x04:
                SetInterval(data, 1);
                break;
            case 0x05:
                SetInterval(data, 8);
                break;
            case 0x06:
                SetInterval(data, 64);
                break;
            case 0x07:
                SetInterval(data, 1024);
                break;
        }
    }

    public void Tick()
    {
        intervalCounter--;
        if( intervalCounter <= 0 )
        {
            DecrementTimer();
        }
    }


    private void SetInterval( byte data, int interval )
    {
        if(interval == 1)
        {
            INTIM = TIM1T = data;
        }
        else if(interval == 8)
        {
            INTIM = TIM8T = data;
        }
        else if(interval == 64)
        {
            INTIM = TIM64T = data;
        }
        else if(interval == 1024)
        {
            INTIM = T1024T = data;
        }

        intervalCounter = currentInterval = interval;
        TIMINT = 0;

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

        intervalCounter--;
        if(intervalCounter == 0)
        {
            INTIM--;
            intervalCounter = currentInterval;
        }

        if( INTIM == 0 )
        {
            TIMINT = TIMER_INTERRUPT_FLAG;
            intervalCounter = currentInterval = 1;
        }
    }

}
