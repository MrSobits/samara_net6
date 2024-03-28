Ext.define('B4.view.claimwork.buildcontract.BuilderViolatorAddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.builderviolatoraddwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    title: 'Добавление',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        
        'B4.store.objectcr.BuildContract',
        'B4.store.dict.ProgramCr',
        
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    fieldLabel: 'Программа КР',
                    flex: 1,
                    textProperty: 'Name',
                    anchor: '100%',
                    store: 'B4.store.dict.ProgramCr',
                    columns: [
                        {
                            text: 'Программа КР', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }
                        }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'BuildContract',
                    fieldLabel: 'Договор подряда',
                    textProperty: 'DocumentNum',
                    anchor: '100%',
                    windowCfg: {
                        width: 850
                    },
                    store: 'B4.store.objectcr.BuildContract',
                    editable: false,
                    columns: [
                        {
                            text: 'Подрядчик',
                            dataIndex: 'BuilderName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'ИНН',
                            dataIndex: 'BuilderInn',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: '№ договора',
                            dataIndex: 'DocumentNum',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'DocumentDateFrom',
                            format: 'd.m.Y',
                            flex: 1,
                            text: 'Дата договора',
                            filter: {
                                xtype: 'datefield',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RoMunicipality',
                            flex: 1.6,
                            text: 'Муниципальный район',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RoSettlement',
                            flex: 1.6,
                            itemId: 'SettlementColumn',
                            text: 'Муниципальное образование',
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Адрес объекта КР',
                            dataIndex: 'RoAddress',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    allowBlank: false
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