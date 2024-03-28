Ext.define('B4.view.objectcr.DesignAssignmentEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.designassignmenteditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    modal: true,
    closable: true,
    width: 600,
    minWidth: 600,
    height: 240,
    minHeight: 240,
    title: 'Проектирование',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.store.objectcr.DesignAssignment',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.objectcr.DesignAssignmentGrid',
        'B4.enums.YesNo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    layout: { type: 'vbox', align: 'stretch' },
                    border: false,
                    frame: true,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Document',
                            fieldLabel: 'Документ',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'Date',
                            fieldLabel: 'Дата размещения в системе',
                            format: 'd.m.Y',
                            itemId: 'tfDocumentDateFrom'
                        },
                        {
                            xtype: 'b4filefield',
                            editable: false,
                            name: 'DocumentFile',
                            fieldLabel: 'Файл'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'TypeWorksCr',
                            fieldLabel: 'Виды работы',
                            itemId: 'daTypeWorkCr',
                            store: 'B4.store.objectcr.TypeWorkCr',
                            selectionMode: 'MULTI',
                            textProperty: 'WorkName',
                            columns: [
                                { text: 'Вид работы', dataIndex: 'WorkName', flex: 1 }
                            ],
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Выводить документ на портал',
                            name: 'UsedInExport',
                            store: B4.enums.YesNo.getStore(),
                            displayField: 'Display',
                            valueField: 'Value'
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
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});