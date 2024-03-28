Ext.define('B4.view.manorg.WorkServiceEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.manorgworkserviceeditwindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.view.manorg.ManOrgBilMkdWorkGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minHeight: 400,
    height: 400,
    bodyPadding: 5,
    trackResetOnLoad: true,
    title: 'Работа/услуга организации',
    closeAction: 'hide',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    frame: true,
                    padding: 0,
                    items: [
                        {
                            xtype: 'container',
                            title: 'Основные параметры',
                            flex: 1,
                            padding: 5,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'BilService',
                                    fieldLabel: 'Наименование',
                                    store: 'B4.store.dict.service.BilServiceDictionaryWork',
                                    editable: true,
                                    allowBlank: false,
                                    textProperty: 'ServiceName',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ServiceName', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Единица измерения', dataIndex: 'MeasureName', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'Код', dataIndex: 'ServiceCode', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'MeasureName',
                                    fieldLabel: 'Единица измерения',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ServiceCode',
                                    fieldLabel: 'Код',
                                    readOnly: true
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'Purpose',
                                    width: 150,
                                    allowBlank: false,
                                    fieldLabel: 'Назначение работ',
                                    enumName: 'B4.enums.ServiceWorkPurpose'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'Type',
                                    width: 150,
                                    allowBlank: false,
                                    fieldLabel: 'Тип работ',
                                    enumName: 'B4.enums.ServiceWorkType'
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    flex: 1,
                                    maxLength: 2000
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            title: 'Работы по содержанию МКД',
                            flex: 1,
                            layout:
                            {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'manorgbilmkdworkgrid',
                                    flex: 1,
                                    title: ''
                                }
                            ]
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
                                    xtype: 'b4savebutton'
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