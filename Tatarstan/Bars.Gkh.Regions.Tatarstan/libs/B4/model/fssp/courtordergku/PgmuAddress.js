Ext.define('B4.model.fssp.courtordergku.PgmuAddress', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PgmuAddress'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ErcCode' },
        { name: 'PostCode' },
        { name: 'Town' },
        { name: 'District' },
        { name: 'Street' },
        { name: 'House' },
        { name: 'Building' },
        { name: 'Apartment' },
        { name: 'Room' }
    ]
});