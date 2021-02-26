using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TaxMeApp.Plans
{
    class PlanSaver
    {

        public static bool SavePlan(string name, Dictionary<string, object> values)
        {

            // Entirely not flexible, completely reliant on the tax brackets not changing
            // But good enough to get us through the presentation 3

            string userPlanPath = @".\userPlans\";

            Directory.CreateDirectory(userPlanPath);

            if (File.Exists(userPlanPath + name + ".xml"))
            {
                try
                {
                    File.Delete(userPlanPath + name + ".xml");
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Failure when overwriting plan: " + e.Message);
                    return false;
                }

            }

            XmlDocument planXML = new XmlDocument();

            XmlElement plan = planXML.CreateElement(String.Empty, "Plan", String.Empty);
            plan.SetAttribute("name", name);
            planXML.AppendChild(plan);



            foreach (var value in values)
            {
                // Just handle TaxRates for now
                if (value.Key.Equals("TaxRates"))
                {

                    XmlElement taxRates = planXML.CreateElement(String.Empty, "TaxRates", String.Empty);

                    foreach (var rate in (ObservableCollection<double>)value.Value)
                    {
                        //Trace.WriteLine(rate);
                        XmlElement rateXML = planXML.CreateElement(String.Empty, "Rate", String.Empty);
                        XmlText rateText = planXML.CreateTextNode(rate.ToString());
                        rateXML.AppendChild(rateText);
                        taxRates.AppendChild(rateXML);
                    }

                    plan.AppendChild(taxRates);
                }

            }

            planXML.Save(userPlanPath + name + ".xml");

            return true;

        }

        public static void DeletePlan(string name)
        {
            string userPlanPath = @".\userPlans\";

            if (File.Exists(userPlanPath + name + ".xml"))
            {
                try
                {
                    File.Delete(userPlanPath + name + ".xml");
                    Trace.WriteLine("Deleted");
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }
            else
            {
                Trace.WriteLine(name + "file don't be exist");

            }


        }



    }
}
