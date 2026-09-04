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
            var records = await ReadRecordsAsync();
            return records.Select(_validator.Validate).ToList();
        }

        public async Task<JobApplication?> GetByIdAsync(string id)
        {
            var all = await GetAllAsync();
            return all.FirstOrDefault(entry => entry.Id == id);
        }

        public async Task AddAsync(JobApplicationRecord record)
        {
            ArgumentNullException.ThrowIfNull(record);

            if (string.IsNullOrWhiteSpace(record.Id))
                record.Id = Guid.NewGuid().ToString("N");

            var records = await ReadRecordsAsync();
            records.Add(record);
            await WriteRecordsAsync(records);
        }

        public async Task DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;

            var records = await ReadRecordsAsync();
            var removed = records.RemoveAll(record => record.Id == id);
            if (removed > 0)
                await WriteRecordsAsync(records);
        }

        public async Task UpdateAsync(JobApplicationRecord record)
        {
            ArgumentNullException.ThrowIfNull(record);

            if (string.IsNullOrWhiteSpace(record.Id))
                return;

            var records = await ReadRecordsAsync();
            var index = records.FindIndex(existing => existing.Id == record.Id);
            if (index < 0)
                return;

            records[index] = record;
            await WriteRecordsAsync(records);
        }

        private async Task<List<JobApplicationRecord>> ReadRecordsAsync()
        {
            await EnsureSeededAsync();

            try
            {
                var json = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<JobApplicationRecord>>(json, SerializerOptions)
                       ?? new List<JobApplicationRecord>();
            }
            catch (JsonException)
            {
                // The whole file is unreadable; treat as empty rather than crashing.
                return new List<JobApplicationRecord>();
            }
        }

        private async Task WriteRecordsAsync(IReadOnlyList<JobApplicationRecord> records)
        {
            var json = JsonSerializer.Serialize(records, SerializerOptions);
            await File.WriteAllTextAsync(_filePath, json);
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
