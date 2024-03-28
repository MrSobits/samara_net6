Ext.define('B4.controller.report.ProgramCrOwnersSpending', {
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.ProgramCrOwnersSpendingPanel',
    mainViewSelector: '#ProgramCrOwnersSpendingPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected',
        'dict.ProgramCrForSelect',
        'dict.ProgramCrForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#ProgramCrOwnersSpendingPanel #tfMunicipality'
        },
        {
            ref: 'ProgramsCrSelectField',
            selector: '#ProgramCrOwnersSpendingPanel #tfCrPrograms'
        },
        {
            ref: 'FinSourcesField',
            selector: '#ProgramCrOwnersSpendingPanel #tfFinSources'
        },
        {
            ref: 'DateStartField',
            selector: '#ProgramCrOwnersSpendingPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#ProgramCrOwnersSpendingPanel #dfDateEnd'
        }
    ],
    
    ReturnedFieldSelector: '#ProgramCrOwnersSpendingPanel #cbReturned',
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programCrOwnersSpendingMunMultiselectwindowaspect',
            fieldSelector: '#ProgramCrOwnersSpendingPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#ProgramCrOwnersSpendingPanelMunicipalitySelectWindow',
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
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programCrOwnersSpendingFinMultiselectwindowaspect',
            fieldSelector: '#ProgramCrOwnersSpendingPanel #tfFinSources',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#ProgramCrOwnersSpendingPanelFinSourcesSelectWindow',
            storeSelect: 'dict.FinanceSourceSelect',
            storeSelected: 'dict.FinanceSourceSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'programCrOwnersSpendingCrMultiselectwindowaspect',
            fieldSelector: '#ProgramCrOwnersSpendingPanel #tfCrPrograms',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#ProgramCrOwnersSpendingPanelProgramsCrSelectWindow',
            storeSelect: 'dict.ProgramCrForSelect',
            storeSelected: 'dict.ProgramCrForSelected',
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
                        url: '/ProgramCr/List'
                    }
                },
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' } }
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
        var programsCr = this.getProgramsCrSelectField().getValue();
        var startDate = this.getDateStartField().getValue();
        var endDate = this.getDateEndField().getValue();

        if (programsCr == null || programsCr === "") {
            return "Выберите программу капитального ремонта!";
        }
        
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
        var programsField = this.getProgramsCrSelectField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();
        var finSourcesField = this.getFinSourcesField();
        var returned = Ext.ComponentQuery.query(this.ReturnedFieldSelector)[0];
        
        return {
            programCrIds: (programsField ? programsField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            finSources: (finSourcesField ? finSourcesField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null),
            returned: (returned ? returned.getValue() : null)
        };
    }
});