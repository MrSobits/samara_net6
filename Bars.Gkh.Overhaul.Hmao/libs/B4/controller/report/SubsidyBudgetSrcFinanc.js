﻿Ext.define('B4.controller.report.SubsidyBudgetSrcFinanc', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.SubsidyBudgetSrcFinancPanel',
    mainViewSelector: 'reportsubsidybudgetsrcfinancpanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update',
        'dict.municipality.ByParam'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: 'reportsubsidybudgetsrcfinancpanel [name=Municipalities]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportSubsidyBudgetSrcFinancPanelMunicipalityAspect',
            fieldSelector: 'reportsubsidybudgetsrcfinancpanel [name=Municipalities]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportSubsidyBudgetSrcFinancPanelMunicipality',
            storeSelect: 'dict.municipality.ByParam',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 3,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        store: Ext.create('B4.store.dict.municipality.ByParam'),
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' }
                    }
                },
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

    validateParams: function() {
        return true;
    },

    getParams: function() {
        var mcpField = this.getMunicipalityTriggerField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null)
        };
    }
});