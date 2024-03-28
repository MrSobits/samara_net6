namespace Bars.Gkh.Entities.Suggestion
{
    using System;

    using B4.Modules.FileStorage;

    public class SuggestionCommentFiles : BaseGkhEntity
    {
        public virtual SuggestionComment SuggestionComment { get; set; }

        public virtual FileInfo DocumentFile { get; set; }

        public virtual string DocumentNumber { get; set; }

        public virtual DateTime? DocumentDate { get; set; }

        public virtual bool isAnswer { get; set; }

        public virtual string Description { get; set; }
    }
}