Ext.define('B4.view.manorg.contract.DirectManagEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.manorgcontractdirmanageditwin',
    
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 600,
    minHeight: 310,
    minWidth: 600,
    bodyPadding: 5,
    title: 'Непосредственное управление',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.view.manorg.Grid',
        'B4.store.ManagingOrganization'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: { type: 'vbox', align: 'stretch' },
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер',
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'StartDate',
                                    fieldLabel: 'Дата начала',
                                    allowBlank: false,
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'EndDate',
                                    fieldLabel: 'Дата окончания',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: { type: 'vbox', align: 'stretch' },
                            defaults: {
                                xtype: 'datefield',
                                format: 'd.m.Y',
                                labelAlign: 'right',
                                labelWidth: 170
                            },
                            items: [
                                {
                                    name: 'DocumentDate',
                                    fieldLabel: 'от'
                                },
                                {
                                    name: 'PlannedEndDate',
                                    fieldLabel: 'Плановая дата окончания'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Файл',
                    name: 'FileInfo',
                    anchor: '100%'
                },
                {
                    xtype: 'textarea',
                    name: 'Note',
                    fieldLabel: 'Примечание',
                    buttonText: 'Выбрать...',
                    msgTarget: 'side',
                    maxLength: 300
                },
                {
                    xtype: 'checkbox',
                    margin: '0 0 0 100',
                    name: 'IsServiceContract',
                    boxLabel: 'Договор оказания услуги  и (или) выполнения работ по содержанию и ремонту общего имущества в данном доме с управляющей организацией'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ManagingOrganization',
                    fieldLabel: 'Управляющая организация',
                    store: 'B4.store.ManagingOrganization',
                    textProperty: 'ContragentName',
                    editable: false,
                    allowBlank: false,
                    disabled: true,
                    columns: [
                        { text: 'Наименование УО', dataIndex: 'ContragentShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                        { text: 'ИНН', dataIndex: 'ContragentInn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'DateStartService',
                    allowBlank: false,
                    fieldLabel: 'Дата начала оказания услуг',
                    format: 'd.m.Y',
                    disabled: true
                },
                {
                    xtype: 'datefield',
                    name: 'DateEndService',
                    fieldLabel: 'Дата окончания оказания услуг',
                    format: 'd.m.Y',
                    disabled: true
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Договор',
                    name: 'ServContractFile',
                    disabled: true
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});