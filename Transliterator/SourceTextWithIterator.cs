using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oggy
{
    public class SourceTextWithIterator
    {
        #region Fields
        private string text;
        private string textLowercase;

        #endregion

        #region Properties
        /// <summary>
        /// Keeps track if the <c>this.position-1</c> is in a word.
        /// </summary>
        public bool InWord;

        public int Position
        {
            get;
            private set;
        }

        public char this[int position]
        {
            get
            {
                return text[position];
            }
        }

        public char CurrentCharacter
        {
            get
            {
                return text[Position];
            }
        }

        public int LeftCharacters
        {
            get
            {
                return text.Length - Position;
            }
        }
        #endregion

        #region Constructors
        public SourceTextWithIterator(string text)
        {
            this.text = text;
            this.textLowercase = text.ToLower();
            this.Position = 0;
            this.InWord = false;
        }
        #endregion

        #region
        public void IncrementPosition()
        {
            this.Position++;
        }
        #endregion
    }
}