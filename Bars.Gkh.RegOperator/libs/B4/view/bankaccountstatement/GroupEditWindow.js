Ext.define('B4.view.bankaccountstatement.GroupEditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    height: 400,
    bodyPadding: 5,
    alias: 'widget.bankaccountstatementgroupeditwindow',
    title: 'Детализация файла экспорта',

    requires: [
        'B4.ux.button.Close',
        'B4.view.bankaccountstatement.Grid'
    ],

    dockedItems: null,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'hidden',
                    readOnly: true,
                    name: 'Id'
                },
                {
                    xtype: 'bankaccstatementgrid',
                    closable: false,
                    title: null,
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
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