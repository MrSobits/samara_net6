/* 
* Контроллер формы редактора рейтинга эффективности УО
*/
Ext.define('B4.controller.efficiencyrating.Constructor',
{
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.metavalueconstructor.Constructor',
        'B4.enums.efficiencyrating.DataMetaObjectType',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    views: ['efficiencyrating.Panel', 'efficiencyrating.CopyConstructorWindow'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'efficiencyrating.Panel',
    mainViewSelector: 'efficiencyRatingPanel',

    aspects: [
        {
            xtype: 'metavalueconstructoraspect',
            name: 'EfConstructorAspect',
            editFormSelector: 'efFactorFormPanel',
            treeSelector: 'efFactorTreePanel',
            editPanelSelector: 'container[name=efMain]',
            editElementSelector: 'efAttributeFormPanel',
            treePanelSelector: 'efAttributeTreePanel',
            treeEditPanelSelector: 'efAttributeFormPanel',
            treeSelectFieldSelector: 'efAttributeFormPanel treeselectfield[name=DataFillerName]',
            defaultCode: 'AAAaaa',
            levels: [
                { level: 0, name: 'фактор', editFormSelector: 'efFactorFormPanel' },
                { level: 1, name: 'коэффициент', editFormSelector: 'efAttributeFormPanel' },
                { level: 2, name: 'атрибут', editFormSelector: 'efAttributeFormPanel' }
            ],

            listeners: {
                aftersetformdata: function(aspect, rec, form) {
                    var me = this,
                        tbCodeCoef = form.down('textfield[name=CodeCoef]');
                    if (rec.get('Level') === 2) {
                        tbCodeCoef.setValue(me.getSelected(1).get('Code'));
                    }
                },

                initiated: function (asp) {
                    asp.controller.control({
                        'efficiencyRatingPanel b4selectfield[name=EfficiencyRatingPeriod]': {
                            'change': {
                                fn: function(sf, newVal) {
                                    asp.fireEvent('onchangegroup', newVal ? newVal.Group.Id : null);
                                },
                                scope: this
                            }
                        }
                    });
                }
            },

            getGroupId: function() {
                var raw = this.controller.getMainView().down('b4selectfield[name=EfficiencyRatingPeriod]').value;
                return raw ? raw.Group.Id : null;
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.View', applyTo: 'b4selectfield[name=EfficiencyRatingPeriod]', selector: 'efficiencyRatingPanel' },

               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.Edit', applyTo: 'b4savebutton', selector: 'efAttributeFormPanel' },
               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.Edit', applyTo: 'b4savebutton', selector: 'efFactorFormPanel' },

               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.Delete', applyTo: 'b4deletebutton', selector: 'efAttributeTreePanel' },
               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.Delete', applyTo: 'b4deletebutton', selector: 'efFactorTreePanel' },

               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.Create', applyTo: 'b4addbutton', selector: 'efAttributeTreePanel' },
               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.Create', applyTo: 'b4addbutton', selector: 'efFactorTreePanel' },
            
               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.Code', applyTo: 'textfield[name=Code]', selector: 'efAttributeFormPanel' },
               { name: 'Gkh.Orgs.EfficiencyRating.Constructor.Code', applyTo: 'textfield[name=Code]', selector: 'efFactorFormPanel' }
            ]
        }
    ],

    copyConstructor: function() {
        var me = this,
            currPeriod = me.getMainView().down('b4selectfield[name=EfficiencyRatingPeriod]').value,
            win;

        if (!currPeriod) {
            Ext.Msg.alert('Ошибка!', 'Не выбран текущий период');
            return;
        }

        win = me.getView('efficiencyrating.CopyConstructorWindow').create();
        win.down('b4selectfield[name=EfficiencyRatingPeriodTo]').setValue(currPeriod);
        win.down('b4savebutton').on('click', function(){ me.processCopy(win, currPeriod) }, me);
        win.getForm().isValid();

        win.down('b4selectfield[name=EfficiencyRatingPeriodFrom]').getStore().on('beforeload', function(store, operation) {
            operation.params.excludedIds = Ext.encode([currPeriod.Id]);
        }, me);

        win.show();
    },

    processCopy: function(win, currPeriod) {
        var me = this,
            periodForm = win.down('b4selectfield[name=EfficiencyRatingPeriodFrom]').value;

        if (!win.getForm().isValid()) {
            Ext.Msg.alert('Ошибка!', 'Выберите период, из которого производится копирование');
            return;
        }

        me.mask('Копирование...', win);
        Ext.Ajax.request({
            url: B4.Url.action('CopyConstructor', 'DataMetaInfo'),
            params: {
                groupFromId: periodForm.Group.Id,
                groupToId: currPeriod.Group.Id
            },
            success: function (response) {
                me.unmask();
                win.close();

                me.getAspect('EfConstructorAspect').onChangeGroup(currPeriod.Group.Id);
            },
            failure: function (response) {
                var obj = Ext.decode(response.responseText);
                me.unmask();
                Ext.Msg.alert('Ошибка', obj.message || 'Не удалось загрузить данные');
            }
        });
    },

    init: function () {
        this.callParent(arguments);

        this.control({
            'efficiencyRatingPanel button[actionName=copyconstructor]': {
                'click': {
                    fn: this.copyConstructor,
                    scope: this
                }
            }
        });
    },

    index: function() {
        var me = this,
           view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);
    }
});