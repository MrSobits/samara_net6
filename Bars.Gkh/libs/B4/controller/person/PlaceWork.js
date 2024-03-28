Ext.define('B4.controller.person.PlaceWork', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'Person',
        'person.PlaceWork'
    ],
    
    stores: [
        'person.PlaceWork'
    ],
    
    views: [
        'person.PlaceWorkGrid',
        'person.PlaceWorkEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'personplaceworkgrid'
        }
    ],

    mainView: 'person.PlaceWorkGrid',
    mainViewSelector: 'personplaceworkgrid',
    
    parentCtrlCls: 'B4.controller.person.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'personPlaceWorkStatePermAspect',
            permissions: [
                { name: 'Gkh.Person.PersonPlaceWork.Create', applyTo: 'b4addbutton', selector: 'personplaceworkgrid' },
                { name: 'Gkh.Person.PersonPlaceWork.Edit', applyTo: 'b4savebutton', selector: 'personplaceworkeditwindow' }
                
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'personPlaceWorkDeleteStatePermAspect',
            permissions: [
                { name: 'Gkh.Person.PersonPlaceWork.Delete' }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'personPlaceWorkGridWindowAspect',
            gridSelector: 'personplaceworkgrid',
            editFormSelector: 'personplaceworkeditwindow',
            modelName: 'person.PlaceWork',
            editWindowView: 'person.PlaceWorkEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.data.Id) {
                        record.data.Person = me.controller.getContextValue(me.controller.getMainComponent(), 'personId');
                    }
                },
                aftersetformdata: function (me, record) {
                    var personId = me.controller.getContextValue(me.controller.getMainView(), 'personId');
                    me.controller.getAspect('personPlaceWorkStatePermAspect').setPermissionsByRecord({ getId: function () { return personId; } });
                }
            },
            deleteRecord: function (rec) {
                // проверка удаления записи Квал Аттестата происходит по статусу Должностного лицв. Внимание
                var me = this,
                    personId = me.controller.getContextValue(me.controller.getMainView(), 'personId'),
                    modelPerson = me.controller.getModel('Person');

                modelPerson.load(personId, {
                    success: function (record) {

                        me.controller.getAspect('personPlaceWorkDeleteStatePermAspect').loadPermissions(record)
                        .next(function (response) {
                            var grants = Ext.decode(response.responseText);
                            if (grants && grants[0]) {
                                grants = grants[0];
                            }
                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе должностного лица запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = me.getModel(rec);
                                        var newRec = new model({ Id: rec.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        newRec.destroy()
                                            .next(function () {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
                                                me.unmask();
                                            }, me)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, me);
                                    }
                                }, me);
                            }
                        }, me);


                    },
                    scope: me
                });

            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['personplaceworkeditwindow b4selectfield[name=Contragent]'] = { 'change': { fn: this.onChangeContragent, scope: this } };
        
        this.control(actions);
        
        me.callParent(arguments);
    },

    onChangeContragent:function(cmp, newValue) {
        var view = cmp.up('personplaceworkeditwindow'),
            fldState = view.down('[name=LicenzeState]');
        
        B4.Ajax.request({
            url: B4.Url.action('GetStateInfo', 'ManOrgLicense'),
            method: 'POST',
            params: { contragentId: (newValue && newValue.Id) || 0 }
        }).next(function (response) {
            var data = Ext.decode(response.responseText);

            fldState.setValue(data.stateName);
        });

    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('personplaceworkgrid'),
            store;

        me.bindContext(view);
        me.setContextValue(view, 'personId', id);
        me.application.deployView(view, 'person_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('personId', id);
        
        me.getAspect('personPlaceWorkStatePermAspect').setPermissionsByRecord({ getId: function () { return id; } });
    }
});