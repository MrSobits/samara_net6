Ext.define('B4.view.generaldata.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Общие сведения',
    itemId: 'generalDataEditPanel',
    layout: 'fit',

    requires: [
        'B4.ux.button.Update',
        'B4.view.Control.GkhWorkModeGrid',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhIntField',
        'B4.form.FileField',
        'B4.view.manorg.DispatchPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Общие сведения',
                            itemId: 'tabGeneral',
                            autoScroll: true,
                            border: false,
                            bodyPadding: 2,
                            frame: true,
                            defaults: {
                                labelWidth: 200
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    padding: 2,
                                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На данной странице представлены данные только для чтения. Для редактирования данных необходимо перейти по кнопке "Контрагенты"/"Управляющая организация".</span>'
                                },
                                {
                                    xtype: 'container',
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'fieldset',
                                            defaults: {
                                                anchor: '100%',
                                                labelAlign: 'right',
                                                labelWidth: 200,
                                                readOnly: true
                                            },
                                            title: 'Общие сведения',
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'ContragentName',
                                                    itemId: 'tfContragentName',
                                                    fieldLabel: 'Наименование юр. лица'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'FioDirector',
                                                    itemId: 'tfFioDirector',
                                                    fieldLabel: 'ФИО руководителя'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'FiasJurAddressName',
                                                    itemId: 'tfFiasJurAddressName',
                                                    fieldLabel: 'Юридический адрес'
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    labelWidth: 350,
                                                    readOnly: false, // так нужн очтобы могли скачать файл
                                                    fieldLabel: 'Устав товарищества собственников жилья или кооператива',
                                                    name: 'DispatchFile',
                                                    itemId: 'tfDispatchFile',
                                                    maxFileSize: 15728640,
                                                    possibleFileExtensions: 'odt,ods,odp,doc,docx,xls,xlsx,ppt,tif,tiff,pptx,txt,dat,jpg,jpeg,png,pdf,gif,rtf',
                                                    onTrigger1Click: function() {},
                                                    onTrigger3Click: function() {}
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'fieldset',
                                            defaults: {
                                                anchor: '100%',
                                                labelWidth: 200
                                            },
                                            title: 'Сведения о регистрации',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: 'column',
                                                    items: [
                                                            {
                                                                labelWidth: 200,
                                                                columnWidth: 0.7,
                                                                labelAlign: 'right',
                                                                xtype: 'textfield',
                                                                name: 'Ogrn',
                                                                itemId: 'tfOgrn',
                                                                fieldLabel: 'ОГРН',
                                                                readOnly: true
                                                            },
                                                            {
                                                                labelWidth: 200,
                                                                columnWidth: 0.3,
                                                                labelAlign: 'right',
                                                                xtype: 'textfield',
                                                                name: 'ActivityDateStart',
                                                                itemId: 'tfActivityDateStart',
                                                                fieldLabel: 'Дата регистрации',
                                                                readOnly: true
                                                            }
                                                        ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelAlign: 'right',
                                                    name: 'OgrnRegistration',
                                                    itemId: 'tfOgrnRegistration',
                                                    fieldLabel: 'Орган, принявший решение о регистрации',
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'fieldset',
                                            defaults: {
                                                anchor: '100%',
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                readOnly: true
                                            },
                                            title: 'Контактная информация',
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'FiasMailAddressName',
                                                    itemId: 'tfFiasMailAddressName',
                                                    fieldLabel: 'Почтовый адрес'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'FiasFactAddressName',
                                                    itemId: 'tfFiasFactAddressName',
                                                    fieldLabel: 'Фактический адрес'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Phone',
                                                    itemId: 'tfPhone',
                                                    fieldLabel: 'Телефон'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Fax',
                                                    itemId: 'tFax',
                                                    fieldLabel: 'Факс'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Email',
                                                    itemId: 'tfEmail',
                                                    fieldLabel: 'Электронный адрес'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OfficialWebsite',
                                                    itemId: 'tfOfficialWebsite',
                                                    fieldLabel: 'Официальный сайт'
                                                },
                                                {
                                                    xtype: 'textarea',
                                                    name: 'FioMemberAudit',
                                                    itemId: 'taFioMemberAudit',
                                                    fieldLabel: 'Члены ревизионной комиссии'
                                                },
                                                {
                                                    xtype: 'textarea',
                                                    name: 'FioMemberManagement',
                                                    itemId: 'taFioMemberManagement',
                                                    fieldLabel: 'Члены правления'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            title: 'Режимы работы',
                            autoScroll: true,
                            border: false,
                            frame: true,
                            items: [
                                {
                                    xtype: 'container',
                                    padding: 2,
                                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На данной странице представлены данные только для чтения. Для редактирования данных необходимо перейти по кнопке "Контрагенты"/"Управляющая организация".</span>'
                                },
                                {
                                    xtype: 'container',
                                    region: 'north',
                                    items: [
                                        {
                                            xtype: 'gkhworkmodegrid',
                                            title: 'Режим работы',
                                            store: 'generaldata.ManOrgWorkMode',
                                            itemId: 'workModeGridDi',
                                            plugins: null,
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
                                                                    xtype: 'b4updatebutton'
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'gkhworkmodegrid',
                                            title: 'Прием граждан',
                                            store: 'generaldata.ManOrgReceptionCitizens',
                                            itemId: 'receptionCitizensGridDi',
                                            plugins: null,
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
                                                                    xtype: 'b4updatebutton'
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'gkhworkmodegrid',
                                            title: 'Работа диспетчерских служб',
                                            store: 'generaldata.ManOrgDispatcherWork',
                                            itemId: 'dispatcherWorkGridDi',
                                            plugins: null,
                                            minHeight: 350,
                                            flex: 1,
                                            dockedItems: [

                                                {
                                                    xtype: 'toolbar',
                                                    dock: 'top',
                                                    items: [
                                                        {
                                                            xtype: 'container',
                                                            layout: 'vbox',
                                                            items: [

                                                                {
                                                                    xtype: 'buttongroup',
                                                                    items: [
                                                                        {
                                                                            xtype: 'b4updatebutton'
                                                                        }
                                                                    ]
                                                                },
                                                                {
                                                                    xtype: 'manorgDispatchPanel',
                                                                    isReadOnly: true,
                                                                    flex: 1
                                                                }
                                                            ]
                                                        }
                                                    ]

                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                }
                            },
                            title: 'Для рейтинга управляющих организаций',
                            border: false,
                            bodyPadding: 2,
                            frame: true,
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'buttongroup',
                                            columns: 1,
                                            items: [
                                                {
                                                    xtype: 'button',
                                                    itemId: 'saveButton',
                                                    text: 'Сохранить',
                                                    tooltip: 'Сохранить',
                                                    iconCls: 'icon-accept'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            items: [
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 200

                                    },
                                    title: 'Численность сотрудников на дату',
                                    items: [
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'NumberEmployees',
                                            fieldLabel: 'Всего',
                                            disabled: true
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'AdminPersonnel',
                                            fieldLabel: 'Административный персонал'
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'Engineer',
                                            fieldLabel: 'Инженеры'
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'Work',
                                            fieldLabel: 'Рабочие'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 200
                                    },
                                    title: 'Уволено за отчетный период',
                                    items: [
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'DismissedAdminPersonnel',
                                            fieldLabel: 'Административный персонал'
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'DismissedEngineer',
                                            fieldLabel: 'Инженеры'
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'DismissedWork',
                                            fieldLabel: 'Рабочие'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 200
                                    },
                                    title: 'Число несчастных случаев',
                                    items: [
                                        {
                                            xtype: 'gkhintfield',
                                            
                                            name: 'UnhappyEventCount',
                                            fieldLabel: 'Число несчастных случаев'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    itemId: 'shareSfMo',
                                    layout: 'column',
                                    items: [
                                            {
                                                name: 'ShareSf',
                                                xtype: 'textfield',
                                                itemId: 'shareSf',
                                                labelWidth: 211,
                                                columnWidth: 0.5,
                                                labelAlign: 'right',
                                                fieldLabel: 'Доля участия СФ (%)',
                                                readOnly: true,
                                                qtipText: 'Данные из [Управляющая организация] - [Общие сведения] - [Доля участия СФ (%)]',
                                                renderer: function (val) {
                                                    if (val) {
                                                        return Ext.util.Format.round(val, 2);
                                                    } else {
                                                        return val;
                                                    }
                                                }
                                            },
                                            {
                                                name: 'ShareMo',
                                                xtype: 'textfield',
                                                itemId: 'shareMo',
                                                labelWidth: 200,
                                                columnWidth: 0.5,
                                                labelAlign: 'right',
                                                fieldLabel: 'Доля участия МО (%)',
                                                qtipText: 'Данные из [Управляющая организация] - [Общие сведения] - [Доля участия МО (%)]',
                                                readOnly: true,
                                                renderer: function (val) {
                                                    if (val) {
                                                        return Ext.util.Format.round(val, 2);
                                                    } else {
                                                        return val;
                                                    }
                                                }
                                            }
                                    ]
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
                            columns: 3,
                            items: [
                            //переопределяем что бы не пуиались экшены с кнопкой обновить гридов режима работы
                                {
                                    xtype: 'button',
                                    itemId: 'updateButton',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить'
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
                                    xtype: 'button',
                                    itemId: 'contragentButton',
                                    text: 'Контрагент',
                                    tooltip: 'Контрагент',
                                    iconCls: 'icon-pencil-go'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'managingOrgButton',
                                    text: 'Управляющая организация',
                                    tooltip: 'Управляющая организация',
                                    iconCls: 'icon-pencil-go'
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
