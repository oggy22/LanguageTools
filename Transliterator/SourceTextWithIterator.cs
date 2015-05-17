using Oggy.Transliterator;
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
		public bool InWord { get; private set; }

		public int Position
		{
			get;
			private set;
		}

		public bool End
		{
			get
			{
				return Position == text.Length;
			}
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
		public WordCapitals IncrementPosition(int increment = 1)
		{
			string tail = text.Substring(Position, increment);
			InWord = char.IsLetter(CurrentCharacter);
			Position += increment;
			return GetStringWordCapitals(tail, End || char.IsLetter(CurrentCharacter));
		}

		static WordCapitals GetStringWordCapitals(string s, bool wordEnd)
		{
			if (/*wordEnd && */char.IsUpper(s[0]) && s.Substring(1).All(c => char.IsLower(c)))
				return WordCapitals.CapitalLetter;

			if (s.All(c => char.IsLower(c)))
				return WordCapitals.LowerCase;

			if (s.All(c => char.IsUpper(c)))
				return WordCapitals.UpperCase;

			return WordCapitals.None;
		}
		#endregion
	}
}