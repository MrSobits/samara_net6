namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Overhaul.Entities;

    public class RealityObjectStructuralElementInterceptor : EmptyDomainInterceptor<RealityObjectStructuralElement>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            if (!string.IsNullOrEmpty(entity.StructuralElement.MutuallyExclusiveGroup))
            {
                var mutuallyExclusiveElems =
                    service.GetAll()
                           .Where(x => x.RealityObject.Id == entity.RealityObject.Id && x.StructuralElement.Id != entity.StructuralElement.Id &&
                               x.StructuralElement.MutuallyExclusiveGroup == entity.StructuralElement.MutuallyExclusiveGroup
                               && x.StructuralElement.Group.Id == entity.StructuralElement.Group.Id)
                           .Where(x => x.State.StartState)
                           .Select(x => x.StructuralElement.Name)
                           .ToArray();

                var message = string.Empty;

                if (mutuallyExclusiveElems.Length > 0)
                {
                    message = mutuallyExclusiveElems.Aggregate(
                        message, (current, str) => current + string.Format(" {0}; ", str));
                }

                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format("Выбраны взаимоисключающие конструктивные элементы: {0};{1}", entity.StructuralElement.Name, message);
                    return Failure(message);
                }
            }

            //пересчет износа
            decimal wa = entity.Wearout;
            entity.WearoutActual = wa;
            entity.Wearout = RecalcWearout(entity);

            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            var programService = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();
            var programServiceStage2 = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage2>>();
            var programServiceStage3 = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>();
            var stage1VersionRep = Container.Resolve<IRepository<VersionRecordStage1>>();
            var stage2VersionRep = Container.Resolve<IRepository<VersionRecordStage2>>();
            var stage3VersionRep = Container.Resolve<IRepository<VersionRecord>>();
            var dpkrCorrService = Container.Resolve<IRepository<DpkrCorrectionStage2>>();
            var shortRecService = Container.Resolve<IRepository<ShortProgramRecord>>();

            #region Получаем идентификаторы которые предположительно будут удалены

            var stage1ElementList = programService.GetAll()
                .Where(x => x.StructuralElement.Id == entity.Id)
                .Select(x => new { x.Id, Stage2_Id = (long?)x.Stage2.Id, Stage3_Id = (long?)x.Stage2.Stage3.Id })
                .ToList();

            // получаем идентификаторы 1го этапа на удаление
            var stage1ForDelete = stage1ElementList.Select(x => x.Id).ToList();

            // получаем идентификаторы 2го этапа на удаление
            var stage2ForDelete =
                stage1ElementList.Where(x => x.Stage2_Id.HasValue)
                    .Select(x => x.Stage2_Id.Value)
                    .Distinct()
                    .ToList();

            // получаем идентификаторы 3го этапа на удаление
            var stage3ForDelete =
                stage1ElementList.Where(x => x.Stage2_Id.HasValue)
                    .Select(x => x.Stage3_Id.Value)
                    .Distinct()
                    .ToList();

            var st1VersionRecs = stage1VersionRep.GetAll()
                .Where(x => x.StructuralElement.Id == entity.Id)
                .Select(x => new
                {
                    Stage1Id = x.Id,
                    Stage2Id = x.Stage2Version.Id,
                    Stage3Id = x.Stage2Version.Stage3Version.Id
                })
                .ToList();

            // получаем идентификаторы 1го этапа на удаление
            var stage1VersionForDelete = st1VersionRecs.Select(x => x.Stage1Id).ToList();

            // получаем идентификаторы 2го этапа на удаление
            var stage2VersionForDelete = st1VersionRecs.Select(x => x.Stage2Id).Distinct().ToList();

            // получаем идентификаторы 3го этапа на удаление
            var stage3VersionForDelete = st1VersionRecs.Select(x => x.Stage3Id).Distinct().ToList();

            #endregion

            // Если данный КЭ уже участвует в краткосрочке и Дом утвержден, то удалять нельзя КЭ
            // пуст ьснимают утверждение и заново пробуют удалить
            if (shortRecService.GetAll()
                .Any(x => stage1VersionForDelete.Contains(x.Stage1.Id)
                          && x.ShortProgramObject.State.FinalState))
            {
                throw new ValidationException("Данный конструктивный элемент участвует в краткосрочной программе и дом Утвержден");
            }

            #region собираем информацию по тем записям котоыре предстоит удалить

            // получаем записи 2 этапа якобы предполагая что они будут обновлятся
            var dictStage2 =
                programServiceStage2.GetAll().Where(x => stage2ForDelete.Contains(x.Id)).ToDictionary(x => x.Id);

            // получаем записи 3 этапа якобы предполагая что они будут обновлятся
            var dictStage3 =
                programServiceStage3.GetAll().Where(x => stage3ForDelete.Contains(x.Id)).ToDictionary(x => x.Id);

            // Поскольку у Одного ООИ может быть несколько КЭ соответсвенно
            // несколько ссылок 1го этапа может ссылатся на одну запись 2 этапа 
            // Следовательно требуется выяснить надо ли обновить суммы
            // и надо ли удалить 2 этап
            // Поскольку если есть несколько записей первого этапа ссылающиеся на 2ой этап то удалять нельзя. Только Update
            programService.GetAll()
                .Where(x => stage2ForDelete.Contains(x.Stage2.Id))
                .Where(x => !stage1ForDelete.Contains(x.Id))
                .ToList()
                .GroupBy(x => x.Stage2.Id)
                .ForEach(x =>
                {
                    // Поскольку мы решили что данный Stage2 завязан с другими элементами 1 этапа
                    // то удаляем его из временного массива на удаление (чтобы неудалить его)
                    stage2ForDelete.Remove(x.Key);

                    var st2 = dictStage2[x.Key];
                    st2.Sum = x.Sum(y => y.Sum + y.ServiceCost);
                    programServiceStage2.Update(st2);

                    programServiceStage2.GetAll()
                        .Where(y => y.Stage3.Id == st2.Stage3.Id)
                        .ToList()
                        .GroupBy(y => y.Stage3.Id)
                        .ForEach(y =>
                        {

                            // Поскольку мы решили что данный Stage3 завязан с другими элементами 1 этапа
                            // то удаляем его из временного массива на удаление (чтобы неудалить его)
                            stage3ForDelete.Remove(y.Key);

                            var st3 = dictStage3[y.Key];
                            st3.Sum = y.Sum(i => i.Sum);
                            programServiceStage3.Update(st3);
                        });
                });

            #endregion

            #region Удаляем записи версии

            // получаем записи 2 этапа якобы предполагая что они будут обновлятся
            var dictStage2Version =
                stage2VersionRep.GetAll().Where(x => stage2VersionForDelete.Contains(x.Id)).ToDictionary(x => x.Id);

            // получаем записи 3 этапа якобы предполагая что они будут обновлятся
            var dictStage3Version =
                stage3VersionRep.GetAll().Where(x => stage3VersionForDelete.Contains(x.Id)).ToDictionary(x => x.Id);

            // С версиями поступаем аналогичным образом
            // Суммы также надо пересчитывать, поскольку пользовтели могут нажать Скопировать из Версии
            // И все суммы подтянутся
            stage1VersionRep.GetAll()
                .Where(x => stage2VersionForDelete.Contains(x.Stage2Version.Id))
                .Where(x => !stage1VersionForDelete.Contains(x.Id))
                .ToList()
                .GroupBy(x => x.Stage2Version.Id)
                .ForEach(x =>
                {
                    // Поскольку мы решили что данный Stage2 завязан с другими элементами 1 этапа
                    // то удаляем его из временного массива на удаление (чтобы неудалить его)
                    stage2VersionForDelete.Remove(x.Key);

                    var st2 = dictStage2Version[x.Key];
                    st2.Sum = x.Sum(y => y.Sum + y.SumService);
                    stage2VersionRep.Update(st2);

                    stage2VersionRep.GetAll()
                        .Where(y => y.Stage3Version.Id == st2.Stage3Version.Id)
                        .ToList()
                        .GroupBy(y => y.Stage3Version.Id)
                        .ForEach(y =>
                        {
                            // Поскольку мы решили что данный Stage3 завязан с другими элементами 1 этапа
                            // то удаляем его из временного массива на удаление (чтобы неудалить его)
                            stage3VersionForDelete.Remove(y.Key);

                            var st3 = dictStage3Version[y.Key];
                            st3.Sum = y.Sum(i => i.Sum);
                            stage3VersionRep.Update(st3);
                        });
                });

            // Удаляем корректировки записей для Stage2 которые требуется удалить
            var dpkrcorrRecs = dpkrCorrService.GetAll()
                .Where(x => stage2VersionForDelete.Contains(x.Stage2.Id))
                .Select(x => x.Id)
                .Distinct()
                .ToList();

            var shortRecs = shortRecService.GetAll()
                .Where(x => stage1VersionForDelete.Contains(x.Stage1.Id))
                .Select(x => x.Id)
                .Distinct()
                .ToList();

            // теперь удаляем все что хотели удалить
            stage1ForDelete.ForEach(x => programService.Delete(x));
            stage2ForDelete.ForEach(x => programServiceStage2.Delete(x));
            stage3ForDelete.ForEach(x => programServiceStage3.Delete(x));

            shortRecs.ForEach(x => shortRecService.Delete(x));
            dpkrcorrRecs.ForEach(x => dpkrCorrService.Delete(x));

            stage1VersionForDelete.ForEach(x => stage1VersionRep.Delete(x));
            stage2VersionForDelete.ForEach(x => stage2VersionRep.Delete(x));
            stage3VersionForDelete.ForEach(x => stage3VersionRep.Delete(x));

            #endregion

            return Success();
        }

        //
        public override IDataResult BeforeUpdateAction(IDomainService<RealityObjectStructuralElement> service, RealityObjectStructuralElement entity)
        {
            //пересчет износа
            entity.Wearout = RecalcWearout(entity);

            return base.BeforeUpdateAction(service, entity);
        }

        //
        private decimal RecalcWearout(RealityObjectStructuralElement entity)
        {
            //пересчет износа конструктивного элемента
            var wearout = entity.Wearout;

            if (wearout <= 0)
            {
                var lastOvrhl = entity.LastOverhaulYear;
                var currYear = System.DateTime.Now.Year;
                if (lastOvrhl == 0)
                {
                    lastOvrhl = entity.RealityObject.BuildYear.HasValue
                        ? entity.RealityObject.BuildYear.Value
                        : currYear;
                }
                var lifeTime = entity.StructuralElement.LifeTime;
                var d = lifeTime > 0 ? 100*(currYear - lastOvrhl)/lifeTime : 100;
                wearout = d;
            }

            if (wearout > 100)
            {
                wearout = 100;
            }

            return wearout;
        }
    }
}