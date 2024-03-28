Ext.define('B4.view.program.ThirdStagePanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.programthirdstagepanel',
    title: 'Долгосрочная программа',
    requires: [
        'B4.view.program.ThirdStageGrid',        
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.view.dict.municipality.Grid'
    ],

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        {
            xtype: 'programthirdstagegrid',
            flex: 1,
            border: 0
        }
    ]
});