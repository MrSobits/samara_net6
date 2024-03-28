Ext.define('B4.model.NonResidentialPlacement', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NonResidentialPlacement'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfoRealityObj', defaultValue: null },
        { name: 'TypeContragentDi', defaultValue: 10 },
        { name: 'Area', defaultValue: null },
        { name: 'ContragentName' },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'DocumentNumApartment' },
        { name: 'DocumentDateApartment', defaultValue: null },
        { name: 'DocumentNameApartment' },
        { name: 'DocumentNumCommunal' },
        { name: 'DocumentDateCommunal', defaultValue: null },
        { name: 'DocumentNameCommunal' }
    ]
});