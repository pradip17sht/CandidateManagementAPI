using CandidateManagementAPI.Data;
using CandidateManagementAPI.Models;
using CandidateManagementAPI.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CandidateManagementAPI.Tests;

public class UnitTest1
{
    private readonly AppDbContext _context;
    private readonly Mock<ICacheManager> _mockCacheManager;
    private readonly CandidateService _candidateService;

    public UnitTest1()
    {
        // In-memory database setup
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        _context = new AppDbContext(options);

        // Mock Cache Manager
        _mockCacheManager = new Mock<ICacheManager>();

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
            Email = "john@example.com"
        };

        // Act
        var result = await _candidateService.AddOrUpdateCandidateAsync(candidate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(candidate.Email, result.Email);
    }

    [Fact]
    public async Task AddOrUpdateCandidateAsync_ShouldUpdateExistingCandidate()
    {
        // Arrange
        var candidate = new Candidate
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        await _candidateService.AddOrUpdateCandidateAsync(candidate);

        // Modify the candidate
        candidate.FirstName = "Jane";

        // Act
        var updatedResult = await _candidateService.AddOrUpdateCandidateAsync(candidate);

        // Assert
        Assert.NotNull(updatedResult);
        Assert.Equal("Jane", updatedResult.FirstName); // Check the update
    }
}