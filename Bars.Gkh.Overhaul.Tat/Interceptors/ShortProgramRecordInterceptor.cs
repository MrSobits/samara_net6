namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Entities;

    public class ShortProgramRecordInterceptor : EmptyDomainInterceptor<ShortProgramRecord>
    {
        public IShortProgramRecordService ShortRecordsDomain { get; set; }

        public IDomainService<ShortProgramRealityObject> ShortObjectsDomain { get; set; }

        public override IDataResult AfterDeleteAction(IDomainService<ShortProgramRecord> service, ShortProgramRecord entity)
        {

            //Если объект удалился то необходиму у дома поменять признак На Неактуальный
            if (entity.ShortProgramObject != null && entity.ShortProgramObject.TypeActuality == TypeActuality.Actual)
            {
                var obj = this.ShortObjectsDomain.GetAll().FirstOrDefault(x => x.Id == entity.ShortProgramObject.Id);

                if (obj != null)
                {
                    obj.TypeActuality = TypeActuality.NoActual;
                    ShortObjectsDomain.Update(obj);
                }
            }

            return base.AfterDeleteAction(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<ShortProgramRecord> service, ShortProgramRecord entity)
        {
            //Если объект удалился то необходиму у дома поменять признак На Неактуальный
            if (entity.ShortProgramObject != null && entity.ShortProgramObject.TypeActuality == TypeActuality.Actual)
            {
                var obj = this.ShortObjectsDomain.GetAll().FirstOrDefault(x => x.Id == entity.ShortProgramObject.Id);

                if (obj != null)
                {
                    obj.TypeActuality = TypeActuality.NoActual;
                    ShortObjectsDomain.Update(obj);
                }
            }

            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ShortProgramRecord> service, ShortProgramRecord entity)
        {
            // Удалять можно только записи, которые добавлены вручную
            if (entity.TypeDpkrRecord != TypeDpkrRecord.UserRecord)
            {
                return Failure("Удалять из краткосрочной программы можно только записи, добавленные вручную");
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}