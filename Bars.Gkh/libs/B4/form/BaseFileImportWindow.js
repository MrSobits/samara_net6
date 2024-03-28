Ext.define('B4.form.BaseFileImportWindow', {
    extend: 'B4.form.Window',
    
    mixins: ['B4.mixins.window.ModalMask'],

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    layout: { type: 'vbox', align: 'stretch' },
    width: 400,
    bodyPadding: 5,
    
    title: 'Импорт',
    resizable: false,

    supportMultipleImport: false,
    getItems: Ext.emptyFn,
    getToolbarItems: Ext.emptyFn,

    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            items: me.getItems() || []
        });
        
        var toolbarItems = me.getToolbarItems();
        if (!this.isEmpty(toolbarItems)) {
            Ext.apply(me, {
                dockedItems: [
                    {
                        dock: 'top',
                        xtype: 'toolbar',
                        items: toolbarItems
                    }
                ]
            });
        }
        else {
            Ext.apply(me, {
                dockedItems: [
                    {
                        dock: 'top',
                        xtype: 'toolbar',
                        items: me.getDefaultToolbarItems()
                    }
                ]
            })
        }

        me.callParent(arguments);
    },
    
    getDefaultToolbarItems: () => [
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
    ],
    
    isEmpty: function (item) {
        return Ext.isObject(item)
            ? !Ext.isObject(item) || Object.keys(obj).length === 0
            : Ext.isEmpty(item);
    },
});
