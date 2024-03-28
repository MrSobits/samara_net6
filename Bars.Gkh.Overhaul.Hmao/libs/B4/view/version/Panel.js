Ext.define('B4.view.version.Panel', {
    extend: 'Ext.panel.Panel',

    closable: true,
    alias: 'widget.programversionpanel',
    title: 'Версии программы',
    requires: [
        'B4.form.ComboBox',
        'B4.view.version.Grid',
        'B4.store.dict.municipality.ByParam'
    ],

    bodyStyle: Gkh.bodyStyle,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        
        {
            xtype: 'programversiongrid',
            flex: 1,
            border: false
        }
    ]
});