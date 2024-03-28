Ext.define('B4.controller.report.Report_5_37_1', {
    extend: 'B4.controller.BaseReportController',

    //Указываем главную форму и ее селектор
    mainView: 'B4.view.report.Report_5_37_1Panel',
    mainViewSelector: '#ReportPanel_5_37_1',

    //Указываем ref, для более удобного получения компонентов
    refs: [
        {
            ref: 'StartReportDateField',
            selector: '#ReportPanel_5_37_1 datefield[name=StartReportDate]'
        },
        {
            ref: 'EndReportDateField',
            selector: '#ReportPanel_5_37_1 datefield[name=EndReportDate]'
        }

    ],

    //метод валидации параметров
    validateParams: function () {
        var startDateField = this.getStartReportDateField();
        var endDateField = this.getEndReportDateField();

        if (!startDateField.getValue() || !endDateField.getValue()) {
            return false;
        }

        return true;
    },

    //Метод получения параметров
    //Этот метод вызывваеться после нажатия на кнопку 'Печать', собирает все параметры и отправляет на сервер
    getParams: function () {

        var startDateField = this.getStartReportDateField();
        var endDateField = this.getEndReportDateField();
        return {
            startReportDate: (startDateField ? startDateField.getValue() : null),
            endReportDate: (endDateField ? endDateField.getValue() : null)
        };
    }
});
