Ext.define('B4.view.inspectionactionisolated.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.inspectionactionisolatedfilterpanel',
    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update',
        'B4.form.SelectField',
        'B4.store.RealityObject',
        'B4.form.ComboBox'
    ],

    initComponent: function () {
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
                    width: 650,
                    layout: {
                        pack: 'start',
                        type: 'vbox'
                    },
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'left',
                        labelWidth: 110,
                        width: 500,
                        editable: false
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObject',
                            store: 'B4.store.RealityObject',
                            textProperty: 'Address',
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
                            fieldLabel: 'Жилой дом'
                        },
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    width: 650,
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    items: [
                        {
                            labelAlign: 'left',
                            labelWidth: 110,
                            fieldLabel: 'Период с',
                            width: 290,
                            name: 'DateStart'
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            fieldLabel: 'по',
                            width: 210,
                            labelAlign: 'right',
                            name: 'DateEnd'
                        },
                        {
                            width: 10,
                            xtype: 'component'
                        },
                        {
                            xtype: 'b4updatebutton',
                            action: 'updateGrid',
                            width: 100
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});