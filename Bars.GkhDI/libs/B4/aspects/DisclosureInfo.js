Ext.define('B4.aspects.DisclosureInfo', {
    extend: 'B4.base.Aspect',
    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.enums.TypeDiTargetAction'
    ],
    alias: 'widget.disclosureinfoaspect',

    mananagingOrgPanelView: null,
    managingOrgDataStoreName: null,
    modelDisclosureInfo: null,
    managingOrgRealityObjPanelView: null,
    mananagingOrgRealityObjGridSelector: null,
    modelDisclosureInfoRealityObj: null,
    modelManagingOrgRealityObject: null,
    actionStoreName: null,

    init: function(controller) {
        var actions = {};
        this.callParent(arguments);

        this.addEvents(
            'changeheader',
            'aftertabmenuload'
        );

        actions['#actionPanel button'] = { 'click': { fn: this.btnActionMenuClick, scope: this } };
        actions['#headerPanel b4selectfield'] = { 'change': { fn: this.onChangeHeader, scope: this } };
        actions['#headerPanel #tfManagingOrgDi'] = { 'beforeload': { fn: this.onBeforeLoadManagingOrg, scope: this } };
        actions[this.buttonRoSelector + ' menuitem'] = { 'click': { fn: this.onMenuItemClick, scope: this } };
        actions[this.buttonManorgSelector + ' menuitem'] = { 'click': { fn: this.onMenuItemClick, scope: this } };
        actions[this.mananagingOrgPanelSelector] = { 'rowaction': { fn: this.discloseInfo, scope: this } };
        actions[this.mananagingOrgRealityObjGridSelector] = { 'rowaction': { fn: this.discloseInfoRealityObj, scope: this } };
        actions['#headerPanel #btnPercentCalc'] = { 'click': { fn: this.btnPercentCalcClick, scope: this } };

        controller.control(actions);
        this.controller.getStore(this.actionStoreName).on('load', this.onRoActionStoreLoad, this);

        // Замут с кнопкой операций
        this.roActionStore = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: ['ControllerName', 'Name', 'Icon', 'Code'],
            proxy: {
                autoLoad: false,
                type: 'ajax',
                url: B4.Url.action('/GroupDi/GetGroupActions'),
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        this.manorgActionStore = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: ['ControllerName', 'Name', 'Icon', 'Code'],
            proxy: {
                autoLoad: false,
                type: 'ajax',
                url: B4.Url.action('/GroupDi/GetGroupActions'),
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        this.roActionStore.on('load', this.onLoadActionStore(this.buttonRoSelector), this);
        this.roActionStore.on('beforeload', this.onBeforeLoadActionStore(true), this);

        this.manorgActionStore.on('load', this.onLoadActionStore(this.buttonManorgSelector), this);
        this.manorgActionStore.on('beforeload', this.onBeforeLoadActionStore(false), this);
    },

    loadRoActionStore: function() {
        this.roActionStore.load();
    },
    loadManorgActionStore: function() {
        this.manorgActionStore.load();
    },

    //метод срабатывающий на переход к раскрытию инф-ии
    discloseInfo: function(grid, action, record) {
        var me = this;

        //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
        if (!me.controller.hideMask) {
            me.controller.hideMask = function() { me.controller.unmask(); };
        }

        me.controller.mask('Загрузка', me.controller.getMainComponent());
        me.controller.params.disclosureInfoId = this.controller.disclosureInfoId;
        me.controller.params.recDi = this.controller.recDi;
        me.controller.loadController(record.get('controller'), me.controller.params, null, me.controller.hideMask);
    },

    //метод срабатывающий на переход к раскрытию инф-ии по объекту недвижимости
    discloseInfoRealityObj: function(grid, action, record) {
        //делаем запрос в контроллер и смотрим есть ли деятельность (id) по данному дому TODO каждый раз запрос не делать!!!Передовать id в листе

        var me = this;
        me.controller.mask('Загрузка', me.controller.getMainComponent());

        B4.Ajax.request(B4.Url.action('GetDisclosureInfo',
                'DisclosureInfoRealityObj',
                {
                    disclosureInfoId: me.controller.disclosureInfoId,
                    realtyObjId: record.get('Id'),
                    year: me.controller.periodDiDateStart,
                    manOrgId: me.controller.recDi.ManagingOrganization.Id
                }))
            .next(function(response) {
                var obj = Ext.JSON.decode(response.responseText);
                var disclosureInfoRealityObjId = obj.Id;
                if (disclosureInfoRealityObjId == 0 || Ext.isEmpty(disclosureInfoRealityObjId)) {

                    Ext.Msg.confirm('Подтверждение начала раскрытия деятельности',
                        'Начать раскрытие деятельности по данному объекту?',
                        function(result) {
                            if (result == 'yes') {
                                B4.Ajax.request(B4.Url.action('SaveDisclosureInfo',
                                        'DisclosureInfoRealityObj',
                                        {
                                            disclosureInfoId: me.controller.disclosureInfoId,
                                            realtyObjId: record.get('Id'),
                                            manOrgId: me.controller.recDi.ManagingOrganization.Id
                                        }))
                                    .next(function(res) {
                                        Ext.Msg.alert('Раскрытие деятельности!',
                                            'Успешное начало раскрытия деятельности');
                                        var object = Ext.JSON.decode(res.responseText);
                                        //инициализируем контроллер и открываем окошко
                                        me.controller.params.disclosureInfoRealityObjId = object.Id;
                                        me.controller.params.disclosureInfoId = object.disclosureInfoId;
                                        me.controller.params.realtyObjId = object.realtyObjId;
                                        me.controller.params.Address = object.Address;

                                        //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                                        if (!me.controller.hideMask) {
                                            me.controller.hideMask = function() { me.controller.unmask(); };
                                        }

                                        me.controller.mask('Загрузка', me.controller.getMainComponent());
                                        me.controller
                                            .loadController('B4.controller.Navigation',
                                                me.controller.params,
                                                null,
                                                me.controller.hideMask);
                                    })
                                    .error(function() {
                                        Ext.Msg.alert('Ошибка!', 'Не удалось начать деятельность');
                                    });
                            }
                        });

                } else {
                    //инициализируем контроллер и открываем окошко
                    me.controller.params.disclosureInfoRealityObjId = disclosureInfoRealityObjId;
                    me.controller.params.disclosureInfoId = obj.disclosureInfoId;
                    me.controller.params.Address = obj.Address;
                    me.controller.params.year = me.controller.periodDiDateStart;
                    me.controller.params.periodDiDateStart = Ext.Date.format(new Date(me.controller.periodDiDateStart), 'd.m.Y');
                    me.controller.params.periodDiDateEnd = Ext.Date.format(new Date(me.controller.periodDiDateEnd), 'd.m.Y');
                    //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
                    if (!me.controller.hideMask) {
                        me.controller.hideMask = function() { me.controller.unmask(); };
                    }

                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    me.controller.loadController('B4.controller.Navigation',
                        me.controller.params,
                        null,
                        me.controller.hideMask);
                }

                me.controller.unmask();
            })
            .error(function() {
                Ext.Msg.alert('Ошибка!', 'Повторите свои действия еще раз');
                me.controller.unmask();
            });

    },

    // вызывает setDisabled для всех вкладок
    setDisabledButtons: function (disabled) {
        var me = this,
            actionPanel = me.controller.getMainComponent().down('#actionPanel');

        if (actionPanel) {
            actionPanel.getChildItemsToDisable().forEach(function (item) {
                item.setDisabled(disabled);
            });
        }
    },
    // вызывает toggle для всех вкладок
    setToggleButtons: function (toggle) {
        var me = this,
            actionPanel = me.controller.getMainComponent().down('#actionPanel');

        if (actionPanel) {
            actionPanel.getChildItemsToDisable().forEach(function (item) {
                item.toggle(toggle);
            });
        }
    },

    //Метод срабатывающий после выставления ук и периода
    onChangeHeader: function() {
        var me = this,
            asp = me,
            mainCmp = me.controller.getMainComponent(),
            headerPanel = mainCmp.down('#headerPanel'),
            actionPanel = mainCmp.down('#actionPanel'),
            mainPanel = mainCmp.down('#mainPanel'),
            btnPrint = headerPanel.down('#btnPrint'),
            tfManagingOrg = headerPanel.down('#tfManagingOrgDi'),
            tfPeriodDi = headerPanel.down('#tfPeriodDi'),
            tab1 = actionPanel.down('#tabObj1'),
            tab2 = actionPanel.down('#tabObj2'),
            manOrgInfoPercentText = 'Сведения об УО',
            realObjsPercentText = 'Объекты в управлении',
            defaultComponent = me.controller.getView('menu.DesktopDi').create();

        mainPanel.removeAll();
        mainPanel.add(defaultComponent);
        mainPanel.doLayout();

        if (tfPeriodDi.getValue() > 0) {
            asp.controller.mask('Загрузка', asp.controller.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetDateStartByPeriod',
                    'DisclosureInfo',
                    {
                        periodDiId: tfPeriodDi.getValue()
                    }))
                .next(function(response) {
                    asp.controller.unmask();
                    var obj = Ext.JSON.decode(response.responseText);
                    asp.controller.periodDiDateStart = obj.DateStart;
                    asp.controller.periodDiDateEnd = obj.DateEnd;
                    tfManagingOrg.setDisabled(false);
                })
                .error(function() {
                    asp.controller.unmask();
                });
        } else {
            tfManagingOrg.setDisabled(true);
            btnPrint.setDisabled(true);
        };

        if (tfManagingOrg.value &&
            tfPeriodDi.value &&
            tfManagingOrg.value.Contragent &&
            tfManagingOrg.value.Contragent.DateRegistration > tfPeriodDi.value.DateEnd) {
            Ext.Msg.alert('Ошибка!', 'Управляющая организация зарегистрирована позднее текущего активного периода');
            tfManagingOrg.setValue(null);
        }

        if (tfManagingOrg.getValue() > 0 && tfPeriodDi.getValue() > 0) {

            btnPrint.setDisabled(false);

            me.controller.getAspect('disclosureinfopringaspect').loadReportStore();
            me.controller.manOrgId = tfManagingOrg.getValue();

            me.controller.mask('Загрузка', me.controller.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetDisclosureInfo',
                    'DisclosureInfo',
                    {
                        managingOrgId: tfManagingOrg.getValue(),
                        periodId: tfPeriodDi.getValue(),
                        manOrgId: tfManagingOrg.getValue()
                    }))
                .next(function(response) {
                    me.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var model = asp.controller.getModel(asp.modelDisclosureInfo);

                    if (Ext.isEmpty(obj.recDi) || obj.recDi.Id == 0) {
                        Ext.Msg.confirm('Подтверждение начала раскрытия деятельности!',
                            'Начать раскрытие деятельности?',
                            function(result) {
                                if (result == 'yes') {

                                    //пользователь хочет раскрываться следовательно формируем модель из выбранных триггер филдов и сохраняем ее
                                    var recordDisInfo =
                                        new model(
                                        {
                                            Id: 0,
                                            ManagingOrganization: tfManagingOrg.getValue(),
                                            PeriodDi: tfPeriodDi.getValue()
                                        });

                                    recordDisInfo.save({ id: recordDisInfo.getId() })
                                        .next(function(res) {
                                                me.setDisabledButtons(false);
                                                headerPanel.down('gkhbuttonimport').setDisabled(false);
                                                Ext.Msg.alert('Раскрытие деятельности!',
                                                    'Успешное начало раскрытия деятельности');
                                                //запоминаем в какой раскрытии информации мы находимся
                                                asp.controller.disclosureInfoId = res.record.getId();
                                                asp.controller.recDi = res.record.getData();
                                                //запоминаем тип управления (для филтрации пунктов меню)
                                                asp.controller.typeManagement = tfManagingOrg.value.TypeManagement;
                                                asp.fireEvent('changeheader',
                                                    asp,
                                                    { Id: res.record.getId(), State: res.record.get('State') });
                                            },
                                            me)
                                        .error(function() {
                                                me.setDisabledButtons(true);
                                                headerPanel.down('gkhbuttonimport').setDisabled(true);
                                                me.setToggleButtons(false);
                                                asp.controller.disclosureInfoId = null;
                                                asp.fireEvent('changeheader', asp);
                                                Ext.Msg.alert('Ошибка!', 'Не удалось начать деятельность');
                                            },
                                            me);

                                } else {
                                    tfPeriodDi.setValue(null);
                                    tfManagingOrg.setValue(asp.controller.manOrg || null);
                                    me.setDisabledButtons(true);
                                    headerPanel.down('gkhbuttonimport').setDisabled(true);
                                    asp.fireEvent('changeheader', asp);
                                    me.setToggleButtons(false);
                                    asp.controller.disclosureInfoId = null;
                                }
                            });

                    } else {
                        me.setDisabledButtons(false);
                        headerPanel.down('gkhbuttonimport').setDisabled(false);
                        actionPanel.down('#tabObj1').toggle(true);
                        //запоминаем в какой раскрытии информации мы находимся
                        asp.controller.disclosureInfoId = obj.recDi.Id;
                        asp.controller.recDi = obj.recDi;
                        //запоминаем тип управления (для филтрации пунктов меню)
                        asp.controller.typeManagement = tfManagingOrg.value.TypeManagement;
                        asp.fireEvent('changeheader', asp, { Id: obj.recDi.Id, State: obj.recDi.State });
                        //открываем сведения об УО активной вкладкой
                        me.btnActionMenuClick(actionPanel.down('#tabObj1'));
                    }

                    var diPercent = '0';

                    if (obj.diperc) {
                        diPercent = Ext.util.Format.round(obj.diperc, 2);
                    }

                    if (obj.manorginfoperc) {
                        manOrgInfoPercentText = Ext.String
                            .format('{0} ( {1} )',
                                manOrgInfoPercentText,
                                Ext.util.Format.round(obj.manorginfoperc, 2) + '%');
                    }

                    if (obj.realobjsperc) {
                        realObjsPercentText = Ext.String.format('{0} ( {1} )',
                            realObjsPercentText,
                            Ext.util.Format.round(obj.realobjsperc, 2) + '%');
                    }

                    tab1.setText(manOrgInfoPercentText);
                    tab2.setText(realObjsPercentText);
                    headerPanel.down('#labelPercent').update({ percent: diPercent });

                })
                .error(function() {
                    Ext.Msg.alert('Ошибка!', 'Повторите свои действия еще раз');
                    me.controller.unmask();
                });

        } else {
            me.setDisabledButtons(true);
            asp.fireEvent('changeheader', asp);
            me.setToggleButtons(false);
            headerPanel.down('#btnPercentCalc').setDisabled(true);
            tab1.setText(manOrgInfoPercentText);
            tab2.setText(realObjsPercentText);
            asp.controller.disclosureInfoId = null;
            btnPrint.setDisabled(true);
        }
    },

    btnPercentCalcClick: function(btn) {
        var actionPanel = this.controller.getMainComponent().down('#actionPanel');
        this.controller.mask('Расчет процентов...', me.controller.getMainComponent());
        B4.Ajax.request({
                url: B4.Url.action('MassCalculate',
                    'PercentCalculation',
                    { disclosureInfoId: this.controller.recDi.Id }),
                timeout: 1000 * 60 * 60 * 24 // 24 часа
            })
            .next(function(response) {
                    var obj = Ext.JSON.decode(response.responseText),
                        messsage = 'Расчет процентов произведен успешно';

                    if (obj.hasOwnProperty('ParentTaskId')) {
                        if (obj.ParentTaskId > 0) {
                            messsage = 'Расчет процентов производится в порядке очереди, статус выполнения можно посмотреть в задачах';
                        } else {
                            messsage = 'По данной управляющей компании в этом периоде уже идет расчет процентов!';
                        }
                    } else {
                        this.controller.getMainComponent()
                            .down('#headerPanel')
                            .down('#labelPercent')
                            .update({ percent: Ext.util.Format.round(obj.diperc, 2) });
                        if (obj.manorginfoperc !== 100 && obj.manorginfoperc > 99) {
                            var manorginfoperc = obj.manorginfoperc.toString();
                            manorginfoperc = manorginfoperc.substr(0, 5);
                            this.controller.getMainComponent()
                                .down('#actionPanel')
                                .down('#tabObj1')
                                .setText(Ext.String.format('Сведения об УО ( {0} )', manorginfoperc + '%'));
                        } else {
                            this.controller.getMainComponent()
                                .down('#actionPanel')
                                .down('#tabObj1')
                                .setText(Ext.String.format('Сведения об УО ( {0} )',
                                    Ext.util.Format.round(obj.manorginfoperc, 2) + '%'));
                        }
                        if (obj.realobjsperc !== 100 && obj.realobjsperc > 99) {
                            var realobjsperc = obj.realobjsperc.toString();
                            realobjsperc = realobjsperc.substr(0, 5);
                            this.controller.getMainComponent()
                                .down('#actionPanel')
                                .down('#tabObj2')
                                .setText(Ext.String.format('Объекты в управлении ( {0} )', realobjsperc + '%'));
                        } else {
                            this.controller.getMainComponent()
                                .down('#actionPanel')
                                .down('#tabObj2')
                                .setText(Ext.String.format('Объекты в управлении ( {0} )',
                                    Ext.util.Format.round(obj.realobjsperc, 2) + '%'));
                        }

                        me.btnActionMenuClick(actionPanel.down('#tabObj1'));
                    }

                    Ext.Msg.alert('Cообщение', messsage);
                    this.controller.unmask();
                },
                this)
            .error(function(response) {
                    Ext.Msg.alert('Ошибка!', response.message || 'Расчет процентов произведен ошибочно');
                    this.controller.unmask();
                },
                this);
    },

    //кликнули по кнопки action меню нужно подгружать соотв панель
    btnActionMenuClick: function(btn) {

        btn.toggle(true);
        var mainPanel = this.controller.getMainComponent().down('#mainPanel');

        var grid;
        if (mainPanel) {
            if (btn.type == 'btnDataManOrg') {
                mainPanel.removeAll();

                grid = Ext.create('B4.view.' + this.mananagingOrgPanelView, { store: this.managingOrgDataStoreName });
                this.controller.getStore(this.managingOrgDataStoreName).load();

                mainPanel.add(grid);
            }

            if (btn.type == 'btnObjects') {
                mainPanel.removeAll();

                var managingOrgRealityObjPanel = Ext.create('B4.view.' + this.managingOrgRealityObjPanelView);
                managingOrgRealityObjPanel.getStore().load();
                //managingOrgRealityObjPanel.down('#groupDiGrid').getStore().load();

                mainPanel.add(managingOrgRealityObjPanel);
            }
            mainPanel.doLayout();
        }
    },

    //формирование action меню
    onRoActionStoreLoad: function(store) {
        me = this;
        var button, i = 0;
        var actionPanel = me.controller.getMainComponent().down('#actionPanel').down('.container');
        if (actionPanel.items.getCount() == 0) {
            actionPanel.removeAll();
            store.each(function(rec) {
                i++;
                button = Ext.create('Ext.tab.Tab', {
                    type: rec.get('type'),
                    text: rec.get('percent').concat(rec.get('text')),
                    itemId: 'tabObj' + i,
                    toggleGroup: 'tabToggleGroup',
                    closable: false,
                    cls: 'b4helper-cls',
                    style: 'border-bottom: none !important; margin: 0 2px 0 0;',
                    disabled: Ext.isEmpty(me.controller.disclosureInfoId)
                });

                actionPanel.add(button);
            });
            me.fireEvent('aftertabmenuload', me, actionPanel);

            actionPanel.doLayout();
        }
    },

    onBeforeLoadManagingOrg: function(field, options) {
        options.params = {};
        //Подгружаем только действующие в рамках выбронного периода УК
        options.params.periodDiDateStart = this.controller.periodDiDateStart;
        options.params.periodDiDateEnd = this.controller.periodDiDateEnd;
        options.params.fromDisinfo = true;
    },

    onLoadActionStore: function(selector) {
        return function(store) {
            var btn = Ext.ComponentQuery.query(selector)[0];

            if (btn) {
                btn.menu.removeAll();

                store.each(function(rec) {
                    btn.menu.add({
                        xtype: 'menuitem',
                        text: rec.get('Name'),
                        textAlign: 'left',
                        iconCls: rec.get('Icon'),
                        actionName: rec.get('ControllerName'),
                        code: rec.get('Code')
                    });
                });
            }
        };
    },

    onBeforeLoadActionStore: function(isReaityObject) {
        return function(store, operation) {
            operation.params = operation.params || {};
            operation.params.disclosureInfoId = this.controller.disclosureInfoId;
            operation.params.typeAction = isReaityObject
                ? B4.enums.TypeDiTargetAction.RealityObject
                : B4.enums.TypeDiTargetAction.ManagingOrganization;
        };
    },

    onMenuItemClick: function(itemMenu) {
        var me = this;
        if (!me.controller.hideMask) {
            me.controller.hideMask = function() { me.controller.unmask(); };
        }
        var groupActionWindow = this.getGroupActionWindow();
        groupActionWindow.removeAll();
        groupActionWindow.setTitle(itemMenu.text);

        me.controller.mask('Загрузка', me.controller.getMainComponent());

        if (itemMenu.actionName) {
            me.controller.params.disclosureInfoId = me.controller.disclosureInfoId;
            me.controller.loadController(itemMenu.actionName,
                me.controller.params,
                me.groupActionWindowSelector,
                me.controller.hideMask);
        } else {
            switch (itemMenu.code) {
                case 'ReformaManorgManualIntegration':
                    Ext.Msg.confirm('Внимание', 'Вы действительно хотите провести интеграцию с системой Реформа ЖКХ?', function(btnId) {
                            if (btnId === 'yes') {
                                B4.Ajax.request(B4.Url.action('ScheduleManorgIntegrationTask', 'ManualIntegration',
                                    {
                                         disclosureInfoId: me.controller.disclosureInfoId
                                    }))
                                    .next(function() {
                                        Ext.Msg.alert('Выборочная интеграция', 'Задача успешно поставлена в очередь');
                                        me.controller.unmask();
                                    })
                                    .error(function(response) {
                                        Ext.Msg.alert('Ошибка!', response.message || response);
                                        me.controller.unmask();
                                    });
                            } else {
                                me.controller.unmask();
                            }
                        });
                    break;
            }
        }

    },

    getGroupActionWindow: function() {

        if (this.groupActionWindowSelector) {
            var groupActionWindow = Ext.ComponentQuery.query(this.groupActionWindowSelector)[0];

            if (groupActionWindow && groupActionWindow.isHidden() && groupActionWindow.rendered) {
                groupActionWindow = groupActionWindow.destroy();
            }

            if (!groupActionWindow) {
                groupActionWindow = this.controller.getView(this.groupActionWindowView)
                    .create({ constrain: true, autoDestroy: true });

                if (B4.getBody().getActiveTab()) {
                    B4.getBody().getActiveTab().add(groupActionWindow);
                } else {
                    B4.getBody().add(groupActionWindow);
                }
            }
            return groupActionWindow;
        }
    }
});