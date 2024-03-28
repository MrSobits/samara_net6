Ext.define('B4.view.longtermprobject.accrualsaccount.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.accrualsaccounteditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    minWidth: 700,
    minHeight: 500,
    height: 400,
    width: 700,
    bodyPadding: 5,
    title: 'Cчет начислений',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.longtermprobject.accrualsaccount.AccountsAggregationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200
            },
            items: [
                {
                    xtype: 'container',
                    frame: true,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    border: false,
                    margins: -1,
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            allowBlank: false,
                            maxLength: 50,
                            name: 'Number',
                            fieldLabel: 'Номер',
                            padding: '5 5 5 5'
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 100,
                                flex: 1,
                                labelAlign: 'right'
                            },
                            padding: '5 5 5 5',
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    name: 'OpenDate',
                                    fieldLabel: 'Дата открытия',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    name: 'CloseDate',
                                    fieldLabel: 'Дата закрытия'
                                }
                            ]
                        },
                        {
                            xtype: 'accountsaggregationgrid',
                            columnLines: true,
                            flex: 1
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