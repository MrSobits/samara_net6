using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Bars.B4;
using Bars.Gkh.Helpers;
using Castle.Windsor;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.FileSystemStorage;
using Bars.Gkh.Entities.Suggestion;
using Bars.B4.DataAccess;

namespace Bars.Gkh.DomainService
{
    class SuggestionSendEmailServise : ISuggestionSendEmailService

    {
        public IWindsorContainer Container { get; set; }

        public BaseDataResult SendAnswerEmail(BaseParams baseParams)
        {

            var citizenSuggestionService = this.Container.ResolveDomain<CitizenSuggestion>();
            var citizenSuggestionFilesService = this.Container.ResolveDomain<CitizenSuggestionFiles>();
           // var fileDomain = this.Container.Resolve<IDomainService<FileInfo>>();
            var fileManager = this.Container.Resolve<IFileManager>();


            //get params email
            var citsugId = baseParams.Params.GetAs<long>("citsugId");
            var citSuggestion = citizenSuggestionService.Get(citsugId);
            var files = citizenSuggestionFilesService.GetAll()
                .Where(x => x.CitizenSuggestion.Id == citsugId)
                .Select( x => x.DocumentFile)
                .ToList();

            var emailTo = citSuggestion.ApplicantEmail;
            var answerText = citSuggestion.AnswerText;
            var subject = $@"Ответ на обращение {citSuggestion.Number} от {citSuggestion.CreationDate.ToString("dd-MM-yyyy")}";   
            
            int maxSizeAttachments = 62914560;
            
            var filesSize = files.Sum(x => x.Size);
            if (filesSize > maxSizeAttachments)
            {
                return new BaseDataResult(false, $@"Превышение допустимого размера вложений. Размер вложений {filesSize} байт Допустимый размер {maxSizeAttachments} байт");
            }
            
            List<Attachment> attachments = new List<Attachment>();

            foreach(var file in files)
            {
                attachments.Add(new Attachment(fileManager.GetFile(file), file.FullName));
            }
            

            var sender = this.Container.Resolve<ICitSugEMailSender>();

            try
            {
                sender.Send(emailTo, subject, answerText, attachments);
            }
            catch (Exception e)
            {

                return new BaseDataResult(false, e.Message);
            }
            

            return new BaseDataResult(true, "Письмо успешно отправлено");



        }
    }
}
