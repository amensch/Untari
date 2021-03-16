using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TIAConstants
{
    // write registers
    public const ushort VSYNC = 0x00;
    public const ushort VBLANK = 0x01;
    public const ushort WSYNC = 0x02;
    public const ushort RSYNC = 0x03;
    public const ushort NUSIZ0 = 0x04;
    public const ushort NUSIZ1 = 0x05;
    public const ushort COLUP0 = 0x06;  // player 0 + missle 0
    public const ushort COLUP1 = 0x07;  // player 1 + missle 1
    public const ushort COLUPF = 0x08;  // playfield + ball
    public const ushort COLUBK = 0x09;  // background
    public const ushort CTRLPF = 0x0a;
    public const ushort REFP0 = 0x0b;
    public const ushort REFP1 = 0x0c;
    public const ushort PF0 = 0x0d;
    public const ushort PF1 = 0x0e;
    public const ushort PF2 = 0x0f;
    public const ushort RESP0 = 0x10;
    public const ushort RESP1 = 0x11;
    public const ushort RESM0 = 0x12;
    public const ushort RESM1 = 0x13;
    public const ushort RESBL = 0x14;
    public const ushort AUDC0 = 0x15;
    public const ushort AUDC1 = 0x16;
    public const ushort AUDF0 = 0x17;
    public const ushort AUDF1 = 0x18;
    public const ushort AUDV0 = 0x19;
    public const ushort AUDV1 = 0x1a;
    public const ushort GRP0 = 0x1b;
    public const ushort GRP1 = 0x1c;
    public const ushort ENAM0 = 0x1d;
    public const ushort ENAM1 = 0x1e;
    public const ushort ENABL = 0x1f;
    public const ushort HMP0 = 0x20;
    public const ushort HMP1 = 0x21;
    public const ushort HMM0 = 0x22;
    public const ushort HMM1 = 0x23;
    public const ushort HMBL = 0x24;
    public const ushort VDELP0 = 0x25;
    public const ushort VDELP1 = 0x26;
    public const ushort VDELBL = 0x27;
    public const ushort RESMP0 = 0x28;
    public const ushort RESMP1 = 0x29;
    public const ushort HMOVE = 0x2a;
    public const ushort HMCLR = 0x2b;
    public const ushort CXCLR = 0x2c;

    // read registers

}
