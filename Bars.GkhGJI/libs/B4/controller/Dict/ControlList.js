Ext.define('B4.controller.dict.ControlList', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid'
    ],

    questionId: null,

    models: [
        'dict.ControlList',
        'dict.ControlListQuestion'
    ],
    stores: [
        'dict.ControlList',
        'dict.ControlListQuestion'
    ],
    views: [
        'controllist.Grid',
        'controllist.EditWindow',
        'controllist.QuestionsGrid'
    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'controllistGridAspect',
            gridSelector: 'controllistgrid',
            editFormSelector: '#controllistEditWindow',
            storeName: 'dict.ControlList',
            modelName: 'dict.ControlList',
            editWindowView: 'controllist.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             //   typeKNDDictId = record.getId();
             //   asp.controller.setTypeKNDDictId(typeKNDDictId);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    questionId = record.getId();
                    
                    var grid = form.down('controllistquestiongrid'),
                    store = grid.getStore();
                    store.filter('clistId', record.getId());
                    asp.controller.setTypeKNDDictId(record.getId());
                }
               
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'ControlListQuestionsGridAspect',
            storeName: 'dict.ControlListQuestion',
            modelName: 'dict.ControlListQuestion',
            gridSelector: 'controllistquestiongrid',
            listeners: {
                beforesave: function (asp, store) {
                    var me = this;
                    if (questionId != null) {
                        Ext.each(store.data.items, function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('ControlList', questionId);
                            }
                        });
                    }
                }
            }
        }
         
    ],

    mainView: 'controllist.Grid',
    mainViewSelector: 'controllistgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'controllistgrid'
        },
        {
            ref: 'controllistquestiongrid',
            selector: 'controllistquestiongrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    controllistEditWindow: '#controllistEditWindow',

    index: function () {
        var view = this.getMainView() || Ext.widget('controllistgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ControlList').load();
    },

    setTypeKNDDictId: function (id) {
        this.questionId = id;

        var editWindow = Ext.ComponentQuery.query(this.controllistEditWindow)[0];

        if (id > 0) {
            editWindow.down('.tabpanel').setDisabled(false);
        } else {
            editWindow.down('.tabpanel').setDisabled(true);
        }
    },
});