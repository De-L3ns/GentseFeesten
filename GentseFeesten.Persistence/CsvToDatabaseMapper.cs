using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Data;

namespace GentseFeesten.Persistence
{
    public class CsvToDatabaseMapper
    {

        public static void InsertCsvIntoDatabase()
        {
            string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=GentseFeesten;Integrated Security=True; Encrypt=False";
            SqlConnection _sqlConnection = new SqlConnection(ConnectionString);
            string _pathLocation = @"C:\Users\Laurens Viaene\Downloads\EvenementenGf.csv";
            _sqlConnection.Open();

            TextFieldParser parser = new TextFieldParser(new StreamReader(_pathLocation));
            parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(";");

            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                SqlCommand command = new SqlCommand(
                        "INSERT INTO EvenementenGF (Id, [End], Start, Childs, Parent, Description, Name, Price) " +
                        "VALUES (@Id, @End, @Start, @Childs, @Parent, @Description, @Name, @Price);", _sqlConnection);

                command.Parameters.Add("@Id", SqlDbType.VarChar);
                command.Parameters["@Id"].Value = fields[0];

                command.Parameters.Add("@End", SqlDbType.DateTime);
                command.Parameters["@End"].Value = string.IsNullOrEmpty(fields[1]) ? DBNull.Value : DateTime.Parse(fields[1].Substring(fields[1].IndexOf("+") +1 ));

                command.Parameters.Add("@Start", SqlDbType.DateTime);
                command.Parameters["@Start"].Value = string.IsNullOrEmpty(fields[2]) ? DBNull.Value : DateTime.Parse(fields[1].Substring(fields[2].IndexOf("+") + 1));

                command.Parameters.Add("@Childs", SqlDbType.VarChar);
                command.Parameters["@Childs"].Value = string.IsNullOrEmpty(fields[3]) ? DBNull.Value : fields[3];

                command.Parameters.Add("@Parent", SqlDbType.VarChar);
                command.Parameters["@Parent"].Value = string.IsNullOrEmpty(fields[4]) ? DBNull.Value : fields[4];

                command.Parameters.Add("@Description", SqlDbType.Text);
                command.Parameters["@Description"].Value = string.IsNullOrEmpty(fields[5]) ? DBNull.Value : fields[5];

                command.Parameters.Add("@Name", SqlDbType.VarChar);
                command.Parameters["@Name"].Value = string.IsNullOrEmpty(fields[6]) ? DBNull.Value : fields[6];

                command.Parameters.Add("@Price", SqlDbType.Int);
                command.Parameters["@price"].Value = string.IsNullOrEmpty(fields[7]) ? DBNull.Value : int.Parse(fields[7]);

                command.ExecuteScalar();
                
            }
            _sqlConnection.Close();
        }
    }
}
