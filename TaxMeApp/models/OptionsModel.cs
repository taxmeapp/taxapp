﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxMeApp.models
{
    public class OptionsModel
    {
        public OptionsModel()
        {
            this.MaxTaxRate = 0;
            this.MaxBracketCount = 0;

            listOfCosts = new List<(int priority, bool ischecked, string name, double cost, double tFunding)>();
            listOfCosts.Add((0, true, "defense", 678000000000.0, 100.0));
            listOfCosts.Add((1, true, "medicaid", 613000000000.0, 100.0));
            listOfCosts.Add((2, true, "welfare", 361000000000.0, 100.0));
            listOfCosts.Add((3, true, "veterans", 79000000000.0, 100.0));
            listOfCosts.Add((4, true, "foodstamps", 68000000000.0, 100.0));
            listOfCosts.Add((5, true, "education", 61000000000.0, 100.0));
            listOfCosts.Add((6, true, "publichousing", 56000000000.0, 100.0));
            listOfCosts.Add((7, true, "health", 53000000000.0, 100.0));
            listOfCosts.Add((8, true, "science", 30000000000.0, 100.0));
            listOfCosts.Add((9, true, "transportation", 29000000000.0, 100.0));
            listOfCosts.Add((10, true, "international", 29000000000.0, 100.0));
            listOfCosts.Add((11, true, "energyandenvironment", 28000000000.0, 100.0));
            listOfCosts.Add((12, true, "unemployment", 24000000000.0, 100.0));
            listOfCosts.Add((13, true, "foodandagriculture", 11000000000.0, 100.0));
            
            listOfCosts.Add((14, false, "sanderscollege", 220000000000.0, 100.0));
            listOfCosts.Add((15, false, "sandersmedicaid", 350000000000.0, 100.0));
            
            listOfCosts.Add((16, false, "yangubi", 2800000000000.0, 100.0));

            this.DefenseChecked = true;
            this.MedicaidChecked = true;
            this.WelfareChecked = true;
            this.VeteransChecked = true;
            this.FoodStampsChecked = true;
            this.EducationChecked = true;
            this.PublicHousingChecked = true;
            this.HealthChecked = true;
            this.ScienceChecked = true;
            this.TransportationChecked = true;
            this.InternationalChecked = true;
            this.EnergyAndEnvironmentChecked = true;
            this.UnemploymentChecked = true;
            this.FoodAndAgricultureChecked = true;
            
            this.SandersCollegeChecked = false;
            this.SandersMedicaidChecked = false;

            this.YangUbiChecked = false;
            this.YangRemoveChecked = false;

            fundingArray = new double[listOfCosts.Count];
        }

        public double MaxTaxRate { get; set; }
        public int MaxBracketCount { get; set; }

        public long revenue { get; set; } = 0;
        public List<(int priority, bool ischecked, string name, double cost, double tFunding)> listOfCosts;
        public double[] fundingArray;

        public bool dc;
        public bool DefenseChecked {
            get {
                return dc;
            } 
            set {
                dc = value;
                listOfCosts[0] = (0, value, "defense", 678000000000.0, listOfCosts[0].tFunding);
            } 
        }
        public bool mc;
        public bool MedicaidChecked
        {
            get
            {
                return mc;
            }
            set
            {
                mc = value;
                listOfCosts[1] = (1, value, "medicaid", 613000000000.0, listOfCosts[1].tFunding);
            }
        }

        public bool wc;
        public bool WelfareChecked
        {
            get
            {
                return wc;
            }
            set
            {
                wc = value;
                listOfCosts[2] = (2, value, "welfare", 361000000000.0, listOfCosts[2].tFunding);
            }
        }

        public bool vc;
        public bool VeteransChecked
        {
            get
            {
                return vc;
            }
            set
            {
                vc = value;
                listOfCosts[3] = (3, value, "veterans", 79000000000.0, listOfCosts[3].tFunding);
            }
        }

        public bool fsc;
        public bool FoodStampsChecked
        {
            get
            {
                return fsc;
            }
            set
            {
                fsc = value;
                listOfCosts[4] = (4, value, "foodstamps", 68000000000.0, listOfCosts[4].tFunding);
            }
        }
        public bool edc;
        public bool EducationChecked
        {
            get
            {
                return edc;
            }
            set
            {
                edc = value;
                listOfCosts[5] = (5, value, "education", 61000000000.0, listOfCosts[5].tFunding);
            }
        }
        public bool phc;
        public bool PublicHousingChecked
        {
            get
            {
                return phc;
            }
            set
            {
                phc = value;
                listOfCosts[6] = (6, value, "publichousing", 56000000000.0, listOfCosts[6].tFunding);
            }
        }

        public bool hltc;
        public bool HealthChecked
        {
            get
            {
                return hltc;
            }
            set
            {
                hltc = value;
                listOfCosts[7] = (7, value, "health", 53000000000.0, listOfCosts[7].tFunding);
            }
        }

        public bool scic;
        public bool ScienceChecked
        {
            get
            {
                return scic;
            }
            set
            {
                scic = value;
                listOfCosts[8] = (8, value, "science", 30000000000.0, listOfCosts[8].tFunding);
            }
        }

        public bool trpc;
        public bool TransportationChecked
        {
            get
            {
                return trpc;
            }
            set
            {
                trpc = value;
                listOfCosts[9] = (9, value, "transportation", 29000000000.0, listOfCosts[9].tFunding);
            }
        }

        public bool intc;
        public bool InternationalChecked
        {
            get
            {
                return intc;
            }
            set
            {
                intc = value;
                listOfCosts[10] = (10, value, "international", 29000000000.0, listOfCosts[10].tFunding);
            }
        }

        public bool eec;
        public bool EnergyAndEnvironmentChecked
        {
            get
            {
                return eec;
            }
            set
            {
                eec = value;
                listOfCosts[11] = (11, value, "transportation", 28000000000.0, listOfCosts[11].tFunding);
            }
        }
        public bool unc;
        public bool UnemploymentChecked
        {
            get
            {
                return unc;
            }
            set
            {
                unc = value;
                listOfCosts[12] = (12, value, "unemployment", 24000000000.0, listOfCosts[12].tFunding);
            }
        }

        public bool fac;
        public bool FoodAndAgricultureChecked
        {
            get
            {
                return fac;
            }
            set
            {
                fac = value;
                listOfCosts[13] = (13, value, "foodandagriculture", 11000000000.0, listOfCosts[13].tFunding);
            }
        }

        public bool scc;
        public bool SandersCollegeChecked
        {
            get
            {
                return scc;
            }
            set
            {
                scc = value;
                listOfCosts[14] = (14, value, "sanderscollege", 220000000000.0, listOfCosts[14].tFunding);
            }
        }

        public bool smc;
        public bool SandersMedicaidChecked
        {
            get
            {
                return smc;
            }
            set
            {
                smc = value;
                listOfCosts[15] = (15, value, "sandersmedicaid", 350000000000.0, listOfCosts[15].tFunding);
            }
        }

        public bool yubic;
        public bool YangUbiChecked
        {
            get
            {
                return yubic;
            }
            set
            {
                yubic = value;
                listOfCosts[16] = (16, value, "yangubi", 2800000000000.0, listOfCosts[16].tFunding);
            }
        }

        public bool yremove;
        public bool YangRemoveChecked
        {
            get
            {
                return yremove;
            }
            set
            {
                yremove = value;
            }
        }

        public void updateFunding() {
            fundingArray = new double[listOfCosts.Count];
            double funding = 0;
            for (int i = 0; i < listOfCosts.Count; i++) {
                if (listOfCosts[i].ischecked)
                {
                    if (revenue > 0)
                    {
                        funding = revenue / (listOfCosts[i].cost * (listOfCosts[i].tFunding / 100));
                        if (funding >= 1)
                        {
                            funding = 1;
                            revenue -= long.Parse((listOfCosts[i].cost * (listOfCosts[i].tFunding / 100)).ToString());
                        }
                        else
                        {
                            revenue = 0;
                        }
                        funding *= 100;
                    }
                    else
                    {
                        funding = 0;
                    }
                }
                else {
                    funding = 0;
                }
                fundingArray[i] = funding;
            }
        }

        public string GetDefenseFunding()
        {
            return this.fundingArray[0].ToString("0.0") + "% Funded";
        }
        public string GetMedicaidFunding()
        {
            return this.fundingArray[1].ToString("0.0") + "% Funded";
        }
        public string GetWelfareFunding()
        {
            return this.fundingArray[2].ToString("0.0") + "% Funded";
        }
        public string GetVeteransFunding()
        {
            return this.fundingArray[3].ToString("0.0") + "% Funded";
        }
        public string GetFoodStampsFunding()
        {
            return this.fundingArray[4].ToString("0.0") + "% Funded";
        }
        public string GetEducationFunding()
        {
            return this.fundingArray[5].ToString("0.0") + "% Funded";
        }
        public string GetPublicHousingFunding()
        {
            return this.fundingArray[6].ToString("0.0") + "% Funded";
        }
        public string GetHealthFunding()
        {
            return this.fundingArray[7].ToString("0.0") + "% Funded";
        }
        public string GetScienceFunding()
        {
            return this.fundingArray[8].ToString("0.0") + "% Funded";
        }
        public string GetTransportationFunding()
        {
            return this.fundingArray[9].ToString("0.0") + "% Funded";
        }
        public string GetInternationalFunding()
        {
            return this.fundingArray[10].ToString("0.0") + "% Funded";
        }
        public string GetEnergyAndEnvironmentFunding()
        {
            return this.fundingArray[11].ToString("0.0") + "% Funded";
        }
        public string GetUnemploymentFunding()
        {
            return this.fundingArray[12].ToString("0.0") + "% Funded";
        }
        public string GetFoodAndAgricultureFunding()
        {
            return this.fundingArray[13].ToString("0.0") + "% Funded";
        }
        public string GetSandersCollegeFunding()
        {
            return this.fundingArray[14].ToString("0.0") + "% Funded";
        }
        public string GetSandersMedicaidFunding()
        {
            return this.fundingArray[15].ToString("0.0") + "% Funded";
        }
        public string GetYangUbiFunding()
        {
            return this.fundingArray[16].ToString("0.0") + "% Funded";
        }
        public double GetTotalBudget() {
            double ans = 0;
            for (int i = 0; i < listOfCosts.Count; i++) {
                if (listOfCosts[i].ischecked) {
                    ans += listOfCosts[i].cost * (listOfCosts[i].tFunding / 100);
                }
            }
            return ans;
        }

        public List<string> GetGovProgramList() {
            List<string> ans = new List<string>();
            for (int i = 0; i < listOfCosts.Count; i++) {
                ans.Add(listOfCosts[i].name);
            }
            return ans;
        }
        public string SelectedGovProgram { get; set; } = "defense";

        public double GetSelectedTargetFunding(int i) {
            return listOfCosts[i].tFunding;
        }
    }
}