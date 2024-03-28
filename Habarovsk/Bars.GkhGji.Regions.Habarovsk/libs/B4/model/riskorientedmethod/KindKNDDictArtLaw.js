Ext.define('B4.model.riskorientedmethod.KindKNDDictArtLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KindKNDDictArtLaw'
    },
    fields: [
         { name: 'Id', useNull: true },
        { name: 'KindKNDDict' },
        { name: 'ArticleLawGji' },
        { name: 'Name' },
        { name: 'Koefficients' }
    ]
});