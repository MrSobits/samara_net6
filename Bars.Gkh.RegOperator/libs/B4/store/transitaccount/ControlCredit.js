Ext.define('B4.store.transitaccount.ControlCredit', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransitAccount',
        listAction: 'CreaditList'
    },
    fields: [
        {name: 'Id' },
        { name: 'Date' },
        { name: 'CreditOrgName' },
        { name: 'CalcAccount' },
        { name: 'Sum' },
        { name: 'ConfirmSum' },
        { name: 'Divergence' }
    ]
});