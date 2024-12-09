//using Bunit;
//using Bunit.TestDoubles;

//namespace H4SoftwareTestOgSikkerhedProject;

//public class TestIsAuthenticatedAssert
//{
//    [Fact]
//    public void IsAdminTestAssert()
//    {
//        // Arrange
//        using var ctx = new TestContext();
//        var authContext = ctx.AddTestAuthorization();
//        authContext.SetAuthorized("test", AuthorizationState.Authorized);
//        //authContext.SetRoles("Admin");

//        // Act
//        var cut = ctx.RenderComponent<H4SoftwareTestOgSikkerhed.Components.Pages.Home>();
//        var myOjbect = cut.Instance;

//        // Assert
//        Assert.True(myOjbect.isAuthenticated);
//    }
//}
