using ReportMicroservice.Controllers;
using ReportMicroservice.Models;

namespace ReportMicroservice
{
    public class TriggerTermsService
    {
        public readonly List<string> TriggerList = new List<string>
        {
            "Hemoglobin A1C",
            "Microalbumin",
            "Body Height",
            "Body Weight",
            "Smoker",
            "Abnormal",
            "Cholesterol",
            "Dizziness",
            "Relapse",
            "Reaction",
            "Antibodies"
        };

        public riskLevel GetRiskLevel(string note, PatientModel patient)
        {
            int numberTriggerTerms = TriggerList.Count(term => note.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            bool over30 = DateTime.UtcNow.Subtract(patient.BirthDate).TotalDays > 30 * 365.25 ? true : false;
            bool isMale = patient.Sex == 'M' ? true : false;

            switch (numberTriggerTerms)
            {
                case int n when (n <= 1):
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
