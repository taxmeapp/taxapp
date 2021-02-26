using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TaxMeApp.viewmodels;

namespace TaxMeApp.Plans
{
    class PlanLoader
    {

        public static bool LoadPlans(ControlViewModel controlVM)
        {
            string userPlanPath = @".\userPlans\";
            string[] plans = Directory.GetFiles(userPlanPath, "*.xml");

            foreach (string plan in plans)
            {
                XmlDocument planXML = new XmlDocument();
                planXML.Load(plan);

                string planName = planXML.GetElementsByTagName("Plan")[0].Attributes[0].Value;

                //Trace.WriteLine(planName);

                ObservableCollection<double> rateList = new ObservableCollection<double>();

                foreach (XmlNode node in planXML.GetElementsByTagName("Rate"))
                {
                    if (Double.TryParse(node.InnerText, out double value))
                    {
                        rateList.Add(value);
                    }
                    else
                    {
                        rateList.Add(0);
                    }
                    
                }

                controlVM.CreateTaxPlan(planName, rateList);

            }

            return true;

        }

    }
}
