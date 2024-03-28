Ext.define('B4.view.realityobj.ProtocolNpaPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.protocolnpapanel',

    title: 'НПА',
    closable: false,
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.enums.NpaStatus',
        'B4.enums.CategoryInformationNpa',
        'B4.enums.ActionLevelNormativeAct',
    ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        var contragentStore = Ext.create('B4.store.Contragent', {
            proxy: {
                type: 'b4proxy',
                controllerName: 'Contragent',
                listAction: 'GetAllActiveContragent'
            }
        });

        Ext.applyIf(me, {
            defaults: {
                allowBlank: false,
                flex: 1,
                labelAlign: 'right',
                labelWidth: 150,
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'NpaName',
                    fieldLabel: 'Наименование',
                    padding: '10 0 0 0',
                    maxLength: 100
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'NpaDate',
                            fieldLabel: 'Дата принятия документа',
                        },
                        {
                            xtype: 'textfield',
                            name: 'NpaNumber',
                            fieldLabel: 'Номер',
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'TypeInformationNpa',
                    fieldLabel: 'Тип информации в НПА',
                    store: 'B4.store.dict.TypeInformationNpa',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Категория',
                            dataIndex: 'Category',
                            flex: 1,
                            renderer: function (val) { return B4.enums.CategoryInformationNpa.displayRenderer(val); },
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                items: B4.enums.CategoryInformationNpa.getItemsWithEmpty([null, '-']),
                                editable: false,
                                valueField: 'Value',
                                displayField: 'Display',
                            }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'TypeNpa',
                    fieldLabel: 'Тип НПА',
                    store: 'B4.store.dict.TypeNpa',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'TypeNormativeAct',
                    fieldLabel: 'Вид нормативного акта',
                    store: 'B4.store.dict.TypeNormativeAct',
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Действует на',
                            dataIndex: 'ActionLevel',
                            flex: 1,
                            renderer: function (val) { return B4.enums.ActionLevelNormativeAct.displayRenderer(val); },
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                items: B4.enums.ActionLevelNormativeAct.getItemsWithEmpty([null, '-']),
                                editable: false,
                                valueField: 'Value',
                                displayField: 'Display'
                            }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'NpaContragent',
                    fieldLabel: 'Орган, принявший НПА',
                    store: contragentStore,
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальный район',
                            dataIndex: 'Municipality',
                            flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'NpaFile',
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'NpaStatus',
                    fieldLabel: 'Статус',
                    enumName: 'B4.enums.NpaStatus',
                    listeners: {
                        change: {
                            fn: function(field, newValue, oldValue) {
                                var cancellationReason = field.up().down('[name=NpaCancellationReason]'),
                                    record = field.up('formwindow').getRecord(),
                                    isCancelled = field.getValue() === B4.enums.NpaStatus.Canceled;

                                cancellationReason.setDisabled(!isCancelled);
                                cancellationReason.allowBlank = !isCancelled;

                                if (!isCancelled) {
                                    cancellationReason.setValue(null);
                                    if (record) {
                                        record.set('NpaCancellationReason', null);
                                    }
                                }

                                cancellationReason.validate();
                            }
                        }
                    }
                },
                {
                    xtype: 'textfield',
                    name: 'NpaCancellationReason',
                    fieldLabel: 'Причина аннулирования',
                    maxLength: 100,
                    disabled: true,
                    allowBlank: true
                }
            ]
        });

        me.callParent(arguments);
    }
});
