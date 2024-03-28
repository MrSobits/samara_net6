Ext.define('B4.controller.report.TestReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.TestReport',
    mainViewSelector: '#testReportPanel',

    stores: ['dict.Municipality'],
    
    refs: [
        {
            ref: 'FieldMunicipality',
            selector: '#testReportPanel b4selectfield[name=Municipality]'
        }
    ],

    //Метод валидации параметров, в этом методе необходимо проверять,
    //если какие-то параметры пользователем заполнены неверно,
    //то необходимо показывать об этом сообщение
    validateParams: function () {
        
        return true;
    },

    //Метод получения параметров
    getParams: function () {
        
        var municipalityField = this.getFieldMunicipality();
        
        return {
            municipalityId: (municipalityField ? municipalityField.getValue() : null)
        };
    }
});