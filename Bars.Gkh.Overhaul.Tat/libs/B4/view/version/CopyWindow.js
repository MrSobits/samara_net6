Ext.define('B4.view.version.CopyWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    width: 800,
    height: 200,
    maxWidth: 900,
    minWidth: 500,
    minHeight: 200,
    autoScroll: true,
    bodyPadding: 5,
    alias: 'widget.versioncopywindow',
    title: 'Копирование версии программы',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    title: 'Текущая версия',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 90,
                        margin: 10
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            maxLength: 300,
                            allowBlank: false,
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            name: 'VersionDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            allowBlank: false,
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    title: 'Новая версия',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 90,
                        margin: 10
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            itemId: 'tfName',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            itemId: 'dfVersionDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            allowBlank: false,
                            readOnly: true
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'copyProgramButton',
                                    text: 'Копировать',
                                    tooltip: 'Копировать',
                                    iconCls: 'icon-accept'
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