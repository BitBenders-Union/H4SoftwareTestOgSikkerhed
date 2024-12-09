using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace H4SoftwareTestOgSikkerhedProject;

public class TestIsAuthenticated
{
    [Fact]
    public void IsAuthenticatedTest()
    {
        // Arrange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("test", AuthorizationState.Authorized);

        // Act
        var cut = ctx.RenderComponent<H4SoftwareTestOgSikkerhed.Components.Pages.Home>();

        // Assert
        cut.MarkupMatches(@"<h1>Hello, world!</h1>
                            <p>Welcome to your new app.</p>
                            	<div>
		                            <p>You are Authenticated!</p>
	                            </div>");


    }
}