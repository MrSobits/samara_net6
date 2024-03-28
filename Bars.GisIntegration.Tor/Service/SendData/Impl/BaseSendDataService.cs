namespace Bars.GisIntegration.Tor.Service.SendData.Impl
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Text;
	using System.Threading;

	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	using Bars.B4.Modules.Analytics.Reports;
	using Bars.B4.Modules.FileStorage;
	using Bars.GisIntegration.Tor.Entities;
	using Bars.GisIntegration.Tor.Enums;
	using Bars.GisIntegration.Tor.Service.LogService;
	using Bars.Gkh.Entities.Base;
	using Bars.GkhGji.Entities;

	using Castle.Windsor;


	using Ionic.Zip;

	using Newtonsoft.Json;

	using NHibernate.Util;

	using FileInfo = B4.Modules.FileStorage.FileInfo;

	public class BaseSendDataService<T> : ISendDataService<T>
		where T : PersistentObject, IUsedInTorIntegration
	{
		public IWindsorContainer Container { get; set; }

		public ITorIntegrationService TorIntegrationService { get; set; }

		public ITorLogService TorLogService { get; set; }

		public IDomainService<TorTask> TaskDomainService { get; set; }

		public IUserInfoProvider UserInfoProvider { get; set; }

		public List<Tuple<string, string, IUsedInTorIntegration>> ListRequests = new List<Tuple<string, string, IUsedInTorIntegration>>();

		public List<Tuple<string, string, IUsedInTorIntegration>> ListResponses = new List<Tuple<string, string, IUsedInTorIntegration>>();

	    public List<string> ListSendRequest = new List<string>();

        public TypeObject TypeObject { get; set; }

		public TypeRequest TypeRequest { get; set; }

		public T SendObject { get; set; }

		public TorTask Task { get; set; }

		public virtual IDataResult PrepareData(BaseParams baseParams)
		{
			return this.SendRequest();
		}

		public virtual IDataResult GetData(BaseParams baseParams)
		{
			return this.SendRequest();
		}

	    public virtual IDataResult SendRequest(bool saveFile = true)
	    {
	        TorTask task = null;

	        if (this.Task == null)
	        {
	            task = this.CreateTask(this.TypeObject, this.SendObject);
	            this.TorLogService.AddLogRecord("Задача создана", task.Id);
	        }
	        else
	        {
	            task = this.Task;
	        }
            
	        try
	        {
	            foreach (var r in this.ListRequests)
	            {
	                if (this.Task != null && this.ListSendRequest.Contains(r.Item2))
	                {
	                    continue;
	                }

	                this.TorIntegrationService.SendRequest(
	                    CancellationToken.None,
	                    this.TypeRequest,
	                    r.Item3,
	                    r.Item2,
	                    r.Item1,
	                    task,
	                    out string response);

	                this.ListSendRequest.Add(r.Item2);

	                if (response is null)
	                {
	                    continue;
	                }

	                this.ListResponses.Add(
	                    new Tuple<string, string, IUsedInTorIntegration>(r.Item1, JsonConvert.DeserializeObject(response).ToString(), r.Item3));
	            }

	            if (task.TorTaskState == TorTaskState.CompleteWithErrors || task.TorTaskState == TorTaskState.NotComplete)
	            {
	                return new BaseDataResult { Success = false };
	            }

	            return new BaseDataResult { Success = true };
	        }
	        catch (Exception e)
	        {
	            this.TorLogService.AddLogRecord(e.Message, task.Id);
	            return new BaseDataResult { Success = false, Message = "Произошла ошибка: " + e };
	        }
	        finally
	        {
	            if (saveFile)
	            {
	                task.RequestFile = this.SaveFile(this.ListRequests, "Request", this.TypeObject, this.SendObject?.Id);
	                task.ResponseFile = this.ListResponses.Any()
	                    ? this.SaveFile(this.ListResponses, "Response", this.TypeObject, this.SendObject?.Id)
	                    : null;
	                task.LogFile = this.TorLogService.SaveLogFile(task.Id, this.Container);
	            }
	            else
	            {
	                this.Task = task;
	            }

	            this.TaskDomainService.Update(task);
	        }
	    }

		protected TorTask CreateTask(TypeObject typeObject, T sendObject)
		{
			var disposalDomain = this.Container.ResolveDomain<Disposal>();

			using (this.Container.Using(disposalDomain))
			{
			    var task = new TorTask()
			    {
			        ObjectCreateDate = DateTime.Now,
			        ObjectEditDate = DateTime.Now,
			        TypeRequest = this.TypeRequest,
			        SendObject = typeObject,
			        Disposal = sendObject is Disposal ? disposalDomain.Get(sendObject.Id) : null,
			        User = this.UserInfoProvider?.GetActiveUser(),
			        TorTaskState = TorTaskState.SendingData
			    };

				this.TaskDomainService.Save(task);
				return task;
			}
		}

		protected FileInfo SaveFile(List<Tuple<string, string, IUsedInTorIntegration>> filesList, string fileType, TypeObject typeObject, long? id)
		{
			var zipStream = new MemoryStream();
			var fileZip = new ZipFile(Encoding.GetEncoding("windows-1251"));
			var fileName = id != null ? $"{fileType}_{typeObject}_{id}" : $"{fileType}_{typeObject}";

			var count = 1;
			foreach (var file in filesList)
			{
				fileZip.AddEntry(string.Format($"{count}_{file.Item1}.txt", 1), Encoding.UTF8.GetBytes(file.Item2));
				count++;
			}

			fileZip.Save(zipStream);
			var fileManager = this.Container.Resolve<IFileManager>();
			using (this.Container.Using(fileManager))
			{
				var file = fileManager.SaveFile(zipStream, $"{fileName}.zip");
				return file;
			}
		}
	}
}