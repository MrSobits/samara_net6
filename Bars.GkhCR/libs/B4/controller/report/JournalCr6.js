Ext.define('B4.controller.report.JournalCr6', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.JournalCr6Panel',
    mainViewSelector: '#journalCr6Panel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    refs: [
        {
            ref: 'SfProgram',
            selector: '#journalCr6Panel #sfProgramCr'
        },
        {
            ref: 'DfReportDate',
            selector: '#journalCr6Panel #dfReportDate'
        },
        {
            ref: 'TfMunicipality',
            selector: '#journalCr6Panel #tfMunicipality'
        }
    ],

    views: [
        'report.JournalCr6Panel',
        'SelectWindow.MultiSelectWindow'
    ],

    aspects: [
        {
            /*
            аспект взаимодействия триггер-поля мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'JournalCr6Multiselectwindowaspect',
            fieldSelector: '#journalCr6Panel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#journalCr6MunicipalitySelectWindow',
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
        var prCrId = this.getSfProgram();
        var date = this.getDfReportDate();
        return (prCrId && prCrId.isValid() && date && date.isValid());
    },

    getParams: function () {

        var progField = this.getSfProgram();
        var repDateField = this.getDfReportDate();
        var mcpField = this.getTfMunicipality();

        return {
            programCrId: (progField ? progField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (repDateField ? repDateField.getValue() : null)
        };
    }
});