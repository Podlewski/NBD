using DiabetesApi.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiabetesApi
{
    public class PatientContext
    {
        private readonly IMongoDatabase _database = null;

        public PatientContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Patient> Patients
        {
            get
            {
                return _database.GetCollection<Patient>("allData");
            }
        }
    }
}
