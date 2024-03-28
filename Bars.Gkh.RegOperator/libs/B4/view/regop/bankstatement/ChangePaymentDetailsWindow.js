Ext.define('B4.view.regop.bankstatement.ChangePaymentDetailsWindow', {
    extend: 'B4.form.Window',

    modal: true,

    width: 700,
    bodyPadding: 5,
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Изменение назначения платежа',
    alias: 'widget.changepaymentdetailswindow',

    initComponent: function() {
        var me = this;
            //store = Ext.create('B4.store.DistributionType');

        //store.on('beforeload', function(s, operation) {
        //    operation.params = operation.params || {};
        //    operation.params.isForDetailWindow = true;
        //    operation.params.distributionId = me.entityId;
        //    operation.params.distributionSource = me.source;
        //});

        Ext.apply(me, {
            items: [
                {
                    xtype: 'textarea',
                    name: 'PaymentDetails',
                    fieldLabel: 'Назначение платежа',
                    labelAlign: 'right'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        'click': function() {
                                            me.close();
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