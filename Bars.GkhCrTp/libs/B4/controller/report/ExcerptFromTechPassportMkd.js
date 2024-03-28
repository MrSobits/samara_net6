Ext.define('B4.controller.report.ExcerptFromTechPassportMkd', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ExcerptFromTechPassportMkdPanel',
    mainViewSelector: '#excerptFromTechPassportMkdPanel',

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
        'report.ExcerptFromTechPassportMkdPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#excerptFromTechPassportMkdPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#excerptFromTechPassportMkdPanel #sfProgramCr'
        }
    ],

    aspects: [
        {
            /*
            аспект взаимодействия триггер-поля мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'excerptFromTechPassportMkdPanelMultiselectwindowaspect',
            fieldSelector: '#excerptFromTechPassportMkdPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#excerptFromTechPassportMkdPanelMunicipalitySelectWindow',
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
        return true;
    },
    
    init: function () {
        this.control({
            '#excerptFromTechPassportMkdPanel #sfProgramCr': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
        });
        this.callParent(arguments);
    },

    getParams: function () {

        var mcpField = this.getMunicipalityTriggerField();        
        var programmField = this.getProgramCrSelectField();
        
        return {
            programCrId: (programmField ? programmField.getValue() : null),
            municipality: (mcpField ? mcpField.getValue() : null)
        };
    }
});