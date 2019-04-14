using DiabetesApi.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiabetesApi
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientContext _context = null;

        public PatientRepository(IOptions<Settings> settings)
        {
            _context = new PatientContext(settings);
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            try
            {
                return await _context.Patients
                        .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after Id or InternalId (BSonId value)
        public async Task<Patient> GetPatient(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.Patients
                                .Find(Patient => Patient.Id == id
                                        || Patient.InternalId == internalId)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after body text, updated time, and header image size
        public async Task<IEnumerable<Patient>> GetPatient(int patient_nbr, DateTime updatedFrom)
        {
            try
            {
                var query = _context.Patients.Find(Patient => Patient.patient_nbr == patient_nbr && Patient.UpdatedOn >= updatedFrom);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public async Task AddPatient(Patient item)
        {
            try
            {
                await _context.Patients.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemovePatient(string id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Patients.DeleteOneAsync(
                        Builders<Patient>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdatePatient(string id, string diabetes_med)
        {
            var filter = Builders<Patient>.Filter.Eq(s => s.Id, id);
            var update = Builders<Patient>.Update
                            .Set(s => s.diabetes_med, diabetes_med)
                            .CurrentDate(s => s.UpdatedOn);

            try
            {
                UpdateResult actionResult
                    = await _context.Patients.UpdateOneAsync(filter, update);

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdatePatient(string id, Patient item)
        {
            try
            {
                ReplaceOneResult actionResult
                    = await _context.Patients
                                    .ReplaceOneAsync(n => n.Id.Equals(id)
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdatePatientNumber(string id, int patient_nbr)
        {
            var item = await GetPatient(id) ?? new Patient();
            item.patient_nbr = patient_nbr;
            item.UpdatedOn = DateTime.Now;

            return await UpdatePatient(id, item);
        }

        public async Task<bool> RemoveAllPatients()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Patients.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
    }
}
