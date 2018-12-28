using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLB
{
    class Program
    {
        static void Main(string[] args)
        {
            uint uCycle = 1;
            //Micro TLB is 10 entries, fully associative, 16 - bit data value
            TLB MicroTLB = new TLB(10, 0, 16);
            MicroTLB.SetData(0x10, 0xABCDEF12, uCycle++);
            MicroTLB.SetData(0x20, 0xBBCDEF12, uCycle++);
            MicroTLB.SetData(0x30, 0xCBCDEF12, uCycle++);
            MicroTLB.SetData(0x40, 0xDBCDEF12, uCycle++);
            MicroTLB.SetData(0x50, 0xEBCDEF12, uCycle++);
            MicroTLB.SetData(0x60, 0xFBCDEF12, uCycle++);
            MicroTLB.Print("MicroTLB1.txt");
            //the following line fires up Notepad to save the time of doing it manually
            System.Diagnostics.Process.Start("notepad.exe", "MicroTLB1.txt");
            MicroTLB.SetData(0x70, 0x0BCDEF12, uCycle++);
            MicroTLB.SetData(0x80, 0x1BCDEF12, uCycle++);
            MicroTLB.SetData(0x90, 0x2BCDEF12, uCycle++);
            MicroTLB.SetData(0xA0, 0x3BCDEF12, uCycle++);
            MicroTLB.SetData(0xB0, 0x4BCDEF12, uCycle++);
            MicroTLB.Print("MicroTLB2.txt");
            System.Diagnostics.Process.Start("notepad.exe", "MicroTLB2.txt");


            //L2 TLB is 512 entries, 4-way, 16-bit data
            uCycle = 0;
            TLB L2TLB = new TLB(512, 4, 16);
            L2TLB.SetData(0x10, 0xABCDEF12, uCycle++);
            L2TLB.SetData(0x20, 0xBBCDEF12, uCycle++);
            L2TLB.SetData(0x30, 0xCBCDEF12, uCycle++);
            L2TLB.SetData(0x40, 0xDBCDEF12, uCycle++);
            L2TLB.SetData(0x50, 0xEBCDEF12, uCycle++);
            L2TLB.SetData(0x60, 0xFBCDEF12, uCycle++);
            L2TLB.SetData(0x70, 0x0BCDEF12, uCycle++);
            L2TLB.SetData(0x80, 0x1BCDEF12, uCycle++);
            L2TLB.SetData(0x90, 0x2BCDEF12, uCycle++);
            L2TLB.SetData(0xA0, 0x3BCDEF12, uCycle++);
            L2TLB.SetData(0xB0, 0x4BCDEF12, uCycle++);
            L2TLB.Print("L2TLB.txt");
            System.Diagnostics.Process.Start("notepad.exe", "L2TLB.txt");
        }
    }
}
