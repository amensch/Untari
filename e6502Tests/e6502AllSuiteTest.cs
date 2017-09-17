﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;

namespace e6502Tests
{
    [TestClass]
    public class e6502AllSuiteTest
    {
        [TestMethod]
        public void RunAllSuiteTest()
        {
            /*
             *  Load and run test program found here:
             *  https://codegolf.stackexchange.com/questions/12844/emulate-a-mos-6502-cpu?rq=1
             *  If the program gets to PC=$45C0 then all tests passed.
             */

            e6502 cpu = new e6502(e6502Type.CMOS);
            TestROM rom = new TestROM( 0x10000, 0x4000, File.ReadAllBytes( @"..\..\Resources\AllSuiteA.bin" ) );
            cpu.Boot( rom, 0x0400 );

            ushort prev_pc;
            long instr_count = 0;
            long cycle_count = 0;
            do
            {
                instr_count++;
                prev_pc = cpu.PC;
                cycle_count += cpu.ExecuteNext();
            } while( prev_pc != cpu.PC );

            Debug.WriteLine( "Cycles: " + cycle_count.ToString( "N0" ) );
            Debug.WriteLine( "Instructions: " + instr_count.ToString( "N0" ) );

            Assert.AreEqual( 0x45c0, cpu.PC, "Test program failed at $" + cpu.PC.ToString( "X4" ) );
            Assert.AreEqual( 0xff, rom.Read( 0x0210 ), "Test value failed" );    
        }
    }
}
