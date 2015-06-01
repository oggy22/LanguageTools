using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dictionary
{
	public partial class KeyboardForm : Form
	{
		readonly Size buttonSize = new Size(30, 30);
		readonly char accent = (char)(0x0301);
		bool callPrevent = false;
		string alphabet;
		System.Windows.Controls.TextBox txtWord;

		public KeyboardForm(System.Windows.Controls.TextBox txtWord, string langId, string alhpabet)
		{
			this.txtWord = txtWord;
			SetLanguage(langId, alhpabet);
			this.Focus();
		}

		private void Alphabet_Resize(object sender, EventArgs e)
		{
			var buttons = Controls;

			int x, y, i;
			if (callPrevent)
			{
				callPrevent = false;
				return;
			}
			for (i = 0, x = 0, y = 0; i < buttons.Count; i++)
			{
				if (i == 0)
					buttons[i].Location = new Point(0, 0);
				else
				{
					x += buttons[i - 1].Width;
					if (x + buttons[i].Width > this.Width - buttonSize.Width)
					{
						x = 0;
						y += buttons[i].Height;
					}
					buttons[i].Location = new Point(x, y);
				}
			}
			callPrevent = true;
			int last = buttons.Count - 1;
			this.Height = buttons[last].Location.Y + buttons[last].Height + buttonSize.Height + 10;
		}

		private int GetWidth(string text, Font font)
		{
			SizeF stringSize;

			// create a graphics object for this form
			using (Graphics gfx = this.CreateGraphics())
			{
				// Get the size given the string and the font
				stringSize = gfx.MeasureString(text, font);
			}

			return (int)Math.Round(stringSize.Width, 0) + 4;
		}

		private Button CreateButton(string text, System.EventHandler handler)
		{
			Button button;
			button = new Button();
			string englishAlhpabet = "abcdefghijklmnopqrstuvwxyz";
			button.Size = buttonSize;
			button.Font = new Font("Ariel", 16.0f, (englishAlhpabet.Contains(text) ? FontStyle.Regular : FontStyle.Bold));
			button.Text = text;
			button.UseVisualStyleBackColor = true;
			button.Width = GetWidth(text, button.Font);
			button.Click += handler;
			return button;
		}

		public void SetLanguage(string langId, string alphabet)
		{
			this.alphabet = alphabet;
			this.Controls.Clear();
			for (int i = 0; i < alphabet.Length; i++)
			{
				Button button = CreateButton("" + alphabet[i], new System.EventHandler(this.button_Click));
				this.Controls.Add(button);
			}
			Controls.Add(CreateButton("	", new System.EventHandler(this.buttonSpace_Click)));
			Controls.Add(CreateButton("" + accent, new System.EventHandler(this.button_Click)));
			Controls.Add(CreateButton("<", new System.EventHandler(this.buttonBackSpace_Click)));
			Controls.Add(CreateButton("CapsLock", new System.EventHandler(this.buttonCAPSLOCK_Click)));
			InitializeComponent();
			Alphabet_Resize(null, null);
			this.Text = "Alphabet " + langId;
		}

		private void AddText(char c)
		{
			txtWord.SelectedText = c.ToString();
			txtWord.SelectionLength = 0;
			txtWord.SelectionStart += 1;
			txtWord.Focus();
		}


		#region Button Handlers
		private void button_Click(object sender, EventArgs e)
		{
			char c = ((Button)(sender)).Text[0];
			AddText(c);
		}

		private void buttonSpace_Click(object sender, EventArgs e)
		{
			AddText(' ');
		}

		private void buttonBackSpace_Click(object sender, EventArgs e)
		{
			if (txtWord.SelectionLength == 0 && txtWord.SelectionStart > 0)
			{
				int selectionLenght = 1;
				int selectionStart = txtWord.SelectionStart - 1;
				
				//Move towards left in the string while there is the accent
				while (selectionStart > 0 && txtWord.Text[selectionStart] == accent)
				{
					selectionStart--;
					selectionLenght++;
				}
				txtWord.SelectionStart = selectionStart;
				txtWord.SelectionLength = selectionLenght;
			}
			txtWord.SelectedText = "";
			txtWord.Focus();
		}

		private void buttonCAPSLOCK_Click(object sender, EventArgs e)
		{
			bool toUpperCase = char.IsLower((Controls[0] as Button).Text[0]);
			foreach (var control in Controls)
			{
				Button button = control as Button;
				string text = button.Text;
				if (text.Length > 1)
					break;
				button.Text = (toUpperCase ? text.ToUpper() : text.ToLower());
			}
		}

		#endregion
	}
}