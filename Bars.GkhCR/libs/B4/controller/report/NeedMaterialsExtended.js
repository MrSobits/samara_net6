Ext.define('B4.controller.report.NeedMaterialsExtended', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.NeedMaterialsExtendedReportPanel',
    mainViewSelector: '#needMaterialsExtendedReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.WorkSelect',
        'dict.WorkSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: '#needMaterialsExtendedReportPanel #sfMunicipality'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#needMaterialsExtendedReportPanel #sfProgramCr'
        },
        {
            ref: 'ReportDateTriggerField',
            selector: '#needMaterialsExtendedReportPanel #dfReportDate'
        },
        {
            ref: 'TypeWorkCrTriggerField',
            selector: '#needMaterialsExtendedReportPanel #tfTypeWorkCr'
        },
        {
            ref: 'ConditionField',
            selector: '#needMaterialsExtendedReportPanel #cbCondition'
        },
        {
            ref: 'SumField',
            selector: '#needMaterialsExtendedReportPanel #nfSum'
        }
    ],
    
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'needMaterialsExtendedReportPanelWorkTypeMultiselectwindowaspect',
            fieldSelector: '#needMaterialsExtendedReportPanel #tfTypeWorkCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#needMaterialsExtendedReportPanelWorkTypeSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store, operation) {
                operation.params = {};
                operation.params.onlyByWorkId = true;
                operation.params.ids = "38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,66,67,68,69";
            }
        }
    ],
    
    validateParams: function () {
        var muId = this.getMunicipalitySelectField();
        var prCrId = this.getProgramCrSelectField();
        var date = this.getReportDateTriggerField();
        var sum = this.getSumField();
        var cond = this.getConditionField();
        return (prCrId && prCrId.isValid() && muId && muId.isValid() && date && date.isValid() && sum && sum.isValid() && cond && cond.isValid());
    },
    
    getParams: function () {

        var programCrField = this.getProgramCrSelectField();
        var mcpField = this.getMunicipalitySelectField();
        var reportDateField = this.getReportDateTriggerField();
        var typeWorkCrField = this.getTypeWorkCrTriggerField();
        var sumField = this.getSumField();
        var conditionField = this.getConditionField();

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            programCr: (programCrField ? programCrField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            typeWorkCr: (typeWorkCrField ? typeWorkCrField.getValue() : null),
            sum: (sumField ? sumField.getValue() : null),
            condition: (conditionField ? conditionField.getValue() : null)

        };
    }
});