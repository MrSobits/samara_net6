namespace Bars.Gkh
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Modules.Ecm7.Framework;

    public abstract class BaseGkhViewCollection : IViewCollection
    {
        public abstract int Number { get; }

        public virtual string GetDeleteScript(DbmsKind dbmsKind, string name)
        {
            return (string)this.GetType()
                    .GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)
                    .Invoke(this, new object[] { dbmsKind });
        }

        public virtual string GetCreateScript(DbmsKind dbmsKind, string name)
        {
            return (string)this.GetType()
                     .GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance)
                     .Invoke(this, new object[] { dbmsKind });
        }

        public virtual List<string> GetCreateAll(DbmsKind dbmsKind)
        {
            var createFunc = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("CreateFunction")).ToList();

            var queries = new List<string>();

            foreach (var method in createFunc)
            {
                var str = (string)method.Invoke(this, new object[] { dbmsKind });
                if (!String.IsNullOrEmpty(str))
                {
                    queries.Add(str);
                }
            }

            var createView = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("CreateView")).ToList();

            foreach (var method in createView)
            {
                var str = (string)method.Invoke(this, new object[] { dbmsKind });
                if (!String.IsNullOrEmpty(str))
                {
                    queries.Add(str);
                }
            }

            return queries;
        }

        public virtual List<string> GetDropAll(DbmsKind dbmsKind)
        {
            var deleteView = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("DeleteView")).ToList();

            var queries = deleteView.Select(method => (string)method.Invoke(this, new object[] { dbmsKind })).Where(str => !String.IsNullOrEmpty(str)).ToList();

            var deleteFunc = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("DeleteFunction")).ToList();

            queries.AddRange(deleteFunc.Select(method => (string)method.Invoke(this, new object[] { dbmsKind })).Where(str => !String.IsNullOrEmpty(str)));

            return queries;
        }

        private const string OracleDeleteViewTemplate = @"DECLARE v_count NUMBER;
                                                         BEGIN
                                                            SELECT COUNT(*) INTO v_count
                                                            FROM user_objects
                                                            WHERE object_type = 'VIEW'
                                                            AND object_name = '{0}';
                                                         
                                                         IF v_count = 1 THEN
                                                            EXECUTE IMMEDIATE 'DROP VIEW {0}';
                                                         END IF;
                                                         
                                                         END;";

        private const string PostgreDeleteViewTemplate = @"DROP VIEW IF EXISTS {0}";

        private const string PostgreDeleteFunction = @"DROP FUNCTION IF EXISTS {0}";

		private const string OracleDeleteFunctionTemplate = @"DECLARE f_count NUMBER;
                                                         BEGIN
                                                            SELECT COUNT(*) INTO f_count
                                                            FROM user_objects
                                                            WHERE object_type = 'FUNCTION'
                                                            AND object_name = '{0}';
                                                         
                                                         IF f_count = 1 THEN
                                                            EXECUTE IMMEDIATE 'DROP FUNCTION {0}';
                                                         END IF;
                                                         
                                                         END;";

        protected string DropViewOracleQuery(string viewName)
        {
            return string.Format(OracleDeleteViewTemplate, viewName);
        }

        protected string DropViewPostgreQuery(string viewName)
        {
            return string.Format(PostgreDeleteViewTemplate, viewName);
        }

		protected string DropFunctionOracleQuery(string funcName)
		{
			return string.Format(OracleDeleteFunctionTemplate, funcName);
		}

        protected string DropFunctionPostgreQuery(string funcName)
        {
            return string.Format(PostgreDeleteFunction, funcName);
        }
    }
}