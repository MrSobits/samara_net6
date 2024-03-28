Ext.define('B4.view.baseplanaction.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.basePlanActionFilterPanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'basePlanActionFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update',
        'B4.form.SelectField'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    border: false,
                    width: 600,
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'FilterPlan',
                            store: 'B4.store.dict.PlanActionGji',
                            width: 500,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ],
                            editable: false,
                            fieldLabel: 'План'
                        },
                        {
                            width: 100,
                            xtype: 'b4updatebutton'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});