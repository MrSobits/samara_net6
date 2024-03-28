Ext.define('B4.view.olap.Cube', {
    extend: 'Ext.form.Panel',
    requires: ['Ext.ux.IFrame'],

    title: 'OLAP кубы',
    alias: 'widget.olapcube',
    layout: 'fit',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [  
                {
                    xtype: 'uxiframe',
                    src: B4.Url.action('/Cube/LoadAlphaBI')
                }
            ]
        });

        me.callParent(arguments);
    }
});