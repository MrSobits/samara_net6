Ext.define('B4.view.infoaboutreductionpayment.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Сведения о случаях снижения платы',
    itemId: 'infoAboutReductionPaymentEditPanel',
    layout: 'fit',

    requires: [
        'B4.view.infoaboutreductionpayment.GridPanel',
        'B4.ux.button.Update',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'infreductpaymgridpanel'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'addInfoAboutReductionPaymentButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    itemId: 'infoAboutReductionPaymentSaveButton'
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
