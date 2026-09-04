namespace Juribi.Models
{
    /// <summary>
    /// Represents the progress of a job application. <see cref="Unknown"/> is used as a
    /// fallback when a persisted value cannot be mapped to a known status.
    /// </summary>
    public enum JobApplicationStatus
    {
        Unknown = 0,
        Applied,
        Interview,
        Offer,
        Rejected,
        Withdrawn
    }
}
