Ext.define('B4.view.gisGkh.DictWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.gisGkh.DictGridStore',
        'B4.view.gisGkh.DictGrid',
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    width: 1000,
    minHeight: 650,
    maxHeight: 650,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 10,
    itemId: 'gisGkhDictWindow',
    title: 'Справочник ГИС ЖКХ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            xtype: 'container',
            layout: 'vbox',
            defaults: {
                margin: '0',
                labelWidth: 120,
                labelAlign: 'right',
                readOnly: true,
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'GisGkhCode',
                    itemId: 'dfGisGkhCode',
                    fieldLabel: 'Код справочника ГИС ЖКХ',
                    allowBlank: true,
                    disabled: true,
                    maxLength: 40,
                    editable: false
                },
                {
                    xtype: 'textfield',
                    name: 'GisGkhName',
                    itemId: 'dfGisGkhName',
                    fieldLabel: 'Наименование справочника ГИС ЖКХ',
                    allowBlank: true,
                    disabled: true,
                    maxLength: 40,
                    editable: false,
                },
                {
                    xtype: 'textfield',
                    name: 'EntityName',
                    itemId: 'dfEntityName',
                    fieldLabel: 'Название локального справочника',
                    allowBlank: true,
                    disabled: true,
                    maxLength: 40,
                    editable: false,
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 180,
                        format: 'd.m.Y H:i',
                        allowBlank: true,
                        disabled: true,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'ModifiedDate',
                            fieldLabel: 'Дата изменения словаря в ГИС ЖКХ',
                            labelWidth: 220,
                            itemId: 'dfModifiedDate',
                        },
                        {
                            name: 'RefreshDate',
                            fieldLabel: 'Дата обновления словаря',
                            itemId: 'dfRefreshDate',
                        },
                        {
                            name: 'MatchDate',
                            fieldLabel: 'Дата сопоставления словаря',
                            itemId: 'dfMatchDate',
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'gisgkhdictitemgrid',
                            flex: 1
                        }
                    ]
                }
                //{
                //    xtype: 'panel',
                //    name: 'dictitempanel',
                //    title: 'Справочники',
                //    items: [
                //        {
                //            xtype: 'gisgkhdictitemgrid',
                //            flex: 1
                //        }
                //    ]
                //}
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