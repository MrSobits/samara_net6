Ext.define('B4.view.skap.Skap', {
    extend: 'Ext.form.Panel',
    requires: [],

    title: 'Скап',
    alias: 'widget.skapskap',
    layout: 'fit',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {            
            items: [{
                xtype: 'box',
                autoEl: {
                    tag: 'iframe',
                    src: ' http://skap_chelny.dpridprod.ru/'
                    //B4.Url.action('/Skap/LoadSkap')
                }
            }]
        });

        me.callParent(arguments);
    }
});