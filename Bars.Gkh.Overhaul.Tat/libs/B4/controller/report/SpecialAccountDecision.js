﻿Ext.define('B4.controller.report.SpecialAccountDecision', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.SpecialAccountDecisionPanel',
    mainViewSelector: '#reportSpecialAccountDecisionPanel',

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
        { ref: 'municipalityTriggerField', selector: '#reportSpecialAccountDecisionPanel #tfMunicipality' },
        { ref: 'dateTimeReportField', selector: '#reportSpecialAccountDecisionPanel datefield[name="DateTimeReport"]' },
        { ref: 'hasInDpkrField', selector: '#reportSpecialAccountDecisionPanel [name = HasInDpkr]' },
        { ref: 'byActualHoldersField', selector: '#reportSpecialAccountDecisionPanel [name = ByActualHolders]' }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportSpecialAccountDecisionPanelMultiselectwindowaspect',
            fieldSelector: '#reportSpecialAccountDecisionPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportSpecialAccountDecisionPanelMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
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

    validateParams: function() {
        return true;
    },

    getParams: function() {
        var mcpField = this.getMunicipalityTriggerField(),
            dateTimeReportField = this.getDateTimeReportField(),
            hasInDpkr = this.getHasInDpkrField(),
            byActualHolders = this.getByActualHoldersField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateTimeReport: (dateTimeReportField ? dateTimeReportField.getValue() : null),
            hasInDpkr: (hasInDpkr ? hasInDpkr.getValue() : null),
            byActualHolders: byActualHolders.getValue()
        };
    }
});