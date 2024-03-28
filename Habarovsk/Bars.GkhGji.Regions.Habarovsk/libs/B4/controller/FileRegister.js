Ext.define('B4.controller.FileRegister', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    afterset: false,
    FileRegister: null,
  
    models: [
        'FileRegister',
    ],
    stores: [
        'FileRegister'

    ],
    views: [
        'fileregister.Grid',
        'fileregister.EditWindow'
    ],
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'fileregisterGridAspect',
            gridSelector: 'fileregistergrid',
            editFormSelector: '#fileregisterEditWindow',
            storeName: 'FileRegister',
            modelName: 'FileRegister',
            editWindowView: 'fileregister.EditWindow',
            otherActions: function (actions) {
                actions['#fileregisterEditWindow #formation'] = { 'click': { fn: this.formation, scope: this } };
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
                        url: B4.Url.action('CreateTask', 'FileRegister'),
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


    mainView: 'fileregister.Grid',
    mainViewSelector: 'fileregistergrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'fileregistergrid'
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
        var view = this.getMainView() || Ext.widget('fileregistergrid');
        this.bindContext(view);
        this.afterset = false;
        this.application.deployView(view);
        this.getStore('FileRegister').load();
    }
});