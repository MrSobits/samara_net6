Ext.define('B4.view.baseomsu.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.baseOMSUFilterPanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'baseOMSUFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Update',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 80,
                labelAlign: 'right',
                width: 500
            },
            items: [
                {
                    itemId: 'sfPlan',
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.PlanJurPersonGji',
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ],
                    fieldLabel: 'План'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'inspectors',
                    itemId: 'trigfInspectors',
                    fieldLabel: 'Инспекторы'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'zonalInspections',
                    itemId: 'trigfZonalInspections',
                    fieldLabel: 'Отделы'
                },
                {
                    xtype: 'container',
                    border: false,
                    width: 650,
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelWidth: 80,
                            fieldLabel: 'Период с',
                            width: 250,
                            itemId: 'dfDateStart'
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 80,
                            fieldLabel: 'по',
                            width: 250,
                            itemId: 'dfDateEnd'
                        },
                        {
                            width: 90,
                            margin: '0 0 0 10',
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