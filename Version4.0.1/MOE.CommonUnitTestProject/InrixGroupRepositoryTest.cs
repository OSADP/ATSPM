using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MOE.Common;
using System.Collections.Generic;

namespace MOE.CommonUnitTestProject
{
    [TestClass]
    public class GroupRepositoryTest
    {
        [TestMethod]
        public void GetAll()
        {
            MOE.Common.Models.Inrix.Repositories.GroupRepository gr = new MOE.Common.Models.Inrix.Repositories.GroupRepository();
            System.Collections.Generic.List< MOE.Common.Models.Inrix.Group> grlist = gr.GetAll();
            Assert.IsNotNull(grlist);
        }

        [TestMethod]
        public void Add()
        {
            MOE.Common.Models.Inrix.Repositories.GroupRepository gr = new MOE.Common.Models.Inrix.Repositories.GroupRepository();
            MOE.Common.Models.Inrix.Group groupExists = gr.SelectGroupByName("test group");
            if(groupExists != null)
            {
                gr.Remove(groupExists);
            }
            MOE.Common.Models.Inrix.Group g = new MOE.Common.Models.Inrix.Group();
            g.Group_Name = "test group";
            g.Group_Description = "This is the test group";
            gr.Add(g);

            groupExists = gr.SelectGroupByName("test group");

            Assert.IsTrue(groupExists.Group_Name == "test group");
            gr.Remove(groupExists);
        }

        

        [TestMethod]
        public void Update()
        {
            MOE.Common.Models.Inrix.Repositories.GroupRepository gr = new MOE.Common.Models.Inrix.Repositories.GroupRepository();
            MOE.Common.Models.Inrix.Group groupExists = gr.SelectGroupByName("test group");
            MOE.Common.Models.Inrix.Group g = new MOE.Common.Models.Inrix.Group();
            if (groupExists == null)
            {
                
                g.Group_Name = "test group";
                g.Group_Description = "This is the test group";
                gr.Add(g);
            }

            g = gr.SelectGroupByName("test group");
            
            g.Group_Name = "Changed Name";

            gr.Update(g);


            groupExists = gr.SelectGroupByName("Changed Name");

            Assert.IsTrue(groupExists.Group_Name == "Changed Name");
            gr.Remove(groupExists);
        }

        [TestMethod]
        public void SelectByIDTest()
        {
            MOE.Common.Models.Inrix.Repositories.GroupRepository gr = new MOE.Common.Models.Inrix.Repositories.GroupRepository();

            List<MOE.Common.Models.Inrix.Group> gl = gr.GetAll();

            if (gl.Count > 1)
            {
                MOE.Common.Models.Inrix.Group g = gr.SelectByID(gl[0].Group_ID);
                Assert.IsTrue(gl[0].Group_ID == g.Group_ID);
            }
        }

        [TestMethod]
        public void RemoveByID()
        {
            MOE.Common.Models.Inrix.Repositories.GroupRepository gr = new MOE.Common.Models.Inrix.Repositories.GroupRepository();
            MOE.Common.Models.Inrix.Group groupExists = gr.SelectGroupByName("test group");
            if (groupExists != null)
            {
                gr.Remove(groupExists);
            }
            MOE.Common.Models.Inrix.Group g = new MOE.Common.Models.Inrix.Group();
            g.Group_Name = "test group";
            g.Group_Description = "This is the test group";
            gr.Add(g);

            groupExists = gr.SelectGroupByName("test group");

            gr.RemoveByID(groupExists.Group_ID);

            MOE.Common.Models.Inrix.Group groupIsGone = gr.SelectByID(groupExists.Group_ID);

            Assert.IsNull(groupIsGone);

        }


    }
}
