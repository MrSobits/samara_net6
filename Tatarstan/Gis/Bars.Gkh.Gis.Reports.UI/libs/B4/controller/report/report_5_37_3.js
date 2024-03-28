Ext.define('B4.controller.report.Report_5_37_3', {
    extend: 'B4.controller.BaseReportController',

    //Указываем главную форму и ее селектор
    mainView: 'B4.view.report.Report_5_37_3Panel',
    mainViewSelector: '#ReportPanel_5_37_3',

    stores: ['dict.Municipality'],
    
    //Указываем ref, для более удобного получения компонентов
    refs: [
        {
            ref: 'StartReportDateField',
            selector: '#ReportPanel_5_37_3 datefield[name=StartReportDate]'
        },
        {
            ref: 'EndReportDateField',
            selector: '#ReportPanel_5_37_3 datefield[name=EndReportDate]'
        },
        {
            ref: 'RegionField',
            selector: '#ReportPanel_5_37_3 b4selectfield[name=Region]'
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
        var regionField = this.getRegionField();
        
        return {
            startReportDate: (startDateField ? startDateField.getValue() : null),
            endReportDate: (endDateField ? endDateField.getValue() : null),
            regionId: (regionField ? regionField.getValue() : null)
        };
    }
});
