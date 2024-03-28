Ext.define('B4.controller.report.TsjProvidedInfo', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.TsjProvidedInfoReportPanel',
    mainViewSelector: '#tsjProvidedInfoReportPanel',

    MunicipalityTriggerFieldSelector: '#tsjProvidedInfoReportPanel gkhtriggerfield',
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

    aspects: [
      {
          xtype: 'gkhtriggerfieldmultiselectwindowaspect',
          name: 'tsjProvidedInfoReportPanelMultiselectwindowaspect',
          fieldSelector: '#tsjProvidedInfoReportPanel gkhtriggerfield',
          multiSelectWindow: 'SelectWindow.MultiSelectWindow',
          multiSelectWindowSelector: '#tsjProvidedInfoReportPanelMultiSelectWindow',
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

    getParams: function() {

        var mcpField = Ext.ComponentQuery.query(this.MunicipalityTriggerFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null)
        };
    }
});