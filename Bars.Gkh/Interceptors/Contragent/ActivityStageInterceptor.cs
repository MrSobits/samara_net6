namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using B4;
    using Entities;

    public class ActivityStageInterceptor : EmptyDomainInterceptor<ActivityStage>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActivityStage> service, ActivityStage entity)
        {
            var validation = this.Validate(service, entity);

            if (!validation.Success)
            {
                return validation;
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActivityStage> service, ActivityStage entity)
        {
            var validation = this.Validate(service, entity);

            if (!validation.Success)
            {
                return validation;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<ActivityStage> service, ActivityStage entity)
        {
            var previousStage = service.GetAll()
                .Where(x => x.EntityType == entity.EntityType && x.EntityId == entity.EntityId)
                .Where(x => x.DateStart < entity.DateStart)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefault();

            if (previousStage != null && previousStage.DateEnd == null)
            {
                previousStage.DateEnd = entity.DateStart.AddDays(-1);
                service.Update(previousStage);
            }

            return this.Success();
        }
        
        private IDataResult Validate(IDomainService<ActivityStage> service, ActivityStage entity)
        {
            if (entity.DateEnd != null && entity.DateStart >= entity.DateEnd)
            {
                return this.Failure("Дата окончания стадии должна быть позже даты начала.");
            }

            var stageList = service.GetAll()
                .Where(x => x.EntityType == entity.EntityType && x.EntityId == entity.EntityId && x.Id != entity.Id)
                .ToArray();

            var stageBefore = stageList
                .Where(x => x.DateStart <= entity.DateStart)
                .OrderByDescending(x => x.DateStart)
                .FirstOrDefault();

            var stageAfter = stageList
                .Where(x => x.DateStart > entity.DateStart)
                .OrderBy(x => x.DateStart)
                .FirstOrDefault();


            if (!this.CheckInterval(entity, stageBefore, stageAfter))
            {
                return this.Failure("Сохранение невозможно. В данном периоде существует запись.");
            }

            return this.Success();
        }

        /// <summary>
        /// Проверка на пересечение интервалов
        /// </summary>
        private bool CheckInterval(ActivityStage entity, ActivityStage stageBefore, ActivityStage stageAfter)
        {
            if (stageBefore != null)
            {
                if ((stageBefore.DateStart >= entity.DateStart.AddDays(-1))
                    || (stageBefore.DateEnd != null && stageBefore.DateEnd >= entity.DateStart))
                {
                    return false;
                }
            }

            if (stageAfter != null)
            {
                if (entity.DateEnd == null
                    || (entity.DateEnd != null && entity.DateEnd >= stageAfter.DateStart))
                {
                    return false;
                }
            }

            return true;
        }
    }
}