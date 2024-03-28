namespace Sobits.RosReg.Controllers
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;

    using Dapper;

    using Sobits.RosReg.Entities;
    using Sobits.RosReg.ExtractTypes;
    using Sobits.RosReg.Map;

    public class ExtractParseController : BaseController
    {
        public void Process()
        {
            this.AssignTypes();
            this.Parse();
            this.SetActive();
        }

        public void Parse()
        {
            var extractList = this.Container.ResolveDomain<Extract>().GetAll().Where(x => x.IsParsed == false);
            var allTypes = this.Container.ResolveAll<IExtractType>();
            foreach (var extract in extractList)
            {
                var parser = allTypes.FirstOrDefault(x => x.Code == extract.Type);
                parser?.Parse(extract);
            }
        }

        //TODO: Переписать с чистого SQL на запрос для NHibernate
        public void AssignTypes()
        {
            var allTypes = this.Container.ResolveAll<IExtractType>();

            using (var statelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            using (var connection = statelessSession.Connection)
            {
                foreach (var type in allTypes)
                {
                    var sqlupdate = $@"update {ExtractMap.SchemaName}.{ExtractMap.TableName}
                            set {nameof(Extract.Type).ToLower()} = {(int)type.Code}
                            where xml::text like E'%{type.Pattern}%' and ({nameof(Extract.Type).ToLower()} is null or {nameof(Extract.Type).ToLower()}=0)";
                    connection.Execute(sqlupdate);
                }
            }
        }

        public void SetActive()
        {
            using (var statelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            using (var connection = statelessSession.Connection)
            {
                {
                    var sqlupdate = $@"update {ExtractMap.SchemaName}.{ExtractMap.TableName} set {nameof(Extract.IsActive).ToLower()}=false;
                                       update {ExtractMap.SchemaName}.{ExtractMap.TableName} set {nameof(Extract.IsActive).ToLower()}=true where id in (
                                       select distinct extractid from (
                                       select row_number() over(partition by ss.{nameof(ExtractEgrn.CadastralNumber)}) as rn,
                                              * from {ExtractMap.SchemaName}.{ExtractEgrnMap.TableName} ss
                                         join (select distinct {nameof(ExtractEgrn.CadastralNumber)},
                                               max({nameof(ExtractEgrn.ExtractDate)}) 
                                                 from {ExtractMap.SchemaName}.{ExtractEgrnMap.TableName}
                                               group by {nameof(ExtractEgrn.CadastralNumber)}) gg
                                           on gg.{nameof(ExtractEgrn.CadastralNumber)}=ss.{nameof(ExtractEgrn.CadastralNumber)}) al
                                       where rn = 1)";
                    connection.Execute(sqlupdate);
                }
            }
        }
    }
}