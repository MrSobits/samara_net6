Ext.define('B4.model.riskorientedmethod.VpResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VpResolution'
    },
    fields: [
        { name: 'ROMCategory' },
        { name: 'Resolution' },
        { name: 'ResolutionDate' },
        { name: 'ArtLaws' },
    ]
});