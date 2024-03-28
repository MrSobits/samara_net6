Ext.define('B4.controller.report.RequestsGisuReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RequestsGisuReportPanel',
    mainViewSelector: 'requestsGisuReportPanel',

    requires: [
         'B4.aspects.GkhTriggerFieldMultiSelectWindow',
         'B4.ux.button.Update'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'B4.view.report.RequestsGisuReportPanel'
    ],

    refs: [
        {
            ref: 'MunicipalitySelectField',
            selector: 'requestsGisuReportPanel #tfMunicipality'
        },
        {
            ref: 'FinanceSourceSelectField',
            selector: 'requestsGisuReportPanel #sfFinanceSource'
        },
        {
            ref: 'AddressSelectField',
            selector: 'requestsGisuReportPanel #tfAddress'
        },
        {
            ref: 'ManOrgField',
            selector: 'requestsGisuReportPanel #tfManagingOrganization'
        },
        {
            ref: 'StartDateField',
            selector: 'requestsGisuReportPanel #dfStartDate'
        },
        {
            ref: 'ProgramCrField',
            selector: 'requestsGisuReportPanel #sfProgramCr'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'muncipalityMultiSelectWindowAspect',
            fieldSelector: 'requestsGisuReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#requestsGisuReportPanelMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function(store, operation) {
                operation.params.levelMun = 1;
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'AddressMultiselectwindowaspect',
            fieldSelector: 'requestsGisuReportPanel #tfAddress',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#requestsGisuReportPanelAddressSelectWindow',
            storeSelect: 'RealtyObjectByMo',
            storeSelected: 'RealtyObjectByMo',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            onBeforeLoad: function (store, operation) {
                var muField = this.controller.getMunicipalitySelectField();
                operation.params.settlementId = muField.getValue();
                if (!operation.params.settlementId) {
                    operation.params.settlementId = 0;
                }
            },
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],
    
    beforeAddressLoad: function (field, operation, store) {
        var mrField = this.getMunicipalitySelectField();

        operation.params = operation.params || {};
        operation.params.settlementId = mrField.getValue();
    },

    onMrChange: function (field, newValue) {
        var adressField = this.getAddressSelectField();

        adressField.setDisabled(!newValue);
        adressField.setValue(null);
    },

    init: function () {
        var me = this;

        me.control({
            'requestsGisuReportPanel #tfMunicipality': {
                change: this.onMrChange
            },
            'requestsGisuReportPanel #tfAddress': {
                beforeload: this.beforeAddressLoad
            }
        });

        this.callParent(arguments);
    },


    getParams: function() {
        var manOrgField = this.getManOrgField();
        var financeSourceSelectField = this.getFinanceSourceSelectField();
        var municipalityField = this.getMunicipalitySelectField();
        var startDateField = this.getStartDateField();
        var addressField = this.getAddressSelectField();
        var programCrField = this.getProgramCrField();

        return {
            manOrgId: (manOrgField ? manOrgField.getValue() : null),
            startDate: (startDateField ? startDateField.getValue() : null),
            programCrId: (programCrField ? programCrField.getValue() : null),
            financeSourceId: (financeSourceSelectField ? financeSourceSelectField.getValue() : null),
            municipalityIds: (municipalityField ? municipalityField.getValue() : null),
            roId: (addressField ? addressField.getValue() : null)
        };
    }
});