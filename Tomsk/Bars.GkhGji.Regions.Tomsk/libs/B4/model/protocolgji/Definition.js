//Todo перекрыл модель поскольку добавились поля
Ext.define('B4.model.protocolgji.Definition', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolDefinition'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol', defaultValue: null },
        { name: 'IssuedDefinition', defaultValue: null },
        { name: 'ExecutionDate' },
        { name: 'DocumentDate' },
        { name: 'DocumentNum' },
        { name: 'Description' },
        { name: 'TypeDefinition', defaultValue: 10 },
        { name: 'TimeDefinition' },
        { name: 'DateOfProceedings' },
        { name: 'PlaceReview' }
    ]
});