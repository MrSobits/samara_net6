namespace Bars.Gkh.StateChanges
{
    using System.Linq;
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Castle.Windsor;


    public class PersonDisqualificationStateRule : IRuleChangeStatus
    {

        public virtual IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "PersonDisqualificationStateRule"; }
        }

        public string Name { get { return "Проверка возможности Дисквалификации должностного лица"; } }
        public string TypeId { get { return "gkh_person"; } }
        public string Description
        {
            get
            {
                return "Проверка на возможность перевода статуса на 'Дисквалифицирован'. Перевести статус разрешается только в случае наличия у ДЛ записи о дисквалификации и дисквалификация должна быть активна в данный момент времени.";
            }
        }

        public ValidateResult Validate(IStatefulEntity entity, State oldState, State newState)
        {
            var disqDomain = Container.ResolveDomain<PersonDisqualificationInfo>();

            try
            {
                var person = entity as Person;

                var date = DateTime.Now;

                var disqualQuery = disqDomain.GetAll().Where(x => x.Person.Id == person.Id)
                                .Select(x => new
                                {
                                    x.DisqDate,
                                    x.EndDisqDate,
                                    x.TypeDisqualification
                                });

                // поулчаем действующую дисквалификацию 
                if ( !disqualQuery.Any(x => x.DisqDate <= date && (!x.EndDisqDate.HasValue || x.EndDisqDate > date ) ) )
                {
                    return ValidateResult.No("Заполните сведения о дисквалификации и повторите перевод статуса");
                }

                return ValidateResult.Yes();

            }
            finally 
            {
                Container.Release(disqDomain);
            }
        }
    }
}
