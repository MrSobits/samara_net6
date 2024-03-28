Ext.define('B4.model.dict.ProgramCrFinSource', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramCrFinSource'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'FinanceSource', defaultValue: null },
        { name: 'ProgramCr', defaultValue: null },
        { name: 'FinanceSourceName' }
    ]
});