Ext.define('B4.view.report.RealityObjectDatareportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.realityobjectdatareportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.RealityObject'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObject',
                    fieldLabel: 'Объект недвижимости',
                    textProperty: 'Address',
                    anchor: '100%',
                    store: 'B4.store.RealityObject',
                    editable: false,
                    columns: [
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    allowBlank: false
                }
            ]
        });

        me.callParent(arguments);
    }
});