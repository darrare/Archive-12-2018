using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace SoritingAlgorithmTest
{
    public partial class Form1 : Form
    {
        enum SortType { Bubble, Selection, Insertion, Quick, Merge };
        int numElements;
        int randomSeed;
        Random rand = new Random();
        Random rand2 = new Random();
        int[] array;

        Dictionary<SortType, Data> data = new Dictionary<SortType, Data>();

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //should we seed rand?
            if (textBox2.Text != "" && Regex.IsMatch(textBox2.Text, "^[0-9]*$"))
            {
                try
                {
                    randomSeed = int.Parse(textBox2.Text);
                    rand = new Random(randomSeed);
                }
                catch(OverflowException ex)
                {
                    ErrorBox.Text = "ERROR: overflow exception on user input. Seed must be between -2b and 2b.";
                    return;
                }
            }
            else if (textBox2.Text != "")
            {
                ErrorBox.Text = "ERROR: Can't create array due to non numeric value in seed field. Leave empty for random seed.";
                return;
            }

            if (textBox2.Text == "")
            {
                randomSeed = rand2.Next(0, int.MaxValue);
                rand = new Random(randomSeed);
            }

            if (textBox1.Text != "" && Regex.IsMatch(textBox1.Text, "^[0-9]*$"))
            {
                numElements = int.Parse(textBox1.Text);
            }
            else
            {
                ErrorBox.Text = "ERROR: Can't create array due to non numeric value in input field.";
                return;
            }

            array = new int[numElements];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.Next(int.MinValue, int.MaxValue);
            }
            ErrorBox.Text = "Array of size " + numElements + " created and ready to use.";
        }


        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            if (array == null)
            {
                ErrorBox.Text = "ERROR: No array created yet.";
                return;
            }

            await RunTests();

            //TODO: Do something with results
            string path = Application.StartupPath + ("\\TestOn" + numElements + "Seed" + randomSeed + "_" + DateTime.Now.ToFileTime() + ".txt");
            File.Create(path).Dispose();
            using (StreamWriter sw = new StreamWriter(path))
            {
                //foreach (KeyValuePair<SortType, Data> d in data)
                //{
                //    sw.WriteLine(d.Key.ToString() + " Sort");
                //    sw.WriteLine("Duration: " + data[d.Key].timeSpan);
                //    sw.WriteLine("Comparisons: " + data[d.Key].comparisons);
                //    sw.WriteLine("Assignments: " + data[d.Key].assignments);
                //    sw.WriteLine();
                //}
            }
        }

        public async Task RunTests()
        {
            data.Clear();
            int[] copy = new int[array.Length];

            //Logic for running the sorts
            if (checkBox1.Checked)
            {
                ErrorBox.Text = "Bubble Sorting";
                array.CopyTo(copy, 0);
                data.Add(SortType.Bubble, await Task.Factory.StartNew(() => BubbleSort(copy, DateTime.Now)));

                ErrorBox.Text = "Verifying Sort";
                if (!VerifySort(copy))
                {
                    ErrorBox.Text = "ERROR: Bubble sort incorrect";
                    return;
                }
            }

            if (checkBox2.Checked)
            {
                ErrorBox.Text = "Selection Sorting";
                array.CopyTo(copy, 0);
                data.Add(SortType.Selection, await Task.Factory.StartNew(() => SelectionSort(copy, DateTime.Now)));

                ErrorBox.Text = "Verifying Sort";
                if (!VerifySort(copy))
                {
                    ErrorBox.Text = "ERROR: Selection sort incorrect";
                    return;
                }
            }

            if (checkBox3.Checked)
            {
                ErrorBox.Text = "Insertion Sorting";
                array.CopyTo(copy, 0);
                data.Add(SortType.Insertion, await Task.Factory.StartNew(() => InsertionSort(copy, DateTime.Now)));

                ErrorBox.Text = "Verifying Sort";
                if (!VerifySort(copy))
                {
                    ErrorBox.Text = "ERROR: Insertion sort incorrect";
                    return;
                }
            }

            if (checkBox4.Checked)
            {
                ErrorBox.Text = "Quick Sorting";
                array.CopyTo(copy, 0);
                data.Add(SortType.Quick, await Task.Factory.StartNew(() => QuickSort(copy, DateTime.Now)));

                ErrorBox.Text = "Verifying Sort";
                if (!VerifySort(copy))
                {
                    ErrorBox.Text = "ERROR: Quick sort incorrect";
                    return;
                }
            }

            if (checkBox5.Checked)
            {
                ErrorBox.Text = "Merge Sorting";
                array.CopyTo(copy, 0);
                data.Add(SortType.Merge, await Task.Factory.StartNew(() => MergeSort(copy, DateTime.Now)));

                ErrorBox.Text = "Verifying Sort";
                if (!VerifySort(copy))
                {
                    ErrorBox.Text = "ERROR: Merge sort incorrect";
                    return;
                }
            }

            ErrorBox.Text = "Finished sorting. Check the txt document next to the executable for results.";
        }

        bool VerifySort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] > array[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        Data BubbleSort(int[] array, DateTime start)
        {
            Data d = new Data();
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 1; i < array.Length; i++)
                {
                    if (array[i - 1] > array[i])
                    {
                        int temp = array[i];
                        array[i] = array[i - 1];
                        array[i - 1] = temp;
                        swapped = true;
                        d.assignments += 2;
                    }
                    d.comparisons++;
                }
            } while (swapped);

            d.timeSpan = DateTime.Now - start;
            return d;
        }

        Data SelectionSort(int[] array, DateTime start)
        {
            Data d = new Data();
            for (int i = 0; i < array.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (array[j] < array[min])
                    {
                        min = j;
                    }
                    d.comparisons++;
                }

                if (min != i)
                {
                    int temp = array[i];
                    array[i] = array[min];
                    array[min] = temp;
                    d.assignments += 2;
                }
            }

            d.timeSpan = DateTime.Now - start;
            return d;
        }

        Data InsertionSort(int[] array, DateTime start)
        {
            Data d = new Data();

            for (int i = 1; i < array.Length; i++)
            {
                int x = array[i];
                int j = i - 1;
                while (j >= 0 && array[j] > x)
                {
                    array[j + 1] = array[j];
                    j--;
                    d.assignments++;
                    d.comparisons++;
                }
                array[j + 1] = x;
                d.assignments++;
            }
            d.timeSpan = DateTime.Now - start;
            return d;
        }

        Data QuickSort(int[] array, DateTime start)
        {
            Data d = new Data();

            QuickSortFunction(array, 0, array.Length - 1, ref d);

            d.timeSpan = DateTime.Now - start;
            return d;
        }

        void QuickSortFunction(int[] array, int low, int high, ref Data d)
        {
            if (low < high)
            {
                int p = Partition(array, low, high, ref d);
                QuickSortFunction(array, low, p - 1, ref d);
                QuickSortFunction(array, p + 1, high, ref d);
            }
        }

        int temp;
        int Partition(int[] array, int low, int high, ref Data d)
        {
            int pivot = array[high];
            int i = low;
            for (int j = low; j < high; j++)
            {
                if (array[j] < pivot)
                {
                    if (i != j)
                    {
                        temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                        d.assignments += 2;
                    }
                    i++;
                }
                d.comparisons++;
            }
            temp = array[i];
            array[i] = array[high];
            array[high] = temp;
            d.assignments += 2;
            return i;
        }


        //Bottom Up strategy
        Data MergeSort(int[] array, DateTime start)
        {
            Data d = new Data();
            int[] b = new int[array.Length];
            BottomUpMergeSort(array, b, array.Length, d);

            d.timeSpan = DateTime.Now - start;
            return d;
        }

        void BottomUpMergeSort(int[] a, int[] b, int n, Data d)
        {
            for (int i = 1; i < n; i *= 2)
            {
                Parallel.For(0, n / (2 * i), j =>
                {
                    int x = j + 2 * i;
                    BottomUpMerge(a, x, (x + i < n) ? x + i : n, (j + 2 * i < n) ? j + 2 * i : n, b, d);
                });

                //for (int j = 0; j < n; j ++)
                //{
                //    int x = j + 2 * i;
                //    BottomUpMerge(a, x, (x + i < n) ? x + i : n, (j + 2 * i < n) ? j + 2 * i : n, b, d);
                //}

                //Parallel.For(0, n, j =>
                //{
                //    a[j] = b[j];
                //});
                for (int j = 0; j < n; j++)
                {
                    a[j] = b[j];
                }

                d.assignments += (uint)n;
            }
        }

        void BottomUpMerge(int[] a, int iLeft, int iRight, int iEnd, int[] b, Data d)
        {
            int i = iLeft, j = iRight;

            for (int k = iLeft; k < iEnd; k++)
            {
                //The below ifs look terrible due to the fact I needed to split it for proper comparison counting
                if (i < iRight)
                {
                    if (j >= iEnd || a[i] <= a[j])
                    {
                        b[k] = a[i];
                        i++;
                        d.comparisons++;
                        d.assignments++;
                    }
                    else
                    {
                        b[k] = a[j];
                        j++;
                        d.assignments++;
                    }
                }
                else
                {
                    b[k] = a[j];
                    j++;
                    d.assignments++;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox1.Text, "^[0-9]*$"))
            {
                ErrorBox.Text = "ERROR: Only numbers allowed for the array size";
            }
            else
            {
                ErrorBox.Text = "";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(textBox2.Text, "^[0-9]*$"))
            {
                ErrorBox.Text = "ERROR: Only numbers allowed for the random seed. Leave blank for a randomly generated seed.";
            }
            else
            {
                ErrorBox.Text = "";
            }
        }

    }
}
