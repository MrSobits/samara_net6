namespace Bars.GkhGji.Regions.Stavropol
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    public class GkhGjiStavropolViewCollection : BaseGkhViewCollection
    {
        public override int Number
        {
            get
            {
                return 1;
            }
        }


        #region Функции
        #region Create

       
        /// <summary>
        /// Функция возвращает номера источников обращения
        /// gjiGetControllerFio(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetExecutantsFio(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetControllerFio(param_id  bigint)
                RETURNS text AS
                $func$
                BEGIN
                RETURN ('');
                END
                $func$ LANGUAGE plpgsql;";
        }

        #endregion Create
        #region Delete

        /// <summary>
        /// gjiGetControllerFio(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteGjiGetControllerFio(DbmsKind dbmsKind)
        {
            return @"DROP FUNCTION if exists gjiGetControllerFio(bigint)";
        }

        #endregion Delete
        #endregion Функции
        
    }
}