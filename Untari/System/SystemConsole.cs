using System;

public class SystemConsole
{
    private e6502 cpu;
    private SystemBus bus;

    public SystemConsole()
    {
        bus = new SystemBus();
        cpu = new e6502( bus );
    }

    public void Boot()
    {
        Console.WriteLine( "System Console Boot" );
        cpu.Boot();
        bus.Boot();
    }

    public void Tick()
    {
        Console.WriteLine( "System Console Tick" );
        cpu.Tick();
        bus.Tick();
    }
}
