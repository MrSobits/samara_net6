Ext.define('B4.view.planreductionexpense.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'border',
    width: 700,
    height: 500,
    bodyPadding: 5,
    itemId: 'planReductionExpenseEditWindow',
    title: 'План мер по снижению расходов',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.view.planreductionexpense.WorksGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    height: 25,
                    defaults: {
                        anchor: '100%',
                        labelWidth: 190,
                        labelAlign: 'right'
                    },  
                    items: [                    
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Сведения о выполнении',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'planreductexpworksgrid',
                    region: 'center'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
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