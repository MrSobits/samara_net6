Ext.define('B4.view.passport.CopyPassportWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.copypassportwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    modal: true,
    layout: 'form',
    width: 500,
    height: 200,
    resizable: false,
    bodyPadding: 5,
    title: 'Копирование структуры паспортов',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this,
            cbMonth = Ext.create('Ext.form.ComboBox', {
                name: 'ValidFromMonth',
                fieldLabel: 'Месяц:',
                queryMode: 'local',
                valueField: 'num',
                displayField: 'name',
                allowBlank: false,
                editable: false,
                store: Ext.create('Ext.data.Store', {
                    fields: ['num', 'name'],
                    data: [
                        { "num": 1, "name": "Январь" },
                        { "num": 2, "name": "Февраль" },
                        { "num": 3, "name": "Март" },
                        { "num": 4, "name": "Апрель" },
                        { "num": 5, "name": "Май" },
                        { "num": 6, "name": "Июнь" },
                        { "num": 7, "name": "Июль" },
                        { "num": 8, "name": "Август" },
                        { "num": 9, "name": "Сентябрь" },
                        { "num": 10, "name": "Октябрь" },
                        { "num": 11, "name": "Ноябрь" },
                        { "num": 12, "name": "Декабрь" }
                    ]
                })
            });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 125,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Параметры новой стркутуры',
                    items: [
                        {
                            xtype: 'container',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Name',
                                            flex: 1,
                                            fieldLabel: 'Наименование',
                                            allowBlank: false
                                        }
                                    ]
                                },
                                cbMonth,
                                {
                                    xtype: 'numberfield',
                                    name: 'ValidFromYear',
                                    fieldLabel: 'Год',
                                    allowDecimals: false,
                                    minValue: 2000,
                                    maxValue: 3000,
                                    allowBlank: false
                                }
                            ]
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Копировать'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Отмена',
                                    listeners: {
                                        click: {
                                            fn: function (btn) {
                                                btn.up('copypassportwin').close();
                                            }
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
