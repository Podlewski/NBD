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
                throw ex;
            }
        }

        public async Task<Patient> GetPatient(int patient_nbr)
        {
            try
            {
                //ObjectId internalId = GetInternalId(patient_nbr);
                return await _context.Patients
                                .Find(Patient => Patient.patient_nbr == patient_nbr
                                        //|| Patient.InternalId == internalId
                                        )
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<Patient>> GetPatient(string gender)
        {
            try
            {
                var query = _context.Patients.Find(Patient => Patient.gender == gender);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
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
                throw ex;
            }
        }

        public async Task<bool> RemovePatient(int patient_nbr)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Patients.DeleteOneAsync(
                        Builders<Patient>.Filter.Eq("patient_nbr", patient_nbr));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdatePatient(int patient_nbr, Patient item)
        {
            try
            {
                ReplaceOneResult actionResult
                    = await _context.Patients
                                    .ReplaceOneAsync(n => n.patient_nbr == patient_nbr
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateNumberOfMedication(int patient_nbr, int numberOfMedication)
        {
            var item = await GetPatient(patient_nbr) ?? new Patient();
            item.num_medications = numberOfMedication;
            item.UpdatedOn = DateTime.Now;

            return await UpdatePatient(patient_nbr, item);
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
                throw ex;
            }
        }
    }
}
