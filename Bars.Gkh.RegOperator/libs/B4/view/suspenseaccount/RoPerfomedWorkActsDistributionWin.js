Ext.define('B4.view.suspenseaccount.RoPerfomedWorkActsDistributionWin', {
    extend: 'B4.form.Window',
    requires: ['B4.view.suspenseaccount.RoPerfomedWorkActsDistributionGrid'],

    alias: 'widget.roperfomedworkactsdistributionwin',

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
                    xtype: 'roperfomedworkactsdistributiongrid',
                    enableCellEdit: me.enableCellEdit,
                    sum: me.sum,
                    flex: 1
                }
            ]
        });
        me.callParent(arguments);
    }
});