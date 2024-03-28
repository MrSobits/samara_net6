Ext.define('B4.controller.servorg.RealityObject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url'
    ],
   
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    models: ['servorg.RealityObject'],
    
    stores: [
        'servorg.RealityObject',
        'dict.RealtyObjForSelect',
        'dict.RealtyObjForSelected',
        'realityobj.ExceptDirectManag'
    ],
    
    views: [
        'servorg.RealityObjectGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'servorg.RealityObjectGrid',
    mainViewSelector: 'servorgrogrid',
    params: null,

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'servorgRealityObjectAspect',
            storeName: 'servorg.RealityObject',
            modelName: 'servorg.RealityObject',
            gridSelector: 'servorgrogrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#realityObjectMultiSelectWindow',
            storeSelect: 'dict.RealtyObjForSelect',
            storeSelected: 'dict.RealtyObjForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            onBeforeLoad: function (store, operation) {
                var me = this;

                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.contragentId = me.controller.getContextValue(me.controller.getMainView(), 'servorgId');
            },
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    header: 'Адрес',
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddRealityObjects', 'ServiceOrgRealityObject', {
                            objectIds: Ext.encode(recordIds),
                            servorgId: asp.controller.getContextValue(asp.controller.getMainView(), 'servorgId')
                        })).next(function (response) {
                            var resp = Ext.JSON.decode(response.responseText);
                            if (resp && !resp.success) {
                                Ext.Msg.alert('Ошибка!', resp.message);
                            }
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
                
            }
            
        }
    ],
    

    init: function () {
        this.getStore('servorg.RealityObject').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('servorgrogrid');

        me.bindContext(view);
        me.setContextValue(view, 'servorgId', id);
        me.application.deployView(view, 'serv_org');

        view.getStore().load();
    },
    
    onBeforeLoad: function (store, operation) {
        operation.params.servorgId = this.getContextValue(this.getMainView(), 'servorgId');
    }
});