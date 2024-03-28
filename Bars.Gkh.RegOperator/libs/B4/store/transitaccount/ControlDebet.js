Ext.define('B4.store.transitaccount.ControlDebet', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransitAccount',
        listAction: 'DebetList'
    },
    fields: [
        { name: 'Number' },
        { name: 'Date' },
        { name: 'PaymentAgentName' },
        { name: 'Sum' },
        { name: 'ConfirmSum' },
        { name: 'Divergence' }
    ]
});