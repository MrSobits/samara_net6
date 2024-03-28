Ext.define('B4.controller.report.FundFormationReport', {    
    extend: 'B4.controller.BaseReportController',
    
    mainView: 'B4.view.report.FundFormationPanel',
    mainViewSelector: '#fundFormationPanel',

    init: function () {
        var me = this;

        me.control({            
            "#fundFormationPanel #municipalityR": {
                change: me.onMrChange
            },
            "#fundFormationPanel #municipalityO": {
                beforeload: me.beforeMoLoad
            },
            "#fundFormationPanel gkhtriggerfield[name=MethodFormFund]": {
                change: me.onMethodChange
            }
        });

        me.callParent(arguments);
    },
    
    onMrChange: function (field, newValue) {
        this.getMunicipalityOTriggerField().setDisabled(!newValue);
    },
    
    beforeMoLoad: function (field, operation, store) {
        var mrField = this.getMunicipalityRTriggerField();

        operation.params = operation.params || {};
        operation.params.parentId = mrField.getValue();
    },

    onMethodChange: function (field, newValue) {
        this.getContragentTriggerField().setDisabled(newValue != 0);
        this.getBankReferenceComboBox().setDisabled(newValue == 3);
    },
    
    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.ux.button.Update',
        'B4.enums.MethodFormFund'
    ],
    
    stores: [
        'contragent.ContragentForSelect',
        'contragent.ContragentForSelected',
        'MethodFormFundForSelect',
        'MethodFormFundForSelected'
    ],
    
    views: [
        'SelectWindow.MultiSelectWindow',
        'B4.view.report.FundFormationPanel'
    ],
    
    refs: [
        {
            ref: 'MunicipalityRTriggerField',
            selector: '#fundFormationPanel #municipalityR'
        },
        {
            ref: 'MunicipalityOTriggerField',
            selector: '#fundFormationPanel #municipalityO'
        },
        {
            ref: 'MethodFormFundTriggerField',
            selector: '#fundFormationPanel gkhtriggerfield[name=MethodFormFund]'
        },
        {
            ref: 'ContragentTriggerField',
            selector: '#fundFormationPanel #tfContragents'
        },
        {
            ref: 'BankReferenceComboBox',
            selector: '#fundFormationPanel #cbBankReference'
        },
        {
            ref: 'IncludeRosNotInPublishedProgramCheckBox',
            selector: '#fundFormationPanel #cbIncludeRosNotInPublishedProgram'
        }
    ],
    
    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'fundFormationPanelHouseTypeMultiselectwindowaspect',
            fieldSelector: '#fundFormationPanel #tfContragents',
            valueProperty: 'Id',
            textProperty: 'Name',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#fundFormationPanelContragentsSelectWindow',
            storeSelect: 'contragent.ContragentForSelect',
            storeSelected: 'contragent.ContragentForSelected',
            columnsGridSelect: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'fundFormationMethodFormFundMultiselectwindowaspect',
            fieldSelector: '#fundFormationPanel gkhtriggerfield[name=MethodFormFund]',
            idProperty: 'Value',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#fundFormationPanelMethodFormFundSelectWindow',
            storeSelect: 'MethodFormFundForSelect',
            storeSelected: 'MethodFormFundForSelected',
            columnsGridSelect: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            columnsGridSelected: [{ header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Display', flex: 1, sortable: false }],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onPageChange: function () {
                var me = this,
                    gridSelect = me.getSelectGrid(),
                    gridSelected = me.getSelectedGrid(),
                    storeSelect = gridSelect.getStore(),
                    storeSelected = gridSelected.getStore(),
                    records = [];

                if (storeSelected.getCount()) {
                    storeSelected.data.each(function (edRec) {
                        storeSelect.data.each(function (tRec) {
                            if (tRec.get('Value') === edRec.get('Value')) {
                                records.push(tRec);
                            }
                        }, me);
                    }, me);

                    gridSelect.getSelectionModel().select(records);
                    me.changeTotalValue();
                }
            },
            selectedGridRowActionHandler: function (action, record) {
                var me = this,
                    gridSelect = me.getSelectGrid(),
                    gridSelected = me.getSelectedGrid();

                if (gridSelected) {
                    gridSelected.fireEvent('rowaction', gridSelected, action, record);
                    gridSelect.getSelectionModel().deselect(gridSelect.getStore().find(me.idProperty, record.get('Value')));
                    me.changeTotalValue();
                }
            }
        }
    ],
    
    validateParams: function () {
        return true;
    },

    getParams: function () {
        var municipalityR = this.getMunicipalityRTriggerField();
        var municipalityO = this.getMunicipalityOTriggerField();
        var methodForm = this.getMethodFormFundTriggerField();
        var contragents = this.getContragentTriggerField();
        var bankRef = this.getBankReferenceComboBox();
        var includeRosNotInPublishedProgram = this.getIncludeRosNotInPublishedProgramCheckBox();

        return {
            municipalityParent: (municipalityR ? municipalityR.getValue() : null),
            municipality: (municipalityO ? municipalityO.getValue() : null),
            formationTypeValuesList: (methodForm ? methodForm.getValue() : null),
            contragentIds: (contragents ? contragents.getValue() : null),
            haveReference: (bankRef ? bankRef.getValue() : null),
            includeRosNotInPublishedProgram: (includeRosNotInPublishedProgram ? includeRosNotInPublishedProgram.getValue() : null)
        };
    }
});