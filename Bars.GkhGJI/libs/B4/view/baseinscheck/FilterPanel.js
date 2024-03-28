Ext.define('B4.view.baseinscheck.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.baseInsCheckFilterPanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'baseInsCheckFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update',
        'B4.view.Control.GkhTriggerField'
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
                    itemId: 'sfPlan',
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.PlanInsCheckGji',
                    width: 500,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ],
                    editable: false,
                    fieldLabel: 'План'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'inspectors',
                    itemId: 'trfInspectors',
                    fieldLabel: 'Инспекторы',
                    width: 500
                },
                {
                    xtype: 'container',
                    border: false,
                    width: 650,
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelWidth: 130,
                            fieldLabel: 'Период с',
                            width: 290,
                            itemId: 'dfDateStart'
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            fieldLabel: 'по',
                            width: 210,
                            itemId: 'dfDateEnd'
                        },
                        {
                            width: 10,
                            xtype: 'component'
                        },
                        {
                            width: 100,
                            itemId: 'updateGrid',
                            xtype: 'b4updatebutton'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});