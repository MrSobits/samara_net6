namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Overhaul.Entities;


    class JobInterceptor : EmptyDomainInterceptor<Job>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Job> service, Job entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Job> service, Job entity)
        {
            return ValidateEntity(entity);
        }

        private IDataResult ValidateEntity(Job entity)
        {
            var errorProps = new List<string>();

            if (entity.Name.IsEmpty())
            {
                errorProps.Add("Наименование");
            }

            if (entity.Work == null)
            {
                errorProps.Add("Вид работ");
            }

            if (entity.UnitMeasure == null)
            {
                errorProps.Add("Единица измерения");
            }

            if (errorProps.IsNotEmpty())
            {
                const string errorMessage = "Не заполнены обязательные поля";
                return Failure(string.Format("{0}:{1}", errorMessage, errorProps.AggregateWithSeparator(", ")));
            }

            return Success();
        }
    }
}
