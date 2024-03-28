Ext.define('B4.controller.config.PaymentSourceTree', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'config.PaymentSourceConfig',
        'config.ExtPaymentSourceConfig'
    ],
    stores: [
        'config.PaymentSourceConfig'
    ],

    views: [
        'config.PaymentSourceTreeGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'config.PaymentSourceTreeGrid',
    mainViewSelector: 'paymentsourcetreegrid',

    refs: [
        {
            ref: 'SourcesTreePanel',
            selector: '#sourcesConfig'
        }
    ],

    modifiedNodes: [],

    init: function () {
        var me = this;

        this.control({
            '#sourcesConfig': {
                'checkchange': function (node, checked) {
                    me.modifiedNodes = [];

                    me.checkChildren(node, checked);
                    me.walkThroughParents(node);                  

                    me.saveSources(checked);
                    me.loadTree();
                }
            }
        });

        me.loadTree();
    },

    loadTree: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('paymentsourcetreegrid'),
            panel = view.down('treepanel');

        me.mask(panel);

        Ext.Ajax.request({
            method: 'GET',
            url: B4.Url.action('GetSourceConfigs', 'PaymentDocSource'),
            success: function (response) {
                var dataTree = Ext.JSON.decode(response.responseText);
                panel.setRootNode(dataTree);

                me.unmask(panel);
            },
            failure: function (response) {
                var obj = Ext.decode(response.responseText);

                me.unmask(panel);
                Ext.Msg.alert('Ошибка', obj.message || 'Не удалось загрузить данные');
            }
        });
    },

    checkChildren: function (node, check) {
        var me = this;

        node.cascadeBy(function (n) {
            if (me.checkSave(n, check)) {
                me.modifiedNodes.push(n);
            }            
        });
    },

    walkThroughParents: function () {
        this.checkParentNodes();
    },

    checkParentNodes: function (beginWith) {
        var me = this,
            tree = this.getSourcesTreePanel(),
            rootNode = beginWith || tree.getRootNode();

        var fn = function (node) {
            if (me.allChildrenAreChecked(node)) {
                node.set('checked', true);
            }
            else {
                if (node.isLeaf()) {
                    node.set('checked', node.data.checked === true);
                }
                else {
                    node.set('checked', false);
                }
            }
            for (var i = 0; i < node.childNodes.length; ++i) {
                fn(node.childNodes[i]);
            }
        };
        fn(rootNode);
    },

    //Проверяет, отмечены ли все дочерние узлы
    allChildrenAreChecked: function(node) {
        var allChildrenAreChecked = false;
        var fn = function(child) {
            allChildrenAreChecked = child.data.checked === true;

            if (child.isLeaf()) {
                return allChildrenAreChecked;
            }

            for (var i = 0; i < child.childNodes.length; ++i) {
                if (fn(child.childNodes[i]) === false) {
                    return false;
                }
            }
        };

        fn(node);

        return allChildrenAreChecked;
    },

    checkSave: function(node, checked) {
        return (node.get('leaf') && node.get('Enabled') !== checked);
    },

    saveSources: function (checked) {
        var me = this,
            data = [];


        var fn = function(n) {
            n.set('Enabled', checked);
            data.push(new B4.model.config.PaymentSourceConfig(n.getData()).getData());
        };

        me.modifiedNodes.forEach(fn);

        Ext.Ajax.request({
            url: B4.Url.action('/PaymentDocSource/SaveSources'),
            params: {
                sources: Ext.encode(data)
            },
            failure: function() {
                Ext.Msg.alert('Ошибка', 'При сохранении произошла ошибка!');
            }
        });
        
    }

});