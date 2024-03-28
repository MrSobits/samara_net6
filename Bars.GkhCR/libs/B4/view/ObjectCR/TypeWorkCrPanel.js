Ext.define('B4.view.objectcr.TypeWorkCrPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,

    alias: 'widget.objectcr_type_work_cr_panel',
    
    title: 'Виды работ',
    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.view.objectcr.TypeWorkCrGrid',
        'B4.view.objectcr.TypeWorkCrHistoryGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'objectcr_type_work_cr_grid',
                    name: 'OnlyTypeWorks',
                    title:'',
                    hidden: true
                },
                {
                    xtype: 'tabpanel',
                    enableTabScroll: true,
                    flex: 1,
                    items: [
                        { xtype: 'objectcr_type_work_cr_grid' },
                        { xtype: 'objectcr_type_work_cr_history_grid' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});