using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program2Cache
{
    class Cache
    {
        const int COLUMN_WIDTH = 10;

        CacheEntry[,] entries;
        int cacheSize;
        int blockSize;
        int associativity;
        int blockIndexBits;
        int setIndexBits;
        int numSets;
        int tagBits;

        /// <summary>
        /// Constructor for the Cache class
        /// </summary>
        /// <param name="cacheSize">size (in bytes) of the cache</param>
        /// <param name="blockSize">size (in bytes) of the blocks</param>
        /// <param name="associativity">associativity</param>
        public Cache(int cacheSize, int blockSize, int associativity)
        {
            entries = new CacheEntry[associativity == 0 ? (cacheSize / blockSize) : (cacheSize / blockSize) / associativity, associativity == 0 ? 1 : associativity];
            for (int i = 0; i < (associativity == 0 ? (cacheSize / blockSize) : (cacheSize / blockSize) / associativity); i++)
            {
                for (int j = 0; j < (associativity == 0 ? 1 : associativity); j++)
                {
                    entries[i, j] = new CacheEntry();
                }
            }

            this.cacheSize = cacheSize;
            this.blockSize = blockSize;
            this.associativity = associativity;

            blockIndexBits = GetNIn2ToTheN(blockSize);
            setIndexBits = GetNIn2ToTheN(associativity == 0 ? 0 : entries.Length / associativity);
            numSets = associativity == 0 ? (cacheSize / blockSize) : (cacheSize / blockSize) / associativity;
            tagBits = 32 - (blockIndexBits + setIndexBits);
        }

        /// <summary>
        /// public get method, decides which private get method to call
        /// </summary>
        /// <param name="uAddress">Address</param>
        /// <param name="uCycle">Cycle</param>
        /// <returns>Whether it was a hit or miss</returns>
        public bool Get(uint uAddress, uint uCycle)
        {
            if (associativity == 0)
            {
                return GetFullyAssociative(uAddress, uCycle);
            }
            else
            {
                return GetNAssociative(uAddress, uCycle);
            }
        }

        /// <summary>
        /// private get method for fully associative
        /// </summary>
        /// <param name="uAddress">Address</param>
        /// <param name="uCycle">Cycle</param>
        /// <returns>Whether it was a hit or miss</returns>
        bool GetFullyAssociative(uint uAddress, uint uCycle)
        {
            uint tag = (uAddress >> 0 + blockIndexBits);
            int invalidIndex = int.MaxValue;
            int LRUIndex = int.MaxValue;
            uint storedLRU = int.MaxValue;
            bool foundInvalid = false;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i, 0].tag == tag)
                {
                    entries[i, 0].LRU = uCycle;
                    return true;
                }
                if (!entries[i, 0].isValid && !foundInvalid)
                {
                    foundInvalid = true;
                    invalidIndex = i;
                }
                else if (!foundInvalid)
                {
                    if (storedLRU > entries[i, 0].LRU)
                    {
                        storedLRU = entries[i, 0].LRU;
                        LRUIndex = i;
                    }
                }
            }

            if (foundInvalid)
            {
                entries[invalidIndex, 0].tag = tag;
                entries[invalidIndex, 0].LRU = uCycle;
                entries[invalidIndex, 0].isValid = true;
                return false;
            }
            else
            {
                entries[LRUIndex, 0].tag = tag;
                entries[LRUIndex, 0].LRU = uCycle;
                return false;
            }
        }

        /// <summary>
        /// private get method for n associative
        /// </summary>
        /// <param name="uAddress">Address</param>
        /// <param name="uCycle">Cycle</param>
        /// <returns>Whether it was a hit or miss</returns>
        bool GetNAssociative(uint uAddress, uint uCycle)
        {
            uint tag = (uAddress >> setIndexBits + blockIndexBits);
            uint set = (uAddress >> blockIndexBits) & (uint)(1 << setIndexBits) - 1;

            int invalidIndex = int.MaxValue;
            int LRUIndex = int.MaxValue;
            uint storedLRU = int.MaxValue;
            bool foundInvalid = false;
            for (int i = 0; i < associativity; i++)
            {
                if (entries[set, i].tag == tag)
                {
                    entries[set, i].LRU = uCycle;
                    return true;
                }
                if (!entries[set, i].isValid && !foundInvalid)
                {
                    foundInvalid = true;
                    invalidIndex = i;
                }
                else if (!foundInvalid)
                {
                    if (storedLRU > entries[set, i].LRU)
                    {
                        storedLRU = entries[set, i].LRU;
                        LRUIndex = i;
                    }
                }
            }

            if (foundInvalid)
            {
                entries[set, invalidIndex].tag = tag;
                entries[set, invalidIndex].LRU = uCycle;
                entries[set, invalidIndex].set = set;
                entries[set, invalidIndex].isValid = true;
                return false;
            }
            else
            {
                entries[set, LRUIndex].tag = tag;
                entries[set, LRUIndex].LRU = uCycle;
                entries[set, LRUIndex].set = set;
                return false;
            }
        }

        /// <summary>
        /// Prints the contents of this data structure to a file.
        /// </summary>
        /// <param name="fileName">The name of the file you want to print to</param>
        public void Print(string fileName)
        {
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + fileName;
            string[] lines = new string[numSets];

            if (!File.Exists(appPath))
                File.Create(appPath).Close();

            for (int i = 0; i < lines.Length; i++)
            {
                string str = "";
                for (int j = 0; j < (associativity == 0 ? 1 : associativity); j++)
                {
                    if (entries[i, j].isValid)
                    {
                        str += GenerateLine((associativity == 0 ? i : i * associativity + j).ToString(), "0x" + entries[i, j].tag.ToString("X" + tagBits / 4), entries[i, j].LRU.ToString());
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
                s.WriteLine("Cache size (bytes): " + cacheSize);
                s.WriteLine("Block size (bytes): " + blockSize);
                s.WriteLine("Associativity: " + associativity + "-way");
                s.WriteLine("Number of blocks: " + cacheSize / blockSize);
                s.WriteLine("Number of block index bits: " + blockIndexBits);
                s.WriteLine("Number of set index bits: " + setIndexBits);
                s.WriteLine("Number of sets: " + numSets);
                s.WriteLine("Number of tag bits: " + tagBits);
                s.WriteLine("Set mask: 0x" + ((uint)(1 << setIndexBits) - 1).ToString("x" + 4));
                s.WriteLine("");
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
        string GenerateLine(string block, string tag, string LRU)
        {
            string str = "";

            int index = 0;
            do
            {
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

                index++;
            } while (index < associativity);

            return block + tag + LRU;
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
                str += "Block";
                for (int i = 0; i < COLUMN_WIDTH - "Block".Length; i++)
                    str += " ";

                str += "Tag";
                for (int i = 0; i < COLUMN_WIDTH - "Tag".Length; i++)
                    str += " ";

                str += "LRU";
                for (int i = 0; i < COLUMN_WIDTH - "LRU".Length; i++)
                    str += " ";
                index++;
            } while (index < associativity);
            return str;
        }

        /// <summary>
        /// Gets n in 2^n
        /// </summary>
        /// <param name="val">the value to find 2^n of</param>
        /// <returns>n</returns>
        int GetNIn2ToTheN(int val)
        {
            return ConvertToBinaryRecursive(val).Length - 1;
        }

        /// <summary>
        /// convert a decimal value into binary. (integers only)
        /// </summary>
        /// <param name="val">the value</param>
        /// <returns>the binary version of it.</returns>
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

    /// <summary>
    /// Cache entry class
    /// </summary>
    class CacheEntry
    {
        public bool isValid;
        public uint tag;
        public uint LRU;
        public uint set;

        /// <summary>
        /// Cache entry constructor
        /// </summary>
        /// <param name="isValid">Is the entry valid or a placeholder</param>
        /// <param name="tag">tag</param>
        /// <param name="LRU">when was this last accessed</param>
        /// <param name="set">set this is a a part of</param>
        public CacheEntry(bool isValid, uint tag, uint LRU, uint set)
        {
            this.isValid = isValid;
            this.tag = tag;
            this.LRU = LRU;
            this.set = set;
        }

        /// <summary>
        /// Constructor to create an empty, invalid cache entry
        /// </summary>
        public CacheEntry()
        {
            this.isValid = false;
        }
    }
}
