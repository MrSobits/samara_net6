Ext.define('B4.view.regop.realty.RealtyPaymentAccountOperationEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realtypayaccoperationeditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    height: 400,
    bodyPadding: 5,
    title: 'Редактирование',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.regop.realty.RealtyPaymentAccountOperationCreditDetailsGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    border: false,
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'realtypayaccopercreddetailsgrid',
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
                                    xtype: 'b4updatebutton'
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