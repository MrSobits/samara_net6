Ext.define('B4.controller.dict.QualifyTestQuestions', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid'
    ],

    questionId: null,

    models: [
        'dict.qualifytest.QualifyTestQuestions',
        'dict.qualifytest.QualifyTestQuestionsAnswers'
    ],
    stores: [
        'dict.qualifytest.QualifyTestQuestionsAnswers',
        'dict.qualifytest.QualifyTestQuestions'
    ],
    views: [

        'dict.qualifytest.QualifyTestQuestionsGrid',
        'dict.qualifytest.QualifyTestQuestionsEditWindow',
        'dict.qualifytest.QualifyTestQuestionsAnswersGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'qualifyTestQuestionsGridAspect',
            gridSelector: 'qtestdictquestionsgrid',
            editFormSelector: '#qualifyTestQuestionsEditWindow',
            storeName: 'dict.qualifytest.QualifyTestQuestions',
            modelName: 'dict.qualifytest.QualifyTestQuestions',
            editWindowView: 'dict.qualifytest.QualifyTestQuestionsEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             //   typeKNDDictId = record.getId();
             //   asp.controller.setTypeKNDDictId(typeKNDDictId);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    questionId = record.getId();
                    var grid = form.down('qtestdictanswersgrid'),
                    store = grid.getStore();
                    store.filter('questionId', record.getId());
                    asp.controller.setTypeKNDDictId(questionId);
                }
               
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'QualifyTestQuestionsAnswersGridAspect',
            storeName: 'dict.qualifytest.QualifyTestQuestionsAnswers',
            modelName: 'dict.qualifytest.QualifyTestQuestionsAnswers',
            gridSelector: 'qtestdictanswersgrid',
            listeners: {
                beforesave: function (asp, store) {
                    var me = this;
                    if (questionId != null) {
                        Ext.each(store.data.items, function (rec) {
                            if (!rec.get('Id')) {
                                rec.set('QualifyTestQuestions', questionId);
                            }
                        });
                    }
                }
            }
        }
         
    ],

    mainView: 'dict.qualifytest.QualifyTestQuestionsGrid',
    mainViewSelector: 'kindknddictgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'qtestdictquestionsgrid'
        },
        {
            ref: 'qualifyTestQuestionsAnswersGrid',
            selector: 'qtestdictanswersgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    qualifyTestQuestionsEditWindowSelector: '#qualifyTestQuestionsEditWindow',

    index: function () {
        var view = this.getMainView() || Ext.widget('qtestdictquestionsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.qualifytest.QualifyTestQuestions').load();
    },

    setTypeKNDDictId: function (id) {
        this.questionId = id;

        var editWindow = Ext.ComponentQuery.query(this.qualifyTestQuestionsEditWindowSelector)[0];

        if (id > 0) {
            editWindow.down('.tabpanel').setDisabled(false);
        } else {
            editWindow.down('.tabpanel').setDisabled(true);
        }
    },
});