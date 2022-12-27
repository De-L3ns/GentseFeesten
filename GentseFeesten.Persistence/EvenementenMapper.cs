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
                        DateTime? einde = (reader["End"] == DBNull.Value) ? null : (DateTime?)reader["End"];
                        DateTime? begin = (reader["Start"] == DBNull.Value) ? null : (DateTime?)reader["Start"];
                        string? childIds = (reader["Childs"] == DBNull.Value) ? null : (string?)reader["Childs"];
                        List<string>? childIdsToList = childIds?.Split(",").ToList();
                        string? beschrijving = (reader["Description"] == DBNull.Value) ? null : (string?)reader["Description"];
                        string? naam = (string)reader["Name"];
                        int? prijs = (reader["Price"] == DBNull.Value) ? null : (int?)reader["Price"];


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
            return GetMissingDateTimes(evenement, "Start");

        }

        public List<DateTime?> GetMissingEndData(Evenement evenement)
        {
            _result.Clear();
            return GetMissingDateTimes(evenement, "End");
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
                        int? prijs = (dr["Price"] == DBNull.Value) ? null : (int?)dr["Price"];
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
                        DateTime? end = (reader["End"] == DBNull.Value) ? null : (DateTime?)reader["End"];
                        DateTime? start = (reader["Start"] == DBNull.Value) ? null : (DateTime?)reader["Start"];
                        string? childIds = (reader["Childs"] == DBNull.Value) ? null : (string?)reader["Childs"];
                        List<string>? childIdsToList = childIds?.Split(",").ToList();
                        string? description = (reader["Description"] == DBNull.Value) ? null : (string?)reader["Description"];
                        string? name = (string)reader["Name"];
                        string? parent = (reader["Parent"] == DBNull.Value) ? null : (string?)reader["Parent"];
                        int? price = (reader["Price"] == DBNull.Value) ? null : (int?)reader["Price"];


                        listOfChilds.Add(new ChildEvenement(id, name, start, end, description, price, childIdsToList, parent));

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
                        DateTime? end = (reader["End"] == DBNull.Value) ? null : (DateTime?)reader["End"];
                        DateTime? start = (reader["Start"] == DBNull.Value) ? null : (DateTime?)reader["Start"];
                        string? childIds = (reader["Childs"] == DBNull.Value) ? null : (string?)reader["Childs"];
                        List<string>? childIdsToList = childIds?.Split(",").ToList();
                        string? description = (reader["Description"] == DBNull.Value) ? null : (string?)reader["Description"];
                        string? name = (string)reader["Name"];
                        int? price = (reader["Price"] == DBNull.Value) ? null : (int?)reader["Price"];


                        return new ParentEvenement(id, name, start, end, description, price, childIdsToList);
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