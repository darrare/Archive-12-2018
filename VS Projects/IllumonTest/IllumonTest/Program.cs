using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SolutionIter : IEnumerable<int>
{
    Stream stream;
    public SolutionIter(Stream stream)
    {
        this.stream = stream;
    }

    public IEnumerator<int> GetEnumerator()
    {
        return new StreamReaderEnumerator(stream);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class StreamReaderEnumerator : IEnumerator<int>
{
    StreamReader stream;
    public StreamReaderEnumerator(Stream stream)
    {
        this.stream = new StreamReader(stream);
    }

    int? _current;

    int IEnumerator<int>.Current
    {
        get
        {
            if (stream == null || _current == null)
            {
                throw new InvalidOperationException();
            }
            return _current.Value;
        }
    }

    object IEnumerator.Current
    {
        get
        {
            return Current1;
        }
    }

    object Current1
    {
        get
        {
            return _current;
        }
    }

    private bool disposedValue = false;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                //dispose of managed resources.
            }
            _current = null;
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
        }
        disposedValue = true;
    }

    public bool MoveNext()
    {
        int temp;
        while (!stream.EndOfStream)
        {
            string curString = stream.ReadLine();
            if (int.TryParse(curString, out temp))
            {
                //Problem: if a line has "X0054", or any characters before the int for the matter, it will include "54" in our list.
                //Problem above solved by taking our string and cutting it down to the first delimiter.
                int index = curString.IndexOfAny(new char[] { '-', '0' });
                if (index > 0)
                {
                    curString = curString.Substring(index, curString.Length - index);
                }
                //Our range is -1m to 1m. ints with leading 0's arent allowed, but the number 0 IS allowed
                //This is ugly, but it does the job.
                if ((temp >= -1000000000 && temp <= 1000000000) &&
                    (temp != 0 ? (curString[0] == '-' ? curString[1] != '0' : curString[0] != '0') : true))
                {
                    _current = temp;
                    return true;
                }
            }
        }
        _current = null;
        return false;

    }

    public void Reset()
    {
        stream.DiscardBufferedData();
        stream.BaseStream.Seek(0, SeekOrigin.Begin);
        _current = null;
    }

    ~StreamReaderEnumerator()
    {
        Dispose(false);
    }
}

/**
 * Example usage:
 *
 * IEnumerable<int> it = new SolutionIter(stream);
 * int[] arr = it.ToArray();
 */
