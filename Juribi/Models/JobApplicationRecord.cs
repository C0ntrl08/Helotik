namespace Juribi.Models
{
    /// <summary>
    /// Raw persisted shape of a job application as stored in the JSON file. All fields are
    /// nullable strings so that malformed/incomplete entries can be read and validated
    /// rather than causing deserialization to fail.
    /// </summary>
    public sealed class JobApplicationRecord
    {
        public string? Id { get; set; }

        public string? JobName { get; set; }

        public string? Company { get; set; }

        public string? Salary { get; set; }

        public string? Status { get; set; }

        public string? JobUrl { get; set; }
    }
}
