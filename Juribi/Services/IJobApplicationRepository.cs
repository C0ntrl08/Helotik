using System.Collections.Generic;
using System.Threading.Tasks;
using Juribi.Models;

namespace Juribi.Services
{
    /// <summary>
    /// Provides read and append access to persisted job applications stored locally on the device.
    /// </summary>
    public interface IJobApplicationRepository
    {
        /// <summary>
        /// Loads all job applications from local storage, seeding sample data on first run.
        /// Individual invalid entries are returned marked as invalid rather than skipped.
        /// </summary>
        Task<IReadOnlyList<JobApplication>> GetAllAsync();

        /// <summary>Returns a single entry by its id, or <c>null</c> when not found.</summary>
        Task<JobApplication?> GetByIdAsync(string id);

        /// <summary>
        /// Appends a new job application record to local storage. A new id is assigned when
        /// the record does not already have one.
        /// </summary>
        Task AddAsync(JobApplicationRecord record);

        /// <summary>Replaces the stored entry that matches <paramref name="record"/>'s id. No-op when not found.</summary>
        Task UpdateAsync(JobApplicationRecord record);

        /// <summary>Removes the entry with the given id from local storage. No-op when not found.</summary>
        Task DeleteAsync(string id);
    }
}
