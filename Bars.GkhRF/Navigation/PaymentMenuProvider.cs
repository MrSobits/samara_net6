namespace Bars.GkhRf.Navigation
{
    using Bars.B4;

    public class PaymentMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "Payment";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки оплат рег фонда";
            }
        }

        public void Init(MenuItem root)
        {
            // root.Add("Общие сведения").AddOption("title", "Общие сведения");
            root.Add("Капремонт").AddOption("title", "Капремонт").AddOption("type", "Cr");
            root.Add("Найм рег. фонда").AddOption("title", "Найм рег. фонда").AddOption("type", "HireRegFund");
            root.Add("Капремонт по 185 ФЗ").AddOption("title", "Капремонт по 185 ФЗ").AddOption("type", "Cr185");
            root.Add("Текущий ремонт жилого здания").AddOption("title", "Текущий ремонт жилого здания").AddOption("type", "BuildingCurrentRepair");
            root.Add("Ремонт сан. тех. сетей").AddOption("title", "Ремонт сан. тех. сетей").AddOption("type", "SanitaryEngineeringRepair");
            root.Add("Ремонт сетей центрального отопления").AddOption("title", "Ремонт сетей центрального отопления").AddOption("type", "HeatingRepair");
            root.Add("Ремонт сетей электроснабжения").AddOption("title", "Ремонт сетей электроснабжения").AddOption("type", "ElectricalRepair");
            root.Add("Ремонт жилого здания и внутридомовых сете").AddOption("title", "Ремонт жилого здания и внутридомовых сетей").AddOption("type", "BuildingRepair");
        }
    }
}