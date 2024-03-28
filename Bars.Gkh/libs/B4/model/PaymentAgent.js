Ext.define('B4.model.PaymentAgent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentAgent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'CtrMunicipality' },
        { name: 'CtrSettlement' },
        { name: 'Code'},
        { name: 'CtrName' },
        { name: 'CtrInn' },
        { name: 'CtrKpp' },
        { name: 'CtrOgrn' },
        { name: 'CtrOrgFormName' },
        { name: 'CtrJurAdress' },
        { name: 'CtrDateReg' },
        { name: 'CtrFactAdress' },
        { name: 'CtrMailAddress' },
        { name: 'CtrMailAddress' },
        { name: 'PenaltyContractId', defaultValue: null },
        { name: 'SumContractId', defaultValue: null }
    ]
});