Ext.define('B4.controller.DisclosureInfo', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.DisclosureInfo',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    params: {},
    customPermissions: {},

    views: [
        'menu.DisclosureInfoPanel',
        'menu.ManagingOrgDataGrid',
        'menu.ManagingOrgRealityObjDataGrid',
        'groups.EditWindow',
        'groups.Grid',
        'SelectWindow.MultiSelectWindow',
        'groups.GroupActionWindow',
        'service.GroupActionWindow',
        'menu.ImportWindow',
        'menu.DesktopDi',
        'menu.DisclosureInfoEmptyFieldsGrid',
        'menu.DisclosureInfoRealityObjEmptyFieldsGrid'
    ],

    stores:
    [
        'menu.ActionMenu',
        'menu.ManagingOrgDataMenu',
        'menu.ManagingOrgRealityObjDataMenu',
        'menu.DisclosureInfoEmptyFields',
        'menu.DisclosureInfoRealityObjEmptyFields',
        'groups.GroupDi',
        'groups.RealityObjGroup',
        'groups.RealityObjSelect',
        'groups.RealityObjSelected',
        'groups.GroupDiSelect',
        'groups.GroupDiSelected'
    ],

    models:
    [
        'DisclosureInfo',
        'DisclosureInfoRealityObj',
        'menu.DisclosureInfoEmptyFields',
        'groups.GroupDi',
        'groups.RealityObjGroup'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'menu.DisclosureInfoPanel',
    mainViewSelector: '#disclosureInfoPanel',

    editWindowSelector: '#groupsDiEditWindow',

    aspects: [
    {
        xtype: 'gkhpermissionaspect',
        permissions: [
            {
                name: 'GkhDi.Import.DisclosureInfoImport.View', applyTo: 'gkhbuttonimport', selector: '#disclosureInfoPanel',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            },
            {
                name: 'GkhDi.Import.DisclosureInfoImport.Active', applyTo: 'gkhbuttonimport', selector: '#disclosureInfoPanel',
                applyBy: function (component, allowed) {
                    if (component) {
                        component.setDisabled(!allowed);
                    }
                }
            },
            {
                name: 'GkhDi.EmptyFieldsLog.DisclosureInfoEmptyFieldsGrid',
                selector: '#actionPanel',
                applyBy: function (component, allowed) {
                    var asp = this;
                    if (asp.controller.customPermissions) {
                        asp.controller.customPermissions.disclosureInfoEmptyFieldsGrid = allowed;
                    }
                }
            },
            {
                name: 'GkhDi.EmptyFieldsLog.DisclosureInfoRealityObjEmptyFieldsGrid',
                selector: '#actionPanel',
                applyBy: function (component, allowed) {
                    var asp = this;
                    if (asp.controller.customPermissions) {
                        asp.controller.customPermissions.disclosureInfoRealityObjEmptyFieldsGrid = allowed;
                    }
                }
            }
        ]
    },
        {
            //Аспект кнопки печати
            xtype: 'gkhbuttonprintaspect',
            name: 'disclosureinfopringaspect',
            buttonSelector: '#disclosureInfoPanel #btnPrint',
            codeForm: 'DisclosureInfo',
            getUserParams: function (reportId) {
                var mainView = this.controller.getMainComponent(),
                    param = {
                        periodId: mainView.down('b4selectfield[name=PeriodDi]').getValue(),
                        manorgId: mainView.down('b4selectfield[name=ManagingOrganization]').getValue()
                    };

                this.params.userParams = Ext.JSON.encode(param);
            },

            printReport: function (itemMenu) {
                var frame = Ext.get('downloadIframe');
                if (frame != null) {
                    Ext.destroy(frame);
                }

                this.getUserParams(itemMenu.actionName);
                Ext.apply(this.params, { reportId: itemMenu.actionName });

                var urlParams = Ext.urlEncode(this.params);

                var newUrl = Ext.urlAppend('/DiReport/Report731Print/?' + urlParams, '_dc=' + (new Date().getTime()));
                newUrl = B4.Url.action(newUrl);
                if (this.openInNewWindow()) {
                    window.open(newUrl);
                } else {
                    Ext.DomHelper.append(document.body, {
                        tag: 'iframe',
                        id: 'downloadIframe',
                        frameBorder: 0,
                        width: 0,
                        height: 0,
                        css: 'display:none;visibility:hidden;height:0px;',
                        src: newUrl
                    });
                }
            }
        },
    {
        /*
        *аспект для импорта
        */
        xtype: 'gkhbuttonimportaspect',
        name: 'disclosureInfoImportAspect',
        buttonSelector: '#disclosureInfoPanel #btnImport',
        codeImport: 'CommunalPay',
        windowImportView: 'menu.ImportWindow',
        windowImportSelector: '#importDisclosureInfoWindow',
        maxFileSize: 104857600,
        getUserParams: function () {
            this.params = this.params || {};
            this.params.PeriodDiId = this.controller.getMainView().down('#tfPeriodDi').getValue();
        }
    },
    {
        /*
        Вешаем аспект пермишинов по статусу
        */
        xtype: 'gkhstatepermissionaspect',
        name: 'disclosureInfoPermissionAspect',
        permissions: [
            { name: 'GkhDi.Disinfo.PercCalc', applyTo: '#btnPercentCalc', selector: '#headerPanel' }
        ]
    },
    {
        /*
        Вешаем аспект смены статуса
        */
        xtype: 'statebuttonaspect',
        name: 'disinfoStateButtonAspect',
        stateButtonSelector: '#disclosureInfoPanel #btnState',
        listeners: {
            transfersuccess: function (asp, entityId) {
                //После успешной смены статуса запрашиваем по Id актуальные данные записи
                //и обновляем панель
                var disclosureinfo = asp.controller.getAspect('disclosureinfo');
                var model = asp.controller.getModel(disclosureinfo.modelDisclosureInfo);
                model.load(entityId, {
                    success: function (rec) {
                        asp.setStateData(rec.getId(), rec.get('State'));
                    },
                    scope: disclosureinfo
                });
                // меняем пермишины по статусу
                asp.controller.acceptPermissions(entityId);

                disclosureinfo.loadActionStore();
            }
        }
    },
    {
        xtype: 'disclosureinfoaspect',
        name: 'disclosureinfo',
        mananagingOrgPanelView: 'menu.ManagingOrgDataGrid',
        mananagingOrgPanelSelector: '#managingOrgDataGrid',
        managingOrgRealityObjPanelView: 'menu.ManagingOrgRealityObjDataGrid',
        managingOrgRealityObjPanelSelector: '#managingOrgRealityObjDataGrid',
        mananagingOrgRealityObjGridSelector: '#managingOrgRealityObjDataGrid',
        actionStoreName: 'menu.ActionMenu',
        managingOrgDataStoreName: 'menu.ManagingOrgDataMenu',
        modelDisclosureInfo: 'DisclosureInfo',
        modelDisclosureInfoRealityObj: 'DisclosureInfoRealityObj',
        buttonRoSelector: '#managingOrgRealityObjDataGrid #btnActionDi',
        buttonManorgSelector: '#managingOrgDataGrid #btnManorgActionDi',
        groupActionWindowView: 'service.GroupActionWindow',
        groupActionWindowSelector: '#groupActionWindow',
        listeners: {
            changeheader: function (asp, rec) {
                var disinfoStateButtonAspect = asp.controller.getAspect('disinfoStateButtonAspect');
                if (!Ext.isEmpty(rec.Id) && !Ext.isEmpty(rec.State)) {
                    disinfoStateButtonAspect.setStateData(rec.Id, rec.State);
                    // меняем пермишины по статусу
                    asp.controller.acceptPermissions(rec.Id);
                } else {
                    disinfoStateButtonAspect.getStateButton().setText('Статус');
                    disinfoStateButtonAspect.getStateButton().menu.removeAll();
                }
            },
            aftertabmenuload: function (asp, tabMenu) {
                asp.controller.onTabMenuCreated(tabMenu);
            }
        }
    },
    {
        xtype: 'grideditwindowaspect',
        name: 'groupsDiGridWindowAspect',
        gridSelector: '#groupDiGrid',
        editFormSelector: '#groupsDiEditWindow',
        storeName: 'groups.GroupDi',
        modelName: 'groups.GroupDi',
        editWindowView: 'groups.EditWindow',
        groupActionWindowView: 'groups.GroupActionWindow',
        groupActionFormSelector: '#groupActionWindow',
        otherActions: function (actions) {
            actions[this.gridSelector + ' #groupActionButton'] = { 'click': { fn: this.onGroupActionBtnClick, scope: this } };
        },
        onSaveSuccess: function (asp, record) {
            asp.controller.setCurrentId(record);
        },
        listeners: {
            aftersetformdata: function (asp, record) {
                asp.controller.setCurrentId(record);
            },
            getdata: function (asp, record) {
                if (this.controller.params && !record.data.Id) {
                    record.set('DisclosureInfo', this.controller.disclosureInfoId);
                }
            }
        },
        onGroupActionBtnClick: function () {
            this.getGroupActionWindow(this).show();
        },
        getGroupActionWindow: function (asp) {
            if (asp.groupActionFormSelector) {
                var groupActionWindow = Ext.ComponentQuery.query(asp.groupActionFormSelector)[0];

                if (!groupActionWindow) {
                    groupActionWindow = asp.controller.getView(asp.groupActionWindowView).create();
                }
                return groupActionWindow;
            }
            return null;
        }
    },
    {
        xtype: 'gkhgridmultiselectwindowaspect',
        name: 'addGroupsRealObjGridAspect',
        gridSelector: '#diRealityObjGroupGrid',
        storeName: 'groups.RealityObjGroup',
        modelName: 'groups.RealityObjGroup',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#groupsRealObjMultiSelectWindow',
        storeSelect: 'groups.RealityObjSelect',
        storeSelected: 'groups.RealityObjSelected',
        titleSelectWindow: 'Выбор жилых домов',
        titleGridSelect: 'Жилые дома для отбора',
        titleGridSelected: 'Выбранные дома',
        columnsGridSelect: [
            { header: 'Адрес дома', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
        ],
        columnsGridSelected: [
            { header: 'Адрес дома', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
        ],
        onBeforeLoad: function (store, operation) {
            operation.params.disclosureInfoId = this.controller.disclosureInfoId;
        },
        listeners: {
            getdata: function (asp, records) {
                var recordIds = [];

                records.each(function (rec) {
                    recordIds.push(rec.get('Id'));
                });

                if (recordIds[0] > 0) {
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddRealityObj', 'RealityObjGroup', {
                        objectIds: recordIds,
                        groupDiId: this.controller.groupDiId
                    })).next(function () {
                        asp.controller.getStore(asp.storeName).load();
                        asp.controller.unmask();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                } else {
                    Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                    return false;
                }
                return true;
            }
        }
    },
    {
        xtype: 'gkhtriggerfieldmultiselectwindowaspect',
        name: 'groupDiMultiSelectWindowAspect',
        fieldSelector: '#groupActionWindow #groupsTriggerField',
        multiSelectWindow: 'SelectWindow.MultiSelectWindow',
        multiSelectWindowSelector: '#groupsSelectWindow',
        storeSelect: 'Groups.GroupDiSelect',
        storeSelected: 'Groups.GroupDiSelected',
        textProperty: 'Name',
        columnsGridSelect: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
        ],
        columnsGridSelected: [
            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
        ],
        titleSelectWindow: 'Выбор групп домов',
        titleGridSelect: 'Группы домов для отбора',
        titleGridSelected: 'Выбранные группы домов',
        onBeforeLoad: function (store, operation) {
            operation.params.disclosureInfoId = this.controller.disclosureInfoId;
        }
    }],

    init: function () {
        var me = this;

        B4.Url.loadCss('content/css/b4Di.css');

        var actions = {};
        actions[me.mainViewSelector] = { 'afterrender': { fn: me.onMainViewAfterRender, scope: me } };
        me.control(actions);

        me.getStore('menu.ManagingOrgRealityObjDataMenu').on('beforeload', me.onBeforeLoad, me, 'LoadRoActionButton');

        me.getStore('menu.ManagingOrgDataMenu').on('beforeload', me.onBeforeLoad, me, 'ManagOrg');

        me.getStore('groups.GroupDi').on('beforeload', me.onBeforeLoad, me);

        me.getStore('groups.RealityObjGroup').on('beforeload', me.onBeforeLoad, me, 'RealityObjGroup');

        me.callParent(arguments);

    },

    onLaunch: function () {
        this.getStore('menu.ActionMenu').load();

        this.getStore('groups.GroupDi').load();

        this.getAspect('disclosureInfoImportAspect').loadImportStore();
    },

    onMainViewAfterRender: function () {

        this.setDefaultManOrg();
    },

    setDefaultManOrg: function () {
        var mainView = this.getMainComponent();
        if (mainView) {

            var sflManOrg = Ext.ComponentQuery.query('#tfManagingOrgDi')[0];

            if (Ext.isEmpty(sflManOrg.getValue())) {
                this.mask('Загрузка', mainView);

                B4.Ajax.request(B4.Url.action('GetOperatorManOrg', 'DisclosureInfo'))
                    .next(function (response) {
                        var obj = Ext.decode(response.responseText);

                        if (obj.count == 1) {
                            sflManOrg.setValue(obj.manOrg);
                            this.manOrg = manOrg;
                        }
                        this.unmask();
                    }, this)
                    .error(function () {
                        this.unmask();
                    }, this);
            }
        }
    },

    onBeforeLoad: function (store, operation, type) {
        var me = this;

        operation.params.disclosureInfoId = me.disclosureInfoId;
        

        if (type === 'ManagOrg') {
            //Передаем тип ук ранее зафиксированный вместе с disclosureInfoId в аспекте
            operation.params.typeManagement = me.typeManagement;
            operation.params.manOrgId = me.recDi.ManagingOrganization.Id
            me.getAspect('disclosureinfo').loadManorgActionStore();
        }
        if (type === 'RealityObjGroup') {
            operation.params.groupDiId = me.groupDiId;
        }
        if (type === 'LoadRoActionButton') {
            operation.params.manOrgId = me.recDi.ManagingOrganization.Id
            me.getAspect('disclosureinfo').loadRoActionStore();
        }
    },

    setCurrentId: function (rec) {
        this.groupDiId = rec.get('Id');

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        var grid = editWindow.down('#diRealityObjGroupGrid');

        var storeRealityObjGroup = this.getStore('groups.RealityObjGroup');
        storeRealityObjGroup.removeAll();

        if (this.groupDiId > 0) {
            storeRealityObjGroup.load();
            grid.setDisabled(false);
        } else {
            grid.setDisabled(true);
        }
    },

    acceptPermissions: function (id) {
        var params = {};
        params.getId = function () { return id; };
        this.getAspect('disclosureInfoPermissionAspect').setPermissionsByRecord(params);
    },

    onTabMenuCreated: function (tabMenu) {
        var me = this,
            actions = {},
            id = tabMenu.items.getCount() + 1,
            logTab,
            logRoTab;

        if (me.customPermissions.disclosureInfoEmptyFieldsGrid) {
            logTab = Ext.create('Ext.tab.Tab', {
                text: 'Журнал заполнения полей УО',
                name: 'emptyFieldsTab',
                itemId: 'tabObj' + id++,
                toggleGroup: 'tabToggleGroup',
                closable: false,
                cls: 'b4helper-cls',
                style: 'border-bottom: none !important; margin: 0 2px 0 0;',
                disabled: Ext.isEmpty(me.disclosureInfoId)
            });
            tabMenu.add(logTab);
            actions['#actionPanel button[name=emptyFieldsTab]'] = { 'click': { fn: me.onEmptyFieldsTabClick, scope: me } };
        }

        if (me.customPermissions.disclosureInfoRealityObjEmptyFieldsGrid) {
            logRoTab = Ext.create('Ext.tab.Tab', {
                text: 'Журнал заполнения полей объектов в управлении',
                name: 'emptyFieldsRoTab',
                itemId: 'tabObj' + id++,
                toggleGroup: 'tabToggleGroup',
                closable: false,
                cls: 'b4helper-cls',
                style: 'border-bottom: none !important; margin: 0 2px 0 0;',
                disabled: Ext.isEmpty(me.disclosureInfoId)
            });
            tabMenu.add(logRoTab);
            actions['#actionPanel button[name=emptyFieldsRoTab]'] = { 'click': { fn: me.onEmptyFieldsRoTabClick, scope: me } };
        }

        me.control(actions);
    },

    onEmptyFieldsTabClick: function () {
        var me = this,
            mainPanel = me.getMainComponent().down('#mainPanel'),
            view = Ext.create('B4.view.menu.DisclosureInfoEmptyFieldsGrid');

        if (mainPanel && view) {

            view.getStore().on('beforeload', me.onBeforeLoad, me, 'DisclosureInfoEmptyFields');

            mainPanel.removeAll();

            view.getStore().load();

            mainPanel.add(view);
            mainPanel.doLayout();
        }
    },

    onEmptyFieldsRoTabClick: function () {
        var me = this,
            mainPanel = me.getMainComponent().down('#mainPanel'),
            view = Ext.create('B4.view.menu.DisclosureInfoRealityObjEmptyFieldsGrid');

        if (mainPanel && view) {

            view.getStore().on('beforeload', me.onBeforeLoad, me, 'DisclosureInfoRealityObjEmptyFields');

            mainPanel.removeAll();

            view.getStore().load();

            mainPanel.add(view);
            mainPanel.doLayout();
        }
    }
});