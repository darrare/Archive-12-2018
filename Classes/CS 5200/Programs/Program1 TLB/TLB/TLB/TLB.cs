using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLB
{
    class TLB
    {
        const int COLUMN_WIDTH = 10;

        TLBEntry[,] entries;
        int associativity;
        int TLBDataBits;
        int setBits = 0;
        int tagBits = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entries">Number of entries</param>
        /// <param name="associativity">Associativity: 0 = fully associative</param>
        /// <param name="TLBDataBits">How many data bits are in the tlb</param>
        public TLB(int entries, int associativity, int TLBDataBits)
        {
            this.associativity = associativity;
            this.TLBDataBits = TLBDataBits;
            this.entries = new TLBEntry[associativity == 0 ? entries : entries/associativity,associativity == 0 ? 1 : associativity];

            setBits = associativity != 0 ? GetNIn2ToTheN(entries / associativity) : 0;
            tagBits = 32 - setBits - TLBDataBits;

            for (int i = 0; i < (associativity == 0 ? entries : entries / associativity); i++)
            {
                for (int j = 0; j < (associativity == 0 ? 1 : associativity); j++)
                {
                    this.entries[i, j] = new TLBEntry();
                }
            }
        }

        /// <summary>
        /// Gets data from the tlb
        /// </summary>
        /// <param name="uAddress">address to check</param>
        /// <param name="uCycle">cycle called</param>
        /// <returns>Data from the tlb matching the address tag, or 0xffffffff if no hit</returns>
        public uint GetData(uint uAddress, uint uCycle)
        {
            uint tag = uAddress >> TLBDataBits;
            //TLBEntry hit = entries.FirstOrDefault(t => t.tag == tag);
            //if (hit != null)
            //{
            //    hit.lru = uCycle;
            //    return hit.data;
            //}
            return 0xffffffff;
        }

        public void SetData(uint uData, uint uAddress, uint uCycle)
        {
            if (associativity == 0)
            {
                SetDataFullyAssociative(uData, uAddress, uCycle);
            }
            else
            {
                SetDataNAssociative(uData, uAddress, uCycle);
            }
        }


        public void SetDataFullyAssociative(uint uData, uint uAddress, uint uCycle)
        {
            uint tag = uAddress >> TLBDataBits;
            bool set = false;
            for (int i = 0; i < entries.Length; i++)
            {
                if (!entries[i,0].valid)
                {
                    entries[i,0] = new TLBEntry(true, tag, uData, uCycle);
                    set = true;
                    break;
                }
            }
            if (!set)
            {
                uint lowestLRU = uint.MaxValue;
                int index = 0;
                for (int i = 0; i < entries.Length; i++)
                {
                    if (entries[i,0].lru < lowestLRU)
                    {
                        index = i;
                        lowestLRU = entries[i,0].lru;
                    }
                }

                entries[index,0] = new TLBEntry(true, tag, uData, uCycle);
            }
        }
        /// <summary>
        /// Sets the data in the tlb
        /// </summary>
        /// <param name="uData">The data</param>
        /// <param name="uAddress">The address</param>
        /// <param name="uCycle">The cycle called</param>
        public void SetDataNAssociative(uint uData, uint uAddress, uint uCycle)
        {
            uint tag = (uAddress >> TLBDataBits + setBits) & (uint)(1 << (32 - TLBDataBits + setBits)) - 1;
            uint set = (uAddress >> TLBDataBits) & (uint)(1 << setBits) - 1;
            uint pageOffset = uAddress >> TLBDataBits;

            //If the TLB already contains this tag
            int index = 0;
            do
            {
                if (entries[set, index] != null && entries[set,index].tag == tag)
                {
                    TLBEntry e = entries[set, index];
                    e.tag = tag;
                    e.data = uData;
                    e.lru = uCycle;
                    e.valid = true;
                    e.set = set;
                    return;
                }
                index++;
            } while (index < associativity);

            index = 0;
            do
            {
                if (entries[set, index] != null && entries[set, index].valid == false)
                {
                    TLBEntry e = entries[set, index];
                    e.tag = tag;
                    e.data = uData;
                    e.lru = uCycle;
                    e.valid = true;
                    e.set = set;
                    return;
                }
                index++;
            } while (index < associativity);

            uint lowestLRU = uint.MaxValue;
            int lowestIndex = 0;
            index = 0;
            do
            {
                if (entries[set, index].lru < lowestLRU)
                {
                    lowestIndex = index;
                    lowestLRU = entries[set, index].lru;
                }
                index++;
            } while (index < associativity);
            entries[set, lowestIndex].tag = tag;
            entries[set, lowestIndex].data = uData;
            entries[set, lowestIndex].lru = uCycle;
            entries[set, lowestIndex].valid = true;
            entries[set, lowestIndex].set = set;
        }

        /// <summary>
        /// Public print method, will choose the right printing method based on associativity
        /// </summary>
        /// <param name="fileName">The filename to print to</param>
        public void Print(string fileName)
        {
            if (associativity == 0)
            {
                PrintFullyAssociative(fileName);
            }
            else
            {
                PrintNAssociative(fileName);
            }
        }

        /// <summary>
        /// Create and print to a file the contents of this TLB
        /// </summary>
        void PrintFullyAssociative(string fileName)
        {
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + fileName;
            string[] lines = new string[entries.Length]; 

            if (!File.Exists(appPath))
                File.Create(appPath).Close();

            for (int i = 0; i < lines.Length; i++)
            {
                if (entries[i,0].valid)
                    lines[i] = GenerateLine(i.ToString(), "0x" + entries[i,0].tag.ToString("X" + TLBDataBits / 4), entries[i, 0].lru.ToString(), "0x" + entries[i, 0].data.ToString("X" + TLBDataBits / 4));
            }

            using (StreamWriter s = new StreamWriter(appPath))
            {
                s.WriteLine(GenerateFirstLine());
                foreach (string st in lines)
                {
                    s.WriteLine(st);
                }
            }
        }

        /// <summary>
        /// Create and print to a file the contents of this TLB
        /// </summary>
        void PrintNAssociative(string fileName)
        {
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + fileName;
            string[] lines = new string[entries.Length / associativity]; //will be # of sets

            if (!File.Exists(appPath))
                File.Create(appPath).Close();

            for (int i = 0; i < lines.Length; i++)
            {
                string str = "";
                for (int j = 0; j < associativity; j++)
                {
                    if (entries[i,j].valid)
                    {
                        str += GenerateLine((i * associativity + j).ToString(), "0x" + entries[i,j].tag.ToString("X" + TLBDataBits / 4), entries[i,j].lru.ToString(), "0x" + entries[i,j].data.ToString("X" + TLBDataBits / 4), "True");
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    string start = i.ToString();
                    if (associativity != 0)
                    {
                        for (int j = 0; j < COLUMN_WIDTH - start.Length; i++)
                            start += " ";
                        str = start + str;
                    }
                }
                lines[i] = str;
            }

            using (StreamWriter s = new StreamWriter(appPath))
            {
                s.WriteLine(GenerateFirstLine());
                foreach (string st in lines)
                {
                    if (!string.IsNullOrEmpty(st))
                        s.WriteLine(st);
                }
            }
        }

        /// <summary>
        /// Generates the line with the specified column width
        /// </summary>
        string GenerateLine(string block, string tag, string LRU, string data, string valid = "")
        {
            string str = "";

            int index = 0;
            do
            {
                if (associativity != 0)
                {
                    for (int i = 0; i < COLUMN_WIDTH - valid.Length; i++)
                        str += " ";
                    valid += str;
                    str = "";
                }
                for (int i = 0; i < COLUMN_WIDTH - block.Length; i++)
                    str += " ";
                block += str;
                str = "";

                for (int i = 0; i < COLUMN_WIDTH - tag.Length; i++)
                    str += " ";
                tag += str;
                str = "";

                for (int i = 0; i < COLUMN_WIDTH - LRU.Length; i++)
                    str += " ";
                LRU += str;
                str = "";

                for (int i = 0; i < COLUMN_WIDTH - data.Length; i++)
                    str += " ";
                data += str;
                str = "";
                index++;
            } while (index < associativity);

            return valid + block + tag + LRU + data;
        }

        /// <summary>
        /// Creates the first line, aka the header of the file.
        /// </summary>
        string GenerateFirstLine()
        {
            string str = "";
            if (associativity != 0)
            {
                str += "Set";
                for (int i = 0; i < COLUMN_WIDTH - "Set".Length; i++)
                    str += " ";
            }

            int index = 0;
            do
            {
                if (associativity != 0)
                {
                    str += "Valid";
                    for (int i = 0; i < COLUMN_WIDTH - "Valid".Length; i++)
                        str += " ";
                }
                
                str += "Block";
                for (int i = 0; i < COLUMN_WIDTH - "Block".Length; i++)
                    str += " ";

                str += "Tag";
                for (int i = 0; i < COLUMN_WIDTH - "Tag".Length; i++)
                    str += " ";

                str += "LRU";
                for (int i = 0; i < COLUMN_WIDTH - "LRU".Length; i++)
                    str += " ";

                str += "data";
                for (int i = 0; i < COLUMN_WIDTH - "Data".Length; i++)
                    str += " ";
                index++;
            } while (index < associativity);
            return str;
        }

        int GetNIn2ToTheN(int val)
        {
            return ConvertToBinaryRecursive(val).Length - 1;
        }

        string ConvertToBinaryRecursive(int val)
        {
            string strVal = "";

            if (val == 0)
                return "";

            if (val % 2 == 0)
                strVal = "0";
            else
                strVal = "1";
            return strVal + ConvertToBinaryRecursive((int)Math.Floor((float)val / 2));
        }
    }

    class TLBEntry
    {
        public bool valid;
        public uint tag;
        public uint data;
        public uint lru;
        public uint set;

        /// <summary>
        /// Constructor for a TLB entry
        /// </summary>
        /// <param name="valid">Whether or not this entry is valid</param>
        /// <param name="tag">The tag for this entry</param>
        /// <param name="data">The data for this entry</param>
        /// <param name="lru">The cycle at which this was last accessed.</param>
        /// <param name="set">The set at which this element is in</param>
        public TLBEntry(bool valid, uint tag, uint data, uint lru, uint set = 0)
        {
            this.valid = valid;
            this.tag = tag;
            this.data = data;
            this.lru = lru;
            this.set = set;
        }

        /// <summary>
        /// default constructor for when originally created
        /// </summary>
        public TLBEntry()
        {
            valid = false;
        }
    }
}
