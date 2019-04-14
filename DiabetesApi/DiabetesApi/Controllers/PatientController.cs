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
        [HttpGet("gender/{gender}")]
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

                #region rest
                num_medications = newPatient.num_medications,
                admission_type_id = newPatient.admission_type_id,
                acarbose = newPatient.acarbose,
                medical_specialty = newPatient.medical_specialty,
                rosiglitazone = newPatient.rosiglitazone,
                glipizide = newPatient.glipizide,
                chlorpropamide = newPatient.chlorpropamide,
                metformin_rosiglitazone = newPatient.metformin_rosiglitazone,
                time_in_hospital = newPatient.time_in_hospital,
                examide = newPatient.examide,
                readmitted = newPatient.readmitted,
                encounter_id = newPatient.encounter_id,
                max_glu_serum = newPatient.max_glu_serum,
                num_lab_procedures = newPatient.num_lab_procedures,
                change = newPatient.change,
                num_procedures = newPatient.num_procedures,
                diag_1 = newPatient.diag_1,
                insulin = newPatient.insulin,
                tolazamide = newPatient.tolazamide,
                a1_cresult = newPatient.a1_cresult,
                number_outpatient = newPatient.number_outpatient,
                glyburide = newPatient.glyburide,
                nateglinide = newPatient.nateglinide,
                glimepiride = newPatient.glimepiride,
                discharge_disposition_id = newPatient.discharge_disposition_id,
                payer_code = newPatient.payer_code,
                age = newPatient.age,
                number_inpatient = newPatient.number_inpatient,
                glimepiride_pioglitazone = newPatient.glimepiride_pioglitazone,
                repaglinide = newPatient.repaglinide,
                miglitol = newPatient.miglitol,
                tolbutamide = newPatient.tolbutamide,
                weight = newPatient.weight,
                pioglitazone = newPatient.pioglitazone,
                citoglipton = newPatient.citoglipton,
                acetohexamide = newPatient.acetohexamide,
                metformin = newPatient.metformin,
                troglitazone = newPatient.troglitazone,
                diag_2 = newPatient.diag_2,
                number_diagnoses = newPatient.number_diagnoses,
                glyburide_metformin = newPatient.glyburide_metformin,
                diag_3 = newPatient.diag_3,
                glipizide_metformin = newPatient.glipizide_metformin,
                metformin_pioglitazone = newPatient.metformin_pioglitazone,
                admission_source_id = newPatient.admission_source_id,
                race = newPatient.race,
                number_emergency = newPatient.number_emergency,
                #endregion

                //CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            });
        }

        // PUT api/Patients/5 - updates a specific Patient
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Patient value)
        {
            _PatientRepository.UpdatePatient(id, value);
        }

        // PUT api/Patients/5 - updates a specific Patient
        [HttpPut("{id}/{numberOfMedication}")]
        public void Put_NumberOfMedication(int id, int numberOfMedication)
        {
            _PatientRepository.UpdateNumberOfMedication(id, numberOfMedication);
        }

        // DELETE api/Patients/5 - deletes a specific Patient
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _PatientRepository.RemovePatient(id);
        }
    }
}