using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using TaxMeApp;
using TaxMeApp.models;
using TaxMeApp.viewmodels;

namespace UnitTests
{
    //[TestClass]
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

            //controlVM.SelectedTaxPlanName = "Anything else";

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

        [TestMethod]
        public void YangUBIChecked()
        {

            OutputViewModel outputVM = new OutputViewModel();
            OptionsModel optionsModel = new OptionsModel();

            outputVM.OptionsModel = optionsModel;

            /*
                Test unchecking ability
            */

            // things which should uncheck
            outputVM.UBIChecked = true;
            outputVM.MedicaidSpendingChecked = true;
            outputVM.WelfareSpendingChecked = true;
            outputVM.FoodStampsSpendingChecked = true;
            outputVM.UnemploymentSpendingChecked = true;

            // our interested check
            outputVM.YangUbiSpendingChecked = true;

            // The one we want to be checked
            Assert.IsTrue(outputVM.YangUbiSpendingChecked);

            // The ones that should not be checked
            Assert.IsFalse(outputVM.UBIChecked);
            Assert.IsFalse(outputVM.MedicaidSpendingChecked);
            Assert.IsFalse(outputVM.WelfareSpendingChecked);
            Assert.IsFalse(outputVM.FoodStampsSpendingChecked);
            Assert.IsFalse(outputVM.UnemploymentSpendingChecked);

            /*
                Ensure that if they're unchecked, they are left alone
            */

            // reset interestd
            outputVM.YangUbiSpendingChecked = false;

            // things that would be unchecked if they were checked
            outputVM.UBIChecked = false;
            outputVM.MedicaidSpendingChecked = false;
            outputVM.WelfareSpendingChecked = false;
            outputVM.FoodStampsSpendingChecked = false;
            outputVM.UnemploymentSpendingChecked = false;


            // check our interested
            outputVM.YangUbiSpendingChecked = true;

            // The one we want to be checked
            Assert.IsTrue(outputVM.YangUbiSpendingChecked);

            // The ones that should not be checked
            Assert.IsFalse(outputVM.UBIChecked);
            Assert.IsFalse(outputVM.MedicaidSpendingChecked);
            Assert.IsFalse(outputVM.WelfareSpendingChecked);
            Assert.IsFalse(outputVM.FoodStampsSpendingChecked);
            Assert.IsFalse(outputVM.UnemploymentSpendingChecked);


        }

        [TestMethod]
        public void UBIChecked()
        {


            // Initialize objects
            OutputViewModel outputVM = new OutputViewModel();
            OptionsModel optionsModel = new OptionsModel();
            outputVM.OptionsModel = optionsModel;

            /*
                Test unchecking ability
            */

            // things which should uncheck
            outputVM.YangUbiSpendingChecked = true;
            outputVM.MedicaidSpendingChecked = true;
            outputVM.WelfareSpendingChecked = true;
            outputVM.FoodStampsSpendingChecked = true;
            outputVM.UnemploymentSpendingChecked = true;

            // our interested check
            outputVM.UBIChecked = true;
            

            // The one we want to be checked
            Assert.IsTrue(outputVM.UBIChecked);

            // The ones that should not be checked
            Assert.IsFalse(outputVM.YangUbiSpendingChecked);
            Assert.IsFalse(outputVM.MedicaidSpendingChecked);
            Assert.IsFalse(outputVM.WelfareSpendingChecked);
            Assert.IsFalse(outputVM.FoodStampsSpendingChecked);
            Assert.IsFalse(outputVM.UnemploymentSpendingChecked);

            /*
                Ensure that if they're unchecked, they are left alone
            */

            // reset interestd
            outputVM.UBIChecked = false;

            // things that would be unchecked if they were checked
            outputVM.YangUbiSpendingChecked = false;
            outputVM.MedicaidSpendingChecked = false;
            outputVM.WelfareSpendingChecked = false;
            outputVM.FoodStampsSpendingChecked = false;
            outputVM.UnemploymentSpendingChecked = false;


            // check our interested
            outputVM.UBIChecked = true;

            // The one we want to be checked
            Assert.IsTrue(outputVM.UBIChecked);

            // The ones that should not be checked
            Assert.IsFalse(outputVM.YangUbiSpendingChecked);
            Assert.IsFalse(outputVM.MedicaidSpendingChecked);
            Assert.IsFalse(outputVM.WelfareSpendingChecked);
            Assert.IsFalse(outputVM.FoodStampsSpendingChecked);
            Assert.IsFalse(outputVM.UnemploymentSpendingChecked);


        }

    }
}
