using GentseFeesten.Domain.Model;
using GentseFeesten.Domain.Repository;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System;
using System.Data;

namespace GentseFeesten.Persistence
{
    public class EvenementenMapper : IEvenementenRepository

    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=GentseFeesten;Integrated Security=True; Encrypt=False";
        private SqlConnection _sqlConnection;
        private List<DateTime?> _result = new List<DateTime?>();
        private int _totalPrice = 0;

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
            _result.Clear();
            return GetMissingDateTimes(evenement, "Begin");

        }

        public List<DateTime?> GetMissingEndData(Evenement evenement)
        {
            _result.Clear();
            return GetMissingDateTimes(evenement, "Einde");
        }

        public int GetMissingPriceData(Evenement evenement)
        {
            _totalPrice = 0;
            return GetMissingPrice(evenement);
        }

        private List<DateTime?> GetMissingDateTimes(Evenement evenement, string columnName)
        {
            
            try
            {
                _sqlConnection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM Evenementen WHERE Parent = '{evenement.Id}';", _sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    foreach(DataRow dr in dt.Rows)
                    {
                        string id = (string)dr["Id"];
                        DateTime? childDateTime = (dr[columnName] == DBNull.Value) ? null : (DateTime?)dr[columnName];
                        if (childDateTime == null)
                        {
                            _sqlConnection.Close();
                            GetMissingDateTimes(GetEventById(id), columnName);
                            

                        } else
                        {
                            _result.Add(childDateTime);
                        }

                    }
                }
                return _result;
            }

            finally { 
                _sqlConnection.Close();
                
            }
        }
        
        private int GetMissingPrice(Evenement evenement)
        {
            
            try
            {
                _sqlConnection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM Evenementen WHERE Parent = '{evenement.Id}';", _sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string id = (string)dr["Id"];
                        int? prijs = (dr["Prijs"] == DBNull.Value) ? null : (int?)dr["Prijs"];
                        if (prijs == null)
                        {
                            _sqlConnection.Close();
                            GetMissingPrice(GetEventById(id));
                        } else
                        {
                            _totalPrice += (int)prijs;
                        }

                    }
                }

                return _totalPrice;
               
            }

            finally
            {
                _sqlConnection.Close();
            }
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

        public Evenement GetEventById(string eventId)
        {
            try
            {
                _sqlConnection.Open();

                SqlCommand cmd = new("SELECT * FROM Evenementen WHERE Id = @Id;", _sqlConnection);
                cmd.Parameters.Add(new SqlParameter("@Id", eventId));
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


                        return new ParentEvenement(id, naam, begin, einde, beschrijving, prijs, childIdsToList);
                    }
                }

                return null;
            }
            finally
            {
                _sqlConnection.Close();
            }
        }
    }
}