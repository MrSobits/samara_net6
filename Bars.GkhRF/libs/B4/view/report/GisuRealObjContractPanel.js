Ext.define('B4.view.report.GisuRealObjContractPanel', {
    extend: 'Ext.form.Panel',
    requires: ['B4.view.Control.GkhTriggerField'],
    title: '',
    itemId: 'gisuRealObjContractPanel',
    layout: 'vbox',
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                }
            ]
        });

        me.callParent(arguments);
    }
});