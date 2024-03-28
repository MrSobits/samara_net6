Ext.define('B4.controller.report.CheckingExecutionOfPrescription', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CheckingExecutionOfPrescriptionPanel',
    mainViewSelector: '#checkingExecutionOfPrescriptionPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],
    
    MunicipalityTriggerFieldSelector: '#checkingExecutionOfPrescriptionPanel #tfMunicipality',
    DateStartFieldSelector: '#checkingExecutionOfPrescriptionPanel #dfDateStart',
    DateEndFieldSelector: '#checkingExecutionOfPrescriptionPanel #dfDateEnd',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'checkingExecutionOfPrescriptionMultiselectwindowaspect',
            fieldSelector: '#checkingExecutionOfPrescriptionPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#checkingExecutionOfPrescriptionMunicipalitySelectWindow',
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
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];
        return (dateStartField && dateStartField.isValid() && dateEndField && dateEndField.isValid());
    },

    getParams: function () {

        var mcpField = Ext.ComponentQuery.query(this.MunicipalityTriggerFieldSelector)[0];
        var dateStart = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEnd = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];
        
        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStart ? dateStart.getValue() : null),
            dateEnd: (dateEnd ? dateEnd.getValue() : null)
        };
    }
});