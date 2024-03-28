Ext.define('B4.model.tatarstanprotocolgji.TatarstanProtocolGjiEyewitness', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanDocumentWitness'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentGji' },
        { name: 'WitnessType' },
        { name: 'Fio' },
        { name: 'FactAddress' },
        { name: 'Phone' }
    ]
});