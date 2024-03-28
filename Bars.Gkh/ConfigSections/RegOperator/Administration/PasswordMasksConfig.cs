namespace Bars.Gkh.ConfigSections.RegOperator.Administration
{
    using System.ComponentModel;


    /// <summary>
    ///     Допустимые символы
    /// </summary>
    [DisplayName(@"Допустимые символы пароля, указывать в квадратных скобках пример:[A-Z]")]
    public class PasswordMasksConfig
    {
        /// <summary>
        ///    Допустимые символы
        /// </summary>
        [DisplayName(@"Значение")]
        public virtual string Mask { get; set; }
    }
}