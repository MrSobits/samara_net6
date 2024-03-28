namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class InstructionController : FileStorageDataController<Instruction>
    {
        public ActionResult GetMainInstruction()
        {
            var fileId = this.Container.Resolve<IInstructionService>().GetMainInstruction();
            var result = Container.Resolve<IFileManager>().LoadFile(fileId);
            if (result.ResultCode == ResultCode.FileNotFound)
            {
                return RedirectToAction("Index", "Home"); 
            }

            return result;
        } 
    }
}
