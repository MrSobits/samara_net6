using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Utils;
using Bars.GkhCr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Bars.Gkh.Overhaul.Hmao.Task
{
    public class BindKPKRandDPKRTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public string ExecutorCode { get; private set; }

        public IDomainService<TypeWorkCrVersionStage1> CrStage1Domain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public IDomainService<StructuralElementWork> StructuralElementWorkDomain { get; set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var isMain = @params.Params.GetAs<string>("IsMain") == "on";

            var versions = ProgramVersionDomain.GetAll()
                .WhereIf(isMain, x => x.IsMain)
                .ToArray();

            //дробление по версиям
            for(int i = 0; i < versions.Count(); i++)
            { 
                indicator?.Report(null, (uint)(i * 100 / versions.Count()), versions[i].Name);

                Process(versions[i]);
            }

            return new BaseDataResult();
        }

        private void Process(ProgramVersion version)
        {
            //id stage1 со связкой
            var crStage1Ids = CrStage1Domain.GetAll()
                .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.Stage1Version.VersionRecordState != Enum.VersionRecordState.NonActual)
                .Where(x => !x.Stage1Version.StructuralElement.State.FinalState)
                .Where(x => x.Stage1Version.StructuralElement.StructuralElement != null)
                .Select(x => x.Stage1Version.Id)
                .ToHashSet();

            //id stage1 без связки
            var stages1 = Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.VersionRecordState != Enum.VersionRecordState.NonActual)
                .Where(x => !x.StructuralElement.State.FinalState)
                .Where(x => x.StructuralElement.StructuralElement != null)
                .AsEnumerable()
                .Where(x => !crStage1Ids.Contains(x.StructuralElement.StructuralElement.Id));                

            foreach(var stage1 in stages1)
            {
                var kpkr = GetSt1TypeWork(stage1);
                if (kpkr == null)
                    continue;

                //создаем связку
                CrStage1Domain.Save(new TypeWorkCrVersionStage1
                {
                    Stage1Version = stage1,
                    TypeWorkCr = kpkr,
                    UnitMeasure = kpkr.Work.UnitMeasure,
                    Volume = stage1.Volume,
                    Sum = stage1.Sum
                });
            }
        }

        private TypeWorkCr GetSt1TypeWork(VersionRecordStage1 stage1)
        {
            var houseId = stage1.RealityObject.Id;
            var jobsId = GetJobs(stage1.StructuralElement.StructuralElement.Id);
            var year = stage1.Year;
            var sum = stage1.Sum;

            //поиск по всем четырем
            var typeworks = TypeWorkCrDomain.GetAll()
                            .Where(x => x.ObjectCr.RealityObject.Id == houseId)
                            .Where(x => jobsId.Contains(x.Work.Id))
                            .Where(x => x.YearRepair == year)
                            .Where(x => x.Sum == sum)
                            .ToArray();

            if (typeworks.Length > 0)
                return typeworks[0];

            //если не нашли, по трем
            typeworks = TypeWorkCrDomain.GetAll()
                            .Where(x => x.ObjectCr.RealityObject.Id == houseId)
                            .Where(x => jobsId.Contains(x.Work.Id))
                            .Where(x => x.YearRepair == year)
                            .ToArray();

            if (typeworks.Length > 0)
                return typeworks[0];

            //если не нашли, по двум
           typeworks = TypeWorkCrDomain.GetAll()
                            .Where(x => x.ObjectCr.RealityObject.Id == houseId)
                            .Where(x => jobsId.Contains(x.Work.Id))
                            .ToArray();

            if (typeworks.Length > 0)
                return typeworks[0];

            return null;
        }

        private List<long> GetJobs(long structuralElementId)
        {
            return StructuralElementWorkDomain.GetAll()
                .Where(x => x.StructuralElement.Id == structuralElementId)
                .Select(x => x.Job.Work.Id)
                .ToList();
        }
    }
}
