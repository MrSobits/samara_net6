namespace Bars.Gkh.RegOperator.Entities.PersonalAccount;

using Bars.Gkh.RegOperator.Utils;

public class PersonalAccountPaymentEntity
{
    [DbfColumn("SUMMA")]
    public virtual decimal ChargeTariff { get; set; }
    
    [DbfColumn("PENIASSD")]
    public virtual decimal Penalty { get; set; }
    
    [DbfColumn("LS")]
    public virtual string AccNum { get; set; }
    
    [DbfColumn("OUTERLS")]
    public virtual string AccOuterNum { get; set; }
    
    [DbfColumn("EPD")]
    public virtual string RkcNum { get; set; }
    
    [DbfColumn("PERRECALC")]
    public virtual decimal RecalcByBaseTariff { get; set; }
}