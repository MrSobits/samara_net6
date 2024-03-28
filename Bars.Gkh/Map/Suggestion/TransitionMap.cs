/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Suggestion
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities.Suggestion;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class TransitionMap : BaseImportableEntityMap<Transition>
///     {
///         public TransitionMap()
///             : base("GKH_TRANSITION")
///         {
///             Map(x => x.EmailSubject, "EMAIL_SUBJECT");
///             Map(x => x.EmailTemplate, "EMAIL_TEMPLATE");
///             Map(x => x.ExecutionDeadline, "EXEC_DEADLINE");
///             Map(x => x.ExecutorEmail, "EXEC_EMAIL");
///             Map(x => x.IsFirst, "IS_FIRST");
///             Map(x => x.Name, "NAME");
///             Map(x => x.InitialExecutorType, "INIT_EXEC_TYPE");
///             Map(x => x.TargetExecutorType, "TARGET_EXEC_TYPE");
/// 
///             References(x => x.Rubric, "RUBRIC_ID");
/// 
///             ManyToOne(x => x.Rubric, m =>
///             {
///                 m.Column("RUBRIC_ID");
///                 m.Cascade(Cascade.DeleteOrphans);
///                 m.Lazy(LazyRelation.Proxy);
///                 m.Fetch(FetchKind.Select);
///                 m.NotNullable(true);
///             });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "Переход обращения граждан с одного состояния обработки в другое"</summary>
    public class TransitionMap : BaseImportableEntityMap<Transition>
    {
        
        public TransitionMap() : 
                base("Переход обращения граждан с одного состояния обработки в другое", "GKH_TRANSITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.IsFirst, "IsFirst").Column("IS_FIRST");
            Reference(x => x.Rubric, "Рубрика, в рамках которой действует данное правило перехода.").Column("RUBRIC_ID").NotNull();
            Property(x => x.InitialExecutorType, "С какого исполнителя меняем").Column("INIT_EXEC_TYPE");
            Property(x => x.TargetExecutorType, "На какого исполнителя меняем").Column("TARGET_EXEC_TYPE");
            Property(x => x.Name, "Наименование перехода.").Column("NAME").Length(250);
            Property(x => x.ExecutionDeadline, "Срок исполнения в днях.").Column("EXEC_DEADLINE");
            Property(x => x.EmailTemplate, "Шаблон письма-уведомления.").Column("EMAIL_TEMPLATE").Length(250);
            Property(x => x.ExecutorEmail, "Email, на который отправляется уведомление при достижении срока исполнения.").Column("EXEC_EMAIL").Length(250);
            Property(x => x.EmailSubject, "Тема письма.").Column("EMAIL_SUBJECT").Length(250);
        }
    }
}
