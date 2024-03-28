Ext.define('B4.model.AgentPIR', {
    extend: 'B4.base.Model',
 
    
    proxy: {
        type: 'b4proxy',
        controllerName: 'AgentPIR'
    },
    fields: [
        { name: 'Id' },
        { name: 'Contragent' },
        { name: 'INN'},
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'ContractDate' },
        { name: 'ContractNumber' },
        { name: 'FileInfo' },
        
    ]
});