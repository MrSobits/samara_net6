Ext.define('B4.controller.constructionobject.Participant', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.permission.constructionobject.Participant'
    ],

    views: [
        'constructionobject.participant.Grid',
        'constructionobject.participant.EditWindow'
    ],

    models: ['constructionobject.Participant'],

    stores: ['constructionobject.Participant'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'constructionobject.participant.Grid',
    mainViewSelector: 'constructionobjectparticipantgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjectparticipantgrid'
        }
    ],

    aspects: [
        {
            xtype: 'constructionobjectparticipantpermission',
            name: 'participantPermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'constructobjParticipantGridWindowAspect',
            gridSelector: 'constructionobjectparticipantgrid',
            editFormSelector: 'constructobjparticipanteditwindow',
            modelName: 'constructionobject.Participant',
            editWindowView: 'constructionobject.participant.EditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' [name=ParticipantType]'] = { 'change': { fn: me.onChangeParticipantType, scope: me } };
                actions[me.editFormSelector + ' [name=Contragent]'] = { 'change': { fn: me.onChangeContragent, scope: me } };
            },
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    record.set('ConstructionObject', me.controller.getContextValue(me.controller.getMainView(), 'constructionObjectId'));
                }
            },
            onChangeParticipantType: function(field, newValue) {
                var me = this,
                    isCustomerType = (newValue === B4.enums.ConstructionObjectParticipantType.Customer),
                    customerTypeField = me.componentQuery(me.editFormSelector + ' [name=CustomerType]');

                customerTypeField.setValue('');
                customerTypeField.setVisible(isCustomerType);
                customerTypeField.allowBlank = !isCustomerType;
            },
            onChangeContragent: function(field, newValue) {
                var me = this,
                    contragent = newValue,
                    innField = me.componentQuery(me.editFormSelector + ' [name=ContragentInn]'),
                    nameField = me.componentQuery(me.editFormSelector + ' [name=ContragentContactName]'),
                    phoneField = me.componentQuery(me.editFormSelector + ' [name=ContragentContactPhone]');

                nameField.setValue('');
                phoneField.setValue('');
                innField.setValue('');

                if (contragent) {
                    B4.Ajax.request({
                        method: 'GET',
                        url: B4.Url.action('GetActualManagerContact', 'Contragent'),
                        params: {
                            contragentId: contragent.Id
                        }
                    }).next(function (response) {
                        if (response) {
                            var contact = Ext.JSON.decode(response.responseText);
                            if (contact) {
                                nameField.setValue(contact.FullName);
                                phoneField.setValue(contact.Phone);
                            }

                            innField.setValue(contragent.Inn);
                        }
                    }).error(function() {
                    });
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjectparticipantgrid');

        me.bindContext(view);
        me.setContextValue(view, 'constructionObjectId', id);
        me.application.deployView(view, 'construction_object_info');
        me.getAspect('participantPermissionAspect').setPermissionsByRecord({ getId: function () { return id; } });

        view.getStore().load();
    },

    init: function () {
        var me = this,
            actions = {};

        actions[this.mainViewSelector] = {
            'store.beforeload': {
                fn: me.onBeforeLoad,
                scope: me
            }
        };

        me.control(actions);
        me.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'constructionObjectId');
    }
});