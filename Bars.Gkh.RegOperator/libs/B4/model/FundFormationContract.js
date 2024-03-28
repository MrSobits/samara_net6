Ext.define('B4.model.FundFormationContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FundFormationContract'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LongTermPrObject', defaultValue: null },
        { name: 'RegOperator', defaultValue: null },
        { name: 'TypeContract', defaultValue: 10 },
        { name: 'ContractNumber' },
        { name: 'ContractDate' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'File', defaultValue: null },
        { name: 'Municipality' }
    ]
});