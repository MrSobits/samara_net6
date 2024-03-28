Ext.define('B4.view.cscalculation.FormulaPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.cscalculationformulapanel',

    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete'
    ],

    title: 'Формула',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [                
                {
                    xtype: 'textarea',
                    name: 'Formula',
                    fieldLabel: 'Формула расчета платы ЖКУ',
                    labelAlign: 'right'
                },
                {
                    xtype: 'fieldcontainer',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'displayfield',
                            name: 'FormulaMsg',
                            style: 'text-align: right;',
                            flex: 1
                        },
                        {
                            xtype: 'splitter'
                        },
                        {
                            xtype: 'button',
                            text: 'Проверить формулу',
                            action: 'checkformula'
                        }
                    ]
                },
                {
                    xtype: 'b4grid',
                    flex: 1,
                    store: Ext.create('B4.store.FormulaParameter'),
                    columns: [
                        { xtype: 'b4editcolumn' },
                        { header: 'Наименование', dataIndex: 'Name', flex: 1 },
                        { header: 'Характеристика', dataIndex: 'DisplayName', flex: 1 },
                        { xtype: 'b4deletecolumn' }
                    ],
                    dockedItems: [
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        {
                                            xtype: 'b4addbutton',
                                            handler: function(btn) {
                                                btn.up('b4grid').fireEvent('gridaction', btn.up('b4grid'), btn.actionName);
                                            }
                                        }
                                    ]
                                }
                            ]
                        }                         
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});