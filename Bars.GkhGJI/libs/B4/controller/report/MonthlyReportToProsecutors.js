Ext.define('B4.controller.report.MonthlyReportToProsecutors', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.MonthlyReportToProsecutorsPanel',
    mainViewSelector: '#monthlyReportToProsecutorsPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.ZonalInspectionForSelect',
        'dict.ZonalInspectionForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],
    
    ZonalInspectionTriggerFieldSelector: '#monthlyReportToProsecutorsPanel #tfZonalInspection',
    DateStartFieldSelector: '#monthlyReportToProsecutorsPanel #dfDateStart',
    DateEndFieldSelector: '#monthlyReportToProsecutorsPanel #dfDateEnd',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'monthlyReportToProsecutorsMultiselectwindowaspect',
            fieldSelector: '#monthlyReportToProsecutorsPanel #tfZonalInspection',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#monthlyReportToProsecutorsPanelZonInspSelectWindow',
            storeSelect: 'dict.ZonalInspectionForSelect',
            storeSelected: 'dict.ZonalInspectionForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Зональное наименование', xtype: 'gridcolumn', dataIndex: 'ZoneName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    validateParams: function () {
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        if (!dateStartField.isValid()) {
            return "Не указан параметр \"Дата начала\"";
        }

        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];
        if (!dateEndField.isValid()) {
            return "Не указан параметр \"Дата окончания\"";
        }

        return true;
    },

    getParams: function () {

        var zonIspField = Ext.ComponentQuery.query(this.ZonalInspectionTriggerFieldSelector)[0];
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];

        return {
            zonInspIds: (zonIspField ? zonIspField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});