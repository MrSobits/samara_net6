namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерфейс для синхронизации реестров жилых и аварийных домов
    /// </summary>
    public interface IEmergencyObjectSyncService
    {
        /// <summary>
        /// Синхронизировать данные аварийного дома на основе жилого
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        void SyncEmergencyObject(RealityObject realityObject);

        /// <summary>
        /// Синхронизировать данные жилого дома на основе аварийного
        /// </summary>
        /// <param name="emergencyObject">Аварийный дом</param>
        void SyncRealityObject(EmergencyObject emergencyObject);

        /// <summary>
        /// Синхронизировать данные о состоянии аварийного дома на основе жилого
        /// </summary>
        /// <param name="realityObject">Жилой дом</param>
        /// <param name="logLight">Лог</param>
        void SyncEmergencyObjectCondition(RealityObject realityObject, EntityLogLight logLight);

        /// <summary>
        /// Обновить статус жилого дома и синхронизировать с ним аварийный
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        void UpdateConditionHouse(BaseParams baseParams);

        /// <summary>
        /// Синхронизировать все поля аварийного дома согласно полям жилого дома
        /// </summary>
        /// <param name="emergencyObject">Аварийный дом</param>
        void QuiteSyncEmergencyObject(EmergencyObject emergencyObject);
    }
}