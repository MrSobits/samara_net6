Ext.define('B4.model.smev.PayRegRequests', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PayRegRequests'
    },
    fields: [
        { name: 'Id' },
        { name: 'MessageId' },
        { name: 'Inspector' },
        { name: 'CalcDate' },
        { name: 'Answer' },
        { name: 'RequestState' },
        { name: 'PayRegPaymentsKind' },
        { name: 'PayRegPaymentsType' },
        { name: 'GetPaymentsStartDate' },
        { name: 'GetPaymentsEndDate' }        
    ]
});