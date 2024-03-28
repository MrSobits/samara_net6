Ext.define('B4.view.copycr.EditPanel', {
    extend: 'Ext.form.Panel',
    trackResetOnLoad: true,
    autoScroll: true,
    border: false,
    height: 600,
    bodyStyle: Gkh.bodyStyle,
    itemId: 'copyCrEditPanel',
    layout: 'anchor',

    requires: [
        'B4.ux.button.Add',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    anchor: '100%',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    layout: {
                        type: 'hbox',
                        pack: 'start'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            editable: false,
                            name: 'ProgramCr',
                            itemId: 'sflProgramCr',
                            fieldLabel: 'Программа КР',
                           

                            store: 'B4.store.dict.ProgramCr',
                            flex: 1,
                            margins: '0 5 5 0'
                        },
                        {
                            xtype: 'button',
                            itemId: 'loadWorkCrButton',
                            text: 'Загрузить',
                            tooltip: 'Загрузить',
                            iconCls: 'icon-add',
                            width: 120,
                            margins: '0 5 0 0'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Перечень формируется из домов, включенных в выбранную программу, и у которых добавлена услуга «Капитальный ремонт»</span>'
                },
                {
                    xtype: 'b4grid',
                    title: 'Дома',
                    store: 'copycr.RealityObj',
                    itemId: 'realityObjCopyCrGrid',
                    columnLines: true,
                    sortableColumns: false,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'AddressName',
                            flex: 1,
                            text: 'Адрес'
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
                                    columns: 2,
                                    items: [
                                        {
                                            xtype: 'b4addbutton'
                                        },
                                        {
                                            xtype: 'button',
                                            itemId: 'btnCopyCrClear',
                                            text: 'Удалить все',
                                            tooltip: 'Удалить все',
                                            iconCls: 'icon-decline'
                                        }
                                    ]
                                }
                            ]
                        }
                    ],
                    anchor: '100% -70'
                }
            ]
        });

        me.callParent(arguments);
    }
});
