// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");



using ConsoleApp5.Models;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text.RegularExpressions;
using Tesseract;
using static System.Net.Mime.MediaTypeNames;

//Bitmap img = new Bitmap("testing.png");


//Bitmap bmp = (Bitmap)Bitmap.FromFile("testing.png");

Pix pics = Pix.LoadFromFile("C:\\Users\\jrrojo\\Downloads\\sample_img13.jpg");
TesseractEngine engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
Page page = engine.Process(pics, PageSegMode.Auto);

string result = page.GetText();


//result = result.Replace("\n", "</br>");

List<TerminalReceipt> rcpList = new List<TerminalReceipt>();

using (StringReader reader = new StringReader(result))
{
    string line = string.Empty;
    string cardType = string.Empty;
    string merchantID = string.Empty;
    int cardNo = 0;
    DateTime tranDateTime = DateTime.MinValue;
    decimal amt = 0;


    while((line = reader.ReadLine()) != null)
    {
        if (line.Contains("CARD TYPE"))
        {
            cardType = line.Substring(10);
        }
        else if (line.Contains("CARD NO") || line.Contains("CARD NQ"))
        {
            string extractedNumbers = new string(line.Where(char.IsDigit).ToArray());

            if (int.TryParse(extractedNumbers, out int number))
            {
                cardNo = number;
            }
            
        }
        else if (line.Contains("DATE/TIME"))
        {
            line = line.Replace("DATE/TIME", "");
            tranDateTime = DateTime.Parse(line);
        }
        else if (line.Contains("AMOUNT"))
        {
            MatchCollection matches = Regex.Matches(line, @"\d+\.\d+");

            foreach (Match match in matches)
            {
                if (decimal.TryParse(match.Value, out decimal number))
                {
                    amt = number;
                }
            }

        }
        else if(line.Contains("MERCHANT ID"))
        {
            merchantID = line.Substring(12).Replace(" ", "");
        }

    }

    TerminalReceipt rcp = new TerminalReceipt();
    rcp.MerchantID = merchantID;
    rcp.CardNumber = cardNo;
    rcp.TranDateTime = tranDateTime;
    rcp.Amount = amt;
    rcp.CardType = cardType;
    rcp.CardNumber = cardNo;
    rcpList.Add(rcp);

    SaveList(rcpList);

    Console.ReadLine();


 }


static void ConnectDB()
{
    string connectionString = @"Provider=SQLOLEDB;Server=localhost\SQLEXPRESS;Database=Abby;Trusted_Connection=yes;";

    using (OleDbConnection connection = new OleDbConnection(connectionString))
    {
        try
        {
            connection.Open();
            Console.WriteLine("Connection opened successfully!");

            // Perform database operations here

            connection.Close();
            Console.WriteLine("Connection closed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}


static void SaveList(List<TerminalReceipt> list)
{
    string connectionString = @"Provider=SQLOLEDB;Server=localhost\SQLEXPRESS;Database=CMS;Trusted_Connection=yes;";

    using (OleDbConnection connection = new OleDbConnection(connectionString))
    {
        connection.Open();

        using (OleDbCommand command = new OleDbCommand())
        {
            command.Connection = connection;
            command.CommandText = "INSERT INTO dbo.TransactionLog (MerchantID, CardType, CardNumber, TranDateTime, Amount) " +
                                  "VALUES (@MerchantID, @CardType, @CardNumber, @TranDateTime, @Amount)";

            foreach (var item in list)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("MerchantID", item.MerchantID);
                command.Parameters.AddWithValue("CardType", item.CardType);
                command.Parameters.AddWithValue("CardNumber", item.CardNumber);
                command.Parameters.AddWithValue("TranDateTime", item.TranDateTime);
                command.Parameters.AddWithValue("Amount", item.Amount);
                command.ExecuteNonQuery();
            }
        }

        connection.Close();
    }

}

static void Save()
{
    string connectionString = @"Provider=SQLOLEDB;Server=localhost\SQLEXPRESS;Database=Abby;Trusted_Connection=yes;";

    using (OleDbConnection connection = new OleDbConnection(connectionString))
    {
        try
        {
            connection.Open();
            Console.WriteLine("Connection opened successfully!");

            string insertQuery = "INSERT INTO [dbo].[Category] (Name, DisplayOrder) VALUES ('Hardware', 4)";

            using (OleDbCommand command = new OleDbCommand(insertQuery, connection))
            {
                // Replace parameter names with actual parameter names in your query
                //command.Parameters.AddWithValue("@Value1", "Hardware");
                //command.Parameters.AddWithValue("@Value2", 4);

                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}






