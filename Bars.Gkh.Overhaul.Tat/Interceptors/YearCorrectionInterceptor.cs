namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>
    /// Интерцепор для <see cref="YearCorrection"/>
    /// </summary>
    public class YearCorrectionInterceptor : EmptyDomainInterceptor<YearCorrection>
    {
        /// <summary>Метод вызывается перед созданием объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeCreateAction(IDomainService<YearCorrection> service, YearCorrection entity)
        {
            return this.ValidateEntity(service, entity);
        }

        /// <summary>Метод вызывается перед обновлением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<YearCorrection> service, YearCorrection entity)
        {
            return this.ValidateEntity(service, entity);
        }

        private IDataResult ValidateEntity(IDomainService<YearCorrection> service, YearCorrection entity)
        {
            if (entity.Year <= 0)
            {
                return this.Failure("Указано неверное значение поля год");
            }

            if (service.GetAll().Any(x => x.Id != entity.Id && x.Year == entity.Year))
            {
                return this.Failure("Запись с указанным годом уже существует");
            }

            return this.Success();
        }
    }
}