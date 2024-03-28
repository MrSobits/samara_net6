Ext.define('B4.model.AgentPIRDocument', {
    extend: 'B4.base.Model',
 
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AgentPIRDocument'
    },
    fields: [
        { name: 'Id' },
        { name: 'Number' },
        { name: 'DocumentDate' },
        { name: 'DebtSum' },
        { name: 'PeniSum' },
        { name: 'Duty' },
        { name: 'AgentPIRDebtor' },
        { name: 'DocumentType' },
        { name: 'File' },
        { name: 'Repaid', defaultValue:0 },
        { name: 'YesNo' }
    ]
});