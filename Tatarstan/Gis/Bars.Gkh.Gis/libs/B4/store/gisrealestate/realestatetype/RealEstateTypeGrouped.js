Ext.define('B4.store.gisrealestate.realestatetype.RealEstateTypeGrouped', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.gisrealestate.realestatetype.RealEstateTypeGrouped'],
    autoLoad: false,
    root: {
        expanded: true
    },
    model: 'B4.model.gisrealestate.realestatetype.RealEstateTypeGrouped'
});