namespace Bars.Gkh1468.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh.Entities;
    
    public class Integration : BaseController
    {
        public ActionResult Execute()
        {
            var housePasspServ = Container.Resolve<IHousePassportService>();
            var userIdent = Container.Resolve<IUserIdentity>();

            if (userIdent == null)
            {
                return JsonNetResult.Failure("Не удалось получить список разрешенных Вам муниципальных образований для выполнения запроса");
            }

            var operatorDomain = Container.Resolve<IDomainService<Operator>>();
            var op = operatorDomain.GetAll().FirstOrDefault(x => x.User.Id == userIdent.UserId);

            if (op == null)
            {
                return JsonNetResult.Failure("Не удалось получить список разрешенных Вам муниципальных образований для выполнения запроса");
            }

            var opMuDomain = Container.Resolve<IDomainService<OperatorMunicipality>>();

            var mus = opMuDomain.GetAll()
                .Where(x => x.Operator.Id == op.Id)
                .Select(x => x.Municipality)
                .ToList();

            var curDate = DateTime.Now;

            foreach (var mu in mus)
            {
                //housePasspServ.FillPassport(mu, curDate.Year, curDate.Month);
            }

            return JsonNetResult.Success;
        }
    }
}