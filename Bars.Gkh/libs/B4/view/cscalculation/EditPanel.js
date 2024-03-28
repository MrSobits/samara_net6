Ext.define('B4.view.cscalculation.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.cscalculationeditpanel',

    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.view.cscalculation.FormulaPanel'
    ],

    closable: true,
    title: 'Формула (ред.)',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            tbar: [
                {
                    xtype: 'buttongroup',
                    items: [{ xtype: 'b4savebutton' }]
                }
            ],
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id'
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    allowBlank: false,
                    hidden: false,
                    fieldLabel: 'Наименование'
                },
                {
                    xtype: 'tabpanel',
                    layout: 'fit',
                    flex: 1,
                    items: [
                        { xtype: 'cscalculationformulapanel' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});