namespace Bars.GkhGji.Regions.Tatarstan.Services
{
    using Bars.Gkh.Services.ServiceContracts.Mail;
    using Bars.GkhGji.Regions.Tatarstan.Models;

    /// <summary>
    /// Провайдер электронной почты для оповещения о новых обращениях в СОПР
    /// </summary>
    public interface IRapidResponseSystemAppealMailProvider : IMailProvider<RapidResponseAppealMailInfo>
    {
        
    }
}