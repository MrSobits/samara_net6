Ext.define('B4.controller.report.RoTechPassportExport', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.RoTechPassportExportPanel',
    mainViewSelector: 'rotechpassportexportpanel',

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
    
    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: 'rotechpassportexportpanel [name = Municipalities]'
        },
        {
            ref: 'TypeManyApartmentsField',
            selector: 'rotechpassportexportpanel [name = typeManyApartments]'
        },
        {
            ref: 'TypeIndividualField',
            selector: 'rotechpassportexportpanel [name = typeIndividual]'
        },
        {
            ref: 'TypeSocialBehaviorField',
            selector: 'rotechpassportexportpanel [name = typeSocialBehavior]'
        },
        {
            ref: 'TypeBlockedBuildingField',
            selector: 'rotechpassportexportpanel [name = typeBlockedBuilding]'
        },
        {
            ref: 'TypeNotSetField',
            selector: 'rotechpassportexportpanel [name = typeNotSet]'
        }
    ],
    
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'rotechpassportexportMultiselectwindowaspect',
            fieldSelector: 'rotechpassportexportpanel [name = Municipalities]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#rotechpassportexportMunicipalitySelectWindow',
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
                        url: '/Municipality/ListWithoutPaging',
                        emptyText: 'Все МО'
                    }
                }
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
            typeBlockedBuildingField = this.getTypeBlockedBuildingField(),
            typeNotSetField = this.getTypeNotSetField();

        if (!typeNotSetField.getValue() && !typeManyApartmentsField.getValue() && !typeIndividualField.getValue() && !typeSocialBehaviorField.getValue() && !typeBlockedBuildingField.getValue()) {
            return 'Необходимо выбрать хотябы один тип дома';
        }

        return true;
    },

    getParams: function () {
        var mcpField = this.getMunicipalityTriggerField(),
            typeManyApartmentsField = this.getTypeManyApartmentsField(),
            typeIndividualField = this.getTypeIndividualField(),
            typeSocialBehaviorField = this.getTypeSocialBehaviorField(),
            typeBlockedBuildingField = this.getTypeBlockedBuildingField(),
            typeNotSetField = this.getTypeNotSetField();

            return {
                municipalityIds: (mcpField ? mcpField.getValue() : null),
                typeManyApartments: (typeManyApartmentsField ? typeManyApartmentsField.getValue() : false),
                typeIndividual: (typeIndividualField ? typeIndividualField.getValue() : false),
                typeSocialBehavior: (typeSocialBehaviorField ? typeSocialBehaviorField.getValue() : false),
                typeBlockedBuilding: (typeBlockedBuildingField ? typeBlockedBuildingField.getValue() : false),
                typeNotSet: (typeNotSetField ? typeNotSetField.getValue() : false)
            };
    }
});