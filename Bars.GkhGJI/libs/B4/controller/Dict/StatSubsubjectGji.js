Ext.define('B4.controller.dict.StatsubsubjectGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'dict.StatSubsubjectGji',
        'dict.FeatureViolGji',
        'dict.StatSubjectGji'
    ],
    
    stores: [
        'dict.StatSubsubjectGji',
        'dict.statsubsubjectgji.Feature',
        'dict.statsubsubjectgji.Subject',
        'dict.FeatureViolGjiForSelect',
        'dict.FeatureViolGjiForSelected',
        'dict.statsubjectgji.Select',
        'dict.statsubjectgji.Selected'
    ],
    
    views: [
        'dict.statsubsubjectgji.EditWindow',
        'dict.statsubsubjectgji.Grid',
        'dict.statsubsubjectgji.FeatureGrid',
        'dict.statsubsubjectgji.SubjectGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'dict.statsubsubjectgji.Grid',
    mainViewSelector: 'statSubsubjectGjiGrid',

    aspects: [
        {
            /**
            * Аспект взаимодействия грида подтематик обращений и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'statSubsubjectGjiGridWindowAspect',
            gridSelector: 'statSubsubjectGjiGrid',
            editFormSelector: '#statSubsubjectGjiEditWindow',
            storeName: 'dict.StatSubsubjectGji',
            modelName: 'dict.StatSubsubjectGji',
            editWindowView: 'dict.statsubsubjectgji.EditWindow',
            onSaveSuccess: function(me, rec) {
                me.controller.setCurrentId(rec.get('Id'));
            },
            listeners: {
                aftersetformdata: function (me, rec) {
                    me.controller.setCurrentId(rec.get('Id'));
                }
            }
        },
        {
            /**
            * аспект множественного выбора характеристик
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'featureMultiselect',
            gridSelector: '#statSubsubjectFeatureGrid',
            storeName: 'dict.statsubsubjectgji.Feature',
            modelName: 'dict.FeatureViolGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#statSubsubjectFeatMultiSelectWindow',
            storeSelect: 'dict.FeatureViolGjiForSelect',
            storeSelected: 'dict.FeatureViolGjiForSelected',
            titleSelectWindow: 'Выбор характеристик',
            titleGridSelect: 'Характеристики для отбора',
            titleGridSelected: 'Выбранные характеристики',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function(me, records) {
                    var ids = [];

                    Ext.each(records.items, function(rec) {
                        ids.push(rec.get('Id'));
                    });
                    
                    if (ids[0]) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddFeature', 'StatSubsubjectGji', {
                            objectIds: ids,
                            subsubjectId: me.controller.subsubjectId
                        })).next(function(response) {
                            me.controller.unmask();
                            me.controller.getStore('dict.statsubsubjectgji.Feature').load();
                            return true;
                        }).error(function() {
                            me.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать характеристики!');
                    }
                }
            }
        },
        {
            /**
            * аспект множественного выбора тематик
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'subjectMultiselect',
            gridSelector: '#statSubsubjectSubjectGrid',
            storeName: 'dict.statsubsubjectgji.Subject',
            modelName: 'dict.StatSubjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#statSubsubjectSubjMultiSelectWindow',
            storeSelect: 'dict.statsubjectgji.Select',
            storeSelected: 'dict.statsubjectgji.Selected',
            titleSelectWindow: 'Выбор тематик',
            titleGridSelect: 'Тематики для отбора',
            titleGridSelected: 'Выбранные тематики',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (me, records) {
                    var ids = [];

                    Ext.each(records.items, function (rec) {
                        ids.push(rec.get('Id'));
                    });

                    if (ids[0]) {
                        me.controller.mask('Сохранение', me.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddSubject', 'StatSubsubjectGji', {
                            objectIds: ids,
                            subsubjectId: me.controller.subsubjectId
                        })).next(function (response) {
                            me.controller.unmask();
                            me.controller.getStore('dict.statsubsubjectgji.Subject').load();
                            return true;
                        }).error(function () {
                            me.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать тематики!');
                    }
                }
            }
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    statSubsubjectGjiEditWindowSelector: '#statSubsubjectGjiEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'statSubsubjectGjiGrid'
        }
    ],

    init: function() {
        this.getStore('dict.statsubsubjectgji.Feature').on('beforeload', this.onBeforeLoad, this);
        this.getStore('dict.statsubsubjectgji.Subject').on('beforeload', this.onBeforeLoad, this);
        
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('statSubsubjectGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.StatSubsubjectGji').load();
    },
    
    onBeforeLoad: function(store, operation) {
        operation.params.subsubjectId = this.subsubjectId;
    },
    
    setCurrentId: function(id) {
        this.subsubjectId = id;

        var editWindow = Ext.ComponentQuery.query(this.statSubsubjectGjiEditWindowSelector)[0];

        editWindow.down('.tabpanel').setDisabled(!id);

        if (id) {
            this.getStore('dict.statsubsubjectgji.Feature').load();
            this.getStore('dict.statsubsubjectgji.Subject').load();
        } else {
            this.getStore('dict.statsubsubjectgji.Feature').removeAll();
            this.getStore('dict.statsubsubjectgji.Subject').removeAll();
        }
    }
});