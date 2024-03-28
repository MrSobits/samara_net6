Ext.define('B4.view.manorglicense.GisExportWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 130,
    maxHeight: 130,
    bodyPadding: 5,
    alias: 'widget.gisexportdetailswindow',
    title: 'Дополнительная информация',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [


        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    width: 280,
                    allowBlank: false,
                    name: 'DateFrom',
                    fieldLabel: 'Дата c',
                    itemId: 'dfDateFrom'
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    width: 280,
                    allowBlank: false,
                    name: 'DateTo',
                    fieldLabel: 'Дата по',
                    itemId: 'dfDateTo'
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    itemId: 'tbSave'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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