using GentseFeesten.Domain.Model;
using GentseFeesten.Domain.Repository;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System;

namespace GentseFeesten.Persistence
{
    public class EvenementenMapper : IEvenementenRepository

    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=GentseFeesten;Integrated Security=True; Encrypt=False";
        private SqlConnection _sqlConnection;

        public EvenementenMapper()
        {
            _sqlConnection = new SqlConnection(ConnectionString);
        }

        public List<Evenement> GetAllParentEvents()
        {
            List<Evenement> result = new();
            try
            {
                _sqlConnection.Open();

                SqlCommand cmd = new("SELECT * FROM Evenementen WHERE Parent IS NULL;", _sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string id = (string)reader["Id"];
                        DateTime? einde = (reader["Einde"] == DBNull.Value) ? null : (DateTime?)reader["Einde"];
                        DateTime? begin = (reader["Begin"] == DBNull.Value) ? null : (DateTime?)reader["Begin"];
                        string? childIds = (reader["Childs"] == DBNull.Value) ? null : (string?)reader["Childs"];
                        List<string>? childIdsToList = childIds?.Split(",").ToList();
                        string? beschrijving = (reader["Beschrijving"] == DBNull.Value) ? null : (string?)reader["Beschrijving"];
                        string? naam = (string)reader["Naam"];
                        int? prijs = (reader["Prijs"] == DBNull.Value) ? null : (int?)reader["Prijs"];


                        result.Add(new ParentEvenement(id, naam, begin, einde, beschrijving, prijs, childIdsToList));
                    }
                }

                return result;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public List<DateTime?> GetMissingStartData(Evenement evenement)
        {
            return GetMissingDateTimes(evenement, "Begin");
        }

        public List<DateTime?> GetMissingEndData(Evenement evenement)
        {
            return GetMissingDateTimes(evenement, "Einde");
        }

        private List<DateTime?> GetMissingDateTimes(Evenement evenement, string columnName)
        {
            List<DateTime?> result = new List<DateTime?>();
            try
            {
                _sqlConnection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM Evenementen WHERE Parent = '{evenement.Id}';", _sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DateTime? childDateTime = (reader[columnName] == DBNull.Value) ? null : (DateTime?)reader[columnName];
                        result.Add(childDateTime);
                    }
                }
                return result;
            }

            finally { _sqlConnection.Close(); }
        }
    

    public List<Evenement> GetChilds(Evenement evenement)
    {
        try
        {
            _sqlConnection.Open();
            List<Evenement> listOfChilds = new List<Evenement>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Evenementen WHERE Parent = @parentId", _sqlConnection);
            cmd.Parameters.Add(new SqlParameter("@parentId", evenement.Id));

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string id = (string)reader["Id"];
                    DateTime? einde = (reader["Einde"] == DBNull.Value) ? null : (DateTime?)reader["Einde"];
                    DateTime? begin = (reader["Begin"] == DBNull.Value) ? null : (DateTime?)reader["Begin"];
                    string? childIds = (reader["Childs"] == DBNull.Value) ? null : (string?)reader["Childs"];
                    List<string>? childIdsToList = childIds?.Split(",").ToList();
                    string? beschrijving = (reader["Beschrijving"] == DBNull.Value) ? null : (string?)reader["Beschrijving"];
                    string? naam = (string)reader["Naam"];
                    string? parent = (reader["Parent"] == DBNull.Value) ? null : (string?)reader["Parent"];
                    int? prijs = (reader["Prijs"] == DBNull.Value) ? null : (int?)reader["Prijs"];


                    listOfChilds.Add(new ChildEvenement(id, naam, begin, einde, beschrijving, prijs, childIdsToList, parent));

                }

            }

            return listOfChilds;

        }
        finally { _sqlConnection.Close(); }
    }
}
}