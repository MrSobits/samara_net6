Ext.define('B4.view.deliveryagent.RealObjAddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.delagentrealobjaddwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    minHeight: 500,
    maxHeight: 500,
    width: 800,
    minWidth: 800,
    bodyPadding: 5,
    title: 'Договор с домами',
    requires: [
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.deliveryagent.AddRealObjGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                bodyStyle: Gkh.bodyStyle,
                border:false,
                padding: '5 5 5 5',
                defaults: {
                    labelWidth: 220,
                    labelAlign: 'right'
                },
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStart',
                            allowBlank: false,
                            fieldLabel: 'Дата начала действия договора',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEnd',
                            allowBlank: false,
                            fieldLabel: 'Дата окончания действия договора',
                            format: 'd.m.Y'
                        }
                    ]
            },
            {
                xtype: 'delagentaddrealobjgrid',
                flex: 1
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