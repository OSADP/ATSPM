using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MOE.CommonUnitTestProject
{
    /// <summary>
    /// Summary description for InrixGroup
    /// </summary>
    ///
    [TestClass]
    public class InrixGroupTest
    {
        [TestMethod]
        public void GroupDefaultConstructor()
        {
            MOE.Common.Business.Inrix.Group g = new Common.Business.Inrix.Group(1, "Test Group", "Test Group Description");
            Assert.IsNotNull(g);
        }

        [TestMethod]
        public void GroupCopyConstructor()
        {
            MOE.Common.Business.Inrix.Group g = new Common.Business.Inrix.Group(1, "Test Group", "Test Group Description");
            MOE.Common.Business.Inrix.Group copyGroup = new Common.Business.Inrix.Group(g);
            Assert.IsNotNull(copyGroup);
        }
        [TestMethod]
        public void AddMember()
        {
            MOE.Common.Business.Inrix.Group g = new Common.Business.Inrix.Group(1, "test group", "test group");

            MOE.Common.Business.Inrix.Route r = new Common.Business.Inrix.Route(1, "test", "test route");
            int beforeCount = g.Items.Count;
            g.AddMember(r);
            int afterCount = g.Items.Count;

            Assert.IsTrue(afterCount == beforeCount+1);
        }
        [TestMethod]
        public void RemoveMember()
        {
            MOE.Common.Business.Inrix.Group g = new Common.Business.Inrix.Group(1, "test group", "test group");

            MOE.Common.Business.Inrix.Route r = new Common.Business.Inrix.Route(1, "test", "test route");
            //int beforeCount = g.Items.Count;
            g.AddMember(r);
            g.RemoveMember(r);
            //int afterCount = g.Items.Count;

            Assert.IsTrue(g.Items.Count == 0);
        }

        
        [TestMethod]
        public void FillMembersTest()
        {
            RouteRepositoryTest test = new RouteRepositoryTest();

            MOE.Common.Models.Inrix.Repositories.RouteRepositoryFactory.SetRepository(test);

            MOE.Common.Business.Inrix.Group g = new Common.Business.Inrix.Group(1, "test group", "test group");

            g.FillMembers();
            

            Assert.IsTrue(g.Items.Count == 1 && g.Items[0].ID == -1);
        }

        [TestMethod]
        public void InsertGroup()
        {
            MOE.Common.Models.Inrix.Repositories.GroupRepository gr = new Common.Models.Inrix.Repositories.GroupRepository();

            MOE.Common.Models.Inrix.Group g = gr.SelectGroupByName("InsertGroupTest");

            if (g != null)
            {
                gr.Remove(g);   
            }
            MOE.Common.Business.Inrix.Group.InsertGroup("InsertGroupTest", "This is an insert Group Test");
            

            MOE.Common.Models.Inrix.Group newg = gr.SelectGroupByName("InsertGroupTest");

            Assert.IsNotNull(newg);

            gr.Remove(newg);

  
        }
        
        public InrixGroupTest()
        {
     
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
       
        }
    }

    public class RouteRepositoryTest : MOE.Common.Models.Inrix.Repositories.IRouteRepository
    {
        public List<MOE.Common.Models.Inrix.Route> GetRoutesByGroupID(int groupID)
        {
            List<MOE.Common.Models.Inrix.Route> routes = new List<Common.Models.Inrix.Route>();
            MOE.Common.Models.Inrix.Route r = new Common.Models.Inrix.Route();

            r.Route_ID = -1;
            r.Route_Name = "test route";
            r.Route_Description = "test routedescription";

            routes.Add(r);

            return routes;
        
        }

        public void Add(MOE.Common.Models.Inrix.Route route)
        {

        }



        public MOE.Common.Models.Inrix.Route GetRouteByName(string name)
        {
            MOE.Common.Models.Inrix.Route route = new MOE.Common.Models.Inrix.Route();
            return route;
        }

    }
}
