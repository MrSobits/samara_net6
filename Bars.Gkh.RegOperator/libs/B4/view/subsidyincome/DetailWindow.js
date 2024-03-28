Ext.define('B4.view.subsidyincome.DetailWindow', {
    extend: 'B4.form.Window',

    modal: true,

    width: 1000,
    height: 500,
    bodyPadding: 5,
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Close',
        'B4.view.subsidyincome.DetailGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Детализация',
    alias: 'widget.subsidyincomedetailwin',

    entityId: null,

    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'subsidyincomedetailgrid',
                    entityId: me.entityId,
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
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            me.down('grid').getStore().load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сопоставить дома',
                                    iconCls: 'icon-accept',
                                    action: 'addrealobj'
                                },
                                {
                                    xtype: 'button',
                                    action: 'confirm',
                                    iconCls: 'icon-money-add',
                                    text: 'Подтвердить'
                                }
                            ]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [{
                                xtype: 'b4closebutton',
                                listeners: {
                                    'click': function() {
                                        me.close();
                                    }
                                }
                            }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});