Ext.define('B4.view.documents.RealityObjEditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    bodyPadding: 5,
    itemId: 'documentsRealityObjEditPanel',
    title: 'Документы',
    autoScroll: true,
    requires: [
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.view.documents.RealityObjProtocolGrid',
        'B4.enums.YesNoNotSet',
        'B4.form.EnumCombo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
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
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    idProperty: 'helpButton',
                                    text: 'Справка',
                                    tooltip: 'Справка',
                                    iconCls: 'icon-help'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'copyDocsRealityObjButton',
                                    text: 'Добавить копированием',
                                    tooltip: 'Добавить копированием',
                                    iconCls: 'icon-folder-page'
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 70
                            },
                            title: 'Акт состояния общего имущества собственников помещений в многоквартирном доме',
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'FileActState',
                                    fieldLabel: 'Файл'
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'DescriptionActState',
                                    fieldLabel: 'Описание',
                                    maxLength: 500
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 70
                            },
                            title: 'Перечень работ по содержанию и ремонту общего имущества собственников помещений',
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'FileCatalogRepair',
                                    fieldLabel: 'Файл'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            itemId: 'fsReportPlanRepair',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 70
                            },
                            title: 'Отчет о выполнении годового плана мероприятий по содержанию и ремонту',
                            items: [
                                {
                                    xtype: 'b4filefield',
                                    name: 'FileReportPlanRepair',
                                    fieldLabel: 'Файл'
                                }
                            ]
                        },
                        {
                            xtype: 'component',
                            itemId: 'cmpGridNotation',
                            padding: '10 0 15 5',
                            html: '<div>Необходимо загрузить следующие документы: <br/>' +
                                'Протоколы общих собраний членов товарищества/кооператива, заседаний правления и  ревизионной комиссии, на которых рассматривались<br/>' +
                                'вопросы, связанные с содержанием и ремонтом общего имущества МКД и (или) организацией  предоставления коммунальных услуг<br/>' +
                                'за текущий год и за год, предшествующий текущему году</div>'
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'component',
                                    flex: 5,
                                    itemId: 'cmpGeneralMeetingNotation',
                                    padding: '10 0 15 5',
                                    html: '<div>Проводились ли общие собрания собственников помещений в многоквартирном доме с участием управляющей организации после 01.12.2014г.?'
                                },
                                {
                                    name: 'HasGeneralMeetingOfOwners',
                                    xtype: 'b4enumcombo',
                                    flex: 1,
                                    enumName: 'B4.enums.YesNoNotSet',
                                    editable: false,
                                    hideTrigger: false,
                                    margins: '0 5 5 0'
                                }
                            ]
                        },
                        {
                            xtype: 'realityobjprotocolgrid',
                            region: 'center'
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});








