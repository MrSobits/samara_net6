Ext.define('B4.controller.report.Report_3_63_3', {
    extend: 'B4.controller.BaseReportController',

    //Указываем главную форму и ее селектор
    mainView: 'B4.view.report.Report_3_63_3Panel',
    mainViewSelector: '#ReportPanel_3_63_3',

    //Указываем ref, для более удобного получения компонентов
    refs: [
        {
            ref: 'ReportDateField',
            selector: '#ReportPanel_3_63_3 datefield[name=ReportDate]'
        }
    ],

    //метод валидации параметров
    validateParams: function () {
        var dateField = this.getReportDateField();
        if (!dateField.getValue()) {
            return false;
        }

        return true;
    },

    //Метод получения параметров
    //Этот метод вызываеться после нажатия на кнопку 'Печать', собирает все параметры и отправляет на сервер
    getParams: function () {

        var dateField = this.getReportDateField();
        return {
            reportDate: (dateField ? dateField.getValue() : null)
        };
    }
});
