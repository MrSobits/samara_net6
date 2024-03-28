Ext.define('B4.model.PersonalAccountCounter', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountCounter'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Service' },
        { name: 'CounterNumber' },
        { name: 'CounterType' },
        { name: 'PrevStatementDate' },
        { name: 'PrevCounterValue' },
        { name: 'StatementDate' },
        { name: 'CounterValue' },
        { name: 'CounterType' }
    ]
});