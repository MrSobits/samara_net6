Ext.define('B4.view.TarifExportPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    alias: 'widget.tarifexportpanel',

    title: 'Экспорт тарифов',
    
    itemId: 'overhaulTarifExportPane',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            // такая блин вечная молодость
        });

        me.callParent(arguments);
    }
});