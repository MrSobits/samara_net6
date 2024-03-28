/**
* модель подтематики обращений
*/
Ext.define('B4.model.dict.StatSubsubjectGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StatSubsubjectGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'StatSubject', defaultValue: null },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'ISSOPR' },
        { name: 'SSTUNameSub' },
        { name: 'SSTUCodeSub' },
        { name: 'TrackAppealCits' },
        { name: 'AppealAnswerText' },
        { name: 'AppealAnswerText2' }
    ]
});