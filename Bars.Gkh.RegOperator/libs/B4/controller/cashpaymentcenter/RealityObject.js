Ext.define('B4.controller.cashpaymentcenter.RealityObject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    models: ['manorg.RealityObject'],
    stores: [
        'cashpaymentcenter.RealityObject',
        'cashpaymentcenter.RealObjForAdd',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],
    views: [
        'SelectWindow.MultiSelectWindow',
        'cashpaymentcenter.RealityObjectGrid',
        'cashpaymentcenter.RealObjAddWindow',
        'cashpaymentcenter.AddRealObjGrid'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.CashPaymentCenter.RealityObject.Create',
                    applyTo: 'b4addbutton', selector: 'cashpaymentcenterrealobjgrid'
                },
                {
                    name: 'Gkh.Orgs.CashPaymentCenter.RealityObject.Delete',
                    applyTo: 'b4deletecolumn', selector: 'cashpaymentcenterrealobjgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'cashpaymentcenterRealObjGridAspect',
            gridSelector: 'cashpaymentcenterrealobjgrid',
            storeName: 'cashpaymentcenter.RealityObject',
            modelName: 'cashpaymentcenter.RealityObject',
            addRecord: function () {
            },
            deleteRecord: function() {
            }
        },

        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'cashpaymentcenterAddRealObjGridAspect',
            gridSelector: 'cashpaymentcenteraddrealobjgrid',
            storeName: 'cashpaymentcenter.RealObjForAdd',
            modelName: 'cashpaymentcenter.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#cashpaymentcenterAddRealObjMultiSelectWindow',
            storeSelect: 'cashpaymentcenter.RealObjForAdd',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            titleSelectWindowPersAcc: 'Выбор лицевых счетов',
            titleGridSelectPersAcc: 'Лицевые счета для отбора',
            titleGridSelectedPersAcc: 'Выбранные лицевые счета',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality', flex: 1,
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
                {
                    header: 'Адрес',
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    header: 'Лицевой счет',
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    hidden: true,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false },
                {
                    header: 'Лицевой счет', xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNum', flex: 1, hidden: true, filter: { xtype: 'textfield' }
                }
            ],
            onBeforeLoad: function (store, operation) {
                var me = this,
                    showPersAcc = me.controller.showAccounts,
                    selectWindow = me.getForm(),
                    selectGrid = me.getSelectGrid(),
                    selectedGrid = me.getSelectedGrid(),
                    persAccColumnSelect = selectGrid.down('[dataIndex=PersonalAccountNum]'),
                    persAccColumnSelected = selectedGrid.down('[dataIndex=PersonalAccountNum]'),
                    adressColumnSelected = selectedGrid.down('[dataIndex=Address]');

                operation.params.cashPaymentCenterId = me.controller.getContextValue(me.controller.getMainView(), 'cashPaymentCenterId');
                operation.params.isShowPersAcc = showPersAcc;

                if (showPersAcc) {
                    persAccColumnSelect.show();
                    persAccColumnSelected.show();
                    adressColumnSelected.hide();

                    selectWindow.setTitle(me.titleSelectWindowPersAcc);
                    selectGrid.setTitle(me.titleGridSelectPersAcc);
                    selectedGrid.setTitle(me.titleGridSelectedPersAcc);
                } else {
                    persAccColumnSelect.hide();
                    persAccColumnSelected.hide();
                    adressColumnSelected.show();

                    selectWindow.setTitle(me.titleSelectWindow);
                    selectGrid.setTitle(me.titleGridSelect);
                    selectedGrid.setTitle(me.titleGridSelected);
                }
            },
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            deleteRecord: function () {
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
               selector: 'cashpaymentcenterrealobjgrid'
           }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    init: function () {
        var me = this;

        me.control({
            'cashpaymentcenterrealobjgrid b4addbutton': { 'click': { fn: me.showAddRealObjsWin } },
            'cashpaymentcenterrealobjgrid #addWithoutRKC': { 'click': { fn: me.setCashPaymentCenter } },
            'cashpaymentcenterrealobjaddwindow b4closebutton': { 'click': { fn: me.closeAddRealObjsWin } },
            'cashpaymentcenterrealobjaddwindow b4savebutton': { 'click': { fn: me.addRealObjs } },
            'cashpaymentcenteraddrealobjgrid': {
                'rowaction': { fn: me.rowAction }
            },
            'cashpaymentcenterrealobjgrid': {
                'rowaction': { fn: me.rowAction }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('cashpaymentcenterrealobjgrid'),
            store;

        me.showAccounts = Gkh.config.RegOperator.GeneralConfig.CachPaymentCenterConnectionType != 10;

        if (me.showAccounts) {
            store = Ext.create('B4.store.cashpaymentcenter.PersonalAccount');
        } else {
            store = Ext.create('B4.store.cashpaymentcenter.RealityObject');
        }

        //перенастраиваем грид
        view.reconfigure(store);

        //перенастраиваем тулбар пагинации
        view.dockedItems.items.forEach(function(el) {
            if (el.xtype == 'b4pagingtoolbar') {
                el.bindStore(store);
            }
        });

        me.bindContext(view);
        me.setContextValue(view, 'cashPaymentCenterId', id);
        me.application.deployView(view, 'cashpayment_center');

        store.clearFilter(true);
        store.filter('cashPaymentCenterId', id);

        
        if (me.showAccounts) {
            view.down('[dataIndex=PersonalAccount]').show();
        } else {
            view.down('[dataIndex=PersonalAccount]').hide();
        }

        //перенастраиваем headerFilter плагин для работы
        view.plugins.forEach(function (elem) {
            if (elem.ptype == 'b4gridheaderfilters') {
                elem.storeLoaded = true;
                elem.reloadStore();
            }
        });
    },
    showAddRealObjsWin: function () {
        var me = this,
            win = Ext.create('B4.view.cashpaymentcenter.RealObjAddWindow',
        {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy',
            ctxKey: me.getCurrentContextKey ? me.getCurrentContextKey() : ''
        });

        if (me.showAccounts) {
            win.down('[dataIndex=PersonalAccountNum]').show();
            win.down('[dataIndex=Settlement]').hide();
        } else {
            win.down('[dataIndex=PersonalAccountNum]').hide();
            win.down('[dataIndex=Settlement]').show();
        }

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
        var me = this;
        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result != 'yes') {
                return;
            }

            me.mask('Удаление', me.getMainComponent());
            B4.Ajax.request(B4.Url.action('DeleteObjects', 'CashPaymentCenter', {
                ids: [record.getId()]
            })).next(function (resp) {
                me.unmask();
                var tryDecoded = {};
                try {
                    tryDecoded = Ext.JSON.decode(resp.responseText);
                } catch (e) { }

                if (tryDecoded.success) {
                    Ext.Msg.alert('Удаление!', "Удалено успешно");
                } else {
                    Ext.Msg.alert('Удаление!', tryDecoded.message);
                }

                grid.getStore().load();
                return true;
            }).error(function (e) {
                me.unmask();
                Ext.Msg.alert('Удаление!', e.message || 'Ошибка удаления');
            });
        });
    },

    addRealObjs: function (btn) {
        var me = this,
            win = btn.up('window'),
            dateStart = win.down('[name=DateStart]').getValue(),
            dateEnd = win.down('[name=DateEnd]').getValue(),
            addRoGrid = win.down('cashpaymentcenteraddrealobjgrid'),
            addRoStore = addRoGrid.getStore(),
            recordIds = [];

        if (!dateStart) {
            Ext.Msg.alert('Ошибка!', 'Необходимо указать дату начала');
            return;
        }

        addRoStore.each(function (rec) {
            recordIds.push(rec.get('Id'));
        });

        if (recordIds.length > 0) {
            me.mask('Сохранение', me.getMainComponent());
            B4.Ajax.request(B4.Url.action('AddObjects', 'CashPaymentCenter', {
                objectIds: Ext.encode(recordIds),
                dateStart: dateStart,
                dateEnd: dateEnd,
                cashPaymentCenterId: me.getContextValue(me.getMainView(), 'cashPaymentCenterId')
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
            }).error(function (e) {
                Ext.Msg.alert('Сохранение!', e.message || 'Ошибка сохранения');
                me.unmask();
            });
        }
        else {
            Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
            return false;
        }
    },

    setCashPaymentCenter: function (btn) {
        var me = this;

        me.mask('Проставляем...', me.getMainComponent());
        B4.Ajax.request(B4.Url.action('SetCashPaymentCenters', 'CashPaymentCenter', {
            cashPaymentCenterId: me.getContextValue(me.getMainView(), 'cashPaymentCenterId')
        })).next(function () {
            me.unmask();
            me.getMainView().getStore().load();
            return true;
        }).error(function (e) {
            //Ext.Msg.alert('Установка РКЦ', e.message || 'Ошибка установки');
            me.unmask();
            me.getMainView().getStore().load();
        });
    }
});