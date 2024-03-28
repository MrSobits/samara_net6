Ext.define('B4.model.gisrealestate.Indicator', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealEstateIndicator'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});