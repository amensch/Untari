using System;

public class SystemConsole
{
    private SystemBus bus;

    public SystemConsole()
    {
        bus = new SystemBus();
    }

    public void Boot()
    {
        bus.Boot();
    }

    public void Tick()
    {
        bus.Tick();
    }
}
