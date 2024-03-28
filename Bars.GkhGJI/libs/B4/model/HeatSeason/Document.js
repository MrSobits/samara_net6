Ext.define('B4.model.heatseason.Document', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'HeatSeasonDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'HeatingSeason', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'TypeDocument', defaultValue: 10 },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'Description' },
        { name: 'State', defaultValue: null },
        { name: 'ManOrgName' },
        { name: 'TypeHouse' },
        { name: 'HeatingSystem' },
        { name: 'MunicipalityName' },
        { name: 'Address' }
    ]
});