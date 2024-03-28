Ext.define('B4.controller.report.ChargeReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.ChargeReportPanel',
    mainViewSelector: 'chargeReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
    ],

    stores: [
        'dict.StateForSelect',
        'dict.StateForSelected',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'manorg.ForSelect',
        'manorg.ForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: 'chargeReportPanel #tfMunicipality'
        },
        {
            ref: 'StartDateField',
            selector: 'chargeReportPanel #dfStartDate'
        },
        {
            ref: 'EndDateField',
            selector: 'chargeReportPanel #dfEndDate'
        },
        {
            ref: 'ManagingOrganizationTriggerField',
            selector: 'chargeReportPanel #tfManagingOrganization'
        },
        {
            ref: 'StateTriggerField',
            selector: 'chargeReportPanel #tfStates'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'chargeReportManOrgMultiselectwindowaspect',
            fieldSelector: 'chargeReportPanel #tfManagingOrganization',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#chargeReportManOrgSelectWindow',
            storeSelect: 'manorg.ForSelect',
            storeSelected: 'manorg.ForSelected',
            textProperty: 'ContragentName',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' }},
                { header: 'Мун. образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' }}

            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ContragentName', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор управляющей организации',
            titleGridSelect: 'Управляющая организация для отбора',
            titleGridSelected: 'Выбранная управляющая организация'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'chargeReportMunicipalityMultiselectwindowaspect',
            fieldSelector: 'chargeReportPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#chargeReportMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор МО',
            titleGridSelect: 'МО для отбора',
            titleGridSelected: 'Выбранное МО'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'chargeReportstateMultiselectwindowaspect',
            fieldSelector: 'chargeReportPanel #tfStates',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#chargeReportStateSelectWindow',
            storeSelect: 'dict.StateForSelect',
            storeSelected: 'dict.StateForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор статуса',
            titleGridSelect: 'Статус для отбора',
            titleGridSelected: 'Выбранный статус',
            onBeforeLoad: function (store, operation) {
                operation.params.typeId = 'rf_transfer_record';
            },
            updateSelectedGrid: function() {
                var me = this,
                    grid = me.getSelectedGrid(),
                    storeSelected;
                
                if (grid || me.selectedData) {
                    storeSelected = grid.getStore();
                    me.selectedData.each(function (rec) {
                        storeSelected.add(rec);
                    });
                }
            },
            listeners: {
                getdata: function(asp, data) {
                    asp.selectedData = data;
                }
            }
        }
    ],

    validateParams: function () {
        var dateStartField = this.getStartDateField().getValue();
        var dateEndField = this.getEndDateField().getValue();
         
        if (dateStartField == null || dateStartField == Date.min) {
            return "Не указан параметр \"Начало периода\"";
        }
        
        if (dateEndField === null || dateEndField == Date.min) {
            return "Не указан параметр \"Окончание периода\"";
        }
        else {
            return true;
        }
    },

    getParams: function () {
        var me = this,
            municipalField = me.getMunicipalityTriggerField(),
            dateStartField = me.getStartDateField(),
            dateEndField = me.getEndDateField(),
            manorgField = me.getManagingOrganizationTriggerField(),
            stateField = me.getStateTriggerField();

        return {
            municipalityIds: (municipalField ? municipalField.getValue() : null),
            startDate: (dateStartField ? dateStartField.getValue() : null),
            endDate: (dateEndField ? dateEndField.getValue() : null),
            manOrgIds: (manorgField ? manorgField.getValue() : null),
            stateIds: (stateField ? stateField.getValue() : null)
        };
    }
});