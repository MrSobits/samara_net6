namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;

    using Entities;

    public class ProgramVersionInterceptor : EmptyDomainInterceptor<ProgramVersion>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            return this.CheckVersions(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            return this.CheckVersions(service, entity);
        }

        // Внимание в связи с огромным количеством удаляемых объектов Удаление произходит в файле ProgramVersionDomainService
        // Путем переопределения метода Delete 
        public override IDataResult BeforeDeleteAction(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            /*
            var versionParamService = Container.Resolve<IDomainService<VersionParam>>();
            var versionRecordService = Container.Resolve<IDomainService<VersionRecord>>();
            var versionRecStage2Service = Container.Resolve<IDomainService<VersionRecordStage2>>();
            var versionRecStage1Service = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var subsidyService = Container.Resolve<IDomainService<SubsidyRecordVersionData>>();
            var subsidyRecService = Container.Resolve<IDomainService<SubsidyRecordVersion>>();
            var shortProgramDifitsitService = Container.Resolve<IDomainService<ShortProgramDifitsit>>();

            subsidyService.GetAll().Where(x => x.Version.Id == entity.Id).Select(x => x.Id).ForEach(x => subsidyService.Delete(x));
            subsidyRecService.GetAll().Where(x => x.Version.Id == entity.Id).Select(x => x.Id).ForEach(x => subsidyRecService.Delete(x));
            versionRecStage1Service.GetAll().Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == entity.Id).Select(x => x.Id).ForEach(x => versionRecStage1Service.Delete(x));
            versionRecStage2Service.GetAll().Where(x => x.Stage3Version.ProgramVersion.Id == entity.Id).Select(x => x.Id).ForEach(x => versionRecStage2Service.Delete(x));
            versionRecordService.GetAll().Where(x => x.ProgramVersion.Id == entity.Id).Select(x => x.Id).ForEach(x => versionRecordService.Delete(x));
            versionParamService.GetAll().Where(x => x.ProgramVersion.Id == entity.Id).Select(x => x.Id).ForEach(x => versionParamService.Delete(x));
            shortProgramDifitsitService.GetAll().Where(x => x.Version.Id == entity.Id).Select(x => x.Id).ForEach(x => shortProgramDifitsitService.Delete(x));
            */

            return base.BeforeDeleteAction(service, entity);
        }

        private BaseDataResult CheckVersions(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            if (entity.IsMain && service.GetAll().Any(x => x.IsMain && x.Id != entity.Id))
            {
                return
                    Failure(
                            "Основная версия программы уже выбрана! Уберите отметку \"Основная\" у старой версии и после этого выберите новую основную версию");
            }

            return this.Success();
        }
    }
}