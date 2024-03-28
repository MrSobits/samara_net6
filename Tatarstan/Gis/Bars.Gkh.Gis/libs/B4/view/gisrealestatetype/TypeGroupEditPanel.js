Ext.define('B4.view.gisrealestatetype.TypeGroupEditPanel', {
    extend: 'B4.form.Window',
    alias: 'widget.gisrealestatetypegroupedit',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],
    
    title: 'Добавление группы',
    modal: true,
    editPanel: undefined,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    width: 400,
                    margin: 5,
                    fieldLabel: 'Наименование',
                    allowBlank: false
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    name: 'SaveGroup',
                                    listeners: {
                                        click: function (button) {
                                            var win = button.up('window'),
                                                name = win.down('textfield[name=Name]').getValue();
                                            
                                            me.editPanel.fireEvent('onAddGroup', me.editPanel, me, name);
                                        }
                                    }
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function(button) {
                                            button.up('window').close();
                                        }
                                    }
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