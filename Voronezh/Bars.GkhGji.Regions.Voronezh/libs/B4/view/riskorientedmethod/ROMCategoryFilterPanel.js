Ext.define('B4.view.riskorientedmethod.ROMCategoryFilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.romcategoryfilterpanel',
    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'rOMCategoryFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update',
        'B4.enums.YearEnums',
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
                        labelAlign: 'right'
                    },
                    items: [
                       {
                           xtype: 'combobox',
                           name: 'YearEnums',
                           labelWidth: 100,
                           fieldLabel: 'Год расчета',
                           width: 200,
                           displayField: 'Display',
                           itemId: 'dfYearEnums',
                           store: B4.enums.YearEnums.getStore(),
                           valueField: 'Value',
                           //value: '2018',
                           value: new Date().getFullYear()
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