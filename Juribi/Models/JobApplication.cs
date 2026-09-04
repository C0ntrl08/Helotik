using System.Collections.Generic;

namespace Juribi.Models
{
    /// <summary>
    /// A single job application entry. Carries its own validation state so invalid
    /// entries can still be displayed (marked as invalid) rather than being discarded.
    /// </summary>
    public sealed class JobApplication
    {
        public string Id { get; init; } = string.Empty;

        public string? JobName { get; init; }

        public string? Company { get; init; }

        /// <summary>
        /// The money offered as described in the job posting. Kept as free-form text to
        /// faithfully represent the source description without currency/culture parsing.
        /// </summary>
        public string? Salary { get; init; }

        public JobApplicationStatus Status { get; init; }

        public string? JobUrl { get; init; }

        /// <summary>Validation problems found for this entry. Empty when the entry is valid.</summary>
        public IReadOnlyList<string> Errors { get; init; } = new List<string>();

        public bool IsValid => Errors.Count == 0;
    }
}
