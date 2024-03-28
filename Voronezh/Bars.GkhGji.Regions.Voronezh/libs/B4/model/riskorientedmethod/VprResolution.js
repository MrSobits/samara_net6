Ext.define('B4.model.riskorientedmethod.VprResolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VprResolution'
    },
    fields: [
        { name: 'ROMCategory' },
        { name: 'Resolution' },
        { name: 'ResolutionNum' },
        { name: 'ResolutionDate' },
        { name: 'ArtLaws' },
    ]
});