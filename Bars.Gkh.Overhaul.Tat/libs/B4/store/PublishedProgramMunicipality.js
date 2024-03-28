Ext.define('B4.store.PublishedProgramMunicipality', {
    extend: 'B4.base.Store',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublishedProgramRecord',
        listAction: 'PublishedProgramMunicipalityList'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});