namespace Bars.GisIntegration.Tor.Map
{
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GisIntegration.Tor.Entities;

	public class TorTaskMap : BaseEntityMap<TorTask>
	{
		public TorTaskMap()
			: base(typeof(TorTask).FullName, "GI_TOR_TASK")
		{
		}

		protected override void Map()
		{
		    this.Property(x => x.TypeRequest, "Тип запроса").Column("REQUEST_TYPE");
            this.Property(x => x.SendObject, "Отправленный объект").Column("SEND_OBJECT_TYPE");
			this.Property(x => x.TorTaskState, "Статус").Column("TASK_STATE");
			this.Reference(x => x.User, "Пользователь").Column("USER_ID").Fetch();
			this.Reference(x => x.Disposal, "Распоряжение").Column("DISPOSAL_ID").Fetch();
			this.Reference(x => x.RequestFile, "Пакет запроса").Column("REQUEST_FILE_ID").Fetch();
			this.Reference(x => x.ResponseFile, "Пакет ответа").Column("RESPONSE_FILE_ID").Fetch();
			this.Reference(x => x.LogFile, "Лог").Column("LOG_FILE_ID").Fetch();
		}
	}
}
