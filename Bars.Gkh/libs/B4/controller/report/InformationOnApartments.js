Ext.define('B4.controller.report.InformationOnApartments', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.InformationOnApartmentsPanel',
    mainViewSelector: '#informationOnApartmentsPanel',

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

    municipalityTriggerFieldSelector: '#informationOnApartmentsPanel #tfMunicipality',
    resettlementProgramIdSelectField: '#informationOnApartmentsPanel #sfResettlementProgram',
    reportDateFieldSelectField: '#informationOnApartmentsPanel #dfReportDate',
        
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'informationOnApartmentsMultiselectwindowaspect',
            fieldSelector: '#informationOnApartmentsPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#informationOnApartmentsPanelMunicipalitySelectWindow',
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
        var reportDate = Ext.ComponentQuery.query(this.reportDateFieldSelectField)[0];
        var program = Ext.ComponentQuery.query(this.resettlementProgramIdSelectField)[0];
        var municipalities = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];

        return (municipalities && municipalities.isValid() && reportDate && reportDate.isValid() && program && program.isValid());
    },
    
    
    getParams: function () {
        var municipalityIdField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var resettlementProgramIdSelectField = Ext.ComponentQuery.query(this.resettlementProgramIdSelectField)[0];
        var reportDateField = Ext.ComponentQuery.query(this.reportDateFieldSelectField)[0];

        return {
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null),
            resettlementProgramId: (resettlementProgramIdSelectField ? resettlementProgramIdSelectField.getValue() : null)
        };
    }
});