Ext.define('B4.controller.baselicensereissuance.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'Disposal',
        'BaseLicenseReissuance',
        'RealityObjectGji'
    ],

    stores: [
        'baselicensereissuance.RealityObject',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'baselicensereissuance.RealityObjectGrid',
        'baselicensereissuance.EditPanel'
    ],

    mainView: 'baselicensereissuance.EditPanel',
    mainViewSelector: 'baselicensereissuanceeditpanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    baseStatementEditPanelSelector: 'baselicensereissuanceeditpanel',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BaseLicApplicants.Edit', applyTo: 'b4savebutton', selector: 'baselicenseappeditpanel' },
                { name: 'GkhGji.Inspection.BaseLicApplicants.Field.InspectionNumber_Edit', applyTo: '#tfInspectionNumber', selector: 'baselicenseappeditpanel' }
            ]
        },
        {
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseLicenseReissCreateButtonAspect',
            buttonSelector: 'baselicensereissuanceeditpanel gjidocumentcreatebutton',
            containerSelector: 'baselicensereissuanceeditpanel',
            typeBase: 135 // Тип проверка обращения
        },
        {
            xtype: 'statebuttonaspect',
            name: 'baseLicenseReissStateButtonAspect',
            stateButtonSelector: 'baselicensereissuanceeditpanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('baseLicenseReissEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            Аспект основной панели Проверки по обращению граждан
            */
            xtype: 'gjiinspectionaspect',
            name: 'baseLicenseReissEditPanelAspect',
            editPanelSelector: 'baselicensereissuanceeditpanel',
            modelName: 'BaseLicenseReissuance',
            otherActions: function (actions) {
                var me = this;
                actions[me.editPanelSelector + ' #btnDelete'] = { 'click': { fn: me.btnDeleteClick, scope: me } };
            },
            
            onSaveSuccess: function (asp, record) {
                asp.controller.setInspectionId(record.get('Id'));
            },

            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {
                    var me = this,
                        inspId = rec.get('Id');

                    asp.controller.params = asp.controller.params || {};

                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    var callbackUnMask = asp.controller.params.callbackUnMask;
                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
                    }
                                 
                    asp.controller.setInspectionId(inspId);
                    
                    //Обновляем статусы
                    me.controller.getAspect('baseLicenseReissStateButtonAspect').setStateData(inspId, rec.get('State'));
                    //Обновляем кнопку Сформировать
                    me.controller.getAspect('baseLicenseReissCreateButtonAspect').setData(inspId);
                }
            },
            btnDeleteClick: function () {
                var me = this,
                    panel = me.getPanel(),
                    record = panel.getForm().getRecord();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function (result) {
                    if (result == 'yes') {
                        me.mask('Удаление', B4.getBody());
                        record.destroy()
                            .next(function (result) {
                                //Обновляем дерево меню
                                me.unmask();
                                var tree = Ext.ComponentQuery.query(me.controller.params.treeMenuSelector)[0];
                                tree.getStore().load();
                            })
                            .error(function (result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                me.unmask();
                            });
                    }
                });
            }
            
        },
    ],

    init: function () {
        var me = this;
        me.getStore('baselicensereissuance.RealityObject').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('baseLicenseReissEditPanelAspect').setData(me.params.inspectionId);

            var mainView = me.getMainComponent();
            if (mainView)
                mainView.setTitle(me.params.title);
        }
        me.getStore('baselicensereissuance.RealityObject').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        if (me.params && me.params.inspectionId > 0)
            operation.params.inspectionId = me.params.inspectionId;
    },

    setInspectionId: function (id) {
        var me = this;

        if (me.params) {
            me.params.inspectionId = id;
        }
    }
});