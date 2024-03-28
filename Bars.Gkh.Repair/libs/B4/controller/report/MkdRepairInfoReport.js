Ext.define('B4.controller.report.MkdRepairInfoReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.MkdRepairInfoReport.Panel',
    mainViewSelector: 'mkdrepairinforeportpanel',

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
            selector: 'mkdrepairinforeportpanel #sfProgram'
        },
        {
            ref: 'DfReportDate',
            selector: 'mkdrepairinforeportpanel #dfReportDate'
        },
        {
            ref: 'TfMunicipality',
            selector: 'mkdrepairinforeportpanel #tfMunicipality'
        }
    ],

    views: [
        'report.MkdRepairInfoReport.Panel',
        'SelectWindow.MultiSelectWindow'
    ],

    aspects: [
        {
            /*
            аспект взаимодействия триггер-поля мун. образований и таблицы объектов КР
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'mkdrepairinforeportMultiselectwindowaspect',
            fieldSelector: 'mkdrepairinforeportpanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#mkdInfoMunicipalitySelectWindow',
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
                }
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
            programId: (progField ? progField.getValue() : null),
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            reportDate: (repDateField ? repDateField.getValue() : null)
        };
    }
});