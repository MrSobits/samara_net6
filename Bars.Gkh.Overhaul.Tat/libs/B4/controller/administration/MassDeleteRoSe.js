Ext.define('B4.controller.administration.MassDeleteRoSe', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax', 'B4.Url',
        'B4.mixins.Context',
        'B4.mixins.MaskBody'
    ],

    stores: [
        'administration.massdelete.RealityObject',
        'administration.massdelete.RealityObjectStructuralElement'
    ],

    views: [
        'administration.massdelete.Panel',
        'administration.massdelete.SelectGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainPanel', selector: 'massdeleterosepanel' },
        { ref: 'selectGrid', selector: 'massdeleterosepanel massdeleteroseselectgrid' },
        { ref: 'sfRobject', selector: 'massdeleterosepanel b4selectfield[name=RealityObject]' }
    ],

    index: function () {
        var view = this.getMainPanel() || Ext.widget('massdeleterosepanel');
        
        this.bindContext(view);
        this.application.deployView(view);
    },
    
    init: function () {
        var me = this,
            actions;

        actions = {
            'massdeleterosepanel': {
                afterrender: {
                    fn: function() {
                        me.getSelectGrid().getStore().on('beforeload', me.onBeforeLoadSelect, me);
                    },
                    scope: me
                }
            },
            'massdeleterosepanel b4selectfield[name=RealityObject]': {
                change: {
                    fn: me.realoadSelectGrid,
                    scope: me
                }
            },
            'massdeleterosepanel massdeleteroseselectgrid b4updatebutton': {
                click: {
                    fn: me.realoadSelectGrid,
                    scope: me
                }
            },
            'massdeleterosepanel massdeleteroseselectgrid button[action=DeleteStructElems]': {
                click: {
                    fn: function () {
                        var records, result, storeSelect, grid;
                        
                        try {
                            grid = me.getSelectGrid();
                            records = grid.selModel.getSelection();
                            result = [];
                            storeSelect = grid.getStore();

                            Ext.each(records, function(record) {
                                result.push(record.getId());
                            });

                            if (result.length > 0) {
                                me.mask('Удаление конструктивных элементов...', me.getMainPanel());
                                B4.Ajax.request({
                                    url: B4.Url.action('MassDelete', 'RealityObjectStructuralElement'),
                                    timeout: 999999,
                                    method: 'POST',
                                    params: {
                                        objectIds: Ext.encode(result)
                                    }
                                }).next(function(resp) {
                                    me.unmask();
                                    var obj = Ext.decode(resp.responseText);
                                    Ext.Msg.alert("Ошибка", obj.message ? obj.message : 'Удаление выполнено успешно!');
                                    storeSelect.load();
                                }).error(function(err) {
                                    me.unmask();
                                    storeSelect.load();
                                    Ext.Msg.alert("Ошибка", err.message ? err.message : 'Во время удаления произошла ошибка!');
                                });
                            } else {
                                B4.QuickMsg.msg('Предупреждение', 'Не выбраны записи для удаления', 'warning');
                            }
                        } catch(e) {
                            me.unmask();
                        }
                    },
                    scope: me
                }
            }
        };

        me.control(actions);

        me.callParent(arguments);
    },
    
    realoadSelectGrid: function() {
        this.getSelectGrid().getStore().load();
    },
    
    onBeforeLoadSelect: function(store, operation) {
        operation.params.objectId = this.getSfRobject().getValue();
    }
});