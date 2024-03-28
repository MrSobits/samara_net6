Ext.define('B4.model.gisrealestate.realestatetype.RealEstateTypeGroup', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisRealEstateTypeGroup'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});