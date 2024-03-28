using Autotests.WebPages.Root;
using Autotests.WebPages.Root.Housing_Inspectorate;

namespace Autotests.WebPages
{
    public static class Pages
    {
        public static Login Login
        {
            get { return new Login(); } 
        }

        public static class Manager
        {
            public static class Websites
            {
            }
        }
        public static Regop_Personal_Account RegionalFund
        {
            get { return new Regop_Personal_Account(); }
        }

        public static Unaccepted_Charges UnacceptingCharges
        {
            get { return new Unaccepted_Charges(); }
        }

        public static Cp_Checking CheckingAndClosingTheMonth
        {
            get { return new Cp_Checking(); }
        }

        public static ReportPanel ReportPanel
        {
            get { return new ReportPanel(); }
        }

        public static AppealToRequestLicense AppealToRequestLicense
        {
            get { return new AppealToRequestLicense(); }
        }
        public static ResolutionOfProsecutors ResolutionOfProsecutors
        {
            get { return new ResolutionOfProsecutors(); }
        }
        public static RegisterOfAplications RegisterOfAplications
        {
            get { return new RegisterOfAplications(); }
        }

        public static Charges_Period ChargesPeriod
        {
            get { return new Charges_Period(); }
        }

        public static Menu Menu
        {
            get { return new Menu();}
        }

        public static Bank_Doc_Import BankDocImport
        {
            get { return new Bank_Doc_Import(); }
        }

        public static Bank_Statement BankStatement
        {
            get { return new Bank_Statement();}
        }
        public static BaseStatement BaseStatement
        {
            get { return new BaseStatement(); }
        }

        public static Dpkr Dpkr
        {
            get { return new Dpkr();}
        }

        public static Subsidy Subsidy
        {
            get { return new Subsidy();}
        }

        public static Programcr Programcr
        {
            get { return new Programcr();}
        }
    }
}
