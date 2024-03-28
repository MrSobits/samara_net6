namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System;
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class TomskProtocolDefinitionInterceptor : Bars.GkhGji.Interceptors.ProtocolDefinitionInterceptor<TomskProtocolDefinition>
    {

        public override IDataResult BeforeCreateAction(IDomainService<TomskProtocolDefinition> service, TomskProtocolDefinition entity)
        {
            UpdateDateOfProcessing(entity);
            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TomskProtocolDefinition> service, TomskProtocolDefinition entity)
        {
            UpdateDateOfProcessing(entity);
            return Success();
        }

        private void UpdateDateOfProcessing(ProtocolDefinition entity)
        {
            
            // Вообщем, если ТипОпределения = О назначении времени и места рассмотрения дела
            // И дата и время рассмотрения дела пыстые
            // Тол необходимо из родительского Протокола дернуть нужные поля и проставить их в определение
            if (entity.TypeDefinition == TypeDefinitionProtocol.TimeAndPlaceHearing && !entity.DateOfProceedings.HasValue && !entity.TimeDefinition.HasValue)
            {
                entity.DateOfProceedings = entity.Protocol.DateOfProceedings;

                if (entity.Protocol.HourOfProceedings > 0 || entity.Protocol.MinuteOfProceedings > 0)
                {
                    var date = entity.DateOfProceedings.HasValue ? entity.DateOfProceedings.Value : DateTime.Today;

                    entity.TimeDefinition = new DateTime(date.Year, date.Month, date.Day, entity.Protocol.HourOfProceedings, entity.Protocol.MinuteOfProceedings, 0);
                }
                
            }
            
        }
    }
}
