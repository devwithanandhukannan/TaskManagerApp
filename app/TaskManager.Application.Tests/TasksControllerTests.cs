using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Controllers;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tests;

public class TasksControllerTests
{
    [Fact]
    public async Task CreateTask_WithValidPayload_ReturnsCreatedTaskResponse()
    {
        // 1. ARRANGE (Set up mock dependencies)
        var mockContext = new Mock<IApplicationDbContext>();
        
        // Mock the Tasks DbSet out of Entity Framework
        var mockDbSet = new Mock<DbSet<TaskItem>>();
        mockContext.Setup(c => c.Tasks).Returns(mockDbSet.Object);

        var controller = new TasksController(mockContext.Object);
        var requestDto = new CreateTaskRequest("Write Automated Tests", "user_abc");

        // 2. ACT (Execute the controller endpoint code)
        var result = await controller.CreateTask(requestDto);

        // 3. ASSERT (Verify the output matches expectations)
        var actionResult = Assert.IsType<ActionResult<TaskResponse>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        var responseBody = Assert.IsType<TaskResponse>(createdResult.Value);

        Assert.Equal("Write Automated Tests", responseBody.Title);
        Assert.Equal("user_abc", responseBody.OwnerId);
        
        // Verify that SaveChangesAsync was actually triggered on the database layer exactly once
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
