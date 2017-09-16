using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KDS.e6502;
using System.IO;
using System.Diagnostics;

namespace e6502Tests
{
    [TestClass]
    public class e6502FuncTest
    {
        [TestMethod]
        public void RunFuncTestProgram()
        {
            /*
             *  This loads a test program that exercises all the standard instructions of the 6502.
             *  If the program gets to PC=$3399 then all tests passed.
             */

            e6502 cpu = new e6502();
            TestROM rom = new TestROM( 0x10000, 0x0000, File.ReadAllBytes( @"..\..\Resources\6502_functional_test.bin" ) );
            cpu.Boot( rom, 0x0400 );

            ushort prev_pc = 0x0400;
            long instr_count = 0;
            long cycle_count = 0;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            int same_pc_count = 0;
            do
            {
                cpu.Tick();
                if( prev_pc == cpu.PC )
                {
                    same_pc_count++;
                }
                else
                {
                    instr_count++;
                    same_pc_count = 0;
                    prev_pc = cpu.PC;
                }
                cycle_count++;
            } while( same_pc_count < 10 );
            sw.Stop();

            cycle_count -= same_pc_count / 2;

            Debug.WriteLine("Time: " + sw.ElapsedMilliseconds.ToString() + " ms");
            Debug.WriteLine("Cycles: " + cycle_count.ToString("N0"));
            Debug.WriteLine("Instructions: " + instr_count.ToString("N0"));

            double mhz = (cycle_count / sw.ElapsedMilliseconds) / 1000;
            Debug.WriteLine("Effective Mhz: " + mhz.ToString("N1"));

            Assert.AreEqual(0x3399, cpu.PC, "Test program failed at $" + cpu.PC.ToString("X4"));
        }
    }
}
