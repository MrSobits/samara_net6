Ext.define('B4.view.regop.bankstatement.distributable_auto.DistributionDetailWinBase', {
    extend: 'B4.form.Window',

    alias: 'widget.distributiondetailwinbase',
    closeAction: 'destroy',
    modal: true,

    width: 700,
    height: 500,
	title: 'Распределение платежа',
    distributionIds: null,
    distributionSource: null,
    code: null,

    gridAlias: null,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            getDistributionParams : function() {
                return {
                    distributionIds: Ext.JSON.encode(me.distributionIds),
                    distributionSource: me.distributionSource,
                    distributableAutomatically: me.distributionIds.length > 1,
                    code: me.code
                }
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: me.gridAlias,
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    action: 'acceptDistribution',
                                    text: 'Применить распределение',
                                    iconCls: 'icon-accept'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('window').destroy();
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