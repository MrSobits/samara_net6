﻿Ext.define('B4.controller.report.RegisterMkdByTypeRepair', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RegisterMkdByTypeRepairPanel',
    mainViewSelector: '#RegisterMkdByTypeRepairPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#RegisterMkdByTypeRepairPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#RegisterMkdByTypeRepairPanel #sfProgramCr'
        },
        {
            ref: 'FinancialTriggerField',
            selector: '#RegisterMkdByTypeRepairPanel #tfFinancial'
        }
    ],

    aspects: [
        {
            /*
            аспект взаимодействия триггер-поля мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'municipMultiselectwindowaspect',
            fieldSelector: '#RegisterMkdByTypeRepairPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#RegisterMkdByTypeRepairPanelMunicipalitySelectWindow',
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
            name: 'financialMultiselectwindowaspect',
            fieldSelector: '#RegisterMkdByTypeRepairPanel #tfFinancial',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#RegisterMkdByTypeRepairPanelFinancialSelectWindow',
            storeSelect: 'dict.FinanceSourceSelect',
            storeSelected: 'dict.FinanceSourceSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'//,
            //textProperty: 'Name'
        }
    ],

    init: function () {
        var actions = {};
        actions['#RegisterMkdByTypeRepairPanel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
    },

    validateParams: function () {
        var prCrId = this.getProgramCrSelectField();
        var fSrc = this.getFinancialTriggerField();
        return (prCrId && prCrId.isValid() && fSrc && fSrc.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var finField = this.getFinancialTriggerField();
        var programmField = this.getProgramCrSelectField();


        //получаем компоне
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            financialSourceIds: (finField ? finField.getValue() : null)
        };
    }
});