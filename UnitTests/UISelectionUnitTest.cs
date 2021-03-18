using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using TaxMeApp;
using TaxMeApp.models;
using TaxMeApp.viewmodels;

namespace UnitTests
{
    [TestClass]
    public class UISelectionUnitTest
    {
        [TestMethod]
        public void TestSelection()
        {

            Loader loader = new Loader();

            ControlViewModel controlVM = loader.ControlVM;
            TaxPlansModel taxPlansModel = loader.TaxPlansModel;

            // Set to slant tax

            controlVM.SelectedTaxPlanName = "Slant Tax";

            Assert.AreEqual((int)TaxPlan.Slant, controlVM.SelectedTaxPlanTabIndex);

            // Try null value - should not change

            controlVM.SelectedTaxPlanName = null;

            Assert.AreEqual((int)TaxPlan.Slant, controlVM.SelectedTaxPlanTabIndex);

            // Set to flat tax

            controlVM.SelectedTaxPlanName = "Flat Tax";

            Assert.AreEqual((int)TaxPlan.Flat, controlVM.SelectedTaxPlanTabIndex);

            // Set to non-existent custom plan - also no change

            controlVM.SelectedTaxPlanName = "Anything else";

            Assert.AreEqual((int)TaxPlan.Flat, controlVM.SelectedTaxPlanTabIndex);

            // Create custom plan with enough values to make it work

            string customName = "Custom plan";

            var collection = new ObservableCollection<double>();
            for (int i = 0; i < 20; i++)
            {
                collection.Add(0);
            }

            // Add plan to our list

            controlVM.CreateTaxPlan(customName, collection);

            Assert.IsTrue(taxPlansModel.TaxPlanList.Contains(customName));

            // Select our custom plan - changes index to "custom"

            controlVM.SelectedTaxPlanName = customName;

            Assert.AreEqual((int)TaxPlan.Custom, controlVM.SelectedTaxPlanTabIndex);

        }

    }
}
