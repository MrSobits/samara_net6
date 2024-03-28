Ext.define('B4.view.resolpros.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.resolProsFilterPanel',
    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'resolProsFilterPanel',
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
                    xtype: 'container',
                    padding: '0 0 5 0',
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
                            labelWidth: 130,
                            fieldLabel: 'Период с',
                            width: 290,
                            itemId: 'dfDateStart',
                            value: new Date(new Date().getFullYear(), 0, 1)
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            fieldLabel: 'по',
                            width: 210,
                            itemId: 'dfDateEnd',
                            value: new Date()
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    width: 650,
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    defaults: {
                        anchor: '100%',
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            itemId: 'sfRealityObject',
                            xtype: 'b4selectfield',
                            store: 'B4.store.RealityObject',
                            textProperty: 'Address',
                            width: 500,
                            editable: false,
                            columns: [
                                {
                                    text: 'Муниципальное образование',
                                    dataIndex: 'Municipality',
                                    flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListWithoutPaging'
                                    }
                                },
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            fieldLabel: 'Жилой дом',
                            labelWidth: 130
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