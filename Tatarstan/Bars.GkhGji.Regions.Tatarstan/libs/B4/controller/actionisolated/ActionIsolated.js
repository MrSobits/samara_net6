Ext.define('B4.controller.actionisolated.ActionIsolated', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridEditForm',
        'B4.enums.TypeBaseAction',
        'B4.enums.TypeObjectAction'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'actionisolated.TaskAction'
    ],

    stores: [
        'actionisolated.TaskAction'],

    views: [
        'actionisolated.MainPanel',
        'actionisolated.FilterPanel',
        'actionisolated.Grid',
        'actionisolated.AddWindow'
    ],

    mainView: 'actionisolated.MainPanel',
    mainViewSelector: 'actionisolatedmainpanel',

    refs: [
        {
            ref: 'ActionIsolatedFilterPanel',
            selector: 'actionisolatedfilterpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhgrideditformaspect',
            name: 'actionisolatedGridWindowAspect',
            gridSelector: 'actionisolatedgrid',
            editFormSelector: '#actionisolatedaddwindow',
            storeName: 'actionisolated.TaskAction',
            modelName: 'actionisolated.TaskAction',
            editWindowView: 'actionisolated.AddWindow',
            controllerEditName: 'B4.controller.actionisolated.Navigation',
            otherActions: function (actions) {
                actions['#actionisolatedaddwindow b4enumcombo[name=TypeBase]'] = { 'change': { fn: this.onChangeTypeBase, scope: this } };
                actions['#actionisolatedaddwindow b4enumcombo[name=TypeObject]'] = { 'change': { fn: this.onChangeTypeObject, scope: this } };
            },
            onSaveSuccess: function (aspect, rec) {
                aspect.getForm().close();
                
                var model = this.controller.getModel(this.modelName);

                model.load(rec.getId(), {
                    success: function (record) {
                        this.editRecord(record);
                    },
                    scope: this
                });
            },
            listeners: {
               
            },
            onChangeTypeBase: function (field, value) {
                var form = this.getForm(),
                    appealCits = form.down('b4selectfield[name=AppealCits]'),
                    plan = form.down('b4selectfield[name=PlanAction]');

                if (value !== '') {
                    this.hideComponent(appealCits, !(value == B4.enums.TypeBaseAction.Appeal));
                    this.hideComponent(plan, !(value == B4.enums.TypeBaseAction.Plan));
                } else {
                    this.hideComponent(appealCits, true);
                    this.hideComponent(plan, true);
                }
            },
            onChangeTypeObject: function (field, value) {
                var form = this.getForm(),
                    typeContragent = form.down('b4enumcombo[name=TypeJurPerson]'),
                    personInfo = form.down('container[name=PersonInfo]'),
                    contragent = form.down('b4selectfield[name=Contragent]');

                if (value !== '') {
                    this.hideComponent(typeContragent, value == B4.enums.TypeObjectAction.Individual);
                    this.hideComponent(contragent, value == B4.enums.TypeObjectAction.Individual);
                    this.hideComponent(personInfo, value == B4.enums.TypeObjectAction.Legal);
                } else {
                    this.hideComponent(typeContragent, true);
                    this.hideComponent(contragent, true);
                    this.hideComponent(personInfo, true);
                }
            },
            hideComponent: function (cmp, isHide) {
                cmp.allowBlank = isHide;
                cmp.setVisible(!isHide);
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'actActionIsolatedButtonExportAspect',
            gridSelector: 'actionisolatedgrid',
            buttonSelector: 'actionisolatedgrid #btnExport',
            controllerName: 'TaskActionIsolated',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'actionisolatedgrid': { 'actionisolatedstore.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'actionisolatedgrid checkbox[name=IsClosed]' : { 'change': { fn: me.updateMainGrid, scope: me } },
            'actionisolatedfilterpanel [action=updateGrid]': {
                'click': { fn: me.updateMainGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        me.updateMainGrid();
    },

    updateMainGrid: function () {
        this.getMainView().down('actionisolatedgrid').getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            filterPanel = me.getActionIsolatedFilterPanel(),
            isClosed = me.getMainView().down('actionisolatedgrid').down('[name=IsClosed]').getValue();

        if (filterPanel) {
            operation.params.planId = filterPanel.down('[name=PlanAction]').getValue();
            operation.params.kindAction = filterPanel.down('[name=KindAction]').getValue();
            operation.params.realityObjectId = filterPanel.down('[name=RealityObject]').getValue();
            operation.params.dateStart = filterPanel.down('[name=DateStart]').getValue();
            operation.params.dateEnd = filterPanel.down('[name=DateEnd]').getValue();
        }
        operation.params.isClosed = isClosed;
    }
});