Ext.define('B4.model.resolution.ArtLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionArtLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', defaultValue: null },
        { name: 'ArticleLawGji', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Code' },
        { name: 'Description', defaultValue: 'Изменена по решению суда' }
    ]
});