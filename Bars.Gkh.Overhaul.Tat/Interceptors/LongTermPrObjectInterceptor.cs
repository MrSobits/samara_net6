using Bars.Gkh.Overhaul.Entities;

namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    public class LongTermPrObjectInterceptor : EmptyDomainInterceptor<LongTermPrObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<LongTermPrObject> service, LongTermPrObject entity)
        {
            var messages = new List<string>();

            if (messages.Any())
            {
                return Failure(string.Format("Существуют связанные записи в следующих таблицах:<br>{0}<br>Удаление отменено.",
                            messages.AggregateWithSeparator(", <br>")));
            }

            return Success();
        }
    }
}
