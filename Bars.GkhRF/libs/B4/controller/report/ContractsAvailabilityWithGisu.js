Ext.define('B4.controller.report.ContractsAvailabilityWithGisu', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ContractsAvailabilityWithGisuPanel',
    mainViewSelector: '#contractsAvailabilityWithGisuPanel',

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
        'SelectWindow.MultiSelectWindow',
        'report.CrWorkTypeSumPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#contractsAvailabilityWithGisuPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#contractsAvailabilityWithGisuPanel #sfProgramCr'
        },
        {
            ref: 'ReportDateField',
            selector: '#contractsAvailabilityWithGisuPanel #dfReportDate'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'contractsAvailabilityMultiselectwindowaspect',
            fieldSelector: '#contractsAvailabilityWithGisuPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#contractsAvailabilityMunicipalitySelectWindow',
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
        var reportDate = this.getReportDateField().getValue();
        var programCr = this.getProgramCrSelectField().getValue();
        
        if (programCr == null || programCr === "") {
            return "Выберите программу капитального ремонта!";
        }

        if (reportDate == null || reportDate == Date.min) {
            return "Не указан параметр \"Дата отчета\"";
        }
        
        return true;
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var programmField = this.getProgramCrSelectField();
        var dateField = this.getReportDateField();

        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (dateField ? dateField.getValue() : null)
        };
    }
});