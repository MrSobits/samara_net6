Ext.define('B4.controller.dict.KindCheckGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.KindCheckGji'],
    
    stores: [
        'dict.KindCheckGji'
    ],

    views: [
        'dict.kindcheckgji.Grid'
    ],

    mainView: 'dict.kindcheckgji.Grid',
    mainViewSelector: 'kindcheckGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'kindcheckGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'kindcheckGjiGrid',
            permissionPrefix: 'GkhGji.Dict.KindCheck'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'kindcheckgridaspect',
            storeName: 'dict.KindCheckGji',
            modelName: 'dict.KindCheckGji',
            gridSelector: 'kindcheckGjiGrid',
            listeners: {
                beforesave: function (me, store) {
                    var codeValidate = true,
                        nameValidate = true,
                        codes = [];
                    Ext.each(store.data.items, function (item) {
                        var length = codes.length,
                            i = 0;
                        for (; i < length; i++) {
                            if (item.get('Code') == codes[i]) {
                                codeValidate = false;
                                return false;
                            }
                        }
                        
                        if (!item.get('Name')) {
                            nameValidate = false;
                            return false;
                        }
                        codes.push(item.get('Code'));
                        return true;
                    });
                    
                    if (!nameValidate) {
                        Ext.Msg.alert('Ошибка!', 'Поле наименование должно быть заполнено у всех записей!');
                        return false;
                    }

                    if (!codeValidate) {
                        Ext.Msg.alert('Ошибка!', 'Коды видов проверок должны быть уникальными!');
                        return false;
                    }

                    return true;
                }
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('kindcheckGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.KindCheckGji').load();
    }
});