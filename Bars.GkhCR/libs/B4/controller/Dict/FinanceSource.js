Ext.define('B4.controller.dict.FinanceSource', {
    /*
    * Контроллер раздела фин. деятельности
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'dict.FinanceSource',
        'dict.FinanceSourceWork'
    ],
    
    stores: [
        'dict.FinanceSource',
        'dict.FinanceSourceWork',
        'dict.WorkSelect',
        'dict.WorkSelected'
    ],
    
    views: [
        'dict.financesource.Grid',
        'dict.financesource.EditWindow',
        'dict.financesource.WorkGrid',
        'SelectWindow.MultiSelectWindow'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context:'B4.mixins.Context'
    },

    mainView: 'dict.financesource.Grid',
    mainViewSelector: 'financeSourceGrid',

    editWindowSelector: '#financeSourceEditWindow',
    refs: [
        {
            ref: 'mainView',
            selector: 'financeSourceGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhCr.Dict.FinanceSource.Create', applyTo: 'b4addbutton', selector: 'financeSourceGrid' },
                { name: 'GkhCr.Dict.FinanceSource.Edit', applyTo: 'b4savebutton', selector: '#financeSourceEditWindow' },
                { name: 'GkhCr.Dict.FinanceSource.Delete', applyTo: 'b4deletecolumn', selector: 'financeSourceGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                { name: 'GkhCr.Dict.FinanceSource.Works.Create', applyTo: 'b4addbutton', selector: '#financeSourceWorkGrid' },
                { name: 'GkhCr.Dict.FinanceSource.Works.Delete', applyTo: 'b4deletecolumn', selector: '#financeSourceWorkGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела фин. деятельности
            */
            xtype: 'grideditwindowaspect',
            name: 'financeSourceGridWindowAspect',
            gridSelector: 'financeSourceGrid',
            editFormSelector: '#financeSourceEditWindow',
            storeName: 'dict.FinanceSource',
            modelName: 'dict.FinanceSource',
            editWindowView: 'dict.financesource.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            listeners: {
                aftersetformdata: function(asp, record) {
                    asp.controller.setCurrentId(record.getId());
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'financeSourceWorksGridAspect',
            gridSelector: '#financeSourceWorkGrid',
            storeName: 'dict.FinanceSourceWork',
            modelName: 'dict.FinanceSourceWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#financeSourceWorksMultiSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];
                    records.each(function(rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddWorks', 'FinanceSourceWork'),
                            method: 'POST',
                            params: {
                                objectIds: recordIds,
                                financeSourceId: asp.controller.financeSourceId
                            }
                        }).next(function() {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function() {
        this.getStore('dict.FinanceSourceWork').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('financeSourceGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.FinanceSource').load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.financeSourceId = this.financeSourceId;
    },

    setCurrentId: function(id) {
        this.financeSourceId = id;

        var store = this.getStore('dict.FinanceSourceWork');
        store.removeAll();

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        if (editWindow) {
            editWindow.down('#financeSourceWorkGrid').setDisabled(id == 0);
        }

        if (id > 0) {
            store.load();
        }
    }
});