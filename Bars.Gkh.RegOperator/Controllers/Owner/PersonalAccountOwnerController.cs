namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Entities;

    public class PersonalAccountOwnerController : B4.Alt.DataController<PersonalAccountOwner>
    {
        private readonly IPersonalAccountOwnerRepository _repository;

        public PersonalAccountOwnerController(IPersonalAccountOwnerRepository repository)
        {
            _repository = repository;
        }

        public ActionResult ListLegalOwners(BaseParams @params)
        {
            return new JsonNetResult(_repository.ListLegalOwners(@params));
        }
    }
}