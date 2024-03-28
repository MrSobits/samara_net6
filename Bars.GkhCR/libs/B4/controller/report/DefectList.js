Ext.define('B4.controller.report.DefectList', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.DefectListPanel',
    mainViewSelector: '#defectListPanel',

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
        'report.DefectListPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#defectListPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrField',
            selector: '#defectListPanel #sfProgramCr'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'defectListMultiselectwindowaspect',
        fieldSelector: '#defectListPanel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#defectListPanelMunicipalitySelectWindow',
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
        var prCrId = this.getProgramCrField();
        return (prCrId && prCrId.isValid());
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