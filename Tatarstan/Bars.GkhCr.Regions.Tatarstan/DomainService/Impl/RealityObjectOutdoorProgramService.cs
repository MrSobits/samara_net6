namespace Bars.GkhCr.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;

    using Castle.Windsor;

    public class RealityObjectOutdoorProgramService : IRealityObjectOutdoorProgramService
    {

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult CopyProgram(BaseParams baseParams)
        {
            var oldProgramId = baseParams.Params.GetAs<long>("oldOutdoorProgramId");

            var outdoorProgramDomain = this.Container.ResolveDomain<RealityObjectOutdoorProgram>();

            using (this.Container.Using(outdoorProgramDomain))
            {
                var oldProgram = outdoorProgramDomain.Get(oldProgramId);
                return oldProgram == null 
                    ? new BaseDataResult(false, "Не удалось получить программу благоустройства двора") 
                    : this.BaseCopyProgram(oldProgram, outdoorProgramDomain, baseParams);
            }
        }

        /// <inheritdoc />
        public IDataResult GetProgramNames(BaseParams baseParams)
        {
            var domainService = this.Container.ResolveDomain<RealityObjectOutdoorProgram>();
            using (this.Container.Using(domainService))
            {
                return domainService.GetAll()
                    .Where(x => x.TypeVisibilityProgram == TypeVisibilityProgramCr.Full)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        private IDataResult BaseCopyProgram(RealityObjectOutdoorProgram oldProgram, 
            IDomainService<RealityObjectOutdoorProgram> outdoorProgramDomain, BaseParams baseParams)
        {
            var fileService = this.Container.Resolve<IFileService>();
            using (this.Container.Using(fileService))
            {
                var program = new RealityObjectOutdoorProgram
                {
                    Name = baseParams.Params["Name"].ToStr(),
                    Code = baseParams.Params["Code"].ToStr(),
                    Period = new Period { Id = baseParams.Params["Period"].ToLong() },
                    TypeVisibilityProgram = baseParams.Params["Visible"].To<TypeVisibilityProgramCr>(),
                    TypeProgram = baseParams.Params["Type"].To<TypeProgramCr>(),
                    TypeProgramState = baseParams.Params["State"].To<TypeProgramStateCr>(),
                    IsNotAddOutdoor = baseParams.Params["IsNotAddOutdoor"].ToBool(),
                    Description = baseParams.Params["Description"].ToStr(),
                    File = fileService.ReCreateFile(oldProgram.File),
                    NormativeDoc = oldProgram.NormativeDoc,
                    GovernmentCustomer = oldProgram.GovernmentCustomer,
                    DocumentNumber = oldProgram.DocumentNumber,
                    DocumentDate = oldProgram.DocumentDate,
                    DocumentDepartment = oldProgram.DocumentDepartment
                };

                outdoorProgramDomain.Save(program);
                this.SaveChangeJournal(program, oldProgram.Name);

                return new BaseDataResult { Success = true };
            }
        }

        /// <summary>
        /// Сохраняет запись в журнал.
        /// </summary>
        private void SaveChangeJournal(RealityObjectOutdoorProgram program, string oldProgramName)
        {
            var journalDomain = this.Container.ResolveDomain<RealityObjectOutdoorProgramChangeJournal>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            using (this.Container.Using(journalDomain, userManager))
            {
                var activeUser = userManager.GetActiveUser();
                journalDomain.Save(new RealityObjectOutdoorProgramChangeJournal
                {
                    RealityObjectOutdoorProgram = program,
                    ChangeDate = DateTime.Now,
                    UserName = activeUser == null ? "Администратор" : activeUser.Name,
                    Description = $"Скопирована с {oldProgramName}"
                });
            }
        }
    }
}
