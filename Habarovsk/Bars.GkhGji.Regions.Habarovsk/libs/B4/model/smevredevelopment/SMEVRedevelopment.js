Ext.define('B4.model.smevredevelopment.SMEVRedevelopment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVRedevelopment'
    },
    fields: [
        { name: 'ReqId'},
        { name: 'RequestDate'},
        { name: 'Inspector'},
        { name: 'RequestState' },
        { name: 'Answer' },
        { name: 'CalcDate' },
        { name: 'GovermentName' },
        { name: 'ActDate' },
        { name: 'ActNum' },
        { name: 'ObjectName' },
        { name: 'Municipality' },
        { name: 'Cadastral' },
        { name: 'RealityObject' },
        { name: 'AnswerFile' },
        { name: 'MessageId' }

    ]
});