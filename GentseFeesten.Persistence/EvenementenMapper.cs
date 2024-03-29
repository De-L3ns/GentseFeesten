﻿using GentseFeesten.Domain;
using GentseFeesten.Domain.Model;
using GentseFeesten.Domain.Repository;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GentseFeesten.Persistence
{
    public class EvenementenMapper : IEvenementenRepository
    {
        // Database related private fields
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=GentseFeesten;Integrated Security=True; Encrypt=False";
        private readonly SqlConnection _sqlConnection;

        // Data related private fields
        private List<Evenement> _allMainEvents = new List<Evenement>();
        private List<DateTime?> _dateTimesToWorkWith = new List<DateTime?>();
        private int _totalPrice = 0;



        public EvenementenMapper()
        {
            _sqlConnection = new SqlConnection(ConnectionString);
        }

        public void InsertCsvIntoDatabase()
        {
            CsvToDatabaseMapper.InsertCsvIntoDatabase();
        }

        public List<Evenement> GetMainEvents()
        {
            if (_allMainEvents.Count == 0)
            {
                GetAllParentEvents();
            }

            return _allMainEvents;
        }

        private void GetAllParentEvents()
        {
            try
            {
                _sqlConnection.Open();

                SqlCommand cmd = new("SELECT * FROM EvenementenGF WHERE Parent IS NULL;", _sqlConnection);
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

                        _allMainEvents.Add(new MainEvenement(id, name, childIdsToList, start, end, description, price));
                    }
                }
            }
            catch
            {
                throw new DatabaseConnectionException("GetParentEvents");
            }
            finally
            {
                _sqlConnection.Close();
            }
        }

        public List<DateTime?> GetMissingStartData(Evenement evenement)
        {
            _dateTimesToWorkWith.Clear();
            return GetMissingDateTimes(evenement, "Start");

        }

        public List<DateTime?> GetMissingEndData(Evenement evenement)
        {
            _dateTimesToWorkWith.Clear();
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
                SqlCommand cmd = new SqlCommand($"SELECT * FROM EvenementenGF WHERE Parent = '{evenement.Id}';", _sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    // Load into a seperate Datatable so the method can be called Recursivly.
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string id = (string)dr["Id"];
                        DateTime? childDateTime = (dr[columnName] == DBNull.Value) ? null : (DateTime?)dr[columnName];
                        if (childDateTime == null)
                        {
                            _sqlConnection.Close();
                            GetMissingDateTimes(GetEventById(id), columnName);
                        }
                        else
                        {
                            _dateTimesToWorkWith.Add(childDateTime);
                        }
                    }
                }
                return _dateTimesToWorkWith;
            }
            catch
            {
                throw new DatabaseConnectionException("GetMissingDateTimes");
            }

            finally
            {
                _sqlConnection.Close();

            }
        }

        private int GetMissingPrice(Evenement evenement)
        {
            try
            {
                _sqlConnection.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM EvenementenGF WHERE Parent = '{evenement.Id}';", _sqlConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    // Load into a seperate Datatable so the method can be called Recursivly.
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
                        }
                        else
                        {
                            _totalPrice += (int)prijs;
                        }
                    }
                }
                return _totalPrice;
            }
            catch
            {
                throw new DatabaseConnectionException("GetMissingPrice");
            }
            finally
            {
                _sqlConnection.Close();
            }
        }


        public void GetChildEvents(Evenement evenement)
        {
            try
            {
                _sqlConnection.Open();
                List<Evenement> listOfChilds = new List<Evenement>();

                SqlCommand cmd = new SqlCommand("SELECT * FROM EvenementenGF WHERE Parent = @parentId", _sqlConnection);
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

                        listOfChilds.Add(new ChildEvenement(id, name, start, end, description, price, childIdsToList, parent, evenement));
                    }

                }

                evenement.SetChilds(listOfChilds);

            }
            catch
            {
                throw new DatabaseConnectionException("GetChildEvents");
            }
            finally { _sqlConnection.Close(); }
        }

        private Evenement GetEventById(string eventId)
        {
            Evenement? evenement = _allMainEvents.Where(e => e.Id == eventId).FirstOrDefault();

            if (evenement == null)
            {
                try
                {
                    _sqlConnection.Open();

                    SqlCommand cmd = new("SELECT * FROM EvenementenGF WHERE Id = @Id;", _sqlConnection);
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

                            Evenement newEvenement = new Evenement(id, name, childIdsToList, start, end, description, price);
                            _allMainEvents.Add(newEvenement);
                            evenement = newEvenement;
                        }
                    }
                }
                catch
                {
                    throw new DatabaseConnectionException("GetEventById");
                }
                finally
                {
                    _sqlConnection.Close();
                }
            }
            return evenement;
        }
    }
}