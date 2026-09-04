using System.Collections.Generic;
using System.Threading.Tasks;
using Juribi.Models;

namespace Juribi.Services
{
    /// <summary>
    /// Provides read access to persisted job applications stored locally on the device.
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
    }
}
