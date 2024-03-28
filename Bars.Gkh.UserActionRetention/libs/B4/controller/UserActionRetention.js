Ext.define('B4.controller.UserActionRetention', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    stores: ['AuditLogMapForSelect', 'AuditLogMapForSelected'],
    views: ['logentity.Grid', 'logentityproperty.DetailWindow', 'logentityproperty.Grid', 'SelectWindow.MultiSelectWindow'],

    mainView: 'logentity.Panel',

    refs: [
        { ref: 'mainPanel', selector: 'logentitypnl' },
        { ref: 'filterPanel', selector: 'logentityfilterpnl' }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'auditLogMapMultiselectWindowAspect',
            fieldSelector: 'logentityfilterpnl gkhtriggerfield[name="EntityName"]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#auditLogMapMultiselectWindow',
            storeSelect: 'AuditLogMapForSelect',
            storeSelected: 'AuditLogMapForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование объекта', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование объекта', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'userLoginMultiselectWindowAspect',
            fieldSelector: 'logentityfilterpnl gkhtriggerfield[name="UserLogin"]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#userLoginMultiselectWindow',
            storeSelect: 'UserLoginForSelect',
            storeSelected: 'UserLoginForSelected',
            columnsGridSelect: [
                {
                    header: 'Логин', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: { xtype: 'textfield' }
                }
            ],
            columnsGridSelected: [
                { header: 'Логин', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        }
    ],

    index: function () {
        var view = this.getMainPanel()
            || Ext.widget('logentitypnl');

        var today = new Date();
        var dateFrom = new Date(today.getFullYear(), today.getMonth(), today.getDate() - 7);
        var dateTo = new Date();

        view.down('datefield[name="DateFrom"]').setValue(dateFrom);
        view.down('datefield[name="DateTo"]').setValue(dateTo);

        this.bindContext(view);
        this.application.deployView(view);
    },

    init: function () {
        var me = this,
            actions = {
                'logentitygrid b4updatebutton': {
                    click: function (btn) {
                        btn.up('logentitygrid').getStore().load();
                    }
                },
                'logentitygrid': {
                    render: { fn: me.onRenderGrid, scope: me },
                    itemdblclick: { fn: me.onItemDblClickGrid, scope: me }
                },
                'logentitygrid b4editcolumn': {
                    click: { fn: me.editBtnClick, scope: me }
                },
                'logentitypropertygrid': {
                    render: { fn: me.onRenderLogEntityPropertyGrid, scope: me }
                },
                'logentityfilterpnl button[name="btnShowLog"]': {
                    click: { fn: me.onBtnShowLogClick, scope: me }
                },
                'logentityfilterpnl button[name="jsonExport"]': {
                    click: { fn: me.onExportBtnClick, scope: me }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    onRenderGrid: function (grid) {
        var me = this;

        grid.getStore().on('beforeload', me.onBeforeLoadGrid, me);
    },

    onRenderLogEntityPropertyGrid: function (grid) {
        var me = this,
            store = grid.getStore();

        store.on('beforeload', me.onBeforeLoadLogEntityPropertyGrid, me);
        store.load();
    },

    onBeforeLoadGrid: function (store, operation) {
        if (!operation.params) {
            operation.params = {};
        }

        var filterPanel = this.getFilterPanel();
        if (filterPanel) {
            operation.params.dateFrom = filterPanel.down('datefield[name="DateFrom"]').getValue();
            operation.params.dateTo = filterPanel.down('datefield[name="DateTo"]').getValue();
            operation.params.entityTypes = filterPanel.down('gkhtriggerfield[name="EntityName"]').getValue();
            operation.params.userIds = filterPanel.down('gkhtriggerfield[name="UserLogin"]').getValue();
        }
    },

    onBeforeLoadLogEntityPropertyGrid: function (store, operation) {
        if (!operation.params) {
            operation.params = {};
        }

        if (this.logEntityId) {
            operation.params.logEntityId = this.logEntityId;
        }
    },

    onBtnShowLogClick: function () {
        var store = this.getMainPanel().down('logentitygrid').getStore();

        if (store) {
            store.load();
        }
    },

    onItemDblClickGrid: function (view, record) {
        var detailWindow = Ext.ComponentQuery.query('detailwindow')[0],
            logEntityId = record.data.Id;

        if (detailWindow) {
            detailWindow.show();
        } else {
            this.logEntityId = logEntityId;
            detailWindow = Ext.create('B4.view.logentityproperty.DetailWindow');
            detailWindow.show();
        }
    },

    editBtnClick: function (gridView, rowIndex, colIndex, el, e, rec) {
        this.onItemDblClickGrid(gridView, rec);
    },

    onExportBtnClick: function () {
        var filterPanel = this.getFilterPanel();
        try {
            var params = Ext.Object.toQueryString({
                dateFrom: filterPanel.down('datefield[name="DateFrom"]').getValue(),
                dateTo: filterPanel.down('datefield[name="DateTo"]').getValue(),
                entityTypes: filterPanel.down('gkhtriggerfield[name="EntityName"]').getValue(),
                userIds: filterPanel.down('gkhtriggerfield[name="UserLogin"]').getValue()
            });

            var url = Ext.urlAppend('/UserActionRetention/ExportToJson', params);

            Ext.DomHelper.append(document.body, {
                tag: 'iframe',
                id: 'downloadIframe',
                frameBorder: 0,
                width: 0,
                height: 0,
                css: 'display:none;visibility:hidden;height:0px;',
                src: B4.Url.action(url)
            });
        } catch (e) {
            Ext.Msg.alert('Ошибка', 'Не удалось произвести выгрузку');
        }
    }
});