Ext.define('B4.controller.report.ReviewAppealsCits', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ReviewAppealsCitsPanel',
    mainViewSelector: '#reviewAppealsCitsPanel',

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
        'SelectWindow.MultiSelectWindow',
        'report.ReviewAppealsCitsPanel'
    ],

    municipalityTriggerFieldSelector: '#reviewAppealsCitsPanel #tfMunicipality',
    dateStartFieldSelector: '#reviewAppealsCitsPanel #dfDateStart',
    dateEndFieldSelector: '#reviewAppealsCitsPanel #dfDateEnd',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'repReviewAppealsCitsMultiSelectWindowAspect',
            fieldSelector: '#reviewAppealsCitsPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reviewAppealsCitsMunicipalitySelectWindow',
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
        var dateStartField = Ext.ComponentQuery.query(this.dateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.dateEndFieldSelector)[0];
        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        return (dateStartField && dateStartField.isValid() && dateEndField && dateEndField.isValid() && mcpField && mcpField.isValid());
    },

    getParams: function () {

        var dateStartField = Ext.ComponentQuery.query(this.dateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.dateEndFieldSelector)[0];
        var mcpField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});