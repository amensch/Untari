using KDS.e6502CPU;

namespace KDS.Untari
{
    public class CPU : e6502, IReadyDevice
    {
        private int Clocks = 0;

        public CPU(IBusDevice bus) : base(bus) { }
        public CPU(IBusDevice bus, e6502Type cpuType) : base(bus, cpuType) { }

        public void ClearReadyLatch()
        {
            RDY = false;
        }

        public void SetReadyLatch()
        {
            RDY = true;
        }

        public void Tick()
        {
            if(Clocks == 0)
            {
                Clocks = ClocksForNext();
            }
            else if(Clocks == 1)
            {
                ExecuteNext();
            }
            Clocks--;
        }
    }
}
