Ext.define('B4.view.paysize.RetWindow', {
    extend: 'Ext.window.Window',

    alias: 'widget.paysizeretwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.SelectField',
        'B4.view.paysize.RetGrid'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minWidth: 700,
    maxWidth: 700,
    minHeight: 275,
    maxHeight: 275,
    bodyPadding: 5,
    title: 'Исключения по типу домов',
    closeAction: 'destroy',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id'
                },
                {
                    xtype: 'paysizeretgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        { xtype: 'b4savebutton' },
                        {
                            xtype: 'b4updatebutton',
                            listeners: {
                                'click': function() {
                                    me.down('paysizeretgrid').getStore().load();
                                }
                            }
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'b4closebutton',
                            listeners: {
                                'click': function() {
                                    me.close();
                                }
                            }
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});