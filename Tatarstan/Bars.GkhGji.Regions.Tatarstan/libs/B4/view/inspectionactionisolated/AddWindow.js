Ext.define('B4.view.inspectionactionisolated.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    alias: 'widget.inspectionactionisolatedaddwindow',
    itemId: 'inspectionactionisolatedaddwindow',
    title: 'Проверка по мероприятию без взаимодействия с контролируемым лицом',
    trackResetOnLoad: true,
    closeAction: 'hide',

    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeObjectAction',
        'B4.enums.TypeJurPerson',
        'B4.enums.TatarstanInspectionFormType',
        'B4.ux.grid.column.Enum'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    idProperty: 'InspectionId',
                    windowCfg: {
                        width: 1000
                    },
                    store: Ext.create('B4.base.Store', {
                        autoLoad: false,
                        fields: [
                            { name: 'InspectionId' },
                            { name: 'ActActionIsolatedNumber'},
                            { name: 'TaskActionIsolatedDocumentDate'},
                            { name: 'ActActionIsolatedDocumentDate'},
                            { name: 'Contragent'},
                            { name: 'PersonName'},
                            { name: 'TypeJurPerson'},
                            { name: 'TypeObject'}
                        ],
                        proxy: {
                            type: 'b4proxy',
                            controllerName: 'InspectionActionIsolated',
                            listAction: 'ListTaskActionIsolated'
                        },
                    }),
                    name: 'ActionIsolated',
                    fieldLabel: 'КНМ без взаимодействия',
                    textProperty: 'ActActionIsolatedNumber',
                    editable: false,
                    columns: [
                        {
                            text: 'Номер акта по КНМ', 
                            dataIndex: 'ActActionIsolatedNumber', 
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            text: 'Дата КНМ', 
                            dataIndex: 'TaskActionIsolatedDocumentDate', 
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            text: 'Дата акта КНМ', 
                            dataIndex: 'ActActionIsolatedDocumentDate', 
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'TypeObject',
                            text: 'Объект проверки',
                            flex: 1,
                            enumName: 'B4.enums.TypeObjectAction',
                            filter: true
                        },
                        {
                            xtype: 'b4enumcolumn',
                            dataIndex: 'TypeJurPerson',
                            text: 'Тип контрагента',
                            flex: 1,
                            enumName: 'B4.enums.TypeJurPerson',
                            filter: true
                        },
                        {
                            text: 'Управляющая организация', 
                            dataIndex: 'Contragent', 
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'ФИО', 
                            dataIndex: 'PersonName', 
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'b4enumcombo',
                    enumName: 'B4.enums.TatarstanInspectionFormType',
                    name: 'TypeForm',
                    fieldLabel: 'Форма проверки',
                    includeNull: false
                }
            ],
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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