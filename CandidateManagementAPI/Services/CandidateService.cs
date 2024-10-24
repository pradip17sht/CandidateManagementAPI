using CandidateManagementAPI.Data;
using CandidateManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagementAPI.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly AppDbContext _context;
        private readonly ICacheManager _cacheManager;

        public CandidateService(AppDbContext context, ICacheManager cacheManager)
        {
            _context = context;
            _cacheManager = cacheManager;
        }

        public async Task<Candidate> AddOrUpdateCandidateAsync(Candidate candidate)
        {
            var existingCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);

            if (existingCandidate != null)
            {
                // Update candidate
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.CallTimeInterval = candidate.CallTimeInterval;
                existingCandidate.LinkedInUrl = candidate.LinkedInUrl;
                existingCandidate.GitHubUrl = candidate.GitHubUrl;
                existingCandidate.Comment = candidate.Comment;
            }
            else
            {
                await _context.Candidates.AddAsync(candidate);
            }

            await _context.SaveChangesAsync();

            // Cache the updated candidate
            _cacheManager.Set(candidate.Email, candidate);

            return candidate;
        }

        public async Task<Candidate> GetCandidateByEmailAsync(string email)
        {
            // Check cache first
            var cachedCandidate = _cacheManager.Get<Candidate>(email);
            if (cachedCandidate != null) return cachedCandidate;

            // Retrieve from database
            return await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
