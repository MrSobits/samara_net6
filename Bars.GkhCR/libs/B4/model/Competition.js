Ext.define('B4.model.Competition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Competition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'NotifNumber' },
        { name: 'NotifDate' },
        { name: 'File', defaultValue: null },
        { name: 'State', defaultValue: null },
        
        { name: 'ReviewDate' },
        { name: 'ReviewTime' },
        { name: 'ReviewPlace' },
        
        { name: 'ExecutionDate' },
        { name: 'ExecutionTime' },
        { name: 'ExecutionPlace' },
        
        { name: 'LotCount' },
        { name: 'LotBidCount' },
        { name: 'WinnersCount' },
        { name: 'ContractCount' },
        { name: 'OpenEnvelopes' },
        { name: 'ReviewBid' }
    ]
});