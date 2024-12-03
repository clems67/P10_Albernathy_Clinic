using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReportMicroservice.Controllers;
using MongoDB.Driver;
using NotesMicroservice.Models;
using System;
using ReportMicroservice.Models;
using ReportMicroservice;
using System;
using Xunit;
using System.Collections.Generic;

namespace TestProject
{

    public class TriggerTermsServiceTests
    {
        [Fact]
        public void GetRiskLevel_LessThan2TriggerTerms_ReturnsNone()
        {
            // Arrange
            var service = new TriggerTermsService();
            var patient = new PatientModel
            {
                BirthDate = DateTime.UtcNow.AddYears(-35),
                Sex = 'M'
            };
            string note = "Patient states that they are feeling terrific. Weight at or below recommended level.";

            // Act
            var result = service.GetRiskLevel(note, patient);

            // Assert
            Assert.Equal(riskLevel.None, result);
        }

        [Fact]
        public void GetRiskLevel_Exactly2TriggerTerms_Over30_ReturnsNone()
        {
            // Arrange
            var service = new TriggerTermsService();
            var patient = new PatientModel
            {
                BirthDate = DateTime.UtcNow.AddYears(-40),
                Sex = 'M'
            };
            string note = "Lab reports Microalbumin elevated. Patient states that they are a Smoker.";

            // Act
            var result = service.GetRiskLevel(note, patient);

            // Assert
            Assert.Equal(riskLevel.None, result);
        }

        [Fact]
        public void GetRiskLevel_Exactly2TriggerTerms_Under30_ReturnsEarlyOnset()
        {
            // Arrange
            var service = new TriggerTermsService();
            var patient = new PatientModel
            {
                BirthDate = DateTime.UtcNow.AddYears(-20),
                Sex = 'M'
            };
            string note = "Lab reports Microalbumin elevated. Patient states that they are a Smoker.";

            // Act
            var result = service.GetRiskLevel(note, patient);

            // Assert
            Assert.Equal(riskLevel.EarlyOnset, result);
        }

        [Fact]
        public void GetRiskLevel_Exactly3TriggerTerms_Under30AndMale_ReturnsBorderline()
        {
            // Arrange
            var service = new TriggerTermsService();
            var patient = new PatientModel
            {
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Sex = 'M'
            };
            string note = "Lab reports Microalbumin elevated. Patient states that they are a Smoker. Reaction to medication.";

            // Act
            var result = service.GetRiskLevel(note, patient);

            // Assert
            Assert.Equal(riskLevel.Borderline, result);
        }

        [Fact]
        public void GetRiskLevel_Exactly3TriggerTerms_Under30AndNotMale_ReturnsEarlyOnset()
        {
            // Arrange
            var service = new TriggerTermsService();
            var patient = new PatientModel
            {
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Sex = 'F'
            };
            string note = "Lab reports Microalbumin elevated. Patient states that they are a Smoker. Reaction to medication.";

            // Act
            var result = service.GetRiskLevel(note, patient);

            // Assert
            Assert.Equal(riskLevel.EarlyOnset, result);
        }

        [Fact]
        public void GetRiskLevel_Exactly4TriggerTerms_Over30_ReturnsEarlyOnset()
        {
            // Arrange
            var service = new TriggerTermsService();
            var patient = new PatientModel
            {
                BirthDate = DateTime.UtcNow.AddYears(-50),
                Sex = 'M'
            };
            string note = "Lab reports Microalbumin elevated. Patient states that they are a Smoker. Reaction to medication. Cholesterol LDL high.";

            // Act
            var result = service.GetRiskLevel(note, patient);

            // Assert
            Assert.Equal(riskLevel.EarlyOnset, result);
        }

        [Fact]
        public void GetRiskLevel_Exactly5TriggerTerms_Under30_ReturnsInDanger()
        {
            // Arrange
            var service = new TriggerTermsService();
            var patient = new PatientModel
            {
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Sex = 'M'
            };
            string note = "Lab reports Microalbumin elevated. Patient states that they are a Smoker. Reaction to medication. Cholesterol LDL high. Hemoglobin A1C above recommended level.";

            // Act
            var result = service.GetRiskLevel(note, patient);

            // Assert
            Assert.Equal(riskLevel.InDanger, result);
        }

        [Fact]
        public void GetRiskLevel_AtLeast6TriggerTerms_ReturnsInDanger()
        {
            // Arrange
            var service = new TriggerTermsService();
            var patient = new PatientModel
            {
                BirthDate = DateTime.UtcNow.AddYears(-25),
                Sex = 'M'
            };
            string note = "Lab reports Microalbumin elevated. Patient states that they are a Smoker. Reaction to medication. Cholesterol LDL high. Hemoglobin A1C above recommended level. Antibodies present elevated.";

            // Act
            var result = service.GetRiskLevel(note, patient);

            // Assert
            Assert.Equal(riskLevel.InDanger, result);
        }
    }

}