Ext.define('B4.view.export.ManagementSysImport', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.form.FileField'
    ],

    title: 'Экспорт данных для ГАС "Управление"',
    alias: 'widget.managementsysimport',
    layout: 'form',
    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
    bodyPadding: 5,
    closable: true,
    closeAction: 'hide',
    
    initComponent: function () {
        var me = this,
            disDate = (function() {
                var arr = [];
                Ext.each(new Array(32), function(val, i) {
                    if (i > 1) {
                        arr.push("^" + (i < 10 ? "0"+i : i));
                    }
                });
                return arr;
            })();
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    border: false,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                    itemId: 'importForm',
                    layout: {
                        type: 'vbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '5 0 5 0',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ServiceUrl',
                                    labelAlign: 'right',
                                    labelWidth: 150,
                                    fieldLabel: 'Адрес ГАСУ сервиса',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'button',
                                    margin: '0 0 0 20',
                                    iconCls: 'icon-table-go',
                                    text: 'Передать данные',
                                    textAlign: 'left',
                                    action: 'Export',
                                    disabled: true
                                },
                                {
                                    xtype: 'button',
                                    margin: '0 0 0 20',
                                    iconCls: 'icon-table-save',
                                    text: 'Сохранить',
                                    textAlign: 'left',
                                    action: 'Save',
                                    disabled: true
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'UserName',
                            fieldLabel: 'Логин',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            inputType: 'password',
                            name: 'UserPassword',
                            fieldLabel: 'Пароль',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            allowBlank: false,
                            labelAlign: 'right',
                            format: 'd.m.Y',
                            name: 'PeriodStart',
                            fieldLabel: 'Начало периода',
                            disabledDates: disDate
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
