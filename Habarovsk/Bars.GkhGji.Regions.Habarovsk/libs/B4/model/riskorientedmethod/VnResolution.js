Ext.define('B4.model.riskorientedmethod.VnResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VnResolution'
    },
    fields: [
        { name: 'ROMCategory' },
        { name: 'Resolution' },
        { name: 'ResolutionNum' },
        { name: 'ResolutionDate' },
        { name: 'ArtLaws' },
    ]
});