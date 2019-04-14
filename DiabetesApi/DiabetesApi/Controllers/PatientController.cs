using DiabetesApi.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiabetesApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PatientsController : Controller
    {
        private readonly IPatientRepository _PatientRepository;

        public PatientsController(IPatientRepository PatientRepository)
        {
            _PatientRepository = PatientRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Patient>> Get()
        {
            return await _PatientRepository.GetAllPatients();
        }

        // GET api/Patients/5 - retrieves a specific Patient using either Id or InternalId (BSonId)
        [HttpGet("{patient_nbr}")]
        public async Task<Patient> Get(int patient_nbr)
        {
            return await _PatientRepository.GetPatient(patient_nbr) ?? new Patient();
        }

        // GET api/Patients/Male - all patients with gender == ""
        // ex: http://localhost:53617/api/Patients/Test/2018-01-01/10000
        [HttpGet("{gender}")]
        public async Task<IEnumerable<Patient>> GetPatient(string gender)
        {
            return await _PatientRepository.GetPatient(gender)
                        ?? new List<Patient>();
        }

        // POST api/Patients - creates a new Patient
        [HttpPost]
        public void Post([FromBody] Patient newPatient)
        {
            _PatientRepository.AddPatient(new Patient
            {
                gender = newPatient.gender,
                patient_nbr = newPatient.patient_nbr,
                diabetes_med = newPatient.diabetes_med,
                //CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });
        }

        // PUT api/Patients/5 - updates a specific Patient
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]int value)
        {
            _PatientRepository.UpdateNumberOfMedication(id, value);
        }

        // DELETE api/Patients/5 - deletes a specific Patient
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _PatientRepository.RemovePatient(id);
        }
    }
}