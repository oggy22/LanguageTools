using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Oggy.Repository.Entities;

namespace Oggy.Repository
{
	/// <summary>
	/// Class that talks to database.
	/// CRUD stands for Create, Retreive, Update and Delete
	/// </summary>
	public class SqlRepository : ARepository
	{
		#region Constants
		public const string database = @"C:\D\SQL DataBases\Dictionary.mdf";

		//public const string d2 = "Data Source=.\SQLEXPRESS;AttachDbFilename=&quot;C:\D\SQL DataBases\Dictionary.mdf&quot;;Integrated Security=True;Connect Timeout=30;User Instance=True"

		public const string CONNECTION_STRING = "Data Source=mssql3.orion.rs;Initial Catalog=translit_OggyDatabase;UID=translit_Oggy;Password=Oggy1981;Connect Timeout=30;MultipleActiveResultSets=true";
		#endregion

		#region Variables
		internal SqlConnection connection;
		#endregion

		#region Constructors and Refresh
		public SqlRepository()
			: this(CONNECTION_STRING)
		{
		}

		public SqlRepository(string connectionString)
		{
			connection = new SqlConnection(connectionString);
			connection.Open();
			SetUp();
		}

		private void Refresh()
		{
			if (connection.State != ConnectionState.Open)
			{
				connection.Close();
				connection.Open();
			}
		}
		#endregion

		#region CRUDL for Languages
		public override bool CreateLanguage(Language language)
		{
			SqlCommand cmd = new SqlCommand("LanguageCreate", connection);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@Lang", SqlDbType.VarChar).Value = language.Code;
			cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = language.Name;
			cmd.Parameters.Add("@Alphabet", SqlDbType.NVarChar).Value = language.Alphabet;
			return cmd.ExecuteNonQuery() == 1;
		}

		public override Language RetreiveLanguage(string code)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand("SELECT * FROM Languages WHERE LangId='" + code + "'", connection);
			SqlDataReader reader = cmd.ExecuteReader();
			if (!reader.Read())
				throw new ArgumentException("The language " + code + " does not exist.", "code");
			Language language = new Language()
			{
				Code = reader["LangId"] as string,
				Name = reader["Name"] as string,
				Alphabet = reader["Alphabet"] as string,
				Accents = reader["Stress"] as string,
				Hint = reader["Hint"] as string
			};
			reader.Close();

			// Load word types
			cmd = new SqlCommand("SELECT WordType FROM WordTypes WHERE LangId='" + code + "'", connection);
			reader = cmd.ExecuteReader();
			language.WordTypes = new HashSet<string>();
			while (reader.Read())
			{
				language.WordTypes.Add(reader.GetString(0));
			}
			reader.Close();
			return language;
		}

		public override void UpdateLanguage(Language language)
		{
			Refresh();
			
			// Update name and alphabet
			SqlCommand cmd = new SqlCommand("UPDATE Languages"
				+ " SET"
				+ " Name=N'" + language.Name + "',"
				+ " Alphabet=N'" + language.Alphabet + "'"
				+ " WHERE LangId='" + language.Code + "'",
				connection);
			cmd.ExecuteNonQuery();
		}

		public override bool DeleteLanguage(string code)
		{
			return false;
		}

		public override IEnumerable<Language> ListLanguages(bool enabledOnly = false)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand(
				 "SELECT * FROM Languages" + (enabledOnly ? " WHERE Enabled='TRUE'" : string.Empty)
				 , connection);
			SqlDataReader reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				yield return new Language()
				{
					Code = reader["LangId"] as string,
					Name = reader["Name"] as string,
					Alphabet = reader["Alphabet"] as string,
					Accents = reader["Stress"] as string,
					Hint = reader["Hint"] as string
				};
			}
			reader.Close();
		}
		#endregion

		#region CRUD Transliteration Rules
		public override List<TransliterationRule> ListRules(bool enabledOnly = true)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand("SELECT * FROM TransPhonetic WHERE LangId1='" +
				 srcLanguage.Code + "' AND LangId2='" + dstLanguage.Code + "'" +
				 (enabledOnly ? " AND Disabled='FALSE'" : string.Empty),
				 connection);
			SqlDataReader reader = cmd.ExecuteReader();

			List<TransliterationRule> list = new List<TransliterationRule>();
			while (reader.Read())
			{
				TransliterationRule rule = new TransliterationRule()
				{
					Source = reader.GetString(2),
					Destination = reader.GetString(3),
					Examples = reader.GetString(4),
					CounterExamples = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
					Comment = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
					Disabled = reader.GetBoolean(7)
				};
				list.Add(rule);
			}
			reader.Close();
			return list;
		}

		public override void CreateRule(TransliterationRule rule)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand(
				 "INSERT INTO TransPhonetic " +
				 "(LangId1, LangId2, Source, Destination, Examples, CounterExamples, Comment, Disabled) " +
				 string.Format("VALUES ('{0}', '{1}', N'{2}', N'{3}', N'{4}', N'{5}', '{6}', '{7}')",
				 this.srcLanguage.Code, this.dstLanguage.Code,
				 rule.Source, rule.Destination, rule.Examples, rule.CounterExamples, rule.Comment, rule.Disabled),
				 connection);
			if (cmd.ExecuteNonQuery() != 1)
				throw new Exception("SQL INSERT did not affect 1 column");
		}

		public override void UpdateRule(TransliterationRule rule, string source)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand(
				 string.Format("UPDATE TransPhonetic SET " +
				 "Source=N'{0}', Destination=N'{1}', Examples=N'{2}', CounterExamples=N'{3}', Comment=N'{4}', Disabled=N'{5}' " +
				 "WHERE LangId1=N'{6}' AND LangId2=N'{7}' AND Source=N'{8}'",
				 rule.Source, rule.Destination, rule.Examples, rule.CounterExamples, rule.Comment, rule.Disabled,
				 this.srcLanguage.Code, this.dstLanguage.Code, source), connection);

			if (cmd.ExecuteNonQuery() != 1)
				throw new Exception("SQL UPDATE did not affect 1 row");
		}

		public override void DeleteRule(string source)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand(
				 string.Format("DELETE FROM TransPhonetic WHERE LangId1='{0}' AND LangId2='{1}' AND Source='{2}'",
				 srcLanguage.Code, dstLanguage.Code, source),
				 connection);
			if (cmd.ExecuteNonQuery() != 1)
				throw new Exception("SQL DELETE did not affect 1 row");
		}
		#endregion

		#region CRUD Transliteration Examples

		public override List<TransliterationExample> ListExamples(bool enabledOnly = false)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand("SELECT * FROM TransExamples WHERE LangId1='"
				 + srcLanguage.Code + "' AND LangId2='" + dstLanguage.Code + "'" +
				 (enabledOnly ? " AND Disabled='FALSE'" : string.Empty),
				 connection);
			SqlDataReader reader = cmd.ExecuteReader();

			List<TransliterationExample> list = new List<TransliterationExample>();
			while (reader.Read())
			{
				TransliterationExample rule = new TransliterationExample()
				{
					Source = reader.GetString(2),
					Destination = reader.GetString(3),
					Disabled = reader.GetBoolean(4)
				};
				list.Add(rule);
			}
			reader.Close();
			return list;
		}

		public override void CreateExample(TransliterationExample example)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand(
				 "INSERT INTO TransExamples (LangId1, LangId2, Word, Transliteration, Disabled) " +
				 string.Format("VALUES ('{0}', '{1}', N'{2}', N'{3}', '{4}')",
				 this.srcLanguage.Code, this.dstLanguage.Code,
				 example.Source, example.Destination, example.Disabled),
				 connection);
			if (cmd.ExecuteNonQuery() != 1)
				throw new Exception("SQL INSERT did not affect 1 column");
		}

		public override void UpdateExample(TransliterationExample example, string word)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand(
				 string.Format("UPDATE TransExamples SET Word=N'{0}', Transliteration=N'{1}', Disabled='{2}' " +
				 "WHERE LangId1='{3}' AND LangId2='{4}' AND Word=N'{5}'",
				 example.Source, example.Destination, example.Disabled,
				 this.srcLanguage.Code, this.dstLanguage.Code, word),
				 connection);

			if (cmd.ExecuteNonQuery() != 1)
				throw new Exception("SQL UPDATE did not affect 1 row");
		}

		public override void DeleteExample(string word)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand(
				 string.Format("DELETE FROM TransExamples WHERE LangId1='{0}' AND LangId2='{1}' AND Word='{2}'",
				 srcLanguage.Code, dstLanguage.Code, word),
				 connection);
			if (cmd.ExecuteNonQuery() != 1)
				throw new Exception("SQL DELETE did not affect 1 row");
		}

		#endregion

		#region Jokers
		public override Dictionary<char, string> GetJokers()
		{
			Refresh();
			Dictionary<char, string> jokers = new Dictionary<char, string>();

			SqlCommand cmd = new SqlCommand("SELECT JokerSign, JokerExchange FROM Jokers WHERE LangId='"
				 + srcLanguage.Code + "'", connection);
			SqlDataReader reader = cmd.ExecuteReader();

			char[] c = new char[1];
			while (reader.Read())
			{
				reader.GetChars(0, 0, c, 0, 1);
				jokers.Add(c[0], reader[1] as string);
			}

			reader.Close();
			return jokers;
		}

		public override void AddJoker(char joker, string substitution)
		{
			throw new NotImplementedException();
		}

		public override void UpdateJoker(char joker, string substitution)
		{
			throw new NotImplementedException();
		}

		public override void DeleteJoker(char joker)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region TextSamples
		public override List<TextSample> ListTextSamples(string langCode)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand("SELECT Title, Text FROM TextSamples WHERE LangId='" +
				 langCode + "'", connection);
			SqlDataReader reader = cmd.ExecuteReader();

			List<TextSample> list = new List<TextSample>();
			while (reader.Read())
			{
				TextSample sample = new TextSample()
				{
					Title = reader.GetString(0),
					Text = reader.GetString(1)
				};
				list.Add(sample);
			}
			reader.Close();
			return list;
		}
		#endregion

		public override Dictionary<char, double> LoadLetterDistribution(string langCode)
		{
			Refresh();
			SqlCommand cmd = new SqlCommand("SELECT Letter, RelativeFrequency FROM Letters WHERE LangId='" +
				 langCode + "'", connection);
			SqlDataReader reader = cmd.ExecuteReader();

			Dictionary<char, double> distribtuion = new Dictionary<char, double>();

			while (reader.Read())
			{
				distribtuion.Add(reader.GetString(0)[0], reader.GetDouble(1));
			}
			reader.Close();
			return distribtuion;
		}

		public override void CreateWord(Word word)
		{
			Refresh();
			string langCode = this.srcLanguage.Code;
			DateTime now = DateTime.Now;
			SqlCommand cmd = new SqlCommand(
				 "INSERT INTO Words (LangId, Word, Trans, Status, WordType, dateCreated, dateModified) " +
				 "VALUES ('"
				 + langCode + "', N'"
				 + word.Name + "', N'"
				 + word.Translation + "', "
				 + word.status + ", N'"
				 + word.Type + "', '"
				 + now.ToString("yyyy-MM-dd HH:mm:ss") + "', '"
				 + now.ToString("yyyy-MM-dd HH:mm:ss") + "')",
				 connection);
			cmd.ExecuteNonQuery();
		}

		public override void DeleteWord(string word)
		{
			Refresh();
			string langCode = this.srcLanguage.Code;
			SqlCommand cmd = new SqlCommand(
				 "DELETE FROM Words " +
				 "WHERE LangId='" + langCode + "' AND Word=N'" + word + "'",
				 connection);
			cmd.ExecuteNonQuery();
		}

		public override void UpdateWord(Word word)
		{
			Refresh();
			string langCode = this.srcLanguage.Code;
			SqlCommand cmd = new SqlCommand(
				 "UPDATE Words " +
				 "SET Status=" + word.status + ", Trans=N'" + word.Translation + "', dateModified='" + word.Modified + "' " +
				 "WHERE LangId='" + langCode + "' AND Word=N'" + word.Name + "'",
				 connection);
			cmd.ExecuteNonQuery();
		}

		public override IEnumerable<Word> ListWords()
		{
			Refresh();
			string langCode = this.srcLanguage.Code;
			SqlCommand cmd = new SqlCommand("SELECT Word, Trans, Status, WordType, dateCreated, dateModified FROM Words WHERE LangId='" +
				 langCode + "'", connection);
			SqlDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				yield return new Word()
				{
					Name = reader.GetString(0),
					Translation = reader.GetString(1),
					status = reader.GetInt16(2),
					Type = reader.GetString(3),
					Created = (DateTime)reader.GetSqlDateTime(4),
					Modified = (DateTime)reader.GetSqlDateTime(5)
				};
			}
			reader.Close();
		}
	}
}