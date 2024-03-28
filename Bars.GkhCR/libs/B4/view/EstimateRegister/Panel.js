Ext.define('B4.view.estimateregister.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр смет',
    alias: 'widget.estimateRegisterPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.view.estimateregister.Grid',
        'B4.store.dict.ProgramCrObj'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    itemId: 'estimateFilterPanel',
                    closable: false,
                    split: false,
                    border: false,
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    header: false,
                    layout: 'anchor',
                    trackResetOnLoad: true,
                    autoScroll: true,
                    defaults: {
                        labelWidth: 90,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Name',
                            editable: false,
                            itemId: 'sfProgramCr',
                           

                            store: 'B4.store.dict.ProgramCrObj',
                            width: 650,
                            fieldLabel: 'Программа КР'
                        }
                    ]
                },
                {
                    xtype: 'estimateregistergrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});
