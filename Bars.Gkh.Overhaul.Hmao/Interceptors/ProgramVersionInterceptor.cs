namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    /// <summary>
    /// Интерцептор <see cref="ProgramVersion"/>
    /// </summary>
    public class ProgramVersionInterceptor : EmptyDomainInterceptor<ProgramVersion>
    {
        /// <summary>
        /// Действие, выполняемое до создания сущности <see cref="ProgramVersion"/>
        /// </summary>
        /// <param name="service">Домен-сервис "ProgramVersion"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            return this.CheckVersions(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности <see cref="ProgramVersion"/>
        /// </summary>
        /// <param name="service">Домен-сервис "ProgramVersion"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            return this.CheckVersions(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности <see cref="ProgramVersion"/>
        /// </summary>
        /// <param name="service">Домен-сервис "ProgramVersion"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        /// Внимание в связи с огромным количеством удаляемых объектов Удаление произходит в файле ProgramVersionDomainService
        /// Путем переопределения метода Delete 
        public override IDataResult BeforeDeleteAction(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            if (service.GetAll().Any(x => x.ParentVersion.Id == entity.Id))
            {
                var nameChildrenProgramVersion = service.GetAll().Where(x => x.ParentVersion.Id == entity.Id).Select(x => x.Name).ToArray();
               
                return this.Failure("Существуют зависимые записи " + string.Join(@", ", nameChildrenProgramVersion));
            }

            return this.Success();
        }

        private BaseDataResult CheckVersions(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            if (entity.IsMain && service.GetAll().Any(x => x.Municipality.Id == entity.Municipality.Id && x.IsMain && x.Id != entity.Id))
            {
                return this.Failure("Основная версия программы уже выбрана! Уберите отметку \"Основная\" у старой версии и после этого выберите новую основную версию");
            }

            return this.Success();
        }
    }
}