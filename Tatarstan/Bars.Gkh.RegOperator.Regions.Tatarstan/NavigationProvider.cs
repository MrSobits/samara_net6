// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Меню, навигация
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.Gkh.RegOperator.Regions.Tatarstan
{
    using Bars.B4;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {
            var dict = root.Add("Региональный фонд").Add("Использование регионального фонда");
            dict.Add("Документы, подтверждающие поступление взносов на КР", "confirmContributionDoc")
                .AddRequiredPermission("GkhRegOp.RegionalFundUse.ConfirmContributionDocs.View");

            var node = root.Add("Региональный фонд").Add("Формирование регионального фонда");
            node.Add("Реестр договоров с управляющими организациями", "contractrf").AddRequiredPermission("GkhRf.ContractRf.View").WithIcon("contractRf");
            node.Add("Реестр оплат капитального ремонта", "payment").AddRequiredPermission("GkhRf.Payment.View").WithIcon("payment");
            node.Add("Информация о перечислениях средств в фонд", "transferrf").AddRequiredPermission("GkhRf.TransferRf.View").WithIcon("transferRf");
            node.Add("Перечисления средств в фонд", "transferfunds").AddRequiredPermission("GkhRf.TransferFunds.View").WithIcon("transferRf");


            node = root.Add("Региональный фонд").Add("Использование регионального фонда");
            node.Add("Заявки на перечисление денежных средств", "requesttransferrf")
                .AddRequiredPermission("GkhRf.RequestTransferRfViewCreate.View")
                .WithIcon("requestTransferRf");

            root.Add("Региональный фонд").Add("Использование регионального фонда")
                .Add("Настройки проверки на наличие лимитов", "limitcheck")
                .AddRequiredPermission("GkhRf.LimitCheck.View");
        }
    }
}
