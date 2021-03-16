using KDS.e6502CPU;

namespace KDS.Untari
{
    public class CPU : e6502, IReadyDevice
    {
        private int Clocks = 0;
        public bool RDY { get; set; } = true;

        public CPU(IBusDevice bus) : base(bus) { }
        public CPU(IBusDevice bus, e6502Type cpuType) : base(bus, cpuType) { }

        public void Tick()
        {
            if (RDY)
            {
                if (Clocks == 0)
                {
                    Clocks = ClocksForNext();
                }
                else if (Clocks == 1)
                {
                    ExecuteNext();
                }
                Clocks--;
            }
        }
    }
}
