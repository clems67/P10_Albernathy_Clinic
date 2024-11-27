using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using ReportMicroservice.Models;

namespace ReportMicroservice.Controllers
{
    public enum riskLevel
    {
        None,
        Borderline,
        InDanger,
        EarlyOnset
    }

    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<string> _triggerTerms = new TriggerTerms().TriggerList;
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

            int numberTriggerTerms = _triggerTerms.Count(term => patientNote.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            bool over30 = DateTime.UtcNow.Subtract(patient.BirthDate).TotalDays > 30 * 365.25 ? true : false;
            bool isMale = patient.Sex == 'M' ? true : false;

            switch (numberTriggerTerms) {
                case int n when (n < 2):
                    return riskLevel.None;
                case 2:
                    if (!over30) return riskLevel.EarlyOnset;
                    else return riskLevel.None;
                case 3:
                    if (!over30 && !isMale) return riskLevel.EarlyOnset;
                    if (!over30 && isMale) return riskLevel.Borderline;
                    else return riskLevel.None;
                case 4:
                    if (over30) return riskLevel.EarlyOnset;
                    if (!over30 && !isMale) return riskLevel.Borderline;
                    if (!over30 && isMale) return riskLevel.InDanger;
                    break;
                case 5:
                    if (over30) return riskLevel.Borderline;
                    if (!over30) return riskLevel.InDanger;
                    break;
                case int n when (n >= 6):
                    return riskLevel.InDanger;
            }
            throw new NotImplementedException();
        }
    }
}
