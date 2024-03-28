Ext.define('B4.controller.report.ListHousesByProgramCr', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ListHousesByProgramCrPanel',
    mainViewSelector: '#listHousesByProgramCrPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],
    
    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ProgramCrForSelect',
        'dict.ProgramCrForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'SfProgramsCr',
            selector: '#listHousesByProgramCrPanel #sfProgramsCr'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: '#listHousesByProgramCrPanel #tfMunicipality'
        },
        {
            ref: 'ReportDateField',
            selector: '#listHousesByProgramCrPanel #dfReportDate'
        },
        {
            ref: 'AssemblyToField',
            selector: '#listHousesByProgramCrPanel #cbAssemblyTo'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'MunMultiselectwindowaspect',
            fieldSelector: '#listHousesByProgramCrPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#listHousesByProgramCrPanelMunicipalitySelectWindow',
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
            name: 'ProgramCrMultiselectwindowaspect',
            fieldSelector: '#listHousesByProgramCrPanel #sfProgramsCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#listHousesByProgramCrPanelProgramsCrSelectWindow',
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
        var programsCr = this.getSfProgramsCr().getValue();
        var reportDate = this.getReportDateField().getValue();
        
        if (programsCr == null || programsCr === "") {
            return "Не указан параметр \"Программы кап. ремонта\"!";
        }
        if (reportDate == null || reportDate == Date.min) {
            return "Не указан параметр \"Дата отчета\"";
        }

        return true;
    },

    getParams: function () {
        var programmField = this.getSfProgramsCr();
        var municipalityIdField = this.getMunicipalityTriggerField();
        var reportDateField = this.getReportDateField();
        var assemblyToField = this.getAssemblyToField();

        return {
            programCrIds: (programmField ? programmField.getValue() : null),
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            assemblyTo: (assemblyToField ? assemblyToField.getValue() : null)
        };
    }
});