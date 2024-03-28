/*
Данный аспект предназначен для кнопки в которой будут подтягиватся возможные пункты для формирваония документов
*/

Ext.define('B4.aspects.GjiDocumentCreateButton', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gjidocumentcreatebuttonaspect',

    buttonSelector: null,
    typeBase: null, // Тут указывается тип енума документа ГЖИ
    typeDocument: null,
    rulesStore: null,
    parentId: 0, // типа будет Идентификатор родителя, который будет инициировать создаие документа 
    params: {},

    constructor: function (config) {
        var me = this;

        Ext.apply(me, config);
        me.callParent(arguments);

        me.addEvents(
            'createsuccess',
            'beforecreate',
            'validateparams'
        );

        me.on('createsuccess', me.onCreateSuccess, me);
        me.on('validateparams', me.onValidateParams, me);
    },
    
    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[me.buttonSelector + ' menuitem'] = { 'click': { fn: me.onMenuItemClick, scope: me } };

        me.rulesStore = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: ['Id', 'Name', 'ActionUrl'],
            proxy: {
                autoLoad: false,
                type: 'ajax',
                url: B4.Url.action('/InspectionGji/GetListRules'),
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        me.rulesStore.on('beforeload', me.onBeforeLoadStore, me);
        me.rulesStore.on('load', me.onLoadStore, me);

        controller.control(actions);
    },

    setData: function (id) {
        var me = this;

        me.parentId = id;
        me.rulesStore.load();
    },

    onCreateSuccess:function() {
        var me = this;
        me.rulesStore.load();
    },

    onBeforeLoadStore: function (store, operation) {
        var me = this;

        operation.params = {};
        operation.params.parentId = me.parentId;
        operation.params.typeDocument = me.typeDocument;
        operation.params.typeBase = me.typeBase;
    },

    onLoadStore: function (store) {
        var btn = Ext.ComponentQuery.query(this.buttonSelector)[0];

        if (btn) {
            btn.setDisabled(true);
            
            btn.menu.removeAll();

            store.each(function (rec) {
                
                btn.setDisabled(false);
                
                btn.menu.add({
                    xtype: 'menuitem',
                    text: rec.data.Name,
                    textAlign: 'left',
                    ruleId: rec.data.Id,
                    actionUrl: rec.data.ActionUrl,
                    iconCls: 'icon-report'
                });
            });
        }
    },

    getParams: function(ruleBtn) {
        return {
            ruleId: ruleBtn.ruleId,
            parentId: this.parentId,
            actionUrl: ruleBtn.actionUrl
        };
    },

    onMenuItemClick: function (item) {
        // Заполняем параметры котоыре длявсех действий будут по умолчанию
        var me = this,
            params = me.getParams(item);
        
        if (me.fireEvent('validateparams', me, params) !== false) {
            me.createDocument(params);
        }
    },

    //Метод проверки основных параметров
    onValidateParams:function(asp, params) {
        if (!params) {
            Ext.Msg.alert('Ошибка формирвоания документа', 'Не заполнены параметры, необходимые для формирвоания документа.');
            
            return false;
        }
        
        if (!params.ruleId) {
            Ext.Msg.alert('Ошибка формирвоания документа', 'Не заполнен параметр ruleId');

            return false;
        }
        
        if (!params.parentId) {
            Ext.Msg.alert('Ошибка формирвоания документа', 'Не заполнен параметр parentId');
            return false;
        }

        return this.onValidateUserParams(params);
    },
    
    //Если необходимо чтобы пользователь сначала заполнил какието дополнительныепараметры
    // Например выбрал по каким домам проверка или по каким нарушениям
    // Он должен значит произвести сначаал какойто ыбор из списка
    // следвоательно втом контроллере где используется аспект необходимо вернуть false чтобы пользователь выбрал то что он хочет
    onValidateUserParams: function(params)
    {
        // по Умолчанию возвращается true
        return true;    
    },

    //в общем иногда ловится такой баг, что создается куча документов одновременно,
    //скорее всего косяк с событиями, в каких-то случаях навешивается один и тот же обработчик,
    //в качестве быстрого решения - проверка флага
    isCreating: false,

    createDocument: function (params) {
        if (this.isCreating) {
            return;
        }

        var me = this,
            container = Ext.ComponentQuery.query(this.containerSelector)[0];

        me.isCreating = true;
        me.controller.mask('Формирование документа', container);

        B4.Ajax.request({
            url: B4.Url.action('CreateDocument', 'InspectionGji'),
            method: 'POST',
            timeout: 9999999,
            params: params
        }).next(function (res) {
           var data = Ext.decode(res.responseText),
               tree = Ext.ComponentQuery.query(me.controller.params.treeMenuSelector)[0],
               docParams = {};

           if (tree) {
                tree.getStore().load();
            }
            
            // Формируем параметры для контроллера редактирования предписания
           
            docParams.inspectionId = data.inspectionId;
            docParams.documentId = data.documentId;
            docParams.containerSelector = me.controller.params.containerSelector;
            docParams.treeMenuSelector = me.controller.params.treeMenuSelector;
            
            me.isCreating = false;
            me.fireEvent('createsuccess', me);
            
            // Для того чтобы маска снялась только после показа новой карточки, формирую функцию обратного вызова
            if (!me.controller.hideMask) {
                me.controller.hideMask = function () { me.controller.unmask(); };
            }
            
            if (params.actionUrl) {
                me.controller.loadController(params.actionUrl, docParams, me.controller.params.containerSelector, null, me.controller.hideMask);
            } else {
                me.controller.hideMask.call();
            }

        }).error(function (e) {
            me.isCreating = false;
            me.controller.unmask();
            Ext.Msg.alert('Ошибка формирования документа!', e.message||e);
        });
    }
});