Ext.define('B4.controller.workscr.Protocol', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.typeworkcr.Protocol',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['objectcr.Protocol'],
    
    stores: ['objectcr.Protocol'],
    
    views: [
        'objectcr.ProtocolGrid',
        'objectcr.ProtocolEditWindow'
    ],

    mainView: 'objectcr.ProtocolGrid',
    mainViewSelector: 'objectcrprotocolgrid',

    aspects: [
        {
            xtype: 'protocoltypeworkcrperm',
            name: 'protocolTypeWorkCrPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'editWindowAspect',
            gridSelector: 'objectcrprotocolgrid',
            editFormSelector: 'objectcrprotocolwin',
            storeName: 'objectcr.Protocol',
            modelName: 'objectcr.Protocol',
            editWindowView: 'objectcr.ProtocolEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('TypeWork', asp.controller.getTypeWorkId());
                        record.set('ObjectCr', asp.controller.getObjectId());
                    }
                },
                beforesetformdata: function (asp, record) {
                    var form = asp.tryGetForm(),
                        storeTypeDocumentCr = form.down('#cbTypeDocumentCr').getStore();

                    storeTypeDocumentCr.filters.clear();
                    storeTypeDocumentCr.filter([
                        { property: "objectCrId", value: asp.controller.getObjectId() },
                        { property: "protocolId", value: record.getId() }
                    ]);
                },
                aftersetformdata: function (asp, record) {
                    var typeDocumentCr = record.get('TypeDocumentCr');

                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());

                    B4.Ajax.request(B4.Url.action('GetDates', 'ProtocolCr', {
                        objectCrId: asp.controller.getObjectId()
                    })).next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText),
                            form = asp.tryGetForm();
                        if (!form) {
                            asp.controller.unmask();
                            return;
                        }

                        asp.controller.DateStart = obj.DateStart;
                        asp.controller.DateEnd = obj.DateEnd;

                        form.down('#dfDateFrom').setMinValue(typeDocumentCr == 30 ? '01.01.1970' : obj.DateStart);
                        form.down('#dfDateFrom').setMaxValue(obj.DateEnd);
                        asp.controller.unmask();
                    }).error(function () {
                        asp.controller.unmask();
                    });
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #cbTypeDocumentCr'] = {
                    'change': { fn: me.changeTypeDocumentCr, scope: me }
                };

                actions[me.editFormSelector + ' #sfContragent'] = {
                    'beforeload': {
                        fn: function(store, operation) {
                            operation.params = operation.params || {};
                            operation.params.showAll = true;
                        }
                    }
                };
            },
            tryGetForm: function () {
                var me = this;
                return me.componentQuery(me.editFormSelector);
            },
            changeTypeDocumentCr: function (newValue) {
                var me = this,
                    form = me.getForm(),
                    nfCountVote = form.down('#nfCountVote'),
                    nfCountVoteGeneral = form.down('#nfCountVoteGeneral'),
                    nfCountAccept = form.down('#nfCountAccept'),
                    nfGradeClient = form.down('#nfGradeClient'),
                    nfGradeOccupant = form.down('#nfGradeOccupant'),
                    nfSumActVerificationOfCosts = form.down('#nfSumActVerificationOfCosts'),
                    isDisabled = newValue.value === 70,
                    minDate = newValue.value === 30 ? '01.01.1970' : me.controller.DateStart,
                    disabledSum = !isDisabled;

                nfCountVote.setDisabled(isDisabled);
                nfCountVoteGeneral.setDisabled(isDisabled);
                nfCountAccept.setDisabled(isDisabled);
                nfGradeClient.setDisabled(isDisabled);
                nfGradeOccupant.setDisabled(isDisabled);

                form.down('#dfDateFrom').setMinValue(minDate);

                if (disabledSum == false) {
                    me.setSumActVerificationOfCosts(nfSumActVerificationOfCosts);
                } else {
                    nfSumActVerificationOfCosts.setDisabled(disabledSum);
                    nfSumActVerificationOfCosts.allowBlank = disabledSum;
                }
            },
            setSumActVerificationOfCosts: function (nfSumActVerificationOfCosts) {
                var me = this;

                B4.Ajax.request(B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                    permissions: Ext.encode(['GkhCr.ObjectCr.Register.Protocol.SumActVerificationOfCosts']),
                    ids: Ext.encode([me.controller.params.get('Id')])
                })).next(function (response) {
                    me.controller.unmask();
                    var perm = Ext.decode(response.responseText)[0];
                    nfSumActVerificationOfCosts.setDisabled(!perm[0]);
                    nfSumActVerificationOfCosts.allowBlank = !perm[0];
                }).error(function () {
                    me.controller.unmask();
                });
            }
        }
    ],

    index: function (id, objectId) {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('objectcrprotocolgrid');
            
            view.getStore().on('beforeload',
                function (s, operation) {
                    operation.params.twId = id;
                    operation.params.objectCrId = objectId;
                }, me);
        }

        me.bindContext(view);
        me.setContextValue(view, 'twId', id);
        me.setContextValue(view, 'objectId', objectId);
        me.application.deployView(view, 'works_cr_info');

        view.getStore().load();

        me.getAspect('protocolTypeWorkCrPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    getTypeWorkId: function () {
        var me = this;
        return me.getContextValue(me.getMainView(), 'twId');
    },

    getObjectId: function () {
        var me = this;
        return me.getContextValue(me.getMainView(), 'objectId');
    }
});