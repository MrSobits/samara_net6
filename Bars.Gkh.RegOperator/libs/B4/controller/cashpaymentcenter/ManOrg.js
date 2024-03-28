Ext.define('B4.controller.cashpaymentcenter.ManOrg', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'cashpaymentcenter.ManOrg',
        'cashpaymentcenter.ManOrgRealObj'
    ],

    stores: [
        'cashpaymentcenter.ManOrg',
        'cashpaymentcenter.ManOrgRealityObjectForAdd',
        'realityobj.RealityObjectForSelected'
    ],
    views: [
        'SelectWindow.MultiSelectWindow',
        'cashpaymentcenter.ManOrgGrid',
        'cashpaymentcenter.ManOrgEditWindow',
        'cashpaymentcenter.ManOrgRealObjGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'cashpaymentcentermanorggrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.CashPaymentCenter.ManOrg.Create',
                    applyTo: 'b4addbutton', selector: 'cashpaymentcentermanorggrid'
                },
                {
                    name: 'Gkh.Orgs.CashPaymentCenter.ManOrg.Edit',
                    applyTo: 'b4editcolumn', selector: 'cashpaymentcentermanorggrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Orgs.CashPaymentCenter.ManOrg.Delete',
                    applyTo: 'b4deletecolumn', selector: 'cashpaymentcentermanorggrid',
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
            xtype: 'grideditwindowaspect',
            name: 'cashpaymentcenterManOrgGridWindowAspect',
            gridSelector: 'cashpaymentcentermanorggrid',
            editFormSelector: 'cashpaymentcentermanorgeditwindow',
            modelName: 'cashpaymentcenter.ManOrg',
            editWindowView: 'cashpaymentcenter.ManOrgEditWindow',
            getRecordBeforeSave: function (record) {
                var me = this,
                    mainView = me.controller.getMainView(),
                    cashPaymCenterValue = me.controller.getContextValue(mainView, 'cashPaymentCenterId');

                record.set('CashPaymentCenter', parseInt(cashPaymCenterValue));
                return record;
            },
            onAfterSetFormData: function (aspect, rec, form) {
                if (form) {
                    var roGrid = form.down('cashpaymentcentermanorgrealobjgrid');
                    form.show();
                    roGrid.getStore().filter('cpcManOrgId', form.getForm().getRecord().getId());
                }
            },
            onSaveSuccess: function (asp) {
                var editWindow = asp.getForm(),
                    caspPaymCenterManOrgId = editWindow.getForm().getRecord().getId(),
                    roGrid = editWindow.down('cashpaymentcentermanorgrealobjgrid'),
                    roStore = roGrid.getStore();

                asp.controller.mask('Сохранение', roGrid);

                if (roStore.data.items.length === 0) {
                    asp.controller.unmask();
                    editWindow.close();
                    return;
                }

                roStore.each(function (rec) {
                    rec.set('CashPaymentCenterManOrg', caspPaymCenterManOrgId);
                });
                roStore.sync({
                    success: function () {
                        roStore.load();
                        asp.controller.unmask();
                        editWindow.close();
                        asp.controller.getMainView().getStore().load();
                    },
                    failure: function (resp) {
                        var errorText = '';
                        if (resp.operations[0]) {
                            errorText = Ext.decode(resp.operations[0].response.responseText).message;
                        }
                        if (!errorText) {
                            errorText = 'Не удалось сохранить управляемые дома';
                        }
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', errorText);
                    },
                    scope: asp.controller
                });
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'cashpaymentcenterAddRealObjManOrgGridAspect',
            gridSelector: 'cashpaymentcentermanorgrealobjgrid',
            storeName: 'cashpaymentcenter.ManOrgRealObj',
            modelName: 'cashpaymentcenter.ManOrgRealObj',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#cashpaymentcenterAddRealObjMultiSelectWindow',
            storeSelect: 'cashpaymentcenter.ManOrgRealityObjectForAdd',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор домов',
            titleGridSelect: 'Дома для выбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальный район',
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
                    header: 'Управляющая компания',
                    xtype: 'gridcolumn',
                    dataIndex: 'ManOrg',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        store = me.getGrid().getStore();

                    records.each(function (rec) {
                        var manOrgRealObjRecord = Ext.create('B4.model.cashpaymentcenter.ManOrgRealObj', {
                            Id: 0,
                            CashPaymentCenterManOrg: null,
                            RealityObject: rec.get('RealityObject'),
                            Municipality: rec.get('Municipality'),
                            Address: rec.get('Address'),
                            DateStart: new Date(),
                            DateEnd: null
                        });
                        store.add(manOrgRealObjRecord);
                    });

                    return true;
                }
            },
            deleteRecord: function (record) {
                var me = this;
                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (res) {
                    if (res === 'yes') {
                        me.getGrid().getStore().remove(record);
                    }
                }, me);
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'cashpaymentcentermanorggrid b4updatebutton': {
                click: function () {
                    me.getMainView().getStore().load();
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('cashpaymentcentermanorggrid'),
            store = view.getStore();

        me.bindContext(view);
        me.setContextValue(view, 'cashPaymentCenterId', id);
        me.application.deployView(view, 'cashpayment_center');

        store.clearFilter(true);
        store.filter('cashPaymentCenterId', id);
    }
});