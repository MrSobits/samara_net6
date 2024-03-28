/*
* Данный аспект предназначен для описание взаимодействия списка обращений граждан с выбором документов для отправки в ЕМСЭД
*/

Ext.define('B4.aspects.DocumentListForEmsed', {
    extend: 'B4.base.Aspect',

    alias: 'widget.documentlistforemsedaspect',
    panelSelector: null,
    gridAppealCitsSelector: null,
    gridDocumentsSelector: null,
    storeAppealCitsName: null,
    storeDocuments: null,
    controller: null,

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents('getdata');
    },

    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        this.controller = controller;

        actions[this.gridAppealCitsSelector] = {
            'gridaction': { fn: this.appealcitsGridAction, scope: this },
            'select': { fn: this.appealcitsGridSelectRow, scope: this }
        };

        actions[this.gridDocumentsSelector] = {
            'gridaction': { fn: this.documentsGridAction, scope: this }
        };

        actions[this.gridAppealCitsSelector + ' b4updatebutton'] = { 'click': { fn: this.appealcitsGridBtnAction, scope: this } };
        actions[this.gridDocumentsSelector + ' b4updatebutton'] = { 'click': { fn: this.documentsGridBtnAction, scope: this } };
        actions[this.gridDocumentsSelector + ' #btnSendDocuments'] = { 'click': { fn: this.sendDocuments, scope: this } };

        var storeAppealCits = this.controller.getStore(this.storeAppealCitsName);
        storeAppealCits.on('load', this.onStoreAppealCitsLoad, this);

        var storeDocuments = this.controller.getStore(this.storeDocuments);
        storeDocuments.on('beforeload', this.onStoreDocumentsBeforeLoad, this);

        controller.control(actions);
    },

    loadAppealCitsStore: function () {
        this.controller.getStore(this.storeAppealCitsName).load();
    },

    loadDocumentStore: function () {
        this.controller.getStore(this.storeDocuments).load();
    },

    getAppealCitsGrid: function () {
        return Ext.ComponentQuery.query(this.gridAppealCitsSelector)[0];
    },

    getDocumentsGrid: function () {
        return Ext.ComponentQuery.query(this.gridDocumentsSelector)[0];
    },

    appealcitsGridBtnAction: function (btn) {
        this.getAppealCitsGrid().fireEvent('gridaction', this.getAppealCitsGrid(), btn.actionName);
    },

    documentsGridBtnAction: function (btn) {
        this.getDocumentsGrid().fireEvent('gridaction', this.getDocumentsGrid(), btn.actionName);
    },

    sendDocuments: function () {
        var data = this.getDocumentsGrid().getSelectionModel().getSelection();
        this.fireEvent('getdata', this, data);
    },
    onStoreAppealCitsLoad: function (store, operation) {
        if (store.getCount() > 0) {
            this.getAppealCitsGrid().getSelectionModel().select(0);
        } else {
            this.controller.getStore(this.storeDocuments).removeAll();
        }
    },

    //Перед обновлением грида документов мы получаем выделенные строки в гриде обращений и передаем Id в параметры
    onStoreDocumentsBeforeLoad: function (store, operation) {
        var rec = this.getAppealCitsGrid().getSelectionModel().getSelection()[0];
        if (!rec) return;

        var appealCitsId = rec.getId();
        operation.params["appealCitsId"] = appealCitsId;

        this.controller.params = this.params || {};
        this.controller.params["appealCitsId"] = appealCitsId;
    },

    appealcitsGridSelectRow: function () {
        //при выделении сроки мы вызываем обновление грида
        this.loadDocumentStore();
    },

    appealcitsGridAction: function (grid, action) {
        switch (action.toLowerCase()) {
            case 'update':
                this.loadAppealCitsStore();
                break;
        }
    },

    documentsGridAction: function (grid, action) {
        switch (action.toLowerCase()) {
            case 'update':
                this.loadDocumentStore();
                break;
        }
    },

    loadEditPanel: function (controllerName, params) {
        var me = this;
        //Накладываю маску чтобы после нажатия пункта меню в дереве нельзя было нажать 10 раз до инициализации контроллера
        if (!me.controller.hideMask) {
            me.controller.hideMask = function () { me.controller.unmask(); };
        }
        me.controller.mask('Загрузка', me.controller.getMainComponent());
        me.controller.loadController(controllerName, params, params.containerSelector, me.controller.hideMask);
    }
});