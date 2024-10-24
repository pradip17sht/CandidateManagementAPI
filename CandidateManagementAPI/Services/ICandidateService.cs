using CandidateManagementAPI.Models;

namespace CandidateManagementAPI.Services
{
    public interface ICandidateService
    {
        Task<Candidate> AddOrUpdateCandidateAsync(Candidate candidate);
        Task<Candidate> GetCandidateByEmailAsync(string email);
    }
}
