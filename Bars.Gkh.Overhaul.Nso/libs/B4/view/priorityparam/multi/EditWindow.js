Ext.define('B4.view.priorityparam.multi.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.priorityparammultiwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    minHeight: 400,
    height: 500,
    width: 800,
    minWidth: 800,
    bodyPadding: 5,
    title: 'Параметр',
    itemId: 'priorityparammultiwindow',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypePresence',
        'B4.form.ComboBox',
        'B4.view.priorityparam.multi.SelectedGrid',
        'B4.view.priorityparam.multi.SelectGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'numberfield',
                    name: 'Point',
                    fieldLabel: 'Балл',
                    labelAlign: 'right',
                    hideTrigger: true,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    items: [
                        {
                            xtype: 'priorityparammultiselectgrid',
                            flex: 1,
                            hidden: false
                        },
                        {
                            xtype: 'priorityparammultiselectedgrid',
                            flex: 1,
                            hidden: false,
                            maxWidth: 240
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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