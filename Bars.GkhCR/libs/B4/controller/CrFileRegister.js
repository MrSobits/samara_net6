Ext.define('B4.controller.CrFileRegister', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    afterset: false,
    CrFileRegister: null,
  
    models: [
        'CrFileRegister',
    ],
    stores: [
        'CrFileRegister'

    ],
    views: [
        'crfileregister.Grid',
        'crfileregister.EditWindow'
    ],
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'crfileregisterGridAspect',
            gridSelector: 'crfileregistergrid',
            editFormSelector: '#crfileregisterEditWindow',
            storeName: 'CrFileRegister',
            modelName: 'CrFileRegister',
            editWindowView: 'crfileregister.EditWindow',
            otherActions: function (actions) {
                actions['#crfileregisterEditWindow #formation'] = { 'click': { fn: this.formation, scope: this } };
            },
            formation: function () {
                var me = this;
                var form = this.getForm();
                var ro = form.down('#sfRealityObject');
                if (ro.value != null) {
                    var roId = ro.value.Id;
                }

                if (ro != null) {
                    B4.Ajax.request({
                        timeout: 999999,
                        url: B4.Url.action('CreateTask', 'CrFileRegister'),
                        params: {
                            roId: roId
                        }
                    }).next(function (response) {
                        var data = Ext.decode(response.responseText);
                        //Ext.Msg.alert('Сообщение', data.data);
                        B4.QuickMsg.msg('Сообщение', data.data, 'success');
                        me.controller.unmask();
                        return true;
                    }).error(function (result) {
                        me.controller.unmask();
                        Ext.Msg.alert('Ошибка', result.message);
                    });
                }
            },
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                this.controller.afterset = false;
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
        }
    ],


    mainView: 'crfileregister.Grid',
    mainViewSelector: 'crfileregistergrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'crfileregistergrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        //this.afterset = false;        
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('crfileregistergrid');
        this.bindContext(view);
        this.afterset = false;
        this.application.deployView(view);
        this.getStore('CrFileRegister').load();
    }
});