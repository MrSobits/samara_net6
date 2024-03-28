Ext.define('B4.controller.report.CountOfRequestInRf', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CountOfRequestInRfPanel',
    mainViewSelector: '#countOfRequestInRfPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#countOfRequestInRfPanel #tfMunicipality'
        },        
        {
            ref: 'DateStartField',
            selector: '#countOfRequestInRfPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#countOfRequestInRfPanel #dfdDateEnd'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'countOfRequestInRfMultiselectwindowaspect',
            fieldSelector: '#countOfRequestInRfPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#countOfRequestInRfMunicipalitySelectWindow',
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
        var startDate = this.getDateStartField().getValue();
        var endDate = this.getDateEndField().getValue();
        
        if (startDate == null || startDate == Date.min) {
            return "Не указан параметр \"Дата начала\"";
        }

        if (endDate === null || endDate == Date.min) {
            return "Не указан параметр \"Дата окончания\"";
        }
        
        return true;
    },

    getParams: function () {

        var municipalField = this.getMunicipalityTriggerField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();

        return {            
            municipalityIds: (municipalField ? municipalField.getValue() : null),
            dateStart:(dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});