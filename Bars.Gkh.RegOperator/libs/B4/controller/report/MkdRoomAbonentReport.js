Ext.define('B4.controller.report.MkdRoomAbonentReport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.MkdRoomAbonentReportPanel',
    mainViewSelector: 'mkdRoomAbonentReportPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.ux.button.Update'
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
            ref: 'MunicipalityField',
            selector: 'mkdRoomAbonentReportPanel #sfMunicipality'
        }
        //{
        //    ref: 'StartDateField',
        //    selector: 'chargeReportPanel #dfStartDate'
        //},
        //{
        //    ref: 'EndDateField',
        //    selector: 'chargeReportPanel #dfEndDate'
        //},
        //{
        //    ref: 'ManagingOrganizationTriggerField',
        //    selector: 'chargeReportPanel #tfManagingOrganization'
        //}
    ],

    aspects: [
        //{
        //    xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        //    name: 'chargeReportManOrgMultiselectwindowaspect',
        //    fieldSelector: 'chargeReportPanel #tfManagingOrganization',
        //    multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        //    multiSelectWindowSelector: '#chargeReportManOrgSelectWindow',
        //    storeSelect: 'manorg.ForSelect',
        //    storeSelected: 'manorg.ForSelected',
        //    textProperty: 'ContragentName',
        //    columnsGridSelect: [
        //        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ContragentName', flex: 1, filter: { xtype: 'textfield' } },
        //        { header: 'Мун. образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } }

        //    ],
        //    columnsGridSelected: [
        //        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ContragentName', flex: 1, sortable: false }
        //    ],
        //    titleSelectWindow: 'Выбор записи',
        //    titleGridSelect: 'Записи для отбора',
        //    titleGridSelected: 'Выбранная запись'
        //},
        /*{
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'chargeReportMunicipalityMultiselectwindowaspect',
            fieldSelector: 'mkdRoomAbonentReportPanel #tfMunicipality',
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
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }*/
    ],

    validateParams: function () {
        //var dateStartField = this.getStartDateField().getValue();
        //var dateEndField = this.getEndDateField().getValue();

        //if (dateStartField == null || dateStartField == Date.min) {
        //    return "Не указан параметр \"Начало периода\"";
        //}

        //if (dateEndField === null || dateEndField == Date.min) {
        //    return "Не указан параметр \"Окончание периода\"";
        //}
        //else {
        //    return true;
        //}

        return true;
    },

    getParams: function () {

        var municipalField = this.getMunicipalityField();
        //var dateStartField = this.getStartDateField();
        //var dateEndField = this.getEndDateField();
        //var manorgField = this.getManagingOrganizationTriggerField();

        return {
            municipalityId: (municipalField ? municipalField.getValue() : null)
            //startDate: (dateStartField ? dateStartField.getValue() : null),
            //endDate: (dateEndField ? dateEndField.getValue() : null),
            //manOrgIds: (manorgField ? manorgField.getValue() : null)
        };
    }
});