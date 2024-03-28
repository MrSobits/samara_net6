Ext.define('B4.controller.report.OperationalDataOfPayments', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.OperationalDataOfPaymentsPanel',
    mainViewSelector: '#operationalDataOfPaymentsPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    municipalityTriggerFieldSelector: '#operationalDataOfPaymentsPanel #tfMunicipality',
    reportDateSelector: '#operationalDataOfPaymentsPanel #dfReportDate',

    views: [
        'report.OperationalDataOfPaymentsPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'operationalDataOfPaymentsPanelMultiselectwindowaspect',
            fieldSelector: '#operationalDataOfPaymentsPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#operationalDataOfPaymentsPanelMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
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
        var date = Ext.ComponentQuery.query(this.reportDateSelector)[0];
        return date && date.isValid();
    },

    getParams: function () {
        var repDateField = Ext.ComponentQuery.query(this.reportDateSelector)[0];
        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (repDateField ? repDateField.getValue() : null)
        };
    }
});