using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oggy.Repository;
using Oggy.Repository.Entities;
using System.Data.SqlClient;

namespace AuxiliaryConsoleApplication
{    
    class Program
    {
        static void Main2(string[] args)
        {
            //await myFunction();
        }

        static /*async*/ void myFunction()
        {

        }

        static void Main(string[] args)
        {
            // Open the SQL connection
            SqlRepository repository;
            try
            {
                Console.WriteLine("Opening the SQL connection...");
                repository = new SqlRepository();
                Console.WriteLine("Connection open.");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Cannot create an instance of SqlRepository");
                Console.WriteLine("" + exc);
                return;
            }

            var connection = repository.connection;

            //insert("RU", "Да и нет", @"..\..\Russian.txt", repository.connection);
            insert_file("HR", "Vremena", @"..\..\Dodju vremena.txt", repository.connection);
            //command("ALTER TABLE [dbo].[Languages] ALTER COLUMN [Name] NVARCHAR (30) NOT NULL", connection);
        }

        static void insert_file(string lang, string title, string fileName, SqlConnection connection)
        {
            System.IO.StreamReader myFile = new System.IO.StreamReader(fileName);
            string text = myFile.ReadToEnd();
            text = text.Replace("'", "''");

            myFile.Close();

            insert(lang, title, text, connection);
        }

        static void insert(string lang, string title, string text, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(
                "INSERT INTO TextSamples (LangId, Title, Text) VALUES ('" + lang + "', N'" + title + "', N'" + text + "')", connection);
            Console.WriteLine("Text:");
            Console.WriteLine(text);

            Console.WriteLine("Command Text:");
            Console.WriteLine(command.CommandText);

            command.ExecuteNonQuery();
        }

        static void command(string cmmd, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(cmmd, connection);
            
            command.ExecuteNonQuery();
            Console.WriteLine("Command Executed!");
        }
    }
}
