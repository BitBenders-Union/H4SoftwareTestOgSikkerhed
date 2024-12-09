//using Bunit;
//using Bunit.TestDoubles;

//namespace H4SoftwareTestOgSikkerhedProject;

//public class TestIsAdminAssert
//{
//    [Fact]
//    public void IsAdminTest()
//    {
//        // Arrange
//        using var ctx = new TestContext();
//        var authContext = ctx.AddTestAuthorization();
//        authContext.SetRoles("Admin");

//        // Act
//        var cut = ctx.RenderComponent<H4SoftwareTestOgSikkerhed.Components.Pages.Home>();
//        var myOjbect = cut.Instance;

//        // Assert
//        Assert.True(myOjbect.isAuthenticated);
//    }
//}
