Ext.define('B4.view.dict.stateduty.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.statedutyeditpanel',

    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.view.dict.stateduty.PetitionGrid',
        'B4.view.dict.stateduty.FormulaPanel'
    ],

    closable: true,
    title: 'Госпошлина (ред.)',

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
                    margins: '5 5 0 0',
                    xtype: 'b4enumcombo',
                    enumName: 'B4.enums.CourtType',
                    name: 'CourtType',
                    fieldLabel: 'Тип суда',
                    labelAlign: 'right'
                },
                {
                    xtype: 'tabpanel',
                    layout: 'fit',
                    flex: 1,
                    items: [
                        { xtype: 'statedutypetitiongrid' },
                        { xtype: 'statedutyformulapanel' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});