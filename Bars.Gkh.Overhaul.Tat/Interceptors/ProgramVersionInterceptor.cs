namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
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

        private BaseDataResult CheckVersions(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
#warning Совсем неожидаемый результат работы интерцептора
            if (entity.IsMain && service.GetAll().Any(x => x.Id != entity.Id && x.IsMain && x.Municipality.Id == entity.Municipality.Id))
            {
                return Failure("Основная версия программы уже выбрана! Уберите отметку \"Основная\" у старой версии и после этого выберите новую основную версию");
            }

            if (entity.IsProgramPublished == true && service.GetAll().Any(x => x.Municipality.Id == entity.Municipality.Id && x.Id != entity.Id && x.IsProgramPublished == true))
            {
                return Failure("Опубликованная программа в рамках одного муниципального района может быть только одна! Уберите отметку \"Программа опубликована\" у опубликованной версии и после этого выберите новую версию для публикации");
            }

            return Success();
        }
    }
}