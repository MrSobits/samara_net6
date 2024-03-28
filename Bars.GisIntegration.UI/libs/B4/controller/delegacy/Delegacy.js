Ext.define('B4.controller.delegacy.Delegacy', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.ux.grid.toolbar.Paging'
    ],

    models: [
        'delegacy.Delegacy'
    ],
    stores: [
        'delegacy.Delegacy'
    ],
    views: [
        'B4.view.delegacy.Grid',
        'B4.view.delegacy.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'delegacygrid' }
    ],

    mainView: 'delegacy.Grid',
    mainViewSelector: 'delegacygrid',

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'delegacyGridMultiSelectAspect',
            gridSelector: 'delegacygrid',
            editFormSelector: 'delegacyeditwindow',
            editWindowView: 'delegacy.EditWindow',
            storeName: 'delegacy.Delegacy',
            modelName: 'delegacy.Delegacy',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#delegacyGridMultiSelectWindow',
            multiSelectWindowWidth: 1100,
            storeSelect: 'delegacy.InformationProviderForSelect',
            storeSelected: 'delegacy.InformationProviderSelected',
            titleSelectWindow: 'Выбор поставщиков информации',
            titleGridSelect: 'Контрагенты',
            titleGridSelected: 'Выбранные контрагенты',
            columnsGridSelect: [
                {
                    text: 'Наименование',
                    flex: 2,
                    dataIndex: 'FullName',
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'ОГРН',
                    flex: 1,
                    dataIndex: 'Ogrn',
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                {
                    text: 'Наименование',
                    flex: 2,
                    dataIndex: 'FullName',
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'ОГРН',
                    flex: 1,
                    dataIndex: 'Ogrn',
                    filter: { xtype: 'textfield' }
                }
            ],
            toolbarItems: [
                {
                    xtype: 'fieldset',
                    padding: '0 0 0 5',
                    flex: 1,
                    border: false,
                    layout: { type: 'hbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 90,
                        labelAlign: 'right',
                        padding: '0 0 0 5',
                        flex: 1,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'OperatorIS',
                            store: 'B4.store.RisContragent',
                            textProperty: 'FullName',
                            fieldLabel: 'Оператор ИС',
                            editable: false,
                            columns: [
                                {
                                    text: 'Наименование',
                                    flex: 2,
                                    dataIndex: 'FullName',
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'ОГРН',
                                    flex: 1,
                                    dataIndex: 'Ogrn',
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Юридический адрес',
                                    flex: 3,
                                    dataIndex: 'JuridicalAddress',
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },
                        {
                            xtype: 'datefield',
                            name: 'StartDate',
                            fieldLabel: 'Дата начала',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'EndDate',
                            fieldLabel: 'Дата окончания',
                            format: 'd.m.Y'
                        }
                    ]
                }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var informationProviderIds = [],
                    form = asp.getForm(),
                    operatorIs = form.down('[name=OperatorIS]').getValue(),
                    startDate = form.down('[name=StartDate]').getValue(),
                    endDate = form.down('[name=EndDate]').getValue();

                    records.each(function (rec) {
                        informationProviderIds.push(rec.get('Id'));
                    });

                    if (!operatorIs) {
                        Ext.Msg.alert('Ошибка', 'Необходимо выбрать оператора информационной системы');
                        return false;
                    }

                    if (!startDate) {
                        Ext.Msg.alert('Ошибка', 'Необходимо указать дату начала делегирования');
                        return false;
                    }

                    if (!endDate) {
                        Ext.Msg.alert('Ошибка', 'Необходимо указать дату окончания делегирования');
                        return false;
                    }

                    if (informationProviderIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                        B4.Ajax.request(B4.Url.action('AddInformationProviders', 'Delegacy', {
                            informationProviderIds: Ext.encode(informationProviderIds),
                            operatorIs: operatorIs,
                            startDate: startDate,
                            endDate: endDate
                        })).next(function () {
                            asp.getGrid().getStore().load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка', 'Необходимо выбрать поставщиков информации');
                        return false;
                    }

                    return true;
                },
                beforesaverequest:function(aspect) {
                    var me = this,
                        form = me.getEditForm().getForm();

                    if (!form.isValid()) {
                        Ext.Msg.alert('Ошибка', 'Не заполнены все обязательные поля');
                        return false;
                    }

                    return true;
                },
                aftersetformdata: function(aspect, record, form) {
                    var editForm = form.getForm();

                    editForm.isValid(); // подсветить незаполненные поля
                }
            }
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('delegacygrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});