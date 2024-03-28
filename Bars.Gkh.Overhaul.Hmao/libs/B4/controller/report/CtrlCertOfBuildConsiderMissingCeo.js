Ext.define('B4.controller.report.CtrlCertOfBuildConsiderMissingCeo', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.CtrlCertOfBuildMissingCeoPanel',
    mainViewSelector: 'reportCtrlCertOfBuildMissingCeoPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.municipality.ByParam',
        'B4.ux.button.Update'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: 'reportCtrlCertOfBuildMissingCeoPanel #tfMunicipality'
        },
        {
            ref: 'TypeManyApartmentsField',
            selector: 'reportCtrlCertOfBuildMissingCeoPanel #cbTypeManyApartments'
        },
        {
            ref: 'TypeIndividualField',
            selector: 'reportCtrlCertOfBuildMissingCeoPanel #cbTypeIndividual'
        },
        {
            ref: 'TypeSocialBehaviorField',
            selector: 'reportCtrlCertOfBuildMissingCeoPanel #cbTypeSocialBehavior'
        },
        {
            ref: 'TypeBlockedBuildingField',
            selector: 'reportCtrlCertOfBuildMissingCeoPanel #cbTypeBlockedBuilding'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'reportCtrlCertOfBuildMissingCeoPanelMultiSelectWin',
            fieldSelector: 'reportCtrlCertOfBuildMissingCeoPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#reportCtrlCertOfBuildMissingCeoPanelMuSelectWin',
            storeSelect: 'dict.municipality.ByParam',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        store: Ext.create('B4.store.dict.municipality.ByParam'),
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' }
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
        var typeManyApartmentsField = this.getTypeManyApartmentsField(),
            typeIndividualField = this.getTypeIndividualField(),
            typeSocialBehaviorField = this.getTypeSocialBehaviorField(),
            typeBlockedBuildingField = this.getTypeBlockedBuildingField();
        
        if (!typeManyApartmentsField.getValue() && !typeIndividualField.getValue() && !typeSocialBehaviorField.getValue() && !typeBlockedBuildingField.getValue()) {
            return 'Необходимо выбрать хотябы один тип дома';
        }
        
        return true;
    },

    getParams: function () {
        var mcpField = this.getMunicipalityTriggerField(),
            typeManyApartmentsField = this.getTypeManyApartmentsField(),
            typeIndividualField = this.getTypeIndividualField(),
            typeSocialBehaviorField = this.getTypeSocialBehaviorField(),
            typeBlockedBuildingField = this.getTypeBlockedBuildingField();
        
        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            typeManyApartments: (typeManyApartmentsField ? typeManyApartmentsField.getValue() : null),
            typeIndividual: (typeIndividualField ? typeIndividualField.getValue() : null),
            typeSocialBehavior: (typeSocialBehaviorField ? typeSocialBehaviorField.getValue() : null),
            typeBlockedBuilding: (typeBlockedBuildingField ? typeBlockedBuildingField.getValue() : null)
        };
    }
});