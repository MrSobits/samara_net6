Ext.define('B4.model.constructionobject.Participant', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ConstructionObjectParticipant',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ConstructionObject', useNull: true },
        { name: 'ParticipantType', defaultValue: 10 },
        { name: 'CustomerType', defaultValue: 10 },
        { name: 'Contragent', defaultValue: null },
        { name: 'ContragentInn', defaultValue: null },
        { name: 'ContragentContactName', defaultValue: null },
        { name: 'ContragentContactPhone', defaultValue: null },
        { name: 'Description', defaultValue: null }
    ]
});