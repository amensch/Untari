using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UntariTests
{
    [TestClass]
    public class TIATests
    {
        private class MockCPU : IReadyDevice
        {
            public bool RDY { get; set; }
        }

        private TIA CreateTIA()
        {
            var tia = new TIA(new MockCPU());
            tia.Boot();
            return tia;
        }

        [TestMethod]
        public void OutputAction()
        {
            var tia = CreateTIA();
            for(int ii = 1; ii < 20000; ii++)
            {
                tia.Tick();
            }
            tia.WritePixelActions();
        }
    }
}
