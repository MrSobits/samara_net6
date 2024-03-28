Ext.define('B4.controller.report.InformationRepublicProgramCrPart2', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.InformationRepublicProgramCrPart2Panel',
    mainViewSelector: '#informationRepublicProgramCrPart2Panel',

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
            selector: '#informationRepublicProgramCrPart2Panel #tfMunicipality'
        },
        {
            ref: 'DateStartField',
            selector: '#informationRepublicProgramCrPart2Panel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#informationRepublicProgramCrPart2Panel #dfDateEnd'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'informationRepublicProgramCrPart2Multiselectwindowaspect',
            fieldSelector: '#informationRepublicProgramCrPart2Panel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#informationRepublicProgramCrPart2PanelMunicipalitySelectWindow',
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
            return "Не указан параметр \"Начало периода\"";
        }

        if (endDate === null || endDate == Date.min) {
            return "Не указан параметр \"Окончание периода\"";
        }
        
        return true;
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});