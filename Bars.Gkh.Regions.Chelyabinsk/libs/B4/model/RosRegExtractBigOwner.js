Ext.define('B4.model.RosRegExtractBigOwner', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'RosRegExtractBigOwner'
    },
    fields: [
        { name: 'Id' },
        { name: 'ExtractId' },
        { name: 'OwnerName' },
        { name: 'AreaShareNum' },
        { name: 'AreaShareDen' },
        { name: 'RightNumber' }        
    ]
});