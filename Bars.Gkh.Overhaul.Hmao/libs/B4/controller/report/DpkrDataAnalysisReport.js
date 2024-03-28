Ext.define('B4.controller.report.DpkrDataAnalysisReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.DpkrDataAnalysisReportPanel',
    mainViewSelector: 'dpkrdataanalysisreportpanel',

    requires: [
        'B4.enums.CoreDecisionType',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.municipality.ByParam',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'dpkrdataanalysisreportpanel #tfMunicipality'
        },
        {
            ref: 'VersionField',
            selector: 'dpkrdataanalysisreportpanel b4combobox'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'municipalityMultiselectwindowaspect',
        fieldSelector: 'dpkrdataanalysisreportpanel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#dpkrdataanalysisreportpanelMunicipalitySelectWindow',
        storeSelect: 'dict.municipality.ByParam',
        storeSelected: 'dict.MunicipalityForSelected',
        columnsGridSelect: [
            {
                header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 2,
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
            }
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
        return true;
    },

    getParams: function () {
        var me = this;
        var municipality = me.getMunicipalitySelectField();
        var version = me.getVersionField();

        return {
            muIds: (municipality ? municipality.getValue() : null),
            programVersion: version.getValue()
        };
    }
});