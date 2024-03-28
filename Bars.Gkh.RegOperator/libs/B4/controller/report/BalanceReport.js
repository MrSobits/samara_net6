Ext.define('B4.controller.report.BalanceReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'report.BalanceReportPanel',
    mainViewSelector: 'balanceReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update',
        'B4.store.realityobj.RealityObjectForSelect',
        'B4.store.realityobj.RealityObjectForSelected'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: 'balanceReportPanel #municipalityR'
        },
        {
            ref: 'SettlementTriggerField',
            selector: 'balanceReportPanel #municipalityO'
        },
        {
            ref: 'AddressTriggerField',
            selector: 'balanceReportPanel #tfRealityObjects'
        },
        {
            ref: 'StartDateField',
            selector: 'balanceReportPanel #dfStartDate'
        },
        {
            ref: 'EndDateField',
            selector: 'balanceReportPanel #dfEndDate'
        }
    ],

    aspects: [
      {
          xtype: 'gkhtriggerfieldmultiselectwindowaspect',
          name: 'adressMultiSelectWindowAspect',
          fieldSelector: 'balanceReportPanel #tfRealityObjects',
          multiSelectWindow: 'SelectWindow.MultiSelectWindow',
          multiSelectWindowSelector: '#balanceReportPanelAddressSelectWindow',
          storeSelect: 'realityobj.RealityObjectForSelect',
          storeSelected: 'realityobj.RealityObjectForSelected',
          textProperty: 'Address',
          columnsGridSelect: [
              { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
          ],
          columnsGridSelected: [
              { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
          ],
          titleSelectWindow: 'Выбор записи',
          titleGridSelect: 'Записи для отбора',
          titleGridSelected: 'Выбранная запись',
          onBeforeLoad: function (store, operation) {

              var mrValue = this.controller.getMunicipalityTriggerField().getValue();

              if (mrValue) {
                  operation.params.municipalityId = mrValue;
              }

              var moValue = this.controller.getSettlementTriggerField().getValue();

              if (moValue) {
                  operation.params.settlementId = moValue;
              }
          }
      }
    ],

    validateParams: function () {
        return true;
    },
    
    init: function () {
        var me = this;

        me.control({
            "balanceReportPanel #municipalityR": {
                change: me.onMrChange
            },
            "balanceReportPanel #municipalityO": {
                beforeload: me.beforeMoLoad
            },
            "balanceReportPanel #address": {
                beforeload: me.beforeAddressLoad
            }
        });

        me.callParent(arguments);
    },
    
    onMrChange: function (field, newValue) {
        var me = this,
            moField = me.getSettlementTriggerField();
            //localityField = me.getLocalityTriggerField(),
           // addressField = me.getAddressTriggerField();

        moField.setDisabled(!newValue);
        moField.onTrigger2Click();
       // localityField.onTrigger2Click();
       // addressField.onTrigger2Click();
    },
  
    beforeMoLoad: function (field, operation) {
        var mrField = this.getMunicipalityTriggerField();
        operation.params = operation.params || {};
        operation.params.parentId = mrField.getValue();
    },

    getParams: function () {
        var municipalField = this.getMunicipalityTriggerField();
        var settlementField = this.getSettlementTriggerField();
        var addressField = this.getAddressTriggerField();
        var startDateField = this.getStartDateField();
        var endDateField = this.getEndDateField();
        
        return {
            addressIds: (addressField ? addressField.getValue() : null),
            municipalityIds: (municipalField ? municipalField.getValue() : null),
            settlementIds: (settlementField ? settlementField.getValue() : null),
            startDate: (startDateField ? startDateField.getValue() : null),
            endDate: (endDateField ? endDateField.getValue() : null)
        };
    }
});