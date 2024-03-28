Ext.define('B4.controller.claimwork.EditBuildContract', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocCreateButton'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'claimwork.BuildContract',
        'objectcr.BuildContract',
        'ObjectCr'
    ],
    
    views: [
        'claimwork.buildcontract.EditPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'buildcontracteditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.BuildContract.Update', applyTo: 'button[actionName=updState]', selector: 'buildcontracteditpanel'
                },
                {
                    name: 'Clw.ClaimWork.BuildContract.Save', applyTo: 'b4savebutton', selector: 'buildcontracteditpanel'
                }
            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'builderClaimWorkCreateButtonAspect',
            buttonSelector: 'buildcontracteditpanel acceptmenubutton',
            containerSelector: 'buildcontracteditpanel',
            createDocument: function (params) {
                var me = this,
                    data,
                    container = me.componentQuery(me.containerSelector);

                me.controller.mask('Формирование документа', container);

                B4.Ajax.request({
                    url: B4.Url.action('CreateDocument', 'ClaimWorkDocument'),
                    method: 'POST',
                    timeout: 9999999,
                    params: params
                }).next(function (res) {
                    data = Ext.decode(res.responseText);

                    me.fireEvent('createsuccess', me);

                    Ext.History.add(Ext.String.format("claimworkbc/BuildContractClaimWork/{0}/{1}/{2}/aftercreatedoc", params.claimWorkId, data.Id, params.actionUrl));

                    me.controller.unmask();
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка формирования документа!', e.message || e);
                });
            },
        },
        {
            xtype: 'gkheditpanel',
            name: 'buildContractEditPanelAspect',
            editPanelSelector: 'buildcontracteditpanel',
            modelName: 'claimwork.BuildContract',
            otherActions: function (actions) {
                var me = this;
                actions['buildcontracteditpanel button[actionName=updState]'] = { 'click': { fn: me.updState, scope: me } };
                
                actions['buildcontracteditpanel button[action=goContract]'] = {
                    'click': { fn: me.goContract, scope: me }
                };
            },
            goContract: function () {
                // Делаю открытие карточки именно так поскольку старый контроллер не переписан на роуты
                var me = this,
                    record = me.getRecord();

                if (record.getId() && record.get('ObjCrId')) {
                    Ext.History.add('objectcredit/' + record.get('ObjCrId') + '/contractcr');
                }
            },
            listeners: {
                'aftersetpaneldata': function (asp, rec, panel) {
                    var violGrid = panel.down('buildcontractviolgrid'),
                        violStore = violGrid.getStore();
                    
                    violStore.clearFilter(true);
                    violStore.filter('builderId', rec.getId());
                }
            },
            updState: function () {
                var me = this,
                    id = me.getRecord().getId();
                
                me.controller.mask("Обновление");
                B4.Ajax.request({
                    url: B4.Url.action('UpdateStates', 'BuildContractClaimWork', { id: me.getRecord().getId() }),
                    timeout: 999999
                }).next(function (response) {
                    me.controller.unmask();
                    var json = Ext.JSON.decode(response.responseText);
                    if (json.success !== true) {
                        B4.QuickMsg.msg('Внимание!', json.message, 'error');
                    } else {
                        me.setData(id);
                    }
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка!', e.message || 'Ошибка обновления статусов');
                });
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('buildcontracteditpanel');

        me.bindContext(view);
        
        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', 'BuildContractClaimWork');
        me.application.deployView(view, 'claim_work_bc');

        me.getAspect('buildContractEditPanelAspect').setData(id);
        me.getAspect('builderClaimWorkCreateButtonAspect').setData(id, 'BuildContractClaimWork');
    }
});