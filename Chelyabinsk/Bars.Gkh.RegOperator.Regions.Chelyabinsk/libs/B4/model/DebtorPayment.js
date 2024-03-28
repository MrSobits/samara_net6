Ext.define('B4.model.DebtorPayment', {
    extend: 'B4.base.Model',
 
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AgentPIRExecute',
        listAction: 'GetListPayment'
    },

    fields: [
        { name: 'Id' },
        { name: 'SumPayment' },
        { name: 'DatePayment' },
        { name: 'Reason'}
    ]
});