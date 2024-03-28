Ext.define('B4.view.administration.executionaction.actionwithparams.CreateTransfersWithOwnersAction', {
    extend: 'Ext.form.Panel',

    border: false,

    initComponent: function () {

        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Трансферы слияния разделены вручную',
                    name: 'IsManuallyMerge'
                },
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Владелец проставлен вручную',
                    name: 'DontSetOwners'
                }
            ]
        });

        me.callParent(arguments);
    }
});