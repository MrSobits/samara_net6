Ext.define('B4.model.outdoor.Image', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OutdoorImage'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateImage' },
        { name: 'ImagesGroup'},
        { name: 'Name' },
        { name: 'Period' },
        { name: 'WorkCr' },
        { name: 'File' },
        { name: 'Description' },
        { name: 'Outdoor' }
    ]
});