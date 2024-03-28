Ext.define('B4.controller.report.WeeklyDI', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.WeeklyDIPanel',
    mainViewSelector: '#weeklyDIPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#weeklyDIPanel #tfMunicipality'
        },        
        {
            ref: 'PeriodDiSelectField',
            selector: '#weeklyDIPanel #sfPeriodDi'
        },
        {
            ref: 'DiStartedField',
            selector: '#weeklyDIPanel #fDiStarted'
        },
        {
            ref: 'DiFullField',
            selector: '#weeklyDIPanel #fDiFull'
        },
        {
            ref: 'TranssferredManagCombobox',
            selector: '#weeklyDIPanel #cbTranssferredManag'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'weeklyDIPanelMultiselectwindowaspect',
            fieldSelector: '#weeklyDIPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#weeklyDIPanelMunicipalitySelectWindow',
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

    getParams: function () {
        var munucipalities = this.getMunicipalityTriggerField();
        var period = this.getPeriodDiSelectField();
        var diStarted = this.getDiStartedField();
        var diFull = this.getDiFullField();
        var transsferredManagCombobox = this.getTranssferredManagCombobox();
        return {
            municipalityIds: (munucipalities ? munucipalities.getValue() : null),
            period: (period ? period.getValue() : null),
            diStarted: (diStarted ? diStarted.getValue() : null),
            diFull: (diFull ? diFull.getValue() : null),
            transsferredManag: (transsferredManagCombobox ? transsferredManagCombobox.getValue() : null)
        };
    }
});