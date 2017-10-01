using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UntariTests
{
    [TestClass]
    public class PIATests
    {
        private const ushort TIM1T = 0x0294;
        private const ushort TIM8T = 0x0295;
        private const ushort TIM64T = 0x0296;
        private const ushort T1024T = 0x0297;

        private const ushort INTIM = 0x0284;

        [TestMethod]
        public void PIAWriteInterval_1()
        {
            PIA pia = new PIA();
            pia.Boot();

            pia.Write( TIM1T, 100 );
            byte result = pia.Read( INTIM );

            Assert.AreEqual( 99, result );
        }

        [TestMethod]
        public void PIATestInterval_1()
        {
            byte result;
            PIA pia = new PIA();
            pia.Boot();
            pia.Write( TIM1T, 100 );
            result = pia.Read( INTIM );
            Assert.AreEqual( 99, result );


            for( int ii = 99; ii >= 1; ii-- )
            {
                pia.Tick();
                result = pia.Read( INTIM );
                Assert.AreEqual( ii-1, result );
            }

            pia.Tick();
            result = pia.Read( INTIM );
            Assert.AreEqual( 255, result );

            pia.Tick();
            result = pia.Read( INTIM );
            Assert.AreEqual( 254, result );
        }

        [TestMethod]
        public void PIAWriteInterval_8()
        {
            PIA pia = new PIA();
            pia.Boot();

            pia.Write( TIM8T, 100 );
            byte result = pia.Read( INTIM );

            Assert.AreEqual( 99, result );
        }

        [TestMethod]
        public void PIATestInterval_8()
        {
            byte result;
            PIA pia = new PIA();
            pia.Boot();
            pia.Write( TIM8T, 100 );

            for( int ii = 1; ii <= 7; ii++ )
                pia.Tick();

            result = pia.Read( INTIM );
            Assert.AreEqual( 99, result );


            for( int ii = 99; ii >= 1; ii-- )
            {
                for( int jj=1; jj <= 8; jj++ )
                {
                    pia.Tick();
                }
                result = pia.Read( INTIM );
                Assert.AreEqual( ii - 1, result );
            }

            pia.Tick();
            result = pia.Read( INTIM );
            Assert.AreEqual( 255, result );

            pia.Tick();
            result = pia.Read( INTIM );
            Assert.AreEqual( 254, result );
        }

        [TestMethod]
        public void PIAWriteInterval_64()
        {
            PIA pia = new PIA();
            pia.Boot();

            pia.Write( TIM64T, 100 );
            byte result = pia.Read( INTIM );

            Assert.AreEqual( 99, result );
        }

        [TestMethod]
        public void PIATestInterval_64()
        {
            byte result;
            PIA pia = new PIA();
            pia.Boot();
            pia.Write( TIM64T, 100 );

            for( int ii = 1; ii <= 63; ii++ )
                pia.Tick();

            result = pia.Read( INTIM );
            Assert.AreEqual( 99, result );


            for( int ii = 99; ii >= 1; ii-- )
            {
                for( int jj = 1; jj <= 64; jj++ )
                {
                    pia.Tick();
                }
                result = pia.Read( INTIM );
                Assert.AreEqual( ii - 1, result );
            }

            pia.Tick();
            result = pia.Read( INTIM );
            Assert.AreEqual( 255, result );

            pia.Tick();
            result = pia.Read( INTIM );
            Assert.AreEqual( 254, result );
        }

        [TestMethod]
        public void PIAWriteInterval_1024()
        {
            PIA pia = new PIA();
            pia.Boot();

            pia.Write( T1024T, 100 );
            byte result = pia.Read( INTIM );

            Assert.AreEqual( 99, result );
        }

        [TestMethod]
        public void PIATestInterval_1024()
        {
            byte result;
            PIA pia = new PIA();
            pia.Boot();
            pia.Write( T1024T, 100 );

            for( int ii = 1; ii <= 1023; ii++ )
                pia.Tick();

            result = pia.Read( INTIM );
            Assert.AreEqual( 99, result );


            for( int ii = 99; ii >= 1; ii-- )
            {
                for( int jj = 1; jj <= 1024; jj++ )
                {
                    pia.Tick();
                }
                result = pia.Read( INTIM );
                Assert.AreEqual( ii - 1, result );
            }

            pia.Tick();
            result = pia.Read( INTIM );
            Assert.AreEqual( 255, result );

            pia.Tick();
            result = pia.Read( INTIM );
            Assert.AreEqual( 254, result );
        }
    }
}
