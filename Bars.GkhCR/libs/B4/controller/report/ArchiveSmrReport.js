Ext.define('B4.controller.report.ArchiveSmrReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ArchiveSmrReportPanel',
    mainViewSelector: 'archiveSmrReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.ArchiveSmrReportPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: 'archiveSmrReportPanel gkhtriggerfield[name=Municipalities]'
        },
        {
            ref: 'ProgramCrField',
            selector: 'archiveSmrReportPanel [name=ProgramCr]'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'archiveSmrReportMultiselectwindowaspect',
        fieldSelector: 'archiveSmrReportPanel gkhtriggerfield[name=Municipalities]',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#archiveSmrReportPanelMunicipalitySelectWindow',
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

    validateParams: function () {
        return true;
    },

    init: function () {
        var actions = {};
        actions['archiveSmrReportPanel [name=ProgramCr]'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
    },

    getParams: function () {

        var programCrField = this.getProgramCrField();
        var mcpField = this.getMunicipalityTriggerField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null)
        };
    }
});