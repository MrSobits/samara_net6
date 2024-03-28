Ext.define('B4.controller.EffectivenessAndPerformanceIndexValue', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.EffectivenessAndPerformanceIndexValue',
        'B4.aspects.GridEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['EffectivenessAndPerformanceIndexValue'],
    stores: ['EffectivenessAndPerformanceIndexValue'],

    views: [
        'effectivenessandperformanceindexvalue.Grid',
        'effectivenessandperformanceindexvalue.EditWindow'],

    mainView: 'effectivenessandperformanceindexvalue.Grid',
    mainViewSelector: 'effectivenessandperformanceindexvaluegrid',

    aspects: [
        {
            xtype: 'effectivenessandperformanceindexvalueperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'effectivenessAndPerformanceIndexValueEditWindowAspect',
            gridSelector: 'effectivenessandperformanceindexvaluegrid',
            modelName: 'EffectivenessAndPerformanceIndexValue',
            storeName: 'EffectivenessAndPerformanceIndexValue',
            editFormSelector: 'effectivenessandperformanceindexvalueeditwindow',
            editWindowView: 'effectivenessandperformanceindexvalue.EditWindow',
            listeners: {
                beforerowaction: function (asp, grid, action, rec) {
                    if (action.toLowerCase() == 'doubleclick') {
                        return false;
                    }
                },
                beforesave: function (asp, rec) {
                    var startDate = rec.get('CalculationStartDate'),
                        endDate = rec.get('CalculationEndDate');
                    if (startDate > endDate) {
                        Ext.Msg.alert('Ошибка', 'Дата окончания расчета не может быть меньше даты начала расчета.');
                        return false;
                    }
                }
            }
        },
    ],

    init: function () {
        var me = this;
        
        me.control({
            'effectivenessandperformanceindexvaluegrid [name=sendToTorButton]': { 'click': { fn: me.onSendToTor, scope: me } },
            'effectivenessandperformanceindexvaluegrid [name=getFromTorButton]': { 'click': { fn: me.onGetFromTorButton, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);

        me.getStore('EffectivenessAndPerformanceIndexValue').load();
    },

    onSendToTor: function(btn) {
        var selectedRecords = btn.up('effectivenessandperformanceindexvaluegrid').getSelectionModel().getSelection(),
            me = this;

        if (selectedRecords.length === 0) {
            Ext.Msg.alert('Ошибка', 'Необходимо выбрать хотя бы одно значение');
            return;
        }
        me.mask('Отправка значений показателей эффективности', me.getMainComponent());
        var ids = [];
        selectedRecords.forEach(function(e) {
            ids.push(e.data.Id)
        });

        //отправка в ТОР
        B4.Ajax.request({
                url: B4.Url.action('SendEpIndexToTor', 'TorIntegration'),
                params: {
                    ids: ids
                },
                timeout: 9999999
            })
            .next(function(response) {
                me.unmask();
                Ext.Msg.alert('Отправка значений показателей эффективности', response.message || 'Выполнено успешно');
            })
            .error(function(e) {
                me.unmask();
                Ext.Msg.alert('Ошибка', e.message || 'Произошла ошибка');
            });
    },

    onGetFromTorButton: function (btn) {
        var me = this;
        me.mask('Получение значений показателей эффективности', me.getMainComponent());
        //получение из ТОР
        B4.Ajax.request({
            url: B4.Url.action('GetEpIndexFromTor', 'TorIntegration'),
            timeout: 9999999
        }).next(function (response) {
            var success = Ext.decode(response.responseText).success;
            if (success) {
                btn.up('effectivenessandperformanceindexvaluegrid').getStore().removeAll();
                btn.up('effectivenessandperformanceindexvaluegrid').getStore().load();
                me.unmask();
                Ext.Msg.alert('Получение значений показателей эффективности', response.message || 'Выполнено успешно');
                return;
            }
            me.unmask();
            Ext.Msg.alert('Получение значений показателей эффективности', 'Ошибка при выполнении');
        }).error(function(e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Произошла ошибка');
        });;
    },
});