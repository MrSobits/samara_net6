Ext.define('B4.model.riskorientedmethod.G1Resolution', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'G1Resolution'
    },
    fields: [
        { name: 'ROMCategory' },
        { name: 'Resolution' },
        { name: 'ResolutionNum' },
        { name: 'ResolutionDate' },
        { name: 'ArtLaws' },
    ]
});