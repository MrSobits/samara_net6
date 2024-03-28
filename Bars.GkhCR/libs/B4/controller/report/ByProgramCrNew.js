Ext.define('B4.controller.report.ByProgramCrNew', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ByProgramCrNewPanel',
    mainViewSelector: '#byProgramCrNewPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ProgramCrForSelect',
        'dict.ProgramCrForSelected'
    ],

    refs: [
        {
            ref: 'TfProgramsCr',
            selector: '#byProgramCrNewPanel #tfProgramsCr'
        },
        {
            ref: 'DfReportDate',
            selector: '#byProgramCrNewPanel #dfReportDate'
        },
        {
            ref: 'TfMunicipality',
            selector: '#byProgramCrNewPanel #tfMunicipality'
        }
    ],

    views: [
        'report.ByProgramCrNewPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'byProgramCrNewMultiselectwindowaspect',
            fieldSelector: '#byProgramCrNewPanel #tfProgramsCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#byProgramCrNewPanelProgramsCrSelectWindow',
            storeSelect: 'dict.ProgramCrForSelect',
            storeSelected: 'dict.ProgramCrForSelected',
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
                        url: '/ProgramCr/List'
                    }
                },
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params = {};
                operation.params.notOnlyHidden = true;
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'ByProgramCrNewMultiselectwindowaspect',
            fieldSelector: '#byProgramCrNewPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#byProgramCrNewMunicipalitySelectWindow',
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
        var prCrId = this.getTfProgramsCr();
        var date = this.getDfReportDate();
        return (prCrId && prCrId.isValid() && date && date.isValid());
    },

    getParams: function () {

        var programsField = this.getTfProgramsCr();
        var repDateField = this.getDfReportDate();
        var mcpField = this.getTfMunicipality();

        return {
            programCrIds: (programsField ? programsField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (repDateField ? repDateField.getValue() : null)
        };
    }
});