using FantasyEsportsBattle.Caches.Interface;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using FantasyEsportsBattle.Models.Tournament;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace FantasyEsportsBattle.Caches
{
    public class CompetitionsCache : ICache<Competition>
    {
        private const string ConnectionString = "Server=.;Database=FantasyEsports;Trusted_Connection=True;";
        private List<Competition> _state;
        private readonly TimeSpan _workerBreakInterval = TimeSpan.FromSeconds(10); 
        public CompetitionsCache()
        {
            _state = Values.ToList();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    CheckForChanges();

                    Thread.Sleep(_workerBreakInterval);
                }
            });
        }


        private void CheckForChanges()
        {
            var newState = Values.ToList();

            foreach(var competition in _state)
            {
                if(newState.All(newCompetition => newCompetition.Id != competition.Id))
                {
                    OnItemRemoved?.Invoke(competition);
                }
            }

            foreach (var competition in newState)
            {
                if (_state.All(newCompetition => newCompetition.Id != competition.Id))
                {
                    OnItemAdded?.Invoke(competition);
                }
            }

            _state = newState;
        }
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
