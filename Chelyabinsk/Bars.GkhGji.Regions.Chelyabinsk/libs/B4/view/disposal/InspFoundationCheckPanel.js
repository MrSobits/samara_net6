Ext.define('B4.view.disposal.InspFoundationCheckPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.disposalinspfoundationcheckpanel',
    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.view.disposal.InspFoundationCheckGrid',
        'B4.view.disposal.NormDocItemGrid'
    ],

    title: 'НПА проверки',

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'disposalinspfoundationcheckgrid',
                    flex: 1
                },
                {
                    xtype: 'normdocitemgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});