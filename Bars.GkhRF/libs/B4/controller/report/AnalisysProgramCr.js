Ext.define('B4.controller.report.AnalisysProgramCr', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.AnalisysProgramCr',
    mainViewSelector: '#rfAnalisysProgramCr',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ProgramCrForSelect',
        'dict.ProgramCrForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.AnalisysProgramCr'
    ],

    refs: [
        {
            ref: 'Municipalities',
            selector: '#rfAnalisysProgramCr #tfMunicipality'
        },
        {
            ref: 'ProgramCr',
            selector: '#rfAnalisysProgramCr #sfProgramCr'
        },
        {
            ref: 'AdditionalProgram',
            selector: '#rfAnalisysProgramCr #tfAddProgram'
        },
        {
            ref: 'ReportDate',
            selector: '#rfAnalisysProgramCr #dfReportDate'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'municipalititesMultiselectwindowaspect',
            fieldSelector: '#rfAnalisysProgramCr #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#AnalisysProgramCrMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
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
            name: 'AddProgramsMultiselectwindowaspect',
            fieldSelector: '#rfAnalisysProgramCr #tfAddProgram',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#AnalisysProgramCrProgramsCrSelectWindow',
            storeSelect: 'dict.ProgramCrForSelect',
            storeSelected: 'dict.ProgramCrForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params.notOnlyHidden = true;
            }
        }
    ],

    init: function () {
        var actions = {};
        actions['#rfAnalisysProgramCr #sfProgramCr'] = { 'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onBeforeLoadProgramCr: function (field, options) {
        options.params = {};
        options.params.notOnlyHidden = true;
    },
    
    validateParams: function () {
        var reportDate = this.getReportDate().getValue();
        var programCr = this.getProgramCr().getValue();
        var municipalities = this.getMunicipalities().getValue();
        
        if (programCr == null || programCr === "") {
            return "Выберите программу капитального ремонта!";
        }

        if (reportDate == null || reportDate == Date.min) {
            return "Не указан параметр \"Дата отчета\"";
        }
        
        if (municipalities == null || municipalities == "") {
            return "Не указан параметр \"Муниципальные образования\"";
        }
        
        return true;
    },

    getParams: function () {
        var mcpField = this.getMunicipalities();
        var programField = this.getProgramCr();
        var addProgramsField = this.getAdditionalProgram();
        var dateReportField = this.getReportDate();

        return {
            programCrId: (programField ? programField.getValue() : null),
            additionalProgrammCrIds: (addProgramsField ? addProgramsField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (dateReportField ? dateReportField.getValue() : null)
        };
    }
});