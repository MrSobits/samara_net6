Ext.define('B4.controller.report.Report_10_61_1', {
    extend: 'B4.controller.BaseGisReportController',

    //Указываем главную форму и ее селектор
    mainView: 'B4.view.report.Report_10_61_1Panel',
    mainViewSelector: '#ReportPanel_10_61_1',

    //Указываем ref, для более удобного получения компонентов
    refs: [
        {
            ref: 'ReportDateField',
            selector: '#ReportPanel_10_61_1 b4gismonthpicker[name=ReportDate]'
        },
        {
            ref: 'DistrictField',
            selector: '#ReportPanel_10_61_1 b4selectfield[name=District]'
        },
        {
            ref: 'ServiceField',
            selector: '#ReportPanel_10_61_1 combobox[name=Service]'
        }
    ],

    //Метод получения параметров
    //Этот метод вызывваеться после нажатия на кнопку 'Печать', собирает все параметры и отправляет на сервер
    getParams: function () {
        var me = this,
            date = me.getReportDateField().getValue(),
            districtId = me.getDistrictField().getValue(),
            serviceId = me.getServiceField().getValue();
        
        return {
            reportDate: date,
            districtId: districtId,
            serviceId: serviceId
        };
    }
});
