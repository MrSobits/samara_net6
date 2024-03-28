Ext.define('B4.view.documentsgjiregister.DecisionGrid', {
    extend: 'B4.view.documentsgjiregister.DisposalGrid',

    store: 'view.Decision',
    itemId: 'docsGjiRegisterDecisionGrid',

    initComponent: function () {
        var me = this;
        
        var currTypeBase = B4.enums.TypeBase.getItemsWithEmpty([null, '-']);
        var newTypeBase = [];

        Ext.iterate(currTypeBase, function (val, key) {
            if (key != 6)
                newTypeBase.push(val);
        });

        Ext.applyIf(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус',
                    width: 125,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_document_disp';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeBase',
                    width: 160,
                    text: 'Основание проверки',
                    renderer: function (val) {
                        if (val != 60)
                            return B4.enums.TypeBase.displayRenderer(val);
                        return '';
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: newTypeBase,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectorNames',
                    flex: 2,
                    text: 'Инспекторы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindCheck',
                    flex: 2,
                    text: 'Вид проверки',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        url: '/KindCheckGji/List',
                        editable: false,
                        storeAutoLoad: false,
                        emptyItem: { Name: '-' },
                        valueField: 'Name'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlType',
                    flex: 2,
                    text: 'Вид контроля',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Disposal/ListControlType'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeSurveyNames',
                    flex: 2,
                    text: 'Типы обследований',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityNames',
                    flex: 2,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging',
                        filterName: 'MunicipalityId'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonInspectionAddress',
                    flex: 2,
                    text: 'Адрес объекта проверки',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 2,
                    text: 'Юридическое лицо',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LicenseNumber',
                    flex: 2,
                    text: 'Номер лицензии',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjectCount',
                    width: 65,
                    text: 'Количество домов',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 65,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    width: 50,
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 70,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    flex: 1,
                    text: 'Начало обследования',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    flex: 1,
                    text: 'Окончание обследования',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErknmRegistrationNumber',
                    width: 160,
                    text: 'Учетный номер решения в ЕРКНМ',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsActCheckExist',
                    width: 65,
                    text: 'Выполнено',
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeAgreementProsecutor',
                    text: 'Согласовано с прокуратурой',
                    hidden: true,
                    flex: 1,
                    renderer: function(val) {
                        return B4.enums.TypeAgreementProsecutor.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'combobox',
                        store: B4.enums.TypeAgreementProsecutor.getItemsWithEmpty([null, '-']),
                        operand: CondExpr.operands.eq,
                        editable: false
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});