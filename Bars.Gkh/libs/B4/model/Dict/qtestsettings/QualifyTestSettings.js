Ext.define('B4.model.dict.qtestsettings.QualifyTestSettings', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QualifyTestSettings'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AcceptebleRate' },
        { name: 'CorrectBall' },
        { name: 'DateTo' },
        { name: 'QuestionsCount' },
        { name: 'TimeStampMinutes' },
        { name: 'DateFrom' }
    ]
});