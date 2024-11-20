using Bunit;
using Bunit.TestDoubles;

namespace H4SoftwareTestOgSikkerhedProject;

public class TestIsAuthenticatedAndIsAdmin
{
    [Fact]
    public void IsAuthenticatedAndIsAdminTest()
    {
        // Arrange
        using var ctx = new TestContext();
        var authContext = ctx.AddTestAuthorization();
        authContext.SetAuthorized("test", AuthorizationState.Authorized);
        authContext.SetRoles("Admin");

        // Act
        var cut = ctx.RenderComponent<H4SoftwareTestOgSikkerhed.Components.Pages.Home>();

        // Assert
        cut.MarkupMatches(@"<h1>Hello, world!</h1>
                            <p>Welcome to your new app.</p>
                            	<div>
		                            <p>You are Authenticated!</p>
	                            </div>
		                        <div>
			                        <p>You are an Admin!</p>
		                        </div>");




    }


}