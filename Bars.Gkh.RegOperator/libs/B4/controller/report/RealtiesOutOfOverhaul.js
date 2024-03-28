Ext.define('B4.controller.report.RealtiesOutOfOverhaul', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RealtiesOutOfOverhaulPanel',
    mainViewSelector: 'reportRealtiesOutOfOverhaulPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.municipality.ByParam',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'report.RealtiesOutOfOverhaulPanel'
    ],

    refs: [
        {
            ref: 'MunicipalityField',
            selector: 'reportRealtiesOutOfOverhaulPanel #tfMunicipality'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportRealtiesOutOfOverhaulMunicipalityMultiSelectWindowAspect',
            fieldSelector: 'reportRealtiesOutOfOverhaulPanel [name=Municipality]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportRealtiesOutOfOverhaulMunicipalityMultiSelectWindow',
            storeSelect: 'dict.municipality.ByParam',
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
        var muId = this.getMunicipalityField();
        return muId && muId.isValid();
    },

    getParams: function () {
        var moField = this.getMunicipalityField();

        return {
            municipalityIds: (moField ? moField.getValue() : null)
        };
    }
});