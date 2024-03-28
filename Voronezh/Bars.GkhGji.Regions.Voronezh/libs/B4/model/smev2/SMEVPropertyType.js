Ext.define('B4.model.smev2.SMEVPropertyType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVPropertyType'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState', defaultValue:0 },
        { name: 'RealityObject' },
        { name: 'Room' },
        { name: 'CadastralNumber' },
        { name: 'Municipality'},
        { name: 'RealityObject' },
        { name: 'PublicPropertyLevel', defaultValue: 10  },
        { name: 'Attr1' },
        { name: 'Attr2' },
        { name: 'Attr3' },
        { name: 'Attr4' },
        { name: 'Attr5' },
        { name: 'CalcDate' },
        { name: 'MessageId' },


    ]
});