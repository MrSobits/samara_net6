Ext.define('B4.view.appealcits.DetailsEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    maximized: false,
    maximizable: true,
    closeAction: 'destroy',
    width: 800,
    height: 600,
    modal: true,
    bodyPadding: 5,
    itemId: 'appealCitsDetailsEditWindow',
    title: 'Описание',
    trackResetOnLoad: true,

    requires: [
       
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                             
                {
                    xtype: 'htmleditor',
                    name: 'Description',
                    itemId: 'dfDescription',
                    flex: 1
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
                            { xtype: 'tbfill' },
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