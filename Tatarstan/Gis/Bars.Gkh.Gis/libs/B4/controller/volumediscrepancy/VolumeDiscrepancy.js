Ext.define('B4.controller.volumediscrepancy.VolumeDiscrepancy', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'volumediscrepancymainview'
        }
    ],

    mainView: 'volumediscrepancy.MainView',
    mainViewSelector: 'volumediscrepancymainview',

    init: function () {
        var me = this;

        me.control(
            {
                'volumediscrepancymainview button[name=Refresh]': {
                    click: {
                        fn: me.show
                    }
                },
                'volumediscrepancymainview button[name=Show]': {
                    click: {
                        fn: me.show
                    }
                },
                'volumediscrepancymainview button[name=Publish]': {
                    click: {
                        fn: me.publish
                    }
                },
                'volumediscrepancymainview b4selectfield[name=MunicipalArea]': {
                    change: {
                        fn: function (control, opts) {
                            me.loadSettlement(control, opts);
                            me.loadStreet(control, opts);
                        }
                    }
                },
                'volumediscrepancymainview b4selectfield[name=Settlement]': {
                    change: {
                        fn: me.loadStreet
                    }
                }
            }
        );

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('volumediscrepancymainview');

        this.bindContext(view);
        this.application.deployView(view);

        this.reloadGrid();
    },

    show: function (control) {
        var view = control.up('volumediscrepancymainview'),
            discrepancyGrid = view.down('gridpanel[name=Discrepancy]');

        this.reloadGrid(control);
    },

    //Публикация
    publish: function (control) {
        var me = this,
            view = control.up('volumediscrepancymainview'),
            grid = view.down('gridpanel[name=Discrepancy]'),
            selected = grid.getSelectionModel().getSelection();

        if (selected.length == 0) {
            B4.QuickMsg.msg('Внимание', 'Выберите данные для публикации', 'warning');
            return;
        }

        view.getEl().mask('Публикация...');
        B4.Ajax.request({
            url: '/HouseServiceRegister/Publish',
            params: {
                Data: Ext.encode(selected.map(function (item) {
                    return item.data;
                }))
            }
        }).next(function () {
            view.getEl().unmask();
            me.reloadGrid(control);
        });
    },

    reloadGrid: function (control) {
        var view = this.getMainView() || Ext.widget('volumediscrepancymainview'),
          discrepancyGrid = view.down('gridpanel[name=Discrepancy]'),
          discrepancyStore = discrepancyGrid.getStore(),
          supplierField = view.down('b4selectfield[name=Supplier]'),
          serviceField = view.down('b4selectfield[name=Service]'),
          periodField = view.down('b4monthpicker[name=Period]'),
          municipalAreaField = view.down('b4selectfield[name=MunicipalArea]'),
          settlementField = view.down('b4selectfield[name=Settlement]'),
          streetField = view.down('b4selectfield[name=Street]'),
          nullValuesField = view.down('checkbox[name=ShowNullValues]');

        //Подписываемся на обновление основного грида и заполняем данные фильтра
        discrepancyStore.on({
            beforeload: function (store, operation) {
                var supplier = supplierField.value,
                    service = serviceField.value,
                    period = periodField.getValue(),
                    municipalAreaValue = municipalAreaField.getValue(),
                    settlementValue = settlementField.getValue(),
                    streetValue = streetField.getValue(),
                    showNullValues = nullValuesField.getValue();

                operation.params.Period = new Date(period.getFullYear(), period.getMonth(), 1);
                operation.params.ShowNullValues = showNullValues;

                if (supplier) {
                    operation.params.Supplier = supplier.Name;
                }
                if (service) {
                    operation.params.Service = service.Id;
                }
                if (municipalAreaValue) {
                    operation.params.MunicipalAreaGuid = municipalAreaValue;
                }
                if (settlementValue) {
                    operation.params.SettlementGuid = settlementValue;
                }
                if (streetValue && streetValue.length > 0 && streetValue != 'All') {
                    operation.params.StreetGuid = Ext.encode(streetValue);
                }
            }
        });

        discrepancyStore.load();
    },

    //Загрузка населенного пункта
    loadSettlement: function (control, opts) {
        var view = control.up('volumediscrepancymainview'),
            streetField = view.down('b4selectfield[name=Street]'),
            settlementField = view.down('b4selectfield[name=Settlement]');

        settlementField.setValue();
        streetField.setValue();

        streetField.disable();

        //поле очищено
        if (!opts) {
            settlementField.disable();
            return;
        }

        settlementField.getStore().on({
            beforeload: function (store, operation) {
                operation.params.ParentGuid = control.getValue();
            },
            load: function (store) {
                if (store.totalCount == 0) {
                    settlementField.disable();
                } else {
                    settlementField.enable();
                }
            }
        });

        settlementField.getStore().load();
    },

    //Загрузка улицы
    loadStreet: function (control, opts) {
        var view = control.up('volumediscrepancymainview'),
            municipalAreaField = view.down('b4selectfield[name=MunicipalArea]'),
            streetField = view.down('b4selectfield[name=Street]');

        streetField.setValue();

        //поле очищено
        if (!opts) {
            streetField.disable();
            return;
        }

        streetField.getStore().on({
            beforeload: function (store, operation) {
                operation.params.PlaceGuid = control.value
                    ? control.getValue()
                    : municipalAreaField.getValue();
            },
            load: function (store) {
                if (store.totalCount == 0) {
                    streetField.disable();
                } else {
                    streetField.enable();
                }
            }
        });

        streetField.getStore().load();
    }
});