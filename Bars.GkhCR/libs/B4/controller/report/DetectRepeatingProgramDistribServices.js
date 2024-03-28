Ext.define('B4.controller.report.DetectRepeatingProgramDistribServices', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.DetectRepeatingProgramDistribServicesPanel',
    mainViewSelector: '#detectRepeatingProgramDistribServicesPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ProgramCrObj',
        'dict.ProgramCrForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#detectRepeatingProgramDistribServicesPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#detectRepeatingProgramDistribServicesPanel #sfProgramCr'
        },
        {
            ref: 'DopProgramField',
            selector: '#detectRepeatingProgramDistribServicesPanel #sfDopProgramCr'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'detectRepeatingProgramDistribServicesPanelMultiselectwindowaspect',
            fieldSelector: '#detectRepeatingProgramDistribServicesPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#detectRepeatingProgramDistribServicesPanelMunicipalitySelectWindow',
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
            name: 'dopprogramCrfieldmultiselectwindowaspect',
            fieldSelector: '#detectRepeatingProgramDistribServicesPanel #sfDopProgramCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#dopProgramCrSelectWindow',
            storeSelect: 'dict.ProgramCr',
            storeSelected: 'dict.ProgramCrForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Разрез финансирования', xtype: 'gridcolumn', dataIndex: 'FinanceSourceName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Контрагент', xtype: 'gridcolumn', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } },
                {
                    header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Period/List'
                    }
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    validateParams: function () {
        var prCrId = this.getProgramCrSelectField();
        var dopPrCrId = this.getDopProgramField();
        return (prCrId && prCrId.isValid() && dopPrCrId && dopPrCrId.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var programmField = this.getProgramCrSelectField();
        var dopProgramField = this.getDopProgramField();

        //получаем компоне
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dopProgram: (dopProgramField ? dopProgramField.getValue() : null)
        };
    }
});