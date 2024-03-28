Ext.define('B4.controller.report.RegisterPayOrder', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RegisterPayOrderPanel',
    mainViewSelector: '#registerPayOrderPanel',

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
            selector: '#registerPayOrderPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrField',
            selector: '#registerPayOrderPanel #sfProgramCr'
        },
        {
            ref: 'DateStartField',
            selector: '#registerPayOrderPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#registerPayOrderPanel #dfDateEnd'
        },
        {
            ref: 'TypeFinGroupField',
            selector: '#registerPayOrderPanel #cbxTypeFinGroup'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'registerPayOrderMultiselectwindowaspect',
        fieldSelector: '#registerPayOrderPanel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#registerPayOrderPanelMunicipalitySelectWindow',
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
        var prCr = this.getProgramCrField();
        var dateStart = this.getDateStartField();
        var dateEnd = this.getDateEndField();
        return (prCr && prCr.isValid() && dateStart && dateStart.isValid() && dateEnd && dateEnd.isValid());
    },

    init: function () {
        var actions = {};
        actions['#registerPayOrderPanel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
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
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();
        var typeFinGroupField = this.getTypeFinGroupField();

        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null),
            typeFinGroup: (typeFinGroupField ? typeFinGroupField.getValue() : null)
        };
    }
});