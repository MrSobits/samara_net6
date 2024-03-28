Ext.define('B4.model.priorityparam.Quality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QualityPriorityParam'
    },
    fields: [
        { name: 'Id' },
        { name: 'EnumDisplay' },
        { name: 'Value' },
        { name: 'Point' },
        { name: 'Code' }
    ]
});