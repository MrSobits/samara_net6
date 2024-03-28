Ext.define('B4.controller.report.PlanedProgramIndicators', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.PlanedProgramIndicatorsPanel',
    mainViewSelector: '#reportPlanedProgramIndicatorsPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#reportPlanedProgramIndicatorsPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#reportPlanedProgramIndicatorsPanel #sfProgramCr'
        }
    ],

    aspects: [
        {
            /*
            аспект взаимодействия триггер-поля мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'builderRegisterMultiselectwindowaspect',
            fieldSelector: '#reportPlanedProgramIndicatorsPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportPlanedProgramIndicatorsPanelMunicipalitySelectWindow',
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
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                // operation.params.levelMun = 1;
            }
        }
    ],
    
    init: function () {
        var actions = {};
        actions['#reportPlanedProgramIndicatorsPanel #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);
        
        this.callParent(arguments);
    },

    validateParams: function () {
        var prCr = this.getProgramCrSelectField();
        return (prCr && prCr.isValid());
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();
        var programmField = this.getProgramCrSelectField();

        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null)
        };
    },
    
    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
    }
});