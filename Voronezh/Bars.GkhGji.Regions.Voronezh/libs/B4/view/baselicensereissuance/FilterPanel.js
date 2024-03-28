Ext.define('B4.view.baselicensereissuance.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.baselicensereissuancefilterpanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.view.contragent.Grid',
        'B4.store.Contragent',
        'B4.store.manorglicense.ListManOrgWithLicense'
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
                    width: 650,
                    padding: '5 0 0 0',
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            labelAlign: 'right',
                            labelWidth: 150,
                            name: 'Contragent',
                            textProperty: 'ShortName',
                            fieldLabel: 'Лицензиат',
                            editable: false,
                            store: 'B4.store.manorglicense.ListManOrgWithLicense',
                            columns: [
                                { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
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
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            width: 10,
                            xtype: 'component'
                        },
                        {
                            width: 100,
                            buttonType: 'updateGrid',
                            xtype: 'b4updatebutton'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});