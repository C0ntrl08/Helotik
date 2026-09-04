using System;
using System.Collections.Generic;
using Juribi.Models;

namespace Juribi.Validation
{
    /// <summary>
    /// Converts a raw <see cref="JobApplicationRecord"/> into a <see cref="JobApplication"/>,
    /// collecting any validation problems instead of throwing. Invalid entries are still
    /// produced so the UI can display them marked as invalid.
    /// </summary>
    public sealed class JobApplicationValidator
    {
        public JobApplication Validate(JobApplicationRecord record)
        {
            ArgumentNullException.ThrowIfNull(record);

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(record.JobName))
                errors.Add("Job name is required.");

            if (string.IsNullOrWhiteSpace(record.Company))
                errors.Add("Company is required.");

            var status = ParseStatus(record.Status, errors);

            ValidateUrl(record.JobUrl, errors);

            return new JobApplication
            {
                Id = string.IsNullOrWhiteSpace(record.Id) ? Guid.NewGuid().ToString("N") : record.Id!,
                JobName = record.JobName,
                Company = record.Company,
                Salary = record.Salary,
                Status = status,
                JobUrl = record.JobUrl,
                Errors = errors
            };
        }

        private static JobApplicationStatus ParseStatus(string? rawStatus, ICollection<string> errors)
        {
            if (string.IsNullOrWhiteSpace(rawStatus))
            {
                errors.Add("Status is required.");
                return JobApplicationStatus.Unknown;
            }

            if (Enum.TryParse<JobApplicationStatus>(rawStatus, ignoreCase: true, out var status)
                && status != JobApplicationStatus.Unknown)
            {
                return status;
            }

            errors.Add($"Status '{rawStatus}' is not a recognized value.");
            return JobApplicationStatus.Unknown;
        }

        private static void ValidateUrl(string? url, ICollection<string> errors)
        {
            if (string.IsNullOrWhiteSpace(url))
                return; // Optional field.

            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                errors.Add($"Job link '{url}' is not a valid URL.");
        }
    }
}
