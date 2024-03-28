Ext.define('B4.view.realityobj.MapPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.realityobjmappanel',
    closable: true,
    title: 'Карта',
    layout: 'fit',
    initComponent: function () {
        var me = this;
        me.domId =  'ya-map-' + Ext.id(),
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'component',
                    renderTpl: new Ext.XTemplate(
                        '<div id="' + me.domId + '" style="width: 100%; height: 100%;"></div>'
                    )
                }
            ]
        });

        me.callParent(arguments);
    }
});
