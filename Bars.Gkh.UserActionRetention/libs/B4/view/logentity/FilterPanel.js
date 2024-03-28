Ext.define('B4.view.logentity.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.logentityfilterpnl',

    requires: ['B4.view.Control.GkhTriggerField', 'B4.form.ComboBox'],

    closable: false,
    header: false,
    bodyPadding: 5,
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    margin: '10 0 0 0',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 50,
                        labelAlign: 'right',
                        width: 220
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'EntityName',
                            fieldLabel: 'Объект',
                            emptyText: 'Все объекты'
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'UserLogin',
                            fieldLabel: 'Логин',
                            emptyText: 'Все пользователи'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 50,
                        labelAlign: 'right',
                        width: 220
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата с',
                            format: 'd.m.Y',
                            name: 'DateFrom'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'по',
                            labelWidth: 30,
                            format: 'd.m.Y',
                            name: 'DateTo'
                        },
                        { xtype: 'container', width: 10 },
                        {
                            xtype: 'button',
                            name: 'btnShowLog',
                            margin: '0 5 0 0',
                            width: 120,
                            text: 'Заполнить'
                        },
                        {
                            xtype: 'button',
                            margin: '0 0 5 0',
                            name: 'jsonExport',
                            width: 120,
                            text: 'Выгрузка в json'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});