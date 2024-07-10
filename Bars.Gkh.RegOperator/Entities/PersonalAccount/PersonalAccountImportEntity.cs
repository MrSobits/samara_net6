namespace Bars.Gkh.RegOperator.Entities.PersonalAccount;

using Bars.Gkh.RegOperator.Utils;

public class PersonalAccountImportEntity
{
    [DbfColumn("PERIOD")]
    public string Period { get; private set; }

    [DbfColumn("KKC")]
    public string ExternalAccountNumber { get; private set; }

    [DbfColumn("SLD_K")]
    public decimal SaldoOut { get; private set; }

    [DbfColumn("NACH")]
    public decimal Charged { get; private set; }

    [DbfColumn("SLD_N")]
    public decimal SaldoIn { get; private set; }

    [DbfColumn("PENY")]
    public decimal Penalty { get; private set; }

    [DbfColumn("RAZV")]
    public decimal Recalc { get; private set; }

    public int RowNum { get; private set; }
}