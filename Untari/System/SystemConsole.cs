using System;

public class SystemConsole
{
    private readonly SystemBus bus;

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
