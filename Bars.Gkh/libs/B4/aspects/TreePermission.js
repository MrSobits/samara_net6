Ext.define('B4.aspects.TreePermission', {
    extend: 'B4.base.Aspect',
    alias: 'widget.treepermissionaspect',

    requires: [
        'B4.mixins.MaskBody'
    ],

    permissionPanelView: '',
    saveUrl: null,
    copyUrl: null,
    copyToRoleFromStore: false,

    _partiallyCheckedStyle: 'x-tree-checkbox-partially-checked',

    getPermissionsTreePanel: function () {
        return Ext.ComponentQuery.query(this.permissionPanelView + ' permissiontreepanel')[0];
    },

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function (controller) {
        var asp = this,
            actions = {};

        actions[asp.permissionPanelView + ' button[name=Update]'] = { 'click': { fn: asp.reloadPermissions, scope: asp } };
        actions[asp.permissionPanelView + ' button[name=Save]'] = { 'click': { fn: asp.savePermissions, scope: asp } };
        actions[asp.permissionPanelView + ' button[name=CollapseAll]'] = { 'click': { fn: asp.collapsePermissions, scope: asp } };
        actions[asp.permissionPanelView + ' button[name=ExpandAll]'] = { 'click': { fn: asp.expandPermissions, scope: asp } };
        actions[asp.permissionPanelView + ' button[name=MarkAll]'] = { 'click': { fn: asp.markAllPermissions, scope: asp } };
        actions[asp.permissionPanelView + ' button[name=UnmarkAll]'] = { 'click': { fn: asp.unmarkAllPermissions, scope: asp } };
        actions[asp.permissionPanelView + ' permissiontreepanel'] = {
            'checkchange': {
                fn: function (node, checked) {
                    asp.checkChildren(node, checked);
                    asp.walkThroughParents(node);
                },
                scope: asp
            },

            'render': {
                fn: function (tree) {
                    tree.getStore().on('load', function () { asp.checkParentNodes(); }, asp);
                },
                scope: asp
            }
        };
        actions[asp.permissionPanelView + ' b4searchfield[name=Search]'] = {
            'specialkey': {
                fn: function (self, e) {
                    if (e.getKey() == Ext.EventObject.ENTER) {
                        asp.onSearch(self.getValue());
                    }
                },
                scope: asp
            },
            'onclear': {
                fn: asp.onSeacrhClear,
                scope: asp
            },
        };

        asp.callParent(arguments);
        controller.control(actions);
    },

    savePermissions: function() {
        throw new Error("Not implemented");
    },

    reloadPermissions: function() {
        var asp = this,
            treePanel = asp.getPermissionsTreePanel(),
            store = treePanel.getStore();

        store.reload();
    },

    collectPermissionsData: function () {
        var asp = this,
            result = {},
            fn = function (node) {
                if (node.get('id') && !Ext.isEmpty(node.get('checked'))) {
                    result[node.get('id')] = node.get('checked');
                }
                node.eachChild(fn);
            };

        fn(asp.getPermissionsTreePanel().getRootNode());

        return result;
    },

    updateTreeView: function (tree, fn) {
        var view = tree.getView();
        view.getStore().loadRecords(fn(tree.getRootNode()));
        view.refresh();
    },

    collapseAll: function (tree) {
        this.updateTreeView(tree, function (root) {
            root.cascadeBy(function (node) {
                if (!node.isRoot() || tree.rootVisible) {
                    node.data.expanded = false;
                }
            });
            return tree.rootVisible ? [root] : root.childNodes;
        });
    },

    expandAll: function (tree) {
        var asp = this;

        asp.updateTreeView(tree, function (root) {
            var nodes = [];
            root.cascadeBy(function (node) {
                if (!node.isRoot() || tree.rootVisible) {
                    node.data.expanded = true;
                    nodes.push(node);
                }
            });
            return nodes;
        });
    },

    collapsePermissions: function () {
        var asp = this;

        asp.collapseAll(asp.getPermissionsTreePanel());
    },

    expandPermissions: function () {
        var asp = this;

        asp.expandAll(asp.getPermissionsTreePanel());
    },

    markAllPermissions: function () {
        var asp = this;
        asp.getPermissionsTreePanel().getRootNode().cascadeBy(function () {
            var node = this;
            if (!Ext.isEmpty(node.get('checked'))) {
                node.set('checked', true);
                asp.setPartiallyChecked(node, false);
            }
        });
    },

    unmarkAllPermissions: function () {
        var asp = this;
        asp.getPermissionsTreePanel().getRootNode().cascadeBy(function () {
            var node = this;
            if (!Ext.isEmpty(node.get('checked'))) {
                node.set('checked', false);
                asp.setPartiallyChecked(node, false);
            }
        });
    },

    getGridPermissionDataFromNode: function (node) {
        var text = node.get('text'),
            parent = node.parentNode;
        if (parent)
            text = parent.get('text') + '/' + text;
        return { id: node.get('id'), text: text };
    },

    onSearch: function (text) {
        var asp = this,
            panel = asp.getPermissionsTreePanel();

        if (!text == '') {
            if (text.length < 3) {
                return false;
            }
            if (Ext.String.trim(text)) {
                panel.setLoading(true);
                asp.filterByText(text, function () {
                    panel.setLoading(false);
                });
            }
        } else {
            asp.clearFilter();
            asp.getPermissionsTreePanel().collapseAll();
        }
    },

    onSeacrhClear: function () {
        var asp = this;
        asp.clearFilter();
        asp.getPermissionsTreePanel().collapseAll();
    },

    filterByText: function (text, func) {
        var asp = this;
        asp.filterBy(text, 'text');
        func.apply(null, []);
    },

    /**
     * Filter the tree on a string, hiding all nodes expect those which match and their parents.
     * @param The term to filter on.
     * @param The field to filter on (i.e. 'text').
     */
    filterBy: function (text, by) {
        var asp = this,
            treeView = asp.getPermissionsTreePanel().getView(),
            treePanel = asp.getPermissionsTreePanel(),
            nodesAndParents = [];

        asp.clearFilter();

        // Find the nodes which match the search term, expand them.
        // Then add them and their parents to nodesAndParents.
        asp.getPermissionsTreePanel().getRootNode().cascadeBy(function () {
            var currNode = this;
            if (currNode.data[by] && currNode.data[by].toString().toLowerCase().indexOf(text.toLowerCase()) > -1) {
                treePanel.expandPath(currNode.getPath());

                if (currNode.hasChildNodes()) {
                    currNode.eachChild(function (child) {
                        nodesAndParents.push(child.id);
                    });
                }
                while (currNode.parentNode) {
                    nodesAndParents.push(currNode.id);
                    currNode = currNode.parentNode;
                }
            }
        }, null, [treePanel, treeView]);

        // Hide all of the nodes which aren't in nodesAndParents
        asp.getPermissionsTreePanel().getRootNode().cascadeBy(function (tree, view) {
            var node = this;
            var uiNode = view.getNodeByRecord(node);
            if (uiNode && !Ext.Array.contains(nodesAndParents, node.id)) {
                Ext.get(uiNode).setDisplayed('none');
            }
        }, null, [treePanel, treeView]);
    },

    clearFilter: function () {
        var asp = this,
            treeView = asp.getPermissionsTreePanel().getView();
        asp.getPermissionsTreePanel().collapseAll();
        asp.getPermissionsTreePanel().getRootNode().cascadeBy(function (tree, view) {
            var node = this;
            var uiNode = view.getNodeByRecord(node);
            if (uiNode) {
                Ext.get(uiNode).setDisplayed('table-row');
            }
        }, null, [asp, treeView]);
    },

    /*
     * Проходит по дереву и отмечает родительский узел в случае, если все дочерние узлы отмечены
     * @param beginWith Узел, с которого начинать обход. По умолчанию обход идет с корня
     */
    checkParentNodes: function (beginWith) {
        var asp = this,
            tree = asp.getPermissionsTreePanel(),
            rootNode = beginWith || tree.getRootNode();

        var fn = function (node) {
            if (asp.allChildrenAreChecked(node)) {
                node.set('checked', true);
                asp.setPartiallyChecked(node, false);
            }
            else {
                if (node.isLeaf()) {
                    node.set('checked', node.data.checked === true);
                    asp.setPartiallyChecked(node, false);
                }
                else {
                    node.set('checked', false);

                    var anyChecked = asp.anyChildrenAreChecked(node);
                    asp.setPartiallyChecked(node, anyChecked);
                }
            }
            for (var i = 0; i < node.childNodes.length; ++i) {
                fn(node.childNodes[i]);
            }
        };
        fn(rootNode);
    },

    /*
     * Проверяет, отмечены ли все дочерние узлы
     */
    allChildrenAreChecked: function (node) {
        var allChildrenAreChecked = false;
        var fn = function (child) {
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

    // отмечен хотя бы один дочерний узел
    anyChildrenAreChecked: function (node) {
        var anyChildrenAreChecked = false;
        var fn = function (child) {
            anyChildrenAreChecked = child.data.checked === true;

            if (child.isLeaf()) {
                return anyChildrenAreChecked;
            }

            for (var i = 0; i < child.childNodes.length; ++i) {
                if (fn(child.childNodes[i]) === true) {
                    return true;
                }
            }
        };

        fn(node);

        return anyChildrenAreChecked;
    },

    //установить как частично отмеченный
    setPartiallyChecked: function (node, partially) {
        var style = partially ? this._partiallyCheckedStyle : '';
        node.set('cls', style);
    },

    /*
     * Установка дочерних узлов в определенное состояние
     * @param node Родительский узел
     * @param check Состояние
     */
    checkChildren: function (node, check) {
        node.cascadeBy(function (n) {
            n.set('checked', check);
        });
    },

    /*
     * Пройтись по родителям и снять/отметить в зависимости от состояния текущего узла
     * @param node Текущий измененный узел
     */
    walkThroughParents: function () {
        this.checkParentNodes();
    },
});