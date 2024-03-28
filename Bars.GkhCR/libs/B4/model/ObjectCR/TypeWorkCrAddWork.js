Ext.define('B4.model.objectcr.TypeWorkCrAddWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeWorkCrAddWork'
    },
    fields: [
        { name: 'Id', useNull: true },       
        { name: 'TypeWorkCr' },
        { name: 'AdditWorkName' },
        { name: 'AdditWork' }, 
        { name: 'Queue' }, 
        { name: 'DateStartWork' },
        { name: 'DateEndWork' },
        { name: 'Required' }
    ]
});