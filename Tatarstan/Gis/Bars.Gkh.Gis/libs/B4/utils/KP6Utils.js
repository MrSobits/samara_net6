/**
 * Содержит различные вспомогательные методы
 */
Ext.define('B4.utils.KP6Utils', {
    statics: {
        renderParameterValue: function (value, record) {

            if (typeof value === "undefined" || value === '' || value === null)
                return value;

            if (record.raw.ValuesList && record.raw.ValuesList.length != 0) {
                for (var i in record.raw.ValuesList) {
                    var currentValueRecord = record.raw.ValuesList[i];
                    if (currentValueRecord.Value == value)
                        return currentValueRecord.ValueName;
                }
            } else {

                //форматирование значений по типу            
                switch (record.get('ParameterTypeNative')) {
                    case 'date':
                        //костылььь!!!
                        //var newValue = Ext.Date.format(Ext.Date.parse(value, 'd.m.Y'), 'd.m.Y');
                        //if (!newValue) newValue = Ext.Date.format(value, 'd.m.Y');
                        return value;
                        //return Ext.util.Format.date(value, 'd.m.Y');
                    case 'int':
                        return Ext.util.Format.number(value, '0/i');
                    case 'float':
                        return value; //return Ext.util.Format.number(value, '0.00000000/i');
                }
            }
            return value;
        },


        numberRenderer: function (value) {
            return Ext.util.Format.number(parseFloat(value), '0,000.00').replace(',', '.');
        },

        getSummary: function (records, dataIndex) {
            var i = 0,
                length = records.length,
                total = 0,
                record;

            for (; i < length; ++i) {
                record = records[i];

                total += record.get(dataIndex);
            }

            return total;
        },

        /*
         * Используется для отображения записей простого справочника, состоящего из кода и наименования, в гридах и полях выбора
         */
        dictRenderer: function (value) {
            return value && (value["Code"] + " - " + value["Name"]);
        },

        nameRenderer: function (value) {
            return value && value["Name"];
        },

        representationRenderer: function (value) {
            return value && value["Representation"];
        },

        yesnoRenderer: function (value) {
            return value ? 'Да' : 'Нет';
        },

        //вытащить текущий расчетный месяц
        showCurMonth: function (inpoint) {
            var con = Ext.ComponentQuery.query('#cmItemId')[0];
            var action = 'GetCurrentData';
            if (inpoint)
                action = 'GetCurrentDataPoint'; //с обновлением данных хоста

            if (con) {
                Ext.Ajax.request({
                    method: 'GET',
                    url: B4.Url.action('/Finance/' + action),
                    success: function (response) {

                        var resp = Ext.JSON.decode(response.responseText);


                        //con.update(resp.data);

                        con.setText(resp.data);
                    },
                    failure: function (response) {

                        con.setText('Нет доступа!');
                    }
                });
            }
        },

        //копирование объекта
        clone: function (o) {
            if (!o || 'object' !== typeof o) {
                return o;
            }
            if ('function' === typeof o.clone) {
                return o.clone();
            }
            var c = '[object Array]' === Object.prototype.toString.call(o) ? [] : {};
            var p, v;
            for (p in o) {
                if (o.hasOwnProperty(p)) {
                    v = o[p];
                    if (v && 'object' === typeof v) {
                        c[p] = Ext.ux.util.clone(v);  //!!!!Ext.clone(v);
                    }
                    else {
                        c[p] = v;
                    }
                }
            }
            return c;
        },

        //--------------------------------------------------------------
        //  показать протокол расчета в виде дерева
        //--------------------------------------------------------------
        onShowTreeProtocol: function (view, personalAccountId, realityObjectId, yy, mm, serviceId, supplierId, isGis, serviceName, version1) {
            var me = this;
            B4.Ajax.request({
                url: B4.Url.action('/Host/GetTree?' +
                    'id=' + personalAccountId + '&realityObjectId=' + realityObjectId + '&year=' + yy + '&month=' + mm +
                '&serviceId=' + serviceId + '&supplierId=' + supplierId + '&isGis=' + isGis
                ),
                method: 'GET',
                timeout: 9999999
            }).next(function (jsonResp) {
                var data = Ext.decode(jsonResp.responseText).data;

                if (data && data.children != null) {
                    if (version1)
                        me.buildTreeVersion1(view, data, serviceName);
                    else
                        me.buildTree(view, data, serviceName);
                } else {

                    Ext.Msg.alert('Внимание!', 'Протокол не может быть получен. Нет данных');

                }

            }).error(function (response) {
                Ext.Msg.alert('Ошибка!', !Ext.isString(response.message) ? 'При построении протокола произошла ошибка!' : response.message);
            });

        },


        //SVG version
        buildTree: function (view, data, serviceName) {
            var pnl = view.up('panel'),
                pnlView = pnl != null ? pnl.getEl() : view.getEl(),
                me = this,
                win = Ext.create('Ext.window.Window', {
                    title: 'Протокол расчета по услуге ' + serviceName,
                    width: 800,
                    height: 500,
                    animateTarget: pnlView,
                    renderTo: B4.getBody().getActiveTab().getEl(),
                    constrain: true,
                    html: ' <div id="dvTreeContainer" style="background: #fff; width: 100%; height: 100%;"></div> ',
                    resizable: false,
                    maximizable: true
                }),
                m = [20, 120, 20, 120],
		        w = win.width - m[0],
		        h = win.height - m[0],
		        i = 0,
		        root,
		        zm,
		        rectW = 210,
		        rectH = 60,
			    tree = d3.layout.tree()
					 .nodeSize([rectW + 40, rectH + 10]),
                diagonal = d3.svg.diagonal()
		                     .projection(function (d) {
		                         return [d.x + rectW / 2, d.y + rectH / 2];
		                     }),
			    vis = d3.select("#dvTreeContainer").append("svg:svg")
					     .attr("width", '100%')
					     .attr("height", '100%')
					     .call(zm = d3.behavior.zoom().scaleExtent([1, 3]).on("zoom", redraw))
                        .append("svg:g")
		                 .attr("transform", "translate(" + (w / 2 - 165) + "," + m[0] + ")");

            root = data;
            root.x0 = h / 2;
            root.y0 = 0;

            function toggleAll(d) {
                if (d.children) {
                    d.children.forEach(toggleAll);
                    toggle(d);
                }
            }

            root.children.forEach(toggleAll);

            update(root);

            function update(source) {
                var duration = d3.event && d3.event.altKey ? 5000 : 500;

                // Compute the new tree layout.
                var nodes = tree.nodes(root);

                // Normalize for fixed-depth.
                nodes.forEach(function (d) { d.y = d.depth * 150; });

                // Update the nodes…
                var node = vis.selectAll("g.node")
                    .data(nodes, function (d) { return d.id || (d.id = ++i); });

                // Enter any new nodes at the parent's previous position.
                var nodeEnter = node.enter().append("svg:g")
                    .attr("class", "node")
                    .attr("transform", function (d) {
                        return "translate(" + source.x0 + "," + source.y0 + ")";
                    })
                    .on("click", function (d) { toggle(d); update(d); });

                nodeEnter.append("svg:rect")
                    .attr("width", rectW)
                    .attr("height", rectH)
                    .attr("stroke", "black")
                    .attr("stroke-width", 1)
                    .style("fill", function (d) {
                        return d._children ? "lightsteelblue" : "#fff";
                    });

                nodeEnter.append("text")
                    .attr("x", function (d) {
                        return d.Operation ? -(d.Operation.length + 16) : '';
                    })
                    .attr("y", -5)
                    .attr("style", "font-size: 18px; font-weight: 700; color: #333;")
                    .attr("text-anchor", "middle")
                    .text(function (d) {
                        var op;
                        if (d.Operation) {
                            op = d.Operation;
                            op = op.replace("(", " ");
                            op = op.replace(")", " ");
                        } else {
                            op = "";
                        }
                        return op;
                    });

                nodeEnter.append("text")
                    .attr("x", rectW / 2)
                    .attr("y", rectH / 2)
                    .attr("dy", "-.95em")
                    .attr("style", "font-size: 12px;")
                    .attr("text-anchor", "middle")
                    .text(function (d) {
                        return d.Value ? d.Value : '';
                    });

                nodeEnter.append("text")
                    .attr("x", rectW / 2)
                    .attr("y", rectH / 2)
                    .attr("dy", ".65em")
                    .attr("style", "font-size: 11px;")
                    .attr("text-anchor", "middle")
                    .text(function (d) {
                        return d.Hint ? d.Hint : '';
                    })
                .call(wrap, rectW);

                // Transition nodes to their new position.
                var nodeUpdate = node.transition()
                    .duration(duration)
                    .attr("transform", function (d) {
                        return "translate(" + d.x + "," + d.y + ")";
                    });

                nodeUpdate.select("text")
                    .style("fill-opacity", 1);

                // Transition exiting nodes to the parent's new position.
                var nodeExit = node.exit().transition()
                    .duration(duration)
                    .attr("transform", function (d) {
                        return "translate(" + source.x + "," + source.y + ")";
                    })
                    .remove();

                nodeExit.select("rect")
                    .attr("width", rectW)
                    .attr("height", rectH);

                nodeExit.select("text");

                // Update the links…
                var link = vis.selectAll("path.link")
                    .data(tree.links(nodes), function (d) {
                        return d.target.id;
                    });

                // Enter any new links at the parent's previous position.
                link.enter().insert("svg:path", "g")
                      .attr("class", "link")
                      .attr("x", rectW / 2)
                      .attr("y", rectH / 2)
                      .attr("d", function (d) {
                          var o = {
                              x: source.x0,
                              y: source.y0
                          };
                          return diagonal({
                              source: o,
                              target: o
                          });
                      });

                // Transition links to their new position.
                link.transition()
                    .duration(duration)
                    .attr("d", diagonal);

                // Transition exiting nodes to the parent's new position.
                link.exit().transition()
                    .duration(duration)
                    .attr("d", function (d) {
                        var o = { x: source.x, y: source.y };
                        return diagonal({ source: o, target: o });
                    })
                    .remove();

                // Stash the old positions for transition.
                nodes.forEach(function (d) {
                    d.x0 = d.x;
                    d.y0 = d.y;
                });
            }

            function toggle(d) {
                if (d.children) {
                    d._children = d.children;
                    d.children = null;
                } else {
                    d.children = d._children;
                    d._children = null;
                }
            }

            function redraw() {
                vis.attr("transform",
                    "translate(" + d3.event.translate + ")"
                    + " scale(" + d3.event.scale + ")");
            }

            function wrap(text, width) {
                text.each(function () {
                    var text = d3.select(this),
                        words = text.text().split(/\s+/).reverse(),
                        word,
                        line = [],
                        lineNumber = 0,
                        lineHeight = 1.1, // ems
                        y = text.attr("y"),
                        dy = parseFloat(text.attr("dy")),
                        tspan = text.text(null).append("tspan").attr("y", y).attr("dy", dy + "em");
                    while (word = words.pop()) {
                        line.push(word);
                        tspan.text(line.join(" "));
                        if (tspan.node().getComputedTextLength() > width) {
                            line.pop();
                            tspan.text(line.join(" "));
                            line = [word];
                            tspan = text.append("tspan").attr("x", width / 2).attr("y", y).attr("dy", ++lineNumber * lineHeight + dy + "em").text(word);
                        }
                    }
                });
            }

            win.show();

        },


        //version 1
        buildTreeVersion1: function (view, data, serviceName) {
            var me = this;

            me.RootNode = me.buildLeaf(data);

            var pnl = view.up('panel');
            var pnlView = pnl != null ? pnl.getEl() : view.getEl();

            new Ext.Window({
                title: 'Протокол расчета по услуге ' + serviceName,
                width: 800,
                height: 600,
                plain: true,
                animateTarget: pnlView, //view.getEl(),
                renderTo: B4.getBody().getActiveTab().getEl(), //pnlView, //view.getEl(),
                constrain: true,
                autoScroll: true,
                html: ' <div id="dvTreeContainer1"></div> ',
                maximizable: true
            }).show();

            me.RefreshTree();
        },
        buildLeaf: function (data) {
            var me = this, node = {};

            var str = (Ext.isString(data.Operation) ? data.Operation + " " : "") + data.Value;
            if (Ext.isString(data.Hint)) {
                str = str + '<hr>' + data.Hint;
            }

            node.Content = str;
            node.ToolTip = data.Hint;
            node.Nodes = new Array();
            node.MyBaseWinController = me;

            Ext.each(data.children, function (item) {


                var child = me.buildLeaf(item);
                node.Nodes.push(child);

            });

            return node;
        },
        RefreshTree: function () {
            var me = this;

            DrawTree({
                Container: document.getElementById("dvTreeContainer1"),
                RootNode: me.RootNode,
                OnNodeDoubleClick: me.NodeDoubleClick
            });
        },
        NodeDoubleClick: function (c1, c2) {

            if (this.Node.Nodes && this.Node.Nodes.length > 0) { // If has children
                this.Node.Collapsed = !this.Node.Collapsed;

                this.Node.MyBaseWinController.RefreshTree();
            }
        }

    }
});