namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Автоматическое проставление признака "Дом не участвует в программе КР"
    /// </summary>
    public class SetIsNotInvolvedCrAction : BaseExecutionAction
    {
        public override string Description =>
            @"Если выбрано значение настройки= использовать И в карточке жилого дома признак ""Дом не участвует в программе КР"" = true, 
то При наведении на чек-бокс, в информационном сообщении выводить наименование признака, по которому он был проставлен :
""Дом не участвует в программе КР, т.к. физический износ более 70%"" ( в случае если выполнено условие 1)
""Дом не участвует в программе КР, т.к. это дом блокированной застройки"" ( в случае если выполнено условие 2)
""Дом не участвует в программе КР, т.к. в нём менее 3х квартир"" ( в случае если выполнено условие 3)
В случе, если выполнено два, или более условий, в информационном сообщении вывести все сообщения через запятую в формате:
""Дом не участвует в программе КР, т.к. в нём менее 3х квартир, физический износ более 70%, это дом блокированной застройки";

        public override string Name => @"Автоматическое проставление признака ""Дом не участвует в программе КР";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

            session.CreateSQLQuery(@"update gkh_reality_object ro
                                     set  is_not_involved_cr = a.physical_wear>0 or a.type_house>0 or a.number_apartments>0,
                                     is_not_involved_cr_reason = a.physical_wear + a.type_house + a.number_apartments 
                                     from  
                                     (select id, 
                                             (case when physical_wear >= 70 then 1 else 0 end) physical_wear, 
                                             (case when type_house = 10 then 2 else 0 end) type_house,
                                             (case when number_apartments <=2 then 4 else 0 end) number_apartments
                                     from gkh_reality_object) a
                                     where a.id=ro.id").ExecuteUpdate();

            return new BaseDataResult
            {
                Success = true
            };
        }
    }
}