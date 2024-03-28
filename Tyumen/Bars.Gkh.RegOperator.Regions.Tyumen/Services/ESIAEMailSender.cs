using Bars.B4;
using Bars.Gkh.Helpers;
using Bars.Gkh.RegOperator.Regions.Tyumen.Entities;
using System.Linq;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Services
{
    internal class ESIAEMailSender : IESIAEMailSender
    {
        public IDomainService<RequestStatePerson> RequestStatePersonDomain { get; set; }
        public IEMailSender EMailSender { get; set; }

        public void SendEmail(string orgname, string ogrn, string name, string login, string password)
        {
            string message = $"Уведомляем Вас, что в системе зарегистрирована новая организация c названием {orgname} и ОГРН {ogrn}.\n" +
                             $"Для данной организации создана учетная запись с именем {name}, логином {login}\n\n" +
                             $"Просим вас назначить этой учетной записи соответствующую роль.\n";

            var emailList = RequestStatePersonDomain.GetAll()
                .Where(x => x.Status == Enums.RequestStatePersonEnum.Esia);

            foreach (RequestStatePerson rsp in emailList)
            {
                EMailSender.Send(rsp.Email,
                                 $"Уважаемый(ая) {rsp.Name}, поступил запрос доступа на редактирование от пользователя",
                                 message);
            }
        }
    }
}
