Ext.define('B4.controller.deliveryagent.RealityObject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    models: ['manorg.RealityObject'],
    stores: [
        'deliveryagent.RealityObject',
        'deliveryagent.RealObjForAdd',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],
    views: [
        'SelectWindow.MultiSelectWindow',
        'deliveryagent.RealityObjectGrid',
        'deliveryagent.RealObjAddWindow',
        'deliveryagent.AddRealObjGrid'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.DeliveryAgent.RealityObject.Create', applyTo: 'b4addbutton', selector: 'deliveryagentrealobjgrid' },
                { name: 'Gkh.Orgs.DeliveryAgent.RealityObject.Delete', applyTo: 'b4deletecolumn', selector: 'deliveryagentrealobjgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'deliveryAgentRealObjGridAspect',
            gridSelector: 'deliveryagentrealobjgrid',
            storeName: 'deliveryagent.RealityObject',
            modelName: 'deliveryagent.RealityObject',
            addRecord: function() {
            }
        },

        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'deliveryagentAddRealObjGridAspect',
            gridSelector: 'delagentaddrealobjgrid',
            storeName: 'deliveryagent.RealObjForAdd',
            modelName: 'deliveryagent.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#deliveryAgentAddRealObjMultiSelectWindow',
            storeSelect: 'deliveryagent.RealObjForAdd',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальный район', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.delAgentId = me.controller.getContextValue(me.controller.getMainView(), 'delAgentId');
            },
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            deleteRecord: function() {
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        store = me.getGrid().getStore();

                    records.each(function (rec) {
                        if (store.find('Id', rec.get('Id'), 0, false, false, true) == -1)
                            store.add(rec);
                    });

                    
                    return true;
                }
            }
        }
    ],

    refs: [
           {
               ref: 'mainView',
               selector: 'deliveryagentrealobjgrid'
           }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    init: function() {
        var me = this;

        me.control({
            'deliveryagentrealobjgrid b4addbutton': { 'click': { fn: me.showAddRealObjsWin } },
            'delagentrealobjaddwindow b4closebutton': { 'click': { fn: me.closeAddRealObjsWin } },
            'delagentrealobjaddwindow b4savebutton': { 'click': { fn: me.addRealObjs } },
            'delagentaddrealobjgrid': { 'rowaction': { fn: me.rowAction } }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('deliveryagentrealobjgrid');
        me.bindContext(view);
        me.setContextValue(view, 'delAgentId', id);
        me.application.deployView(view, 'delivery_agent');

        view.getStore().filter('delAgentId', id);
    },
    
    showAddRealObjsWin: function () {
        var me = this,
            win = Ext.create('B4.view.deliveryagent.RealObjAddWindow',
        {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy',
            ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : ''
        });
        
        win.show();
    },
    
    closeAddRealObjsWin: function (btn) {
        btn.up('window').close();
    },
    
    rowAction: function (grid, action, record) {
        if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'delete':
                    this.deleteRecord(grid, record);
                    break;
            }
        }
    },
    
    deleteRecord: function (grid, record) {
        grid.getStore().remove(record);
    },
    
    addRealObjs: function (btn) {
        var me = this,
            win = btn.up('window'),
            dateStart = win.down('[name=DateStart]').getValue(),
            dateEnd = win.down('[name=DateEnd]').getValue(),
            addRoStore = win.down('delagentaddrealobjgrid').getStore(),
            recordIds = [];

        if (!dateStart) {
            Ext.Msg.alert('Ошибка!', 'Необходимо указать дату начала');
            return;
        }
        
        addRoStore.each(function (rec) {
            recordIds.push(rec.getId());
        });

        if (recordIds[0] > 0) {
            me.mask('Сохранение', me.getMainComponent());
            B4.Ajax.request(B4.Url.action('AddRealityObjects', 'DeliveryAgent', {
                objectIds: Ext.encode(recordIds),
                dateStart: dateStart,
                dateEnd : dateEnd,
                delAgentId: me.getContextValue(me.getMainView(), 'delAgentId')
            })).next(function (resp) {
                me.unmask();
                var tryDecoded = {};
                try {
                    tryDecoded = Ext.JSON.decode(resp.responseText);
                } catch (e) {

                }
                me.getMainView().getStore().load();

                if (tryDecoded.message) {
                    Ext.Msg.alert('Сохранение!', tryDecoded.message);
                }
                win.close();
                return true;
            }).error(function () {
                me.unmask();
            });
        }
        else {
            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
            return false;
        }

    }
});