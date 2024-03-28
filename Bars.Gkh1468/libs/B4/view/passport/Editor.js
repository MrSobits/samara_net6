Ext.define('B4.view.passport.Editor', {
    extend: 'Ext.Container',

    alias: 'widget.passpeditor',

    requires: [
        'B4.view.realityobj.Grid',
        'B4.store.RealityObject',
        'Ext.form.FieldContainer',
        'B4.form.SelectField',
        'B4.view.contragent.Grid',
        'B4.store.Contragent',
        'B4.view.dict.municipality.Grid',
        'B4.store.dict.Municipality'
    ],

    title: 'Паспорт по ПП РФ 1468',
    closable: true,

    layout: {
        type: 'border',
        padding: 5
    },
    
    initComponent: function () {
        var me = this;

        var store = Ext.create('Ext.data.TreeStore', {
            defaultRootProperty: 'Childrens',

            fields: [
                { name: 'Id' },
                { name: 'Name' },
                { name: 'Attribute' },
                { name: 'Part' },
                { name: 'Value' },
                { name: 'ValueId' },
                { name: 'Code' }
            ],
            root: {
                expanded: true
            }
        });

        Ext.apply(me, {
            defaults: {
                split: true
            },
            items: [
                {
                    region: 'north',
                    xtype: 'form',
                    bodyPadding: 5,
                    defaults: {
                        labelWidth: 170
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Municipality',
                            anchor: '100%',
                            readOnly: true,
                            maxWidth: 700,
                            fieldLabel: 'Муниципальное образование',
                           

                            store: 'B4.store.dict.Municipality',
                            editable: false,
                            columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'RealityObject',
                            anchor: '100%',
                            readOnly: true,
                            maxWidth: 700,
                            fieldLabel: 'Жилой дом',
                           

                            store: 'B4.store.RealityObject',
                            editable: false,
                            textProperty: 'Address',
                            columns: [
                                { text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1 },
                                { text: 'Адрес', dataIndex: 'Address', flex: 1 }]
                        },
                        {
                            xtype: 'fieldcontainer',
                            layout: 'hbox',
                            fieldLabel: 'Отчетный год',
                            items: [{
                                xtype: 'numberfield',
                                name: 'ReportYear',
                                readOnly: true,
                                minValue: 1900,
                                maxValue: 2100,
                                value: 2013
                            },
                            {
                                labelwidth: 170,
                                labelAlign: 'right',
                                name: 'ReportMonth',
                                fieldLabel: 'Отчетный месяц',
                                xtype: 'combo',
                                readOnly: true,
                                store: [
                                    [1, 'Январь'],
                                    [2, 'Февраль'],
                                    [3, 'Март'],
                                    [4, 'Апрель'],
                                    [5, 'Май'],
                                    [6, 'Июнь'],
                                    [7, 'Июль'],
                                    [8, 'Август'],
                                    [9, 'Сентябрь'],
                                    [10, 'Октябрь'],
                                    [11, 'Ноябрь'],
                                    [12, 'Декабрь']
                                ]
                            }]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            fieldLabel: 'Поставщик информации',
                            store: 'B4.store.Contragent',
                            editable: false,
                            anchor: '100%',
                            readOnly: true,
                            maxWidth: 700,
                            columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1 }]
                        }
                    ]
                },
                {
                    region: 'west',
                    width: 300,
                    rootVisible: false,
                    xtype: 'treepanel',
                    treetype: 'parttree',
                    store: store,
                    hideHeaders: true,
                    columns: [{
                        xtype: 'treecolumn',
                        flex: 1,
                        dataIndex: 'Name',
                        renderer: function(val, meta, rec) {
                            var code = rec.get('Code');
                            if (code) {
                                return '<b>' + code + '</b> ' + val;
                            }

                            return val;
                        }
                    }]
                },
                {
                    region: 'center',
                    xtype: 'form',
                    autoScroll: true,
                    tbar: [
                        {
                            text: 'Сохранить', iconCls: 'icon-disk', cmd:"save"
                        }
                    ],
                    formtype: 'attrvaleditor',
                    defaults: {
                        margin: 10,
                        labelWidth: 350,
                        anchor: '100%',
                        flex: 1
                    }
                }
            ]
        });

        me.callParent();
    }
});