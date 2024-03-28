Ext.define('B4.view.efficiencyrating.analitics.Panel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.view.efficiencyrating.analitics.GraphPanel',
        'B4.view.efficiencyrating.analitics.ConstructorPanel'
    ],

    title: 'Аналитические показатели рейтинга эффективности УО',
    alias: 'widget.efanaliticspanel',

    layout: { type: 'vbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,
    closable: true,
    border: false,

    initComponent: function() {
        var me = this;
        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'tabpanel',
                    name: 'maintab',
                    anchor: '100%',
                    flex: 1,
                    defaults: {
                        closable: false
                    },
                    items: [
                        {
                            xtype: 'efanaliticsgraphpanel',
                            itemId: 'GraphPanel'
                        },
                        {
                            xtype: 'efanaliticsconstructorpanel',
                            itemId: 'ConstructorPanel'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});