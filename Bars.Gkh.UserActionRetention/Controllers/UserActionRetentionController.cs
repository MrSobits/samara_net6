namespace Bars.Gkh.UserActionRetention.Controllers
{
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.Gkh.UserActionRetention.DomainService;
    using Newtonsoft.Json;

    public class UserActionRetentionController: BaseController
    {
        public FileResult ExportToJson(BaseParams baseParams)
        {
            var result = Container.Resolve<IUserActionRetentionService>().ListWithoutPaging(baseParams);
            byte[] contents = GetBytes(JsonConvert.SerializeObject(result));
            return File(contents, "application/json", "export.json");
        }
        private byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
