Ext.define('B4.view.menu.DisclosureInfoPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Раскрытие информации о деятельности',
    itemId: 'disclosureInfoPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.store.ManagingOrganization',
        'B4.store.dict.PeriodDi',
        'B4.view.Control.GkhButtonImport',
        'B4.form.SelectField',
        'B4.view.menu.DesktopDi',
        'B4.view.Control.GkhButtonPrint'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    split: false,
                    border: false,
                    style: 'background: none repeat scroll 0 0 #DFE9F6',
                    itemId: 'headerPanel',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'container',
                            style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Начните работу, выбрав период отчета.</span>'
                        },
                        {
                            xtype: 'container',
                            padding: '10px 0 0 0',
                            style: 'background: none repeat scroll 0 0 #DFE9F6',
                            defaults: {
                                labelAlign: 'right'
                            },
                            layout: {
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'vbox',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            editable: false,
                                            name: 'PeriodDi',
                                            labelWidth: 160,
                                            labelAlign: 'right',
                                            width: 440,
                                            itemId: 'tfPeriodDi',
                                            fieldLabel: 'Период отчета',
                                            //

                                            store: 'B4.store.dict.PeriodDi',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            editable: false,
                                            name: 'ManagingOrganization',
                                            labelWidth: 160,
                                            labelAlign: 'right',
                                            width: 440,
                                            disabled: true,
                                            itemId: 'tfManagingOrgDi',
                                            fieldLabel: 'Управляющая организация',
                                            store: 'B4.store.ManagingOrganization',
                                            flex: 1,
                                            textProperty: 'ContragentShortName',
                                            columns: [
                                                {
                                                    text: 'Муниципальное образование',
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
                                                        url: '/Municipality/ListWithoutPaging'
                                                    }
                                                },
                                                { text: 'Наименование', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                                                { text: 'ИНН', dataIndex: 'ContragentInn', flex: 1, filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            width: 440,
                                            items: [
                                                {
                                                    xtype: 'label',
                                                    text: 'Состояние:',
                                                    margin: '5px 0 0 90px'
                                                },
                                                {
                                                    xtype: 'button',
                                                    iconCls: 'icon-accept',
                                                    margin: '0 0 5px 10px',
                                                    itemId: 'btnState',
                                                    text: 'Состояние',
                                                    menu: [],
                                                    flex: 1
                                                },
                                                {
                                                    margin: '0 0 5px 0',
                                                    xtype: 'gkhbuttonimport',
                                                    disabled: true,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'gkhbuttonprint',
                                                    itemId: 'btnPrint',
                                                    disabled: true
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'vbox',
                                    items: [
                                        {
                                            xtype: 'container',
                                            html: '<span style="">Общий процент по раскрытию информации</span>',
                                            margin: '0 0 0 20px'
                                        },
                                        {
                                            xtype: 'container',
                                            itemId: 'labelPercent',
                                            margin: '10px 0 0 20px',
                                            data: {
                                                percent: '0'
                                            },
                                            tpl: '<div style="width: 250px; text-align: center; font-size:24px; color: #0a4f84;">{percent} %</div>',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'button',
                                            margin: '5px 0 0 75px',
                                            itemId: 'btnPercentCalc',
                                            disabled: true,
                                            scale: 'large',
                                            text: 'Пересчитать проценты',
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '10px 0 0 0',
                    style: 'background: none repeat scroll 0 0 #DFE9F6',
                    itemId: 'actionPanel',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox'
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    flex: 1,
                    layout: 'fit',
                    itemId: 'mainPanel',
                    enableTabScroll: true,
                    items: [
                        {
                            xtype: 'desktopdi'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
