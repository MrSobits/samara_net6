Ext.define('B4.controller.report.AllocationFundsToPeopleInCr', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.AllocationFundsToPeopleInCrPanel',
    mainViewSelector: '#allocationFundsToPeopleInCrPanel',

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
            ref: 'MunicipalityTriggerField',
            selector: '#allocationFundsToPeopleInCrPanel #tfMunicipality'
        },
        {
            ref: 'DateStartField',
            selector: '#allocationFundsToPeopleInCrPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#allocationFundsToPeopleInCrPanel #dfdDateEnd'
        },
        {
            ref: 'ProgramCrSelectField',
            selector: '#allocationFundsToPeopleInCrPanel #sfProgramCr'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'allocationFundsToPeopleInCrMultiselectwindowaspect',
            fieldSelector: '#allocationFundsToPeopleInCrPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#allocationFundsToPeopleInCrMunicipalitySelectWindow',
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
        var dateStartField = this.getDateStartField().getValue();
        var dateEndField = this.getDateEndField().getValue();
         
        var programCr = this.getProgramCrSelectField().getValue();
        if (programCr == null || programCr === "") {
            return "Выберите программу капитального ремонта!";
        }
        
        if (dateStartField == null || dateStartField == Date.min) {
            return "Не указан параметр \"Дата начала\"";
        }
        
        if (dateEndField === null || dateEndField == Date.min) {
            return "Не указан параметр \"Дата окончания\"";
        }
        else {
            return true;
        }
    },

    getParams: function () {

        var municipalField = this.getMunicipalityTriggerField();
        var dateStartField = this.getDateStartField();
        var dateEndField = this.getDateEndField();
        var programCrfield = this.getProgramCrSelectField();

        return {
            municipalityIds: (municipalField ? municipalField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null),
            programCrIds: (programCrfield ? programCrfield.getValue() : null)
        };
    }
});