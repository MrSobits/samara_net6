Ext.define('B4.view.indicator.AnalysisOfIndicators', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.form.SelectField',
        'B4.form.MonthPicker'
    ],

    title: 'Анализ индикаторов',

    alias: 'widget.indicatoranalysisofinicators',
    closable: true,

    layout: 'anchor',
    form: true,
    
    padding: '25px 0 0 40px',
    initComponent: function () {
        var me = this,
            menu = new Ext.menu.Menu(),
            array = new Array(
                { text: 'Excel', iconCls: 'icon-page-white-excel', formatId: 12 },
                { text: 'Excel 2007', iconCls: 'icon-page-white-excel', formatId: 14 }
                //{ text: 'Adobe Acrobat', iconCls: 'icon-page-white-acrobat', formatId: 1 }
            );

        Ext.each(array, function (item) {
            menu.add(item);
        });

        var date = Ext.Date.format(new Date(new Date().setDate(1)), 'F, Y');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'component',
                    margin: '0 0 40px 0',
                    html: '<h1 style="color:#65A2DB; font-size: 26px; ">Параметры отчета</h1>'
                },
                {
                    xtype: 'b4selectfield',
                    margin: 10,
                    name: 'RealEstateType',
                    fieldLabel: 'Тип дома',
                    store: 'B4.store.gisrealestate.realestatetype.RealEstateType',
                    idProperty: 'Id',
                    textProperty: 'Name',
                    labelWidth: 200,
                    allowBlank: false,
                    editable: false,
                    isGetOnlyIdProperty: true,
                    columns: [
                        {
                            text: 'Тип',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    labelAlign: 'right',
                    modalWindow: true,
                    width: 550
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    margin: 10,
                    fieldLabel: 'Муниципальное образование',
                    store: 'B4.store.dict.Municipality',
                    idProperty: 'Id',
                    textProperty: 'Name',
                    labelWidth: 200,
                    allowBlank: false,
                    editable: false,
                    isGetOnlyIdProperty: true,
                    selectionMode: 'MULTI',
                    columns: [
                        {
                            text: 'Тип',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    labelAlign: 'right',
                    modalWindow: true,
                    width: 550
                },
                {
                    xtype: 'b4selectfield',
                    margin: 10,
                    name: 'RealEstateTypeIndicator',
                    fieldLabel: 'Индикаторы',
                    store: 'B4.store.gisrealestate.realestatetype.RealEstateTypeIndicator',
                    idProperty: 'Id',
                    textProperty: 'RealEstateIndicatorName',
                    allowBlank: false,
                    editable: false,
                    labelWidth: 200,
                    isGetOnlyIdProperty: true,
                    selectionMode: 'MULTI',
                    columns: [
                        {
                            text: 'Тип',
                            dataIndex: 'RealEstateIndicatorName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    labelAlign: 'right',
                    modalWindow: true,
                    width: 550,
                    disabled: true
                },
                {
                    xtype: 'container',
                    width: 550,
                    margin: 10,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4monthpicker',
                            name: 'DateFrom',
                            flex: 1,
                            fieldLabel: 'Период формирования отчета с',
                            labelWidth: 200,
                            labelAlign: 'right',
                            value: date,
                            format: 'F, Y',
                            allowBlank: false,
                            validator: function (value) {
                                if (!value) return 'Выберите начало периода!';
                                var currentFieldDate = Ext.Date.parse(value, "F, Y");
                                currentFieldDate.setDate(1);
                                var anotherDateField = this.up('container').down('b4monthpicker[name=DateTo]');
                                if (anotherDateField.getValue().setDate(1) >= currentFieldDate) {
                                    return true;
                                }
                                return 'Дата начала периода должна быть не позднее даты окончания!';
                            }
                        },
                        {
                            xtype: 'b4monthpicker',
                            name: 'DateTo',
                            width: 200,
                            fieldLabel: 'по',
                            labelWidth: 40,
                            labelAlign: 'right',
                            value: date,
                            format: 'F, Y',
                            allowBlank: false,
                            validator: function (value) {
                                if (!value) return 'Выберите конец периода!';
                                var currentFieldDate = Ext.Date.parse(value, "F, Y");
                                currentFieldDate.setDate(1);
                                var anotherDateField = this.up('container').down('b4monthpicker[name=DateFrom]');
                                if (anotherDateField.getValue().setDate(1) <= currentFieldDate) {
                                    return true;
                                }
                                return 'Дата окончания периода должна быть не раньше даты начала!';
                            }
                        }
                    ]
                },
                {
                    xtype: 'button',
                    text: 'Печать отчет',
                    tooltip: 'Печать отчет',
                    iconCls: 'icon-printer',
                    margin: '5 5 5 20',
                    name: 'PrintReport',
                    menu: menu
                }
            ]
        });

        me.callParent(arguments);
    }
});