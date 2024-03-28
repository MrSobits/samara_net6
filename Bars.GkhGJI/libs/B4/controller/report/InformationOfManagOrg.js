Ext.define('B4.controller.report.InformationOfManagOrg', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.InformationOfManagOrgPanel',
    mainViewSelector: '#informationOfManagOrgPanel',

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

    municipalityTriggerFieldSelector: '#informationOfManagOrgPanel #tfMunicipality',
    orgTypeTriggerFieldSelector: '#informationOfManagOrgPanel #tfOrgTypes',
    statusManOrgFieldSelector: '#informationOfManagOrgPanel #cbStatusManOrg',
    reportDateFieldSelector: '#informationOfManagOrgPanel #dfReportDate',
    
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'MunMultiselectwindowaspect',
            fieldSelector: '#informationOfManagOrgPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#informationOfManagOrgPanelMunicipalitySelectWindow',
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
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'OrgTypesMultiselectwindowaspect',
            fieldSelector: '#informationOfManagOrgPanel #tfOrgTypes',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#informOfManagOrgPanelMunicipalitySelectWindow',
            columnsGridSelect: [
                { header: 'Тип организации', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Тип организации', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            getForm: function () {
                var win = Ext.ComponentQuery.query(this.multiSelectWindowSelector)[0],
                    stSelected,
                    stSelect;

                if (win && !win.getBox().width) {
                    win = win.destroy();
                }

                if (!win) {
                    stSelected = Ext.create('Ext.data.Store', {
                        fields: [
                            { name: 'Id', type: 'int' },
                            { name: 'Name', type: 'string' }
                        ]
                    });
                    stSelected.on('beforeload', this.onSelectedBeforeLoad, this);

                    stSelect = Ext.create('Ext.data.Store', {
                        fields: [
                            {name: 'Id',  type: 'int'},
                            {name: 'Name', type: 'string'}
                        ],
                        data: [
                            { Id: '10', Name: 'Управляющие компании' },
                            { Id: '20', Name: 'Товарищества собственников жилья' },
                            { Id: '30', Name: 'Жилищно-строительные кооперативы' },
                            { Id: '40', Name: 'Поставщики коммунлаьных услуг' },
                            { Id: '50', Name: 'Поставщики жилищных услуг' }
                        ]
                    });
                    stSelect.on('beforeload', this.onBeforeLoad, this);

                    win = this.controller.getView(this.multiSelectWindow).create({
                        itemId: this.multiSelectWindowSelector.replace('#', ''),
                        storeSelect: stSelect,
                        storeSelected: stSelected,
                        columnsGridSelect: this.columnsGridSelect,
                        columnsGridSelected: this.columnsGridSelected,
                        title: this.titleSelectWindow,
                        titleGridSelect: this.titleGridSelect,
                        titleGridSelected: this.titleGridSelected,
                        selModelMode: this.selModelMode,
                        constrain: true,
                        modal: false,
                        closeAction: 'destroy',
                        renderTo: B4.getBody().getActiveTab().getEl()
                    });

                    stSelected.sorters.clear();
                    stSelect.sorters.clear();
                }

                return win;
            }
        }
    ],

    validateParams: function () {
        var municipalities = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var orgTypes = Ext.ComponentQuery.query(this.orgTypeTriggerFieldSelector)[0];
        var status = Ext.ComponentQuery.query(this.statusManOrgFieldSelector)[0];
        var reportDateField = Ext.ComponentQuery.query(this.reportDateFieldSelector)[0];
        
        return (municipalities && municipalities.isValid() && status && status.isValid() && reportDateField.isValid() && orgTypes.isValid());
    },

    getParams: function () {
        var municipalityIdField = Ext.ComponentQuery.query(this.municipalityTriggerFieldSelector)[0];
        var status = Ext.ComponentQuery.query(this.statusManOrgFieldSelector)[0];
        var reportDateField = Ext.ComponentQuery.query(this.reportDateFieldSelector)[0];
        var orgTypesField = Ext.ComponentQuery.query(this.orgTypeTriggerFieldSelector)[0];
        
        return {
            municipalityIds: (municipalityIdField ? municipalityIdField.getValue() : null),
            orgTypeIds: (orgTypesField ? orgTypesField.getValue() : null),
            statusId: (status ? status.getValue() : null),
            reportDate: (reportDateField ? reportDateField.getValue() : null)
        };
    }
});