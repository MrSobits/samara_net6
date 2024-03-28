namespace Bars.Gkh.Gis.Controllers.PersonalAccount
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Entities.PersonalAccount;

    public class PersonalAccountSaldoController : BaseController
    {
        public ActionResult List(IDomainService<PersonalAccountSaldo> domainService, BaseParams baseParams)
        {
            throw new NotImplementedException("Метод устарел, не работает в данной архитектуре!");
        }
    }
}
