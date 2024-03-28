Ext.define('B4.model.HouseCounter', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseCounter'
    },
    fields: [
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