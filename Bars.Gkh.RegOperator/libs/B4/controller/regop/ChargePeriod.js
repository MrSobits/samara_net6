Ext.define('B4.controller.regop.ChargePeriod', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.store.regop.ChargePeriod'
    ],

    models: [
        'regop.ChargePeriod'
    ],

    stores: [
        'regop.ChargePeriod'
    ],

    views: [
        'regop.chargeperiod.Grid',
        'regop.chargeperiod.EditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'chargeperiodgrid'
        }
    ],
    
    //mainView: 'regop.chargeperiod.Grid',
    //mainViewSelector: 'chargeperiodgrid',

    //aspects: [
        //{
        //    xtype: 'grideditwindowaspect',
        //    name: 'chargesPeriodGridAspect',
        //    gridSelector: 'chargeperiodgrid',
        //    editFormSelector: 'chargeperiodwindow',
        //    editWindowView: 'regop.chargeperiod.EditWindow',
        //    modelName: 'regop.ChargePeriod',
        //    storeName: 'regop.ChargePeriod',
        //    rowAction: function (grid, action, record) {
        //        if (!grid || grid.isDestroyed) return;
        //        if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
        //            switch (action.toLowerCase()) {
        //                case 'edit':
        //                    this.editRecord(record);
        //                    break;
        //                case 'delete':
        //                    if (record.get('IsClosed')) {
        //                        B4.QuickMsg.msg('Предупреждение', 'Можно удалить только открытый период', 'warning');
        //                        return;
        //                    }
        //                    this.deleteRecord(record);
        //                    break;
        //            }
        //        }
        //    }
        //}
    //],

    init: function() {
        var me = this;
        me.control({
            'chargeperiodgrid [action="CloseCurrentPeriod"]': {
                click: {
                    fn: me.onRunCloseCurrentPeriod,
                    scope: me
                }
            },
            'chargeperiodgrid' : {
                render: function(grid) {
                    grid.getStore().on('load', me.onStoreLoad, me);
                    grid.getStore().load();
                }
            },
            'chargeperiodgrid b4updatebutton': {
                click: function(b) {
                    b.up('chargeperiodgrid').getStore().load();
                }
            }
        });
        
        // При удалении автоматически создаем новый период и уведомляем пользователя
        //me.getAspect('chargesPeriodGridAspect').on('deletesuccess', function () {
        //    B4.QuickMsg.msg('Успешно', 'Был создан новый период', 'success');
        //});
        
        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('chargeperiodgrid');
        me.bindContext(view);
        me.application.deployView(view);
    },
    
    onRunCloseCurrentPeriod: function () {
        var me = this;

        me.mask(me.getMainView());

        B4.Ajax.request({
            url: B4.Url.action('GetCountDocumentFormedInOpenPerod', 'ChargePeriod'),
            timeout: 360000
        })
        .next(function (resp) {
            var res = Ext.decode(resp.responseText);
            if (res.data > 0) {
                me.unmask();
                Ext.Msg.confirm('Внимание', Ext.String.format('Сформировано документов в открытом периоде по {0} лс. Уверены ли Вы, что необходимо закрыть период?',
                    res.data), function (result) {

                        if (result === 'yes') {
                            me.onCloseCurrentPeriod();
                        }
                    });
            } else {
                me.onCloseCurrentPeriod();
            }
        })
        .error(function (err) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', err.message || err, 'error');
        });
    },

    onCloseCurrentPeriod: function() {
        var me = this;
        
        me.mask(me.getMainView());

        B4.Ajax.request({
            url: B4.Url.action('CloseCurrentPeriod', 'ChargePeriod'),
            timeout: 999999
        }).next(function (resp) {
            var res = Ext.decode(resp.responseText);
            if (res.data && res.data.message) {
                me.unmask();
                B4.QuickMsg.msg('Информация', res.data.message, 'info');
            } else {
                me.unmask();
                me.getMainView().getStore().load();
                
                Ext.Msg.alert('Закрытие периода', "Задача успешно поставлена в очередь на обработку. " +
                    "Информация о статусе закрытия периода содержится в пункте меню \"Процессы\"");
            }
        }).error(function (err) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', err.message || err, 'error');
        });
    },
    
    onStoreLoad: function (store) {
        var me = this;
        
        if (store.getCount() == 0) {
            me.mask('Создание периода', me.getMainView());
            B4.Ajax.request({
                url: B4.Url.action('CreateFirstPeriod', 'ChargePeriod'),
                timeout: 9999999
            }).next(function (response) {
                try {
                    var res = Ext.decode(response.responseText);
                    if (res.data) {
                        B4.QuickMsg.msg('Информация', res.data.message, 'info');
                        me.unmask();
                    } else {
                        me.unmask();
                        store.load();
                    }
                } catch(e) {
                    me.unmask();
                } 
            }).error(function() {
                me.unmask();
            });
        }
    }
});