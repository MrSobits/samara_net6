Ext.define('B4.model.dict.MunicipalityTree', {
    extend: 'Ext.data.Model',
    idProperty: 'id',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Group' },
        { name: 'Name' },
        { name: 'text' },
        { name: 'Okato' },
        { name: 'Description' },
        { name: 'FederalNumber' },
        { name: 'Cut' },
        { name: 'FiasId' },
        { name: 'DinamicFias', defaultValue: null },
        { name: 'Code' },
        { name: 'CheckCertificateValidity' },
        { name: 'Oktmo' },
          { name: 'Level' },
        { name: 'ParentMoId' }
    ]
});