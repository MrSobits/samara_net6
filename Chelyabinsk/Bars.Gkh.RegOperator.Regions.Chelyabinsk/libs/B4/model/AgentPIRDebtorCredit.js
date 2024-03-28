Ext.define('B4.model.AgentPIRDebtorCredit', {
    extend: 'B4.base.Model',
 
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AgentPIRDebtorCredit'
    },
    fields: [
        { name: 'Id' },
        { name: 'Debtor' },
        { name: 'Credit' },
        { name: 'Date' },
        { name: 'User' },
        { name: 'File' }
    ]
});