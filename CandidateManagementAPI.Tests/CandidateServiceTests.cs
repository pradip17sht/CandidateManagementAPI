using CandidateManagementAPI.Data;
using CandidateManagementAPI.Models;
using CandidateManagementAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CandidateManagementAPI.Tests
{
    public class CandidateServiceTests
    {
        private readonly AppDbContext _context;
        private readonly Mock<ICacheManager> _mockCacheManager;
        private readonly CandidateService _candidateService;

        public CandidateServiceTests()
        {
            // In-memory database setup with fixed database name to persist data between test methods
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestCandidateDb")
                .Options;

            _context = new AppDbContext(options);

            // Mock Cache Manager
            _mockCacheManager = new Mock<ICacheManager>();

            // Mock cache behavior: return null so no caching is used
            _mockCacheManager.Setup(c => c.Get<Candidate>(It.IsAny<string>())).Returns((Candidate)null);

            // CandidateService using in-memory DB context and mock cache
            _candidateService = new CandidateService(_context, _mockCacheManager.Object);
        }

        [Fact]
        public async Task AddOrUpdateCandidateAsync_ShouldAddNewCandidate()
        {
            // Arrange
            var candidate = new Candidate
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "123456789",
                Comment = "Candidate comment"
            };

            // Act
            var result = await _candidateService.AddOrUpdateCandidateAsync(candidate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(candidate.Email, result.Email);

            // Verify if the candidate is really in the context (database)
            var insertedCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);
            Assert.NotNull(insertedCandidate); // Check if candidate is inserted
            Assert.Equal("John", insertedCandidate.FirstName);
            Assert.Equal("Doe", insertedCandidate.LastName);
        }

        [Fact]
        public async Task AddOrUpdateCandidateAsync_ShouldUpdateExistingCandidate()
        {
            // Arrange
            var candidate = new Candidate
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "123456789",
                Comment = "Candidate comment"
            };

            // Add a candidate first
            await _candidateService.AddOrUpdateCandidateAsync(candidate);

            // Modify the candidate
            candidate.FirstName = "Jane";
            candidate.LastName = "Smith";

            // Act
            var updatedResult = await _candidateService.AddOrUpdateCandidateAsync(candidate);

            // Assert
            Assert.NotNull(updatedResult);
            Assert.Equal("Jane", updatedResult.FirstName);
            Assert.Equal("Smith", updatedResult.LastName);

            // Verify the update is actually applied in the database
            var updatedCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);
            Assert.NotNull(updatedCandidate);
            Assert.Equal("Jane", updatedCandidate.FirstName);  // Check if FirstName was updated
            Assert.Equal("Smith", updatedCandidate.LastName);   // Check if LastName was updated
        }
    }
}
