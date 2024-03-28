namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
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
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using Castle.Windsor;

    public class PublishedProgramService : IPublishProgramService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<PublishedProgram> PublishedProgramDomain { get; set; }
        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public IDataResult PublishedProgramMunicipalityList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();

            var publProgMunicipalityIds = this.PublishedProgramDomain.GetAll().Select(x => x.ProgramVersion.Municipality.Id).Distinct().ToArray();

            var data = this.MunicipalityDomain.GetAll()
                .Where(x => publProgMunicipalityIds.Contains(x.Id))
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                .Where(x => x.ParentMo == null)
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
        }

        public IQueryable<PublishedProgramRecord> GetPublishedProgramRecords(PublishedProgram program)
        {
            var service = this.Container.Resolve<IDomainService<PublishedProgramRecord>>();
            try
            {
                return service.GetAll()
                    .Where(x => x.PublishedProgram.Id == program.Id)
                    .OrderBy(x => x.IndexNumber)
                    .AsQueryable();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public IDataResult DeletePublishedProgram(BaseParams baseParams)
        {
            var publishDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();

            var programId = baseParams.Params.GetAsId("programId");

            PublishedProgram program;

            using (this.Container.Using(publishDomain))
            {
                program = publishDomain.Get(programId);
            }

            if (program == null)
            {
                return BaseDataResult.Error("Программа не найдена");
            }

            if (program.State.FinalState)
            {
                return new BaseDataResult(false, "Программа не может быть удалена, т.к. у нее конечный статус.");
            }

            using (var provider = this.Container.Resolve<ISessionProvider>())
            {
                var session = provider.OpenStatelessSession();
                using (var transaction = session.BeginTransaction())
                {
                    session.CreateSQLQuery($"delete from OVRHL_PUBLISH_PRG_REC where PUBLISH_PRG_ID = {programId};")
                        .ExecuteUpdate();

                    session.CreateSQLQuery($"delete from OVRHL_PUBLISH_PRG where id = {programId};")
                        .ExecuteUpdate();

                    transaction.Commit();

                    return new BaseDataResult();
                }
            }
        }

        public IDataResult GetPublishedProgram(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.Resolve<IDomainService<ProgramVersion>>();
            var publishedPrgDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();

            try
            {
                var muId = baseParams.Params.GetAs<long>("muId");

                var version = programVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == muId);

                if (version == null)
                {
                    return new BaseDataResult(false, "Для выбранного муниципального образования не задана основная версия");
                }

                var publish = publishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

                if (publish == null)
                {
                    return new BaseDataResult(false, "Для основной версии не существует опубликованной программы");
                }

                return new BaseDataResult(publish);
            }
            finally 
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(publishedPrgDomain);
            }
        }

        public IDataResult GetValidationForCreatePublishProgram(BaseParams baseParams)
        {
            var programVersionDomain = this.Container.Resolve<IDomainService<ProgramVersion>>();
            var publishedPrgDomain = this.Container.Resolve<IDomainService<PublishedProgram>>();

            try
            {
                var moId = baseParams.Params.GetAs<long>("mo_id");

                if (moId <= 0)
                {
                    return new BaseDataResult(false, "Не удалось получить муниципальное образование");
                }

                var version = programVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == moId);

                if (version == null)
                {
                    return new BaseDataResult(false, "Не задана основная версия");
                }

                var publish = publishedPrgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == version.Id);

                if (publish != null && publish.State.FinalState)
                {
                    return new BaseDataResult(false, "Опубликованная программа уже утверждена");
                }

                return new BaseDataResult(true, "Вы уверены, что хотите опубликовать программу?");
            }
            finally
            {
                this.Container.Release(programVersionDomain);
                this.Container.Release(publishedPrgDomain);
            }
        }
    }
}