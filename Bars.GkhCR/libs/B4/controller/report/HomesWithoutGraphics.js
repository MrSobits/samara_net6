Ext.define('B4.controller.report.HomesWithoutGraphics', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.HomesWithoutGraphicsPanel',
    mainViewSelector: '#homesWithoutGraphicsPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.WorksProgressPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#homesWithoutGraphicsPanel #tfMunicipality'
        },
        {
            ref: 'ProgramCrField',
            selector: '#homesWithoutGraphicsPanel #sfProgramCr'
        },
        {
            ref: 'FinSourcesTriggerField',
            selector: '#homesWithoutGraphicsPanel #tfFinSources'
        },
        {
            ref: 'ReportDateField',
            selector: '#homesWithoutGraphicsPanel #dfReportDate'
        }
    ],

    aspects: [
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'homesWithoutGraphicsMultiselectwindowaspect',
        fieldSelector: '#homesWithoutGraphicsPanel #tfMunicipality',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#homesWithoutGraphicsMunicipalitySelectWindow',
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
    },
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'finSourcesMultiselectwindowaspect',
        fieldSelector: '#homesWithoutGraphicsPanel #tfFinSources',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#homesWithoutGraphicsFinSourcesSelectWindow',
        storeSelect: 'dict.FinanceSourceSelect',
        storeSelected: 'dict.FinanceSourceSelected',
        columnsGridSelect: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
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
        var finId = this.getFinSourcesTriggerField();
        var reportDate = this.getReportDateField();
        return (reportDate && reportDate.isValid() && prCrId && prCrId.isValid() && finId && finId.isValid());
    },

    getParams: function () {

        var programCrField = this.getProgramCrField();
        var municipalityIdField = this.getMunicipalityTriggerField();
        var finSourcesField = this.getFinSourcesTriggerField();
        var reportDateField = this.getReportDateField();
        
        return {
            programCrId: (programCrField ? programCrField.getValue() : null),
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            finSourceIds: (finSourcesField ? finSourcesField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});