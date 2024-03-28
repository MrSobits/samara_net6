/**
*   @class B4.aspects.GkuInfoToolbar
*
*   Аспект для добавления и управления Toolbar'ом на панеле информации о ЖКУ дома
*
*/
Ext.define('B4.aspects.GkuInfoToolbar', {
    extend: 'B4.base.Aspect',
    alias: 'widget.gkuInfoToolbar',

    requires: [
        'B4.ux.button.Update',
        'B4.form.MonthPicker',
        'B4.QuickMsg',
        'B4.plugins.CalculationMonthWatcher'
    ],
    
    init: function (controller) {
        var me = this,
            actions = {};

        actions[controller.mainViewSelector] = {
            'afterrender': {
                fn: me.addToolBar,
                scope: me
            }
        };
        
        actions[controller.mainViewSelector + ' b4updatebutton'] = {
            'click': {
                fn: me.refresh,
                scope: me
            }
        };
        
        actions[controller.mainViewSelector + ' b4monthpicker[name=Month]'] = {
            'change': {
                fn: me.refresh,
                scope: me
            }
        };
        
        actions[controller.mainViewSelector + ' combobox[name=Code]'] = {
            'change': {
                fn: me.refresh,
                scope: me
            }
        };

        controller.control(actions);
        
        me.callParent(arguments);
    },
    
    addToolBar: function(grid) {
        var me = this,
            controller = me.controller,
            toolbar;

        controller.mask();

        toolbar = Ext.create('Ext.toolbar.Toolbar', {
            dock: 'top',
            
            items: [{
                    xtype: 'buttongroup',
                    columns: 3,
                    items: [
                        { xtype: 'b4updatebutton' },
                        {
                            xtype: 'b4monthpicker',
                            name: 'Month',
                            editable: false,
                            plugins: [
                            {
                                ptype: 'calculationmonthwatcher',
                                afterTriggerEvent: function () {
                                    me.refresh();
                                }
                            }]
                        },
                        {
                            xtype: 'combobox',
                            name: 'Code',
                            editable: false,
                            queryMode: 'local',
                            visible: false,
                            displayField: 'Id',
                            valueField: 'Id',
                            fieldLabel: 'Секция',
                            labelAlign: 'right'
                        }
                    ]
                }
            ]
        });
        grid.dockedItems.add(toolbar);

        grid.getStore().on('beforeload', me.onBeforeStoreLoad, me);
        
        B4.Ajax.request({
            method: 'GET',
            url: B4.Url.action('GetCodes', 'RealityObjectExtra'),
            params: {
                id: controller.getContextValue(controller.getMainView(), 'realityObjectId')
            }
        }).next(function (response) {
            var resp = Ext.decode(response.responseText),
                combo = grid.down('combobox[name=Code]');
            
            if (!resp.success || !resp.codes) {
                B4.QuickMsg.msg('Ошибка', 'Ошибка получения секций', 'error');
                controller.unmask();
                return;
            }
            var codes = resp.codes;
            if (codes.length === 0) {
                B4.QuickMsg.msg('Внимание', 'Не найдены секции для просмотра сведений о ЖКУ', 'error');
                controller.unmask();
                return;
            }
            var store = Ext.create('Ext.data.Store', {
                fields: ['Id']
            });
            Ext.iterate(codes, function (item) { store.add({ Id: item }); });
            combo.bindStore(store);
            combo.setVisible(codes.length > 1);
            combo.setValue(codes[0]);
            controller.unmask();
        }).error(function () {
            controller.unmask();
        });
    },

    onBeforeStoreLoad: function (store, operation) {
        
        var me = this,
            controller = me.controller,
            view = controller.getMainView(),
            date = view.down('b4monthpicker[name=Month]').getValue(),
            code = view.down('combobox[name=Code]').getValue(),
            realityObjectId = controller.getContextValue(controller.getMainView(), 'realityObjectId');
        
        if (!code || !date) {
            return false;
        }
        
        operation.params.realityObjectId = realityObjectId;
        operation.params.date = date;
        operation.params.code = code;
        return true;
    },
    
    refresh: function() {
        var me = this,
            controller = me.controller,
            view = controller.getMainView();

        view.getStore().reload();
    }
});