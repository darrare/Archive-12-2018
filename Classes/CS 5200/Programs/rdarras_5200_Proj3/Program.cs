using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Program3
{
    class Program
    {
        static string strInput = "FPVA.txt";
        static string strOutput = "StatsFPVA.txt";

        static int nMicroTLBHit = 0;
        static int nMicroTLBMiss = 0;
        static int nL2TLBHit = 0;
        static int nL2TLBMiss = 0;
        static int nL1CacheHit = 0;
        static int nL1CacheMiss = 0;
        static int nL2CacheHit = 0;
        static int nL2CacheMiss = 0;

        static uint uCycle = 1;
        static TLB microTLB = new TLB(10, 0, 16);
        static TLB L2TLB = new TLB(512, 4, 16);
        static Cache L1Cache = new Cache(32 * 1024, 64, 4);
        static Cache L2Cache = new Cache(1 * 1024 * 1024, 64, 16);

        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TxtFiles\\L2TLBPreload.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split('\t');
                    L2TLB.SetData(Convert.ToUInt32(parts[0], 16), Convert.ToUInt32(parts[1], 16), uCycle++);
                }
            }

            using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TxtFiles\\" + strInput))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    GetDataFromVA(Convert.ToUInt32(line, 16), uCycle++);
                }
            }

            using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Output\\" + strOutput))
            {
                sw.WriteLine("uCycle = " + uCycle);
                sw.WriteLine("nMicroTLBHit = " + nMicroTLBHit);
                sw.WriteLine("nMicroTLBMiss = " + nMicroTLBMiss);
                sw.WriteLine("nL2TLBHit = " + nL2TLBHit);
                sw.WriteLine("nL2TLBMiss = " + nL2TLBMiss);
                sw.WriteLine("nL1CacheHit = " + nL1CacheHit);
                sw.WriteLine("nL1CacheMiss = " + nL1CacheMiss);
                sw.WriteLine("nL2CacheHit = " + nL2CacheHit);
                sw.WriteLine("nL2CacheMiss = " + nL2CacheMiss);
            }
        }

        static void GetDataFromVA(uint uVirtualAddress, uint uCycle)
        {
            uint physAddress;
            if (L1Cache.Get(uVirtualAddress, uCycle))
            {
                nL1CacheHit++;
                return;
            }
            else
            {
                nL1CacheMiss++;
                uint microVal = microTLB.GetData(uVirtualAddress, uCycle);
                if (microVal != 0xffff)
                {
                    nMicroTLBHit++;
                    uint pageOffset = uVirtualAddress << 16; //Shift left to clear tag data.
                    pageOffset = pageOffset >> 48; //shift right to contain only page offset data
                    microVal = microVal << 16; //shift left to leave 16 bits of 0's
                    physAddress = microVal + pageOffset; //add the two, replacing the 0's in the microVal with the page offset
                }
                else
                {
                    nMicroTLBMiss++;
                    uint l2Val = L2TLB.GetData(uVirtualAddress, uCycle);
                    if (l2Val != 0xffff)
                    {
                        nL2TLBHit++;
                        microTLB.SetData(l2Val, uVirtualAddress, uCycle);
                        uint pageOffset = uVirtualAddress << 16; //Shift left to clear tag data.
                        pageOffset = pageOffset >> 48; //shift right to contain only page offset data
                        l2Val = l2Val << 16; //shift left to leave 16 bits of 0's
                        physAddress = l2Val + pageOffset; //add the two, replacing the 0's in the l2val with the page offset
                    }
                    else
                    {
                        nL2TLBMiss++;
                        return;
                    }
                }
                if (L2Cache.Get(physAddress, uCycle))
                {
                    nL2CacheHit++;
                    return;
                }
                else
                {
                    nL2CacheMiss++;
                    return;
                }
            }
        }
    }
}
