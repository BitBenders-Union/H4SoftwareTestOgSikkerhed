//using Bunit;
//using Bunit.TestDoubles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace H4SoftwareTestOgSikkerhedProject
//{
//    public class TestIsAuthenticatedAndIsAdminAssert
//    {
//        [Fact]

//        public void IsAuthenticatedAndIsAdminTestAssert()
//        {
//            //Arrange
//            using var ctx = new TestContext();
//            var authContext = ctx.AddTestAuthorization();
//            authContext.SetAuthorized("test", AuthorizationState.Authorized);
//            authContext.SetRoles("Admin");

//            //Act
//            var cut = ctx.RenderComponent<H4SoftwareTestOgSikkerhed.Components.Pages.Home>();
//            var myOjbect = cut.Instance;

//            //Assert
//            Assert.True(myOjbect.isAuthenticated);
//            Assert.True(myOjbect.isAdmin);


//        }
//    }
//}
