Ext.define('B4.controller.report.InformationCashReceivedRegionalFund', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.InformationCashReceivedRegionalFundPanel',
    mainViewSelector: '#informationCashReceivedRegionalFundPanel',

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

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#informationCashReceivedRegionalFundPanel #tfMunicipality'
        },
        {
            ref: 'DateStartField',
            selector: '#informationCashReceivedRegionalFundPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#informationCashReceivedRegionalFundPanel #dfDateEnd'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'informationCashReceivedRegionalFundMultiselectwindowaspect',
            fieldSelector: '#informationCashReceivedRegionalFundPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#informationCashReceivedRegionalFundPanelMunicipalitySelectWindow',
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

        var mcpField = this.getMunicipalityTriggerField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();

        //получаем компоне
        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});