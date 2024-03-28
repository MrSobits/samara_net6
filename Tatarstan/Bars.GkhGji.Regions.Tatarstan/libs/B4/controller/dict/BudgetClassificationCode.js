Ext.define('B4.controller.dict.BudgetClassificationCode', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.BudgetClassificationCode',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'dict.BudgetClassificationCode'
    ],
    stores: [
        'dict.BudgetClassificationCode',
        'dict.Municipality',
        'dict.MunicipalityForSelected',
        'dict.MunicipalityForSelect'
    ],
    views: [
        'dict.budgetclassificationcode.EditWindow',
        'dict.budgetclassificationcode.Grid'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.budgetclassificationcode.Grid',
    mainViewSelector: 'budgetclassificationcodegrid',
    aspects: [
        {
            xtype: "budgetclassificationcodeperm"
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'kbkGridWindowAspect',
            gridSelector: 'budgetclassificationcodegrid',
            editFormSelector: 'budgetclassificationcodeeditwindow',
            modelName: 'dict.BudgetClassificationCode',
            editWindowView: 'dict.budgetclassificationcode.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var kbkId = record.getId();
                    asp.controller.setContextValue(asp.controller.getMainView(), 'kbkId', kbkId);

                    B4.Ajax.request(B4.Url.action('GetInfo', 'BudgetClassificationCode', {
                        kbkId: kbkId
                        }))
                        .next(function (response) {
                            var obj = Ext.JSON.decode(response.responseText).data;
                            if (obj) {
                                var fieldMunicipalities = form.down('[name=Municipalities]');
                                if (fieldMunicipalities) {
                                    fieldMunicipalities.updateDisplayedText(obj.municipalitiesNames);
                                    fieldMunicipalities.setValue(obj.municipalitiesIds);
                                }
                            }
                        });
                }
            },

            onSaveSuccess: function (asp) {
                var form = asp.getForm();
                if (form) {
                    form.close();
                }

                var kbkId = asp.controller.getContextValue(asp.controller.getMainView(), 'kbkId'),
                    municipalitiesIds = asp.controller.getContextValue(asp.controller.getMainView(), 'municipalitiesIds');

                B4.Ajax.request(B4.Url.action('SaveMunicipalities', 'BudgetClassificationCode', {
                    kbkId: kbkId,
                    municipalitiesIds: municipalitiesIds
                })).next(function (response) {
                    asp.controller.getMainView().getStore().load();
                });
            },
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'budgetclassificationcodemunicipalitiesMultiSelectWindowAspect',
            fieldSelector: 'budgetclassificationcodeeditwindow [name=Municipalities]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#budgetclassificationcodemunicipalitiesSelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            textProperty: 'Name',
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
            titleGridSelected: 'Выбранная запись',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    
                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);

                    asp.controller.setContextValue(asp.controller.getMainView(), 'municipalitiesIds', recordIds);
                }
            }
        },
    ],
    
    index: function () {
        var view = this.getMainComponent();
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});