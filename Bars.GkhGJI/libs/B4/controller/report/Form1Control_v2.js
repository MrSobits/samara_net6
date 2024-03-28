Ext.define('B4.controller.report.Form1Control_v2', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.Form1Control_v2Panel',
    mainViewSelector: '#form1Control_v2Panel',

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

    MunicipalityTriggerFieldSelector: '#form1Control_v2Panel #tfMunicipality',
    DateStartFieldSelector: '#form1Control_v2Panel #dfDateStart',
    DateEndFieldSelector: '#form1Control_v2Panel #dfDateEnd',

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'form1Control_v2PanelMultiselectwindowaspect',
            fieldSelector: '#form1Control_v2Panel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#form1Control_v2PanelMunicipalitySelectWindow',
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
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];
        return (dateStartField && dateStartField.isValid() && dateEndField && dateEndField.isValid());
    },

    getParams: function () {

        var mcpField = Ext.ComponentQuery.query(this.MunicipalityTriggerFieldSelector)[0];
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});