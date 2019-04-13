using DiabetesApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiabetesApi
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllPatients();
        Task<Patient> GetPatient(string id);

        // query after multiple parameters
        Task<IEnumerable<Patient>> GetPatient(int patient_nbr, DateTime updatedFrom);

        // add new Patient document
        Task AddPatient(Patient item);

        // remove a single document / Patient
        Task<bool> RemovePatient(string id);

        // update just a single document / Patient
        Task<bool> UpdatePatient(string id, string body);
        
        // demo interface - full document update
        Task<bool> UpdatePatientNumber(string id, int patient_nbr);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllPatients();
        
    }
}
