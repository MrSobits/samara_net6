Ext.define('B4.model.regoperator.CalcAccountForSelect', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegOpCalcAccount',
        listAction: 'ListByRegop',
        timeout: 4 * 60 * 1000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AccountNumber' }
    ]
});