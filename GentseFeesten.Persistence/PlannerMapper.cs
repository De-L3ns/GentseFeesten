using GentseFeesten.Domain.Model;
using GentseFeesten.Domain.Repository;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GentseFeesten.Persistence
{
    public class PlannerMapper : IPlannerRepository
    {
        // Database related private fields
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=GentseFeesten;Integrated Security=True; Encrypt=False";
        private readonly SqlConnection _sqlConnection;

        // Data related private fields
        private List<Evenement> _eventsOnPlanner = new List<Evenement>();

        public PlannerMapper()
        {
            _sqlConnection = new SqlConnection(ConnectionString);
        }

        public List<Evenement> GetAllEventsOnPlanner()
        {
            GetAllEventsOnPlannerFromDatabase();
            return _eventsOnPlanner;
        }

        public int GetCurrentTotalPrice()
        {
            int totalPrice = 0;
            _eventsOnPlanner.ForEach(e => totalPrice += (int)e.Price);

            return totalPrice;

        }

        public void AddEventToPlanner(Evenement evenement)
        {
            if (!_eventsOnPlanner.Contains(evenement))
            {
                AddEventToPlannerDatabase(evenement);
                _eventsOnPlanner.Add((PlannerEvenement)evenement);
            }
            else
            {
                throw new Exception("The Event is allready on the planner");
            }
        }

        public void RemoveEventFromPlanner(Evenement evenement)
        {
            RemoveEventFromPlannerDatabase(evenement);
            _eventsOnPlanner.Remove((PlannerEvenement)evenement);

        }

        private void AddEventToPlannerDatabase(Evenement evenement)
        {
            try
            {
                _sqlConnection.Open();
                SqlCommand command = new SqlCommand(
                    "INSERT INTO Planner (Id, [End], Start, Description, Name, Price) " +
                    "VALUES (@Id, @End, @Start, @Description, @Name, @Price);", _sqlConnection);

                command.Parameters.Add("@Id", SqlDbType.VarChar);
                command.Parameters["@Id"].Value = evenement.Id;

                command.Parameters.Add("@End", SqlDbType.DateTime);
                command.Parameters["@End"].Value = evenement.EndDateToDatabase;

                command.Parameters.Add("@Start", SqlDbType.DateTime);
                command.Parameters["@Start"].Value = evenement.StartDateToDatabase;

                command.Parameters.Add("@Description", SqlDbType.Text);
                command.Parameters["@Description"].Value = evenement.Description == null ? "" : evenement.Description;

                command.Parameters.Add("@Name", SqlDbType.VarChar);
                command.Parameters["@Name"].Value = evenement.Name;

                command.Parameters.Add("@Price", SqlDbType.Int);
                command.Parameters["@price"].Value = evenement.Price;

                command.ExecuteScalar();

            }

            finally
            {
                _sqlConnection.Close();
            }
        }

        private void RemoveEventFromPlannerDatabase(Evenement evenement)
        {
            try
            {
                _sqlConnection.Open();
                SqlCommand command = new SqlCommand($"DELETE FROM Planner WHERE id = @Id;", _sqlConnection);

                command.Parameters.Add("@Id", SqlDbType.VarChar);
                command.Parameters["@Id"].Value = evenement.Id;

                command.ExecuteNonQuery();


            }
            finally
            {
                _sqlConnection.Close();
            }
        }
        private void GetAllEventsOnPlannerFromDatabase()
        {
            try
            {
                _sqlConnection.Open();

                SqlCommand cmd = new("SELECT * FROM Planner", _sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                List<Evenement> eventsFromDatabase = new List<Evenement>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string id = (string)reader["Id"];
                        DateTime? end = (reader["End"] == DBNull.Value) ? null : (DateTime?)reader["End"];
                        DateTime? start = (reader["Start"] == DBNull.Value) ? null : (DateTime?)reader["Start"];
                        string? description = (reader["Description"] == DBNull.Value) ? null : (string?)reader["Description"];
                        string? name = (string)reader["Name"];
                        int? price = (reader["Price"] == DBNull.Value) ? null : (int?)reader["Price"];

                        eventsFromDatabase.Add(new PlannerEvenement(id, name, start, end, description, price));
                    }
                }

                _eventsOnPlanner = eventsFromDatabase;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

    }
}
