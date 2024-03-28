Ext.define('B4.view.payment.AddWindow', {
    extend: 'B4.form.Window',
    requires: [
    'B4.form.SelectField',
    'B4.store.RealityObject',
    'B4.ux.button.Close',
    'B4.ux.button.Save'
    ],
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'paymentAddWindow',
    title: 'Оплата капитального ремонта',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    fieldLabel: 'Жилой дом',
                   

                    store: 'B4.store.RealityObject',
                    allowBlank: false,
                    editable: false,
                    labelWidth: 75,
                    labelAlign: 'right',
                    textProperty: 'Address',
                    columns: [
                        { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1 },
                        { text: 'Адрес', dataIndex: 'Address', flex: 1 }
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