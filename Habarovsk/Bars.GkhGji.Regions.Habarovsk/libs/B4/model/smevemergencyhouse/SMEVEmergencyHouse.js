Ext.define('B4.model.smevemergencyhouse.SMEVEmergencyHouse', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVEmergencyHouse'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'Answer' },
        { name: 'Room' },
        { name: 'Municipality' },
        { name: 'CalcDate' },
        { name: 'MessageId' },
        { name: 'EmergencyTypeSGIO', defaultValue: 10},
        { name: 'Room' },
        { name: 'RealityObject' },
        { name: 'AnswerFile' }
    ]
});