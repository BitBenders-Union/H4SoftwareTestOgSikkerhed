//using Bunit;
//using Bunit.TestDoubles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace H4SoftwareTestOgSikkerhedProject
//{
//    public class TestNotAuthenticatedAssert
//    {
//        [Fact]
//        public void NotAuthenticatedTest()
//        {
//            // Arrange
//            using var ctx = new TestContext();
//            var authContext = ctx.AddTestAuthorization();

//            // Act
//            var cut = ctx.RenderComponent<H4SoftwareTestOgSikkerhed.Components.Pages.Home>();
//            var myOjbect = cut.Instance;

//            // Assert
//            Assert.False(myOjbect.isAuthenticated);
//        }
//    }
//}
