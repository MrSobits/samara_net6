Ext.define('B4.view.suspenseaccount.credit.DistributionWin', {
    extend: 'B4.form.Window',

    alias: 'widget.suspacccreditdistributionwin',

    requires: [
        'B4.view.suspenseaccount.credit.DistributionGrid'
    ],

    title: 'Распределение платежа',

    modal: true,

    width: 800,
    height: 500,

    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            defaults: {
                margin: '5 5 5 5'
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            border: 0,
            items: [
                {
                    xtype: 'suspacccreditdistributiongrid',
                    enableCellEdit: me.enableCellEdit,
                    sum: me.sum,
                    flex: 1,
                    distrtype: me.distrtype
                }
            ]
        });
        me.callParent(arguments);
    }
});