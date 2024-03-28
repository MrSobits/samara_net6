Ext.define('B4.view.version.CopyWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.versioncopywindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    bodyPadding: 5,
    minWidth: 500,
    title: 'Копирование версии программы',
    closable: false,
    requires: [
        'B4.form.SelectField',
        
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    bodyPadding: 5,
                    title: 'Текущая версия',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 90
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            name: 'VersionDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    bodyPadding: 5,
                    title: 'Новая версия',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 90
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'NewName',
                            fieldLabel: 'Наименование',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'NewDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            value : new Date(),
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
                                    text: 'Копировать',
                                    tooltip: 'Копировать',
                                    action: 'copy',
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