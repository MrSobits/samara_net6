Ext.define('B4.view.dict.crperiod.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.crperiodwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    modal: true,
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Редактирование',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'numberfield',
                    name: 'YearStart',
                    fieldLabel: 'Начальный год',
                    hideTrigger: true,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    fieldLabel: 'Конечный год',
                    name: 'YearEnd',
                    hideTrigger: true,
                    allowBlank: false
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
                                    xtype: 'b4savebutton'
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
                                    xtype: 'b4closebutton'
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