Ext.define('B4.controller.report.EconomyByTypeWork', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.EconomyByTypeWorkPanel',
    mainViewSelector: '#economyByTypeWorkPanel',

    views: [
        'report.EconomyByTypeWorkPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    requires: [
        'B4.form.ComboBox',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    refs: [
        {
            ref: 'SfProgramCr1',
            selector: '#economyByTypeWorkPanel #sfProgramCr1'
        },
        {
            ref: 'SfProgramCr2',
            selector: '#economyByTypeWorkPanel #sfProgramCr2'
        },
        {
            ref: 'MunicipalityTriggerField',
            selector: '#economyByTypeWorkPanel #tfMunicipality'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'economyByTypeWorkPanelMultiselectwindowaspect',
            fieldSelector: '#economyByTypeWorkPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#economyByTypeWorkPanelMunicipalitySelectWindow',
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
        var prCr1Id = this.getSfProgramCr1();
        var prCr2Id = this.getSfProgramCr2();
        return (prCr1Id && prCr1Id.isValid() && prCr2Id && prCr2Id.isValid());
    },

    init: function () {
        this.control(
            {
            '#economyByTypeWorkPanel #sfProgramCr1': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            },
            '#economyByTypeWorkPanel #sfProgramCr2': {
                beforeload: function (store, operation) {
                    operation.params = {};
                    operation.params.notOnlyHidden = true;
                }
            }
            }
        );
        this.callParent(arguments);
    },

    getParams: function () {
        var programmField1 = this.getSfProgramCr1();
        var programmField2 = this.getSfProgramCr2();
        var municipalityIdField = this.getMunicipalityTriggerField();

        return {
            programCrId1: (programmField1 ? programmField1.getValue() : null),
            programCrId2: (programmField2 ? programmField2.getValue() : null),
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null)
        };
    }
});