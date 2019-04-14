using DiabetesApi.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiabetesApi
{
    public interface IPatientRepository
    {
        // get all patients as collection
        Task<IEnumerable<Patient>> GetAllPatients();
        Task<Patient> GetPatient(int patient_nbr);

        // query for list of patients
        Task<IEnumerable<Patient>> GetPatient(string gender);

        // add new Patient document
        Task AddPatient(Patient item);

        // remove a single document / Patient
        Task<bool> RemovePatient(int id);

        // update just a single document / Patient
        Task<bool> UpdatePatient(int id, Patient item);

        // update Patient number
        Task<bool> UpdateNumberOfMedication(int patient_nbr, int numberOfMedication);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllPatients();

    }
}
