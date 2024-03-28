Ext.define('B4.view.shortprogram.Panel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.shortprogrampanel',

    requires: [
        'B4.ux.button.Save',
        'B4.store.dict.Municipality',
        'B4.form.Combobox',
        'B4.view.shortprogram.RealityObjectGrid',
        'B4.view.shortprogram.RecordGrid',
        'B4.view.shortprogram.DefectListGrid',
        'B4.view.shortprogram.ProtocolGrid',
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.store.ShortProgramYear'
    ],

    minWidth: 750,
    width: 750,
    autoScroll: true,
    bodyPadding: 5,
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border'
    },
    defaults: {
        split: true
    },
    title: 'Краткосрочная программа',
    trackResetOnLoad: true,
    frame: true,

    initComponent: function() {
        var me = this,
            yearStore = Ext.create('B4.store.ShortProgramYear');
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'hidden',
                    name: 'MunicipalityId'
                },
                {
                    xtype: 'shortprogramrogrid',
                    region: 'west',
                    minWidth: 550
                },
                {
                    xtype: 'container',
                    region: 'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'breadcrumbs',
                            text: 'Адрес дома'
                        },
                        {
                            xtype: 'tabpanel',
                            flex: 1,
                            region: 'center',
                            layout: {
                                align: 'stretch'
                            },
                            enableTabScroll: true,
                            items: [
                                {
                                    xtype: 'shortprogramrecordgrid',
                                    flex: 1
                                },
                                {
                                    xtype: 'shortprogramdefectlistgrid',
                                    flex: 1
                                },
                                {
                                    xtype: 'shortprogramprotocolgrid',
                                    flex: 1
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
                            xtype: 'container',
                            layout: {
                                type: 'hbox'
                            },
                            flex: 1,
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        {
                                            xtype: 'button',
                                            action: 'ActualizeVersion',
                                            text: 'Актуализировать ДПКР',
                                            iconCls: 'icon-table-go'
                                        },
                                        {
                                            xtype: 'button',
                                            action: 'MassStateChange',
                                            text: 'Массовая смена статусов',
                                            iconCls: 'icon-table-go'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'tbseparator',
                                    width: 10
                                },
                                {
                                    xtype: 'b4combobox',
                                    name: 'Municipality',
                                    storeAutoLoad: false,
                                    editable: false,
                                    labelWidth: 160,
                                    labelAlign: 'right',
                                    fieldLabel: 'Муниципальное образование',
                                    url: '/Municipality/ListWithoutPaging',
                                    flex: 2
                                },
                                {
                                    xtype: 'b4combobox',
                                    name: 'Year',
                                    storeAutoLoad: false,
                                    editable: false,
                                    labelWidth: 200,
                                    labelAlign: 'right',
                                    fieldLabel: 'Период краткосрочной программы',
                                    store: yearStore,
                                    flex: 1
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