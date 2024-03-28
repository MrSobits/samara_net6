Ext.define('B4.controller.report.Report_SZ_Security', {
    extend: 'B4.controller.BaseReportController',

    //Указываем главную форму и ее селектор
    mainView: 'B4.view.report.Report_SZ_SecurityPanel',
    mainViewSelector: '#ReportPanel_SZ_Security',

    //Указываем ref, для более удобного получения компонентов
    refs: [
        {
            ref: 'ReportDateField',
            selector: '#ReportPanel_SZ_Security datefield[name=ReportDate]'
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
    //Этот метод вызывваеться после нажатия на кнопку 'Печать', собирает все параметры и отправляет на сервер
    getParams: function () {

        var dateField = this.getReportDateField();
        return {
            reportDate: (dateField ? dateField.getValue() : null)
        };
    }
});
