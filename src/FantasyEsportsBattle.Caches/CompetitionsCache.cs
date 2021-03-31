using FantasyEsportsBattle.Caches.Interface;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using FantasyEsportsBattle.Models.Tournament;
using System.Collections.Generic;
using System;
using System.Linq;

namespace FantasyEsportsBattle.Caches
{
    public class CompetitionsCache : ICache<Competition>
    {
        private const string ConnectionString = "Server=.;Database=FantasyEsports;Trusted_Connection=True;";

        public ICollection<Competition> Values {
            get
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return conn.Query<Competition>("SELECT * FROM Competitions").ToList();
                }
            } }
        public Action<Competition> OnItemAdded { get; set; }
        public Action<Competition> OnItemRemoved { get; set; }

        public bool AddItem(Competition item)
        {
            throw new NotImplementedException();
        }

        public bool BulkAddItem(ICollection<Competition> items)
        {
            throw new NotImplementedException();
        }

        public bool BulkRemoveItem(ICollection<Competition> items)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();               

                foreach(var item in items)
                {
                    conn.Execute($"DELETE FROM Competitions WHERE Id = {item.Id}");
                    OnItemRemoved?.Invoke(item);
                }
                return true;
            }
        }

        public bool RemoveItem(Competition item)
        {
            throw new NotImplementedException();
        }
    }
}
