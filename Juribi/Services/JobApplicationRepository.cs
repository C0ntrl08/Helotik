using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Juribi.Models;
using Juribi.Validation;
using Microsoft.Maui.Storage;

namespace Juribi.Services
{
    /// <summary>
    /// Stores job applications as a JSON file in the app's private data directory.
    /// The file can be replaced/edited externally; malformed entries are surfaced as
    /// invalid rather than causing a load failure.
    /// </summary>
    public sealed class JobApplicationRepository : IJobApplicationRepository
    {
        private const string FileName = "jobapplications.json";

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        private readonly JobApplicationValidator _validator;
        private readonly string _filePath;

        public JobApplicationRepository(JobApplicationValidator validator)
        {
            _validator = validator;
            _filePath = Path.Combine(FileSystem.AppDataDirectory, FileName);
        }

        public async Task<IReadOnlyList<JobApplication>> GetAllAsync()
        {
            await EnsureSeededAsync();

            List<JobApplicationRecord> records;
            try
            {
                var json = await File.ReadAllTextAsync(_filePath);
                records = JsonSerializer.Deserialize<List<JobApplicationRecord>>(json, SerializerOptions)
                          ?? new List<JobApplicationRecord>();
            }
            catch (JsonException)
            {
                // The whole file is unreadable; treat as empty rather than crashing.
                records = new List<JobApplicationRecord>();
            }

            return records.Select(_validator.Validate).ToList();
        }

        public async Task<JobApplication?> GetByIdAsync(string id)
        {
            var all = await GetAllAsync();
            return all.FirstOrDefault(entry => entry.Id == id);
        }

        private async Task EnsureSeededAsync()
        {
            if (File.Exists(_filePath))
                return;

            var json = JsonSerializer.Serialize(CreateSampleData(), SerializerOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }

        private static IReadOnlyList<JobApplicationRecord> CreateSampleData() => new List<JobApplicationRecord>
        {
            new()
            {
                Id = Guid.NewGuid().ToString("N"),
                JobName = "Senior .NET Developer",
                Company = "Contoso",
                Salary = "€75,000 / year",
                Status = "Applied",
                JobUrl = "https://example.com/jobs/senior-dotnet"
            },
            new()
            {
                Id = Guid.NewGuid().ToString("N"),
                JobName = "Mobile Engineer (MAUI)",
                Company = "Fabrikam",
                Salary = "€68,000 / year",
                Status = "Interview",
                JobUrl = "https://example.com/jobs/maui-engineer"
            },
            new()
            {
                Id = Guid.NewGuid().ToString("N"),
                JobName = "Backend Developer",
                Company = "Northwind",
                Salary = "€60,000 / year",
                Status = "Offer",
                JobUrl = "https://example.com/jobs/backend"
            },
            new()
            {
                // Intentionally invalid sample: missing company and unrecognized status,
                // to demonstrate inline "invalid" display.
                Id = Guid.NewGuid().ToString("N"),
                JobName = "Data Analyst",
                Company = null,
                Salary = "Unspecified",
                Status = "Pending",
                JobUrl = "not-a-valid-url"
            }
        };
    }
}
