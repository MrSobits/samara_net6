Ext.define('B4.view.structelements.ViewPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'border',
    bodyPadding: 5,
    itemId: 'disinfostructelementspanel',
    title: 'Конструктивные элементы',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.grid.Panel',
        'B4.form.ComboBox'
    ],
    alias: 'widget.disinfostructelementspanel',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    region: 'north',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На данной странице представлены данные только для чтения. Для редактирования данных необходимо перейти в Реестр жилых домов/паспорт дома</span>'
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Описание - Конструктивные элементы',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'BasementType',
                                    fieldLabel: 'Тип фундамента',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [5.1 Фундаменты]'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'BasementArea',
                                    fieldLabel: 'Площадь подвала по полу (кв. м)',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 540,
                                    readOnly: true,
                                    qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [5.4 Полы] - [Площадь подвала по полу]'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            region: 'north',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 320
                                    },
                                    title: 'Группа "Несущие конструкции"',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'TypeFloor',
                                            fieldLabel: 'Тип перекрытий',
                                            hideTrigger: true,
                                            flex: 0.8,
                                            maxWidth: 540,
                                            readOnly: true,
                                            qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [5.3 Перекрытия] - [Тип перекрытий]'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TypeWalls',
                                            fieldLabel: 'Материал несущих стен',
                                            hideTrigger: true,
                                            flex: 0.8,
                                            maxWidth: 540,
                                            readOnly: true,
                                            qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [5.2 Стены и перегородки] - [Тип стен]'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            region: 'north',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 320
                                    },
                                    title: 'Группа "Мусоропроводы"',
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ConstructionChute',
                                            fieldLabel: 'Тип мусоропровода',
                                            hideTrigger: true,
                                            flex: 0.8,
                                            maxWidth: 540,
                                            readOnly: true,
                                            qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.9 Мусоропроводы] - [Конструкция мусоропровода]'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'ChutesNumber',
                                            fieldLabel: 'Количество мусоропроводов (ед.)',
                                            hideTrigger: true,
                                            allowDecimals: false,
                                            flex: 0.8,
                                            maxWidth: 540,
                                            readOnly: true,
                                            qtipText: 'Данные из [Жилой дом] - [Паспорт тех. объекта] - [3.9 Мусоропроводы] - [Количество мусоропроводов]'
                                        }

                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            region: 'north',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 320
                                    },
                                    title: 'Фасады (заполняется по каждому типу фасада)',
                                    items: [
                                        {
                                            xtype: 'grid',
                                            store:
                                                {
                                                    fields: [
                                                        'FacadeType'
                                                    ]
                                                },
                                            border: false,
                                            columnLines: true,
                                            itemId: 'disinfofacadegrid',
                                            columns: [
                                                {
                                                    xtype: 'gridcolumn',
                                                    text: 'Тип фасада',
                                                    dataIndex: 'FacadeType',
                                                    flex: 1
                                                }
                                            ],
                                            viewConfig: {
                                                loadMask: true
                                            }
                                        }

                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            region: 'north',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        anchor: '100%',
                                        labelAlign: 'right',
                                        labelWidth: 320
                                    },
                                    title: 'Крыши (заполняется по каждому типу крыши)',
                                    items: [
                                        {
                                            xtype: 'grid',
                                            store:
                                                {
                                                    fields: [
                                                        'RoofType',
                                                        'RoofingType'
                                                    ]
                                                },
                                            border: false,
                                            columnLines: true,
                                            itemId: 'disinforoofinggrid',
                                            columns: [
                                                {
                                                    xtype: 'gridcolumn',
                                                    text: 'Тип крыши',
                                                    dataIndex: 'RoofType',
                                                    flex: 1,
                                                    width: 300
                                                },
                                                {
                                                    xtype: 'gridcolumn',
                                                    text: 'Тип кровли',
                                                    dataIndex: 'RoofingType',
                                                    flex: 1
                                                }
                                            ],
                                            viewConfig: {
                                                loadMask: true
                                            }
                                        }
                                    ]
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