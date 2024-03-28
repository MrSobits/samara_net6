Ext.define('B4.model.claimwork.ActViolIdentification', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'ClaimWork' },
        { name: 'ClaimWorkTypeBase' },
        { name: 'DocumentType' },
        { name: 'DocumentDate' },
        { name: 'DocumentNumber' },
        { name: 'State' },
        { name: 'SendDate' },
        { name: 'FactOfReceiving', defaultValue: 0 },
        { name: 'ActType', defaultValue: 0 },
        { name: 'FactOfSigning', defaultValue: 0 },
        { name: 'SignDate' },
        { name: 'SignTime' },
        { name: 'SignPlace' },
        { name: 'FormDate' },
        { name: 'FormPlace' },
        { name: 'FormTime' },
        { name: 'Persons' },
        { name: 'BaseInfo' },
        { name: 'Municipality' },
        { name: 'Address' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'ActViolIdentificationClw'
    }
});