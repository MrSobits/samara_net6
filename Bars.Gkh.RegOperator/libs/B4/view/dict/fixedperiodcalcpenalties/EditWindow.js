Ext.define('B4.view.dict.fixedperiodcalcpenalties.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minHeight: 200,
    maxHeight: 200,
    bodyPadding: 5,
    alias: 'widget.fixedperiodcalcpenaltieseditwindow',
    title: 'Установка границ периода',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'numberfield',
                    name: 'StartDay',
                    fieldLabel: 'День начала периода',
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    name: 'EndDay',
                    fieldLabel: 'День окончания периода',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    fieldLabel: 'Действует с',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    fieldLabel: 'Дата по',
                    readOnly: true
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