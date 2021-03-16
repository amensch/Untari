using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UntariTests
{
    [TestClass]
    public class TIATests
    {
        private class MockCPU : IReadyDevice
        {
            public bool RDY { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        }

        private TIA CreateTIA()
        {
            var tia = new TIA(new MockCPU());
            tia.Boot();
            return tia;
        }

        [TestMethod]
        public void Test1()
        {
            var tia = CreateTIA();

        }
    }
}
