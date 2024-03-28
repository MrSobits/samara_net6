Ext.define('B4.view.prescription.ClosePanel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.prescrclosepanel',

    requires: [
        'B4.enums.YesNoNotSet',
        'B4.form.EnumCombo',
        'B4.ux.grid.Panel',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.model.PrescriptionCloseDoc'
    ],

    title: 'Закрытие предписания',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.prescription.CloseDoc');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.apply(me, {
            items: [
                {
                    xtype: 'container',
                    margin: '5 5 5 5',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                align: 'stretch',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    enumName: 'B4.enums.YesNoNotSet',
                                    fieldLabel: 'Предписание закрыто',
                                    includeEmpty: false,
                                    labelWidth: 150,
                                    labelAlign: 'right',
                                    name: 'Closed',
                                    listeners: {
                                        'change': function (combo, newVal) {
                                            var parent = combo.up('prescrclosepanel'),
                                                reasonCombo = parent.down('b4enumcombo[name=CloseReason]'),
                                                noteText = parent.down('textarea'),
                                                grid = parent.down('b4grid');

                                            reasonCombo.setDisabled(newVal != B4.enums.YesNoNotSet.Yes);
                                            noteText.setDisabled(newVal != B4.enums.YesNoNotSet.Yes);
                                            grid.setDisabled(newVal != B4.enums.YesNoNotSet.Yes);
                                        }
                                    }
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    disabled: true,
                                    labelAlign: 'right',
                                    enumName: 'B4.enums.PrescriptionCloseReason',
                                    margin: '5 5 10 30',
                                    name: 'CloseReason',
                                    width: 350,
                                    fieldLabel: 'Причина'
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            disabled: true,
                            labelAlign: 'right',
                            name: 'CloseNote',
                            labelWidth: 150,
                            fieldLabel: 'Примечание'
                        }
                    ]
                },
                {
                    xtype: 'b4grid',
                    disabled: true,
                    title: 'Документы',
                    flex: 1,
                    store: store,
                    columns: [
                        {
                            xtype: 'b4editcolumn'
                        },
                        {
                            text: 'Наименование документа',
                            dataIndex: 'Name',
                            flex: 1
                        },
                        {
                            xtype: 'datecolumn',
                            text: 'Дата',
                            dataIndex: 'Date',
                            flex: 1,
                            format: 'd.m.Y'
                        },
                        {
                            text: 'Файл',
                            dataIndex: 'File',
                            flex: 1,
                            renderer: function(v) {
                                if (v) {
                                    return '<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">' + v.FullName + '</a>';
                                }
                                return '';
                            }
                        },
                        {
                            xtype: 'b4deletecolumn'
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    },
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
                                            xtype: 'b4addbutton'
                                        },
                                        {
                                            xtype: 'b4updatebutton'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: store,
                            dock: 'bottom'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});