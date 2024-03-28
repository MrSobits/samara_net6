Ext.define('B4.view.program.CopyPricesWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.copypriceswin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',

        'B4.form.SelectField',
        'B4.view.version.Grid',
        'B4.store.version.ProgramVersion'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    closeAction: 'destroy',
    title: 'Скопировать стоимости',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 170,
                flex: 1
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.version.ProgramVersion',
                    name: 'Version',
                    fieldLabel: 'Версия для копирования',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'Муниципальное образование', dataIndex: 'Municipality', width: 180 },
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 },
                        { text: 'Дата', dataIndex: 'VersionDate', flex: 1 },
                        { text: 'Основная', dataIndex: 'IsMain', flex: 1, renderer: function (v) { return v ? 'Да' : 'Нет'; } }
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
                                    text: 'Применить'
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
                                    listeners: {
                                        click: {
                                            fn: function (btn) {
                                                btn.up('copypriceswin').close();
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