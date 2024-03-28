Ext.define('B4.model.gisrealestate.realestatetype.RealEstateType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisRealEstateType'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Group' }
    ]
});