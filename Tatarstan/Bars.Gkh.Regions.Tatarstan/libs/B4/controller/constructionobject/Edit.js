Ext.define('B4.controller.constructionobject.Edit', {
    /* 
    * Контроллер формы редактирования объектов капремонта
    */
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton',
        'B4.aspects.permission.constructionobject.Passport'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['ConstructionObject'],
    stores: ['constructionobject.NavigationMenu'],
    views: ['constructionobject.EditPanel'],

    mainView: 'constructionobject.EditPanel',
    mainViewSelector: 'constructionobjeditpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjectpassportpermission',
            name: 'passportPermissionAspect',
            afterSetRequirements: function (rec) {
                var me = this,
                    fieldsets = me.controller.getMainView().query('fieldset');

                Ext.each(fieldsets, function(fieldset) {
                    var visibledFields = fieldset.query('[name]:not([hidden])');
                    fieldset.setVisible(visibledFields.length > 0);
                });
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'constructionobjStateButtonAspect',
            stateButtonSelector: 'constructionobjeditpanel #btnState',
            listeners: {
                transfersuccess: function(asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('constructionobjEditPanelAspect').setData(entityId);
                    asp.controller.getStore('constructionobject.NavigationMenu').load();
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела объекта КР
            */
            xtype: 'gkheditpanel',
            name: 'constructionobjEditPanelAspect',
            editPanelSelector: 'constructionobjeditpanel',
            modelName: 'ConstructionObject',
            listeners: {
                aftersetpaneldata: function(asp, rec) {
                    this.controller.getAspect('constructionobjStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    this.updateTitle(rec.get('Address'));
                }
            },

            otherActions: function(actions) {
                actions[this.editPanelSelector + ' [name=SumSmr]'] = {
                    'change': {
                        fn: this.updateLimitOnHouse,
                        scope: this
                    }
                };
                actions[this.editPanelSelector + ' [name=SumDevolopmentPsd]'] = {
                    'change': {
                        fn: this.updateLimitOnHouse,
                        scope: this
                    }
                };
            },

            updateLimitOnHouse: function() {
                var smr = this.componentQuery(this.editPanelSelector + ' [name=SumSmr]'),
                    psd = this.componentQuery(this.editPanelSelector + ' [name=SumDevolopmentPsd]'),
                    limit = this.componentQuery(this.editPanelSelector + ' [name=LimitOnHouse]');

                limit.setValue(smr.getValue() + psd.getValue() || void 0);
            },

            updateTitle: function(address) {
                var com = Ext.ComponentQuery.query('constructionobjNavigationPanel breadcrumbs')[0];

                if (com) {
                    com.update({ text: address });
                }   
            }
        }
    ],

    index: function(id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjeditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view, 'construction_object_info');

        me.getAspect('constructionobjEditPanelAspect').setData(id);
        me.getAspect('passportPermissionAspect').setPermissionsByRecord({ getId: function () { return id; } });
    }
});