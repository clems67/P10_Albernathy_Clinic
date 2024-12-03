using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using ReportMicroservice.Models;

namespace ReportMicroservice.Controllers
{
    public enum riskLevel
    {
        None,
        EarlyOnset,
        Borderline,
        InDanger
    }

    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;
        public ReportController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<riskLevel>> GetRiskLevelAsync(int patientId)
        {
            var query = new Dictionary<string, string>()
            {
                ["patientId"] = patientId.ToString(),
            };
            var uri = QueryHelpers.AddQueryString("http://patientMicroservice:8080/Patient", query);
            var patientInfoRequest = await _httpClient.GetAsync(uri);
            if(patientInfoRequest.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("Patient Id not found");
            }
            var patient = JsonConvert.DeserializeObject<PatientModel>(await patientInfoRequest.Content.ReadAsStringAsync());

            query = new Dictionary<string, string>()
            {  
                ["patientId"] = patientId.ToString(),
            };
            uri = QueryHelpers.AddQueryString("http://notesMicroservice:8080/Notes", query);
            var notesRequest = await _httpClient.GetAsync(uri);
            if(notesRequest.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("No notes found for this patient");
            }
            string patientNote = await notesRequest.Content.ReadAsStringAsync();

            return new TriggerTermsService().GetRiskLevel(patientNote, patient);
        }
    }
}
    