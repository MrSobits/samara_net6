Ext.define('B4.controller.riskorientedmethod.KindKNDDict', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
         'B4.aspects.GkhInlineGridMultiSelectWindow',
    ],

    typeKNDDictId: null,

    models: [
        'riskorientedmethod.KindKNDDict',
        'riskorientedmethod.KindKNDDictArtLaw'
    ],
    stores: [
        'riskorientedmethod.KindKNDDict',
        'riskorientedmethod.KindKNDDictArtLaw',
        'riskorientedmethod.KindKNDDictArtLawForSelect',
        'riskorientedmethod.KindKNDDictArtLawForSelected'
    ],
    views: [

        'riskorientedmethod.KindKNDDictGrid',
        'riskorientedmethod.KindKNDDictArtLawGrid',
        'riskorientedmethod.KindKNDDictEditWindow'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'kindKNDDictGridAspect',
            gridSelector: 'kindknddictgrid',
            editFormSelector: '#kindKNDDictEditWindow',
            storeName: 'riskorientedmethod.KindKNDDict',
            modelName: 'riskorientedmethod.KindKNDDict',
            editWindowView: 'riskorientedmethod.KindKNDDictEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             //   typeKNDDictId = record.getId();
             //   asp.controller.setTypeKNDDictId(typeKNDDictId);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    typeKNDDictId = record.getId();
                    var grid = form.down('kindknddictartlawgrid'),
                    store = grid.getStore();
                    store.filter('parentId', record.getId());
                    asp.controller.setTypeKNDDictId(typeKNDDictId);
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
         {
             /* 
             Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
             По нажатию на Добавить открывается форма выбора предоставляемых документов.
             По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
             И сохранение предоставляемых документов
             */
             xtype: 'gkhinlinegridmultiselectwindowaspect',
             name: 'kindKNDDictArtLawAspect',
             gridSelector: 'kindknddictartlawgrid',
             storeName: 'riskorientedmethod.KindKNDDictArtLaw',
             modelName: 'riskorientedmethod.KindKNDDictArtLaw',
             multiSelectWindow: 'SelectWindow.MultiSelectWindow',
             multiSelectWindowSelector: '#kindKNDDictArtLawForSelectedMultiSelectWindow',
             storeSelect: 'riskorientedmethod.KindKNDDictArtLawForSelect',
             storeSelected: 'riskorientedmethod.KindKNDDictArtLawForSelected',
             titleSelectWindow: 'Выбор статей закона',
             titleGridSelect: 'Статьи закона',
             titleGridSelected: 'Выбранные статьи закона',
             columnsGridSelect: [
                 { header: 'Код записи', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } },
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
             ],
             columnsGridSelected: [
                 { header: 'Код записи', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, sortable: false },
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
             ],
             onBeforeLoad: function (store, operation) {
                 operation.params.typeKNDId = typeKNDDictId;
             },
             listeners: {
                 getdata: function (me, records) {
                     var store = me.controller.getStore(me.storeName);
                   //  asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                     records.each(function (rec) {
                         if (rec.get('Id')) {
                             var recordArtLaw = me.controller.getModel('riskorientedmethod.KindKNDDictArtLaw').create();
                             recordArtLaw.set('KindKNDDict', typeKNDDictId);
                             recordArtLaw.set('ArticleLawGji', rec.get('Id'));
                             recordArtLaw.set('Name', rec.get('Name'));

                             store.insert(0, recordArtLaw);
                         }
                     });
                 //    asp.controller.unmask();
                     return true;
                 }
                 //getdata: function (asp, records) {
                 //    var recordIds = [];
                 //    records.each(function (rec, index) {
                 //        recordIds.push(rec.get('Id'));
                 //    });

                 //    if (recordIds[0] > 0) {
                 //        debugger;
                 //        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                 //        debugger;
                 //        B4.Ajax.request(B4.Url.action('AddArticleLaw', 'KindKNDDictArtLaw', {
                 //            artLawIds: recordIds,
                 //            parentId: typeKNDDictId
                 //        })).next(function (response) {
                 //            asp.controller.unmask();
                 //            asp.controller.getStore(asp.storeName).load();
                 //            return true;
                 //        }).error(function () {
                 //            asp.controller.unmask();
                 //        });
                 //    } else {
                 //        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать статью закона');
                 //        return false;
                 //    }
                 //    return true;
                 //}
             }
         },
    ],

    mainView: 'riskorientedmethod.KindKNDDictGrid',
    mainViewSelector: 'kindknddictgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'kindknddictgrid'
        },
        {
            ref: 'kindKNDDictArtLawGrid',
            selector: 'kindknddictartlawgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    kindKNDDictEditWindowSelector: '#kindKNDDictEditWindow',

    index: function () {
        var view = this.getMainView() || Ext.widget('kindknddictgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('riskorientedmethod.KindKNDDict').load();
    },

    setTypeKNDDictId: function (id) {
        this.typeSurveyId = id;

        var editWindow = Ext.ComponentQuery.query(this.kindKNDDictEditWindowSelector)[0];

        if (id > 0) {
            editWindow.down('.tabpanel').setDisabled(false);
        } else {
            editWindow.down('.tabpanel').setDisabled(true);
        }
    },
});