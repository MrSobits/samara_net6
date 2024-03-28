Ext.define('B4.view.suspenseaccount.RealObjDistributionWindowWithGrid', {
    extend: 'B4.form.Window',

    alias: 'widget.realobjdistributionwindowwithgrid',

    requires: [
        'B4.view.suspenseaccount.RealObjDistributionGrid'
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
                    xtype: 'realobjdistributiongrid',
                    enableCellEdit: me.enableCellEdit,
                    sum: me.sum,
                    flex: 1
                }                   
            ]
        });
        me.callParent(arguments);
    }
});