Ext.define('B4.controller.report.Report_SZ_Collection_Service', {
    extend: 'B4.controller.BaseReportController',

    //Указываем главную форму и ее селектор
    mainView: 'B4.view.report.Report_SZ_Collection_ServicePanel',
    mainViewSelector: '#ReportPanel_SZ_Collection_Service',

    //Указываем ref, для более удобного получения компонентов
    refs: [
        {
            ref: 'ReportDateField',
            selector: '#ReportPanel_SZ_Collection_Service datefield[name=ReportDate]'
        },
        {
            ref: 'ReportTypeCombobox',
            selector: '#ReportPanel_SZ_Collection_Service b4combobox[name=ReportType]'
        },
        {
            ref: 'ReportAreaCombobox',
            selector: '#ReportPanel_SZ_Collection_Service b4combobox[name=ReportArea]'
        },
        {
            ref: 'ReportMunicipalityCombobox',
            selector: '#ReportPanel_SZ_Collection_Service b4combobox[name=ReportMunicipality]'
        }
    ],
    
    init: function () {
        this.control(
            {
                '#ReportPanel_SZ_Collection_Service b4combobox[name=ReportArea]': {
                    change: this.onAreaChanged
                }
            }
        );
    },

    //метод валидации параметров
    validateParams: function () {
        var dateField = this.getReportDateField(),
            typeCombobox = this.getReportTypeCombobox();

        if (!dateField.getValue() || !typeCombobox.getValue()) {
            return false;
        }

        return true;
    },

    //Метод получения параметров
    //Этот метод вызывваеться после нажатия на кнопку 'Печать', собирает все параметры и отправляет на сервер
    getParams: function () {
        var dateField = this.getReportDateField(),
            typeCombobox = this.getReportTypeCombobox();
        return {
            reportDate: (dateField ? dateField.getValue() : null),
            reportType: (typeCombobox ? typeCombobox.getValue() : null)
        };
    },
    
    onAreaChanged: function(combo, value) {
        var municipalityComboboxStore = this.getReportMunicipalityCombobox().getStore();
        municipalityComboboxStore.getProxy().setExtraParam('AreaId', value);
        municipalityComboboxStore.load();
    }
});
