Ext.define('B4.controller.baseplanaction.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'BasePlanAction',
        'Disposal'
    ],

    views: [
        'baseplanaction.EditPanel'
    ],

    mainView: 'baseplanaction.EditPanel',
    mainViewSelector: 'basePlanActionEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    //baseInsCheckEditPanelSelector : '#basePlanActionEditPanel',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [
                { name: 'GkhGji.Inspection.BasePlanAction.Edit', applyTo: 'b4savebutton', selector: 'basePlanActionEditPanel' }
            ],
            name: 'editPlanActionStatePerm',
            editFormAspectName: 'basePlanActionEditPanelAspect',
            setPermissionEvent: 'aftersetpaneldata'
        },
        {
            //Аспект формирвоания документов для данного основания проверки
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'basePlanActionCreateButtonAspect',
            buttonSelector: 'basePlanActionEditPanel gjidocumentcreatebutton',
            containerSelector: 'basePlanActionEditPanel',
            typeBase: 110 // Тип инспекционная проверка
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'basePlanActionStateButtonAspect',
            stateButtonSelector: 'basePlanActionEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('basePlanActionEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
             *Аспект основной панели проверки по плану мероприятий
             */
            xtype: 'gjiinspectionaspect',
            name: 'basePlanActionEditPanelAspect',
            editPanelSelector: 'basePlanActionEditPanel',
            modelName: 'BasePlanAction',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' [name=Contragent]'] = {
                    'beforeload': { fn: this.onBeforeLoadContragent, scope: this },
                    'change': { fn: this.onChangeContragent, scope: this },
                    'triggerClear': { fn: this.onClearContragent, scope: this }
                };
                actions[this.editPanelSelector + ' [name=PersonInspection]'] = { 'change': { fn: this.changeTypePersonInspection, scope: this } };
            },
            
            onBeforeLoadContragent: function (store, operation) {
                var form = this.getPanel(),
                    typeJurOrg = form.down('[name=TypeJurPerson]').getValue();

                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.typeJurOrg = typeJurOrg;
            },
            
            onClearContragent: function (fld) {
                fld.setValue(null);
            },
            
            onChangeContragent: function(fld) {
                var me = this,
                    form = fld.up(this.editPanelSelector),
                    fldContragentOgrn = form.down('[name=ContragentOgrn]'),
                    fldContragentAddress = form.down('[name=ContragentAddress]'),
                    fldContragentInn = form.down('[name=ContragentInn]');
                
                me.controller.mask('Информация по контрагенту', me.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetContragentInfo', 'BasePlanAction', {
                    contragentId: fld.getValue()
                })).next(function(response) {
                    me.controller.unmask();
                    
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    fldContragentOgrn.setValue(obj.contragentOgrn);
                    fldContragentInn.setValue(obj.contragentInn);
                    fldContragentAddress.setValue(obj.contragentAddress);
                    
                }).error(function () {
                    me.controller.unmask();
                });
                    
            },
            
            changeTypePersonInspection: function (fld) {
                var form = fld.up(this.editPanelSelector),
                    fldContragentAddress = form.down('[name=ContragentAddress]'),
                    fldPersonAddress = form.down('[name=PersonAddress]'),
                    fldContragent = form.down('[name=Contragent]'),
                    fldTypeJurPerson = form.down('[name=TypeJurPerson]'),
                    fldPerson = form.down('[name=PhysicalPerson]'),
                    fldContragentOgrn = form.down('[name=ContragentOgrn]'),
                    fldContragentInn = form.down('[name=ContragentInn]');

                if (fld.getValue() == 20) {
                    fldTypeJurPerson.setDisabled(false);
                    fldContragent.setDisabled(false);
                    fldPerson.setDisabled(true);
                    fldContragentAddress.show();
                    fldPersonAddress.hide();
                    fldContragentInn.show();
                    fldContragentOgrn.show();
                } else {
                    fldTypeJurPerson.setDisabled(true);
                    fldContragent.setDisabled(true);
                    fldPerson.setDisabled(false);
                    fldContragentAddress.hide();
                    fldPersonAddress.show();
                    fldContragentInn.hide();
                    fldContragentOgrn.hide();
                }
            },

            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {

                    asp.controller.params = asp.controller.params || {};
                    
                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    var callbackUnMask = asp.controller.params.callbackUnMask;
                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
                    }
                    
                    //Обновляем статусы
                    this.controller.getAspect('basePlanActionStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    //Обновляем кнопку Сформировать
                    this.controller.getAspect('basePlanActionCreateButtonAspect').setData(rec.get('Id'));
                }
            }
        }
        
    ],

    onLaunch: function () {
        if (this.params) {
            this.getAspect('basePlanActionEditPanelAspect').setData(this.params.inspectionId);
            var mainView = this.getMainComponent();
            if(mainView)
                mainView.setTitle(this.params.title);
        }
    }
});