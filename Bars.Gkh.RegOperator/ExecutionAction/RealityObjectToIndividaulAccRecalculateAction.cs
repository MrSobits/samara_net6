namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using B4.Modules.FIAS;
    using Dapper;
    using Gkh.Utils;

    /// <summary>
    /// Действие перерасчета Кодов домов для Физических лиц согластно фактического адреса
    /// </summary>
    public class RealityObjectToIndividaulAccRecalculateAction : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Действие заполнения таблицы сопоставления ID дома с фактическим адресом абонента";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Заполнение таблицы сопоставления ID дома с фактическим адресом абонента";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var sessions = this.Container.Resolve<ISessionProvider>();

            using (var session = sessions.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {

                        switch (this.Container.GetGkhConfig<Bars.Gkh.ConfigSections.General.GeneralConfig>().UseFiasHouseIdentification)
                        {
                            case Gkh.Enums.UseFiasHouseIdentification.NotUse:

                                session.CreateSQLQuery(@"update regop_individual_acc_own iac
                                                            set ro_id = ro.id 
                                                         FROM gkh_reality_object as ro,
                                                         B4_FIAS_ADDRESS as fa_ro,
                                                         B4_FIAS_ADDRESS as fa_iac
                                                         where fa_ro.id = ro.fias_address_id and iac.fias_fact_address_id = fa_iac.id
                                                         and coalesce(fa_iac.place_guid,'') = coalesce(fa_ro.place_guid,'')
                                                            and coalesce(fa_iac.street_guid,'') = coalesce(fa_ro.street_guid,'')
                                                        
                                                        and lower(regexp_replace(coalesce(fa_iac.house,''), '^(\d+?)\W+(\D*)$', E'\\1\\2')||coalesce(fa_iac.letter,'')) = 
                                                        lower(regexp_replace(coalesce(fa_ro.house,''), '^(\d+?)\W+(\D*)$', E'\\1\\2')||coalesce(fa_ro.letter,''))
                                                            and coalesce(fa_iac.building,'') = coalesce(fa_ro.building,'');
                                                      ").ExecuteUpdate();
                                break;
                            case Gkh.Enums.UseFiasHouseIdentification.Use:

                                session.CreateSQLQuery(@"update regop_individual_acc_own iac
                                                set ro_id = ro.id 
                                             FROM gkh_reality_object as ro, 
                                                  B4_FIAS_ADDRESS as fa_ro,		
	                                              B4_FIAS_ADDRESS as fa_iac
                                             where fa_ro.id = ro.fias_address_id 
                                                and fa_iac.Address_guid = fa_ro.Address_guid
                                                and iac.fias_fact_address_id = fa_iac.id;
                                                      ").ExecuteUpdate();
                                break;
                            default:
                                break;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction.Commit();
                }
            }

            return new BaseDataResult();
        }
    }
}