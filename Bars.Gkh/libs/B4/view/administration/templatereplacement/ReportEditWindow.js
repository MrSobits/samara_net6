Ext.define('B4.view.administration.templatereplacement.ReportEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.administration.templatereplacement.Grid'
    ],

    mixins: [
        'B4.mixins.window.ModalMask'
    ],

    layout: 'fit',
    width: 700,
    height: 500,
    maximizable: true,
    resizable: true,
    bodyPadding: 5,
    itemId: 'templateReplacementReportEditWindow',
    title: 'Шаблон документа',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'panel',
                            layout: 'form',
                            border: false,
                            height: 60,
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                labelAlign: 'right',
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfName',
                                    name: 'Name',
                                    fieldLabel: 'Наименование'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDescription',
                                    name: 'Description',
                                    fieldLabel: 'Описание'
                                }
                            ]
                        },
                        {
                            xtype: 'templatereplacementgrid',
                            flex: 1
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