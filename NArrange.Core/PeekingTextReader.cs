using System;
using System.Collections.Generic;
using System.IO;

namespace NArrange.Core
{
    /// <summary>
    /// The peeking text editor allows infinite peeking compared to the normal text reader which only allows one character at a time.
    /// </summary>
    public class PeekingTextReader : TextReader
    {
        #region Fields

        private readonly List<int> _peeks;
        private readonly TextReader _textReader;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance of the peeking text reader using the provided text reader as the underlying source.
        /// </summary>
        /// <param name="textReader"></param>
        public PeekingTextReader(TextReader textReader)
        {
            _textReader = textReader;
            _peeks = new List<int>();
        }

        #endregion Constructors

        #region Methods

        /// <see cref="TextReader.Close" />
        public override void Close()
        {
            _textReader.Close();
        }

        /// <summary>
        /// When called will peek the next character that was not yet read.
        /// Same as <see cref="TextReader"/>
        /// </summary>
        /// <returns></returns>
        public override int Peek()
        {
            // still return the next "not yet read" character
            // as we don't want to break the parser as it sometimes doesn't cache the peek result
            // but instead queries the reader multiple times.
            if (_peeks.Count > 0)
                return _peeks[0];
            return _textReader.Peek();
        }

        /// <summary>
        /// Peek method that will advance one character on each peek without changing the readers state (it does so by reading to an internal buffer that is later fed back to <see cref="Read"/>).
        /// You can read all the way to the end of the file without affecting <see cref="Read"/>.
        /// The next time read is called the internal buffer will be dequeue until it is empty.
        /// </summary>
        /// <returns></returns>
        public int PeekAhead()
        {
            var c = _textReader.Read();
            _peeks.Add(c);
            return c;
        }

        /// <see cref="TextReader.ReadLine" />
        public override int Read()
        {
            if (_peeks.Count > 0)
            {
                var next = _peeks[0];
                _peeks.RemoveAt(0);
                return next;
            }
            return _textReader.Read();
        }

        /// <see cref="TextReader.Read" />
        public override int Read(char[] buffer, int index, int count)
        {
            int read = 0;
            if (_peeks.Count > 0)
            {
                var toCopy = Math.Min(count, _peeks.Count);
                Array.Copy(_peeks.ToArray(), 0, buffer, index, toCopy);
                index += toCopy;
                count -= toCopy;
                read += toCopy;
            }
            if (count == 0)
                return read;
            return read + _textReader.Read(buffer, index, count);
        }

        /// <see cref="TextReader.ReadBlock" />
        public override int ReadBlock(char[] buffer, int index, int count)
        {
            return Read(buffer, index, count);
        }

        /// <see cref="TextReader.ReadLine" />
        public override string ReadLine()
        {
            return _textReader.ReadLine();
        }

        /// <see cref="TextReader.ReadToEnd" />
        public override string ReadToEnd()
        {
            return _textReader.ReadToEnd();
        }

        /// <see cref="IDisposable.Dispose" />
        protected override void Dispose(bool disposing)
        {
            _textReader.Dispose();
        }

        #endregion Methods
    }
}