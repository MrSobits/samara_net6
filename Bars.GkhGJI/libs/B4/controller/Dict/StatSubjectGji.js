Ext.define('B4.controller.dict.StatSubjectGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'dict.StatSubjectGji',
        'dict.StatSubsubjectGji'
    ],
    stores: [
        'dict.StatSubjectGji',
        'dict.statsubsubjectgji.Select',
        'dict.statsubsubjectgji.Selected',
        'dict.statsubjectgji.Subsubject'
    ],

    views: [
        'dict.statsubjectgji.Grid',
        'dict.statsubjectgji.EditWindow',
        'dict.statsubjectgji.SubsubjectGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'dict.statsubjectgji.Grid',
    mainViewSelector: 'statSubjectGjiGrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'statementSubjectGjiGridAspect',
            gridSelector: 'statSubjectGjiGrid',
            editFormSelector: '#statSubjectGjiEditWindow',
            storeName: 'dict.StatSubjectGji',
            modelName: 'dict.StatSubjectGji',
            editWindowView: 'dict.statsubjectgji.EditWindow',
            onSaveSuccess: function(me, rec) {
                me.controller.setCurrentId(rec.get('Id'));
            },
            listeners: {
                aftersetformdata: function(me, rec) {
                    me.controller.setCurrentId(rec.get('Id'));
                }
            }
        },
        {
            /**
            * аспект множественного выбора подтематик
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'subjectSubsubjectGridAspect',
            gridSelector: '#statSubjectSubsubjectGrid',
            storeName: 'dict.statsubjectgji.Subsubject',
            modelName: 'dict.StatSubsubjectGji',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#statSubjectSubsMultiSelectWindow',
            storeSelect: 'dict.statsubsubjectgji.Select',
            storeSelected: 'dict.statsubsubjectgji.Selected',
            titleSelectWindow: 'Выбор подтематик',
            titleGridSelect: 'Подтематики для отбора',
            titleGridSelected: 'Выбранные подтематики',
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
                        B4.Ajax.request(B4.Url.action('AddSubsubject', 'StatSubjectGji', {
                            objectIds: ids,
                            subjectId: me.controller.subjectId
                        })).next(function (response) {
                            me.controller.unmask();
                            me.controller.getStore('dict.statsubjectgji.Subsubject').load();
                            return true;
                        }).error(function () {
                            me.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать подтематики!');
                    }
                }
            }
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    statSubjectGjiEditWindowSelector: '#statSubjectGjiEditWindow',
    
    refs: [
        {
            ref: 'mainView',
            selector: 'statSubjectGjiGrid'
        }
    ],

    init: function() {
        this.getStore('dict.statsubjectgji.Subsubject').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('statSubjectGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.StatSubjectGji').load();
    },

    setCurrentId: function(id) {
        this.subjectId = id;

        var editWindow = Ext.ComponentQuery.query(this.statSubjectGjiEditWindowSelector)[0]; 

        editWindow.down('#statSubjectSubsubjectGrid').setDisabled(!id);

        var store = this.getStore('dict.statsubjectgji.Subsubject');
        
        if (id)
            store.load();
        else
            store.removeAll();
    },
    
    onBeforeLoad: function(store, operation) {
        operation.params.subjectId = this.subjectId;
    }
});