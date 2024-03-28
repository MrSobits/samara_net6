Ext.define('B4.model.AgentPIRDebtor', {
    extend: 'B4.base.Model',
 
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AgentPIRDebtor'
    },
    fields: [
        { name: 'Id' },
        { name: 'Status' },
        { name: 'BasePersonalAccount' },
        { name: 'BaseTariffDebt' },
        { name: 'PenaltyDebt' },
        { name: 'Address' },
        { name: 'PersonalAccountNum' },
        { name: 'UnifiedAccountNumber' },
        { name: 'SaldoOut' },
        { name: 'BasePersonalAccountId' },
        { name: 'DebtBaseTariff' }, 
        { name: 'PenaltyDebt' },
        { name: 'DebtStartDate' },
        { name: 'DebtEndDate' },
        { name: 'UseCustomDate', defaultValue:false },
        { name: 'CustomDate' },
        { name: 'Ordering' },
        { name: 'Credit' },
        { name: 'State' },
        { name: 'AreaShare' },
        { name: 'DebtCalc' },
        { name: 'PenaltyCharge' },
        { name: 'Fio' }
    ]
});