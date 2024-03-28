Ext.define('B4.controller.dict.PaymentPenalties', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'dict.PaymentPenalties',
        'dict.PaymentPenaltiesExcludePersAcc'
    ],
    stores: [
        'dict.PaymentPenalties',
        'dict.PaymentPenaltiesExcludePersAcc',
        'dict.PaymentPenaltiesBasePersonalAccount'
    ],
    views: [
        'dict.paymentpenalties.Grid',
        'dict.paymentpenalties.EditWindow',
        'dict.paymentpenalties.ExcludePersAccGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.paymentpenalties.Grid',
    mainViewSelector: 'paymentpenaltiesGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'paymentpenaltiesGrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'paymentpenaltiesGridWindowAspect',
            gridSelector: 'paymentpenaltiesGrid',
            editFormSelector: '#paymentpenaltiesEditWindow',
            storeName: 'dict.PaymentPenalties',
            modelName: 'dict.PaymentPenalties',
            editWindowView: 'dict.paymentpenalties.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    var form = asp.getForm(),
                        grid = form.down('paymentpenaltiesexcludepersaccgrid'),
                        store = grid.getStore();

                    asp.controller.setContextValue(asp.controller.getMainView(), 'payPenaltiesId', record.get('Id'));

                    store.filter('payPenaltiesId', record.get('Id'));
                }
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'paymentpenaltiesExcludePersAccAspect',
            gridSelector: 'paymentpenaltiesexcludepersaccgrid',
            storeName: 'dict.PaymentPenaltiesExcludePersAcc',
            modelName: 'dict.PaymentPenaltiesExcludePersAcc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#paymentpenaltiesExcludePersAccMultiSelectWindow',
            storeSelect: 'dict.PaymentPenaltiesBasePersonalAccount',
            storeSelected: 'dict.PaymentPenaltiesBasePersonalAccount',
            titleSelectWindow: 'Выбрать исключения',
            titleGridSelect: 'Лицевые счета для отбора',
            titleGridSelected: 'Выбранные лицевые счета',
            columnsGridSelect: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'МР', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RoomAddress', flex: 1, filter: { xtype: 'textfield' } }

            ],
            columnsGridSelected: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', flex: 1, sortable: false },
                { header: 'МР', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RoomAddress', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                var me = this,
                    grid = me.getGrid(),
                    window = grid.up('#paymentpenaltiesEditWindow'),
                    crFundTypeField = window.down('[name=DecisionType]'),
                    payPenaltiesId = me.controller.getContextValue('payPenaltiesId');

                operation.params.crFoundType = crFundTypeField.getValue();
                operation.params.payPenaltiesId = payPenaltiesId;
            },
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddExcludePersAccs', 'PaymentPenalties'),
                            method: 'POST',
                            params: {
                                persAccIds: Ext.encode(recordIds),
                                payPenaltiesId: asp.controller.getContextValue(asp.controller.getMainView(), 'payPenaltiesId')
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.getGrid().getStore().load();
                            Ext.Msg.alert('Сохранение!', 'Исключения сохранены успешно');
                            return true;
                        }).error(function (response) {
                            asp.controller.unmask();

                            if (!response.success) {
                                Ext.Msg.alert('Ошибка!', response.message);
                            }
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать исключения');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('paymentpenaltiesGrid');
        me.bindContext(view);
        me.getStore('dict.PaymentPenalties').load();
    }
});