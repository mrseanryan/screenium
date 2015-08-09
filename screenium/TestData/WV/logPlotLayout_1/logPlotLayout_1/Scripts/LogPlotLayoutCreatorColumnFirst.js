/**
 * create layout to be used by the log plot view.
 */
var LogPlotLayoutCreator = function (containerDivId, callback) {
    this._callback = callback;

    this._containerDiv = $('#' + containerDivId);

    this._columnCount = 0;

    this._columnIdToVerticalSplitter = null;

    this._horizontalSplitter = null;

    this._columnPrefixes = [];

    this._rowSuffixes = [
        '-pane-heading', '-pane-footer'
    ];

    this._rowSuffixesIncludingMain = [
        '-pane-heading', '-pane-footer', '-pane-main'
    ];
};

LogPlotLayoutCreator.prototype.construct = function (columnCount, rulerPosition, initialWidth, initialHeight) {
    if (!columnCount || typeof (rulerPosition) === 'undefined' || !initialWidth || !initialHeight) {
        throw 'bad args!';
    }

    this._columnCount = columnCount;
    this._createColumnPrefixes(rulerPosition);

    this._initialWidth = initialWidth;
    this._initialHeight = initialHeight;
    
    this._createDivs();

    this._createKendoSplitters();

    //jQuery resizable:
    this._containerDiv.find('#logPlot').resizable({
        //handles: "e"
    });

    this._makeColumnsDraggable();
};

LogPlotLayoutCreator.prototype._createDivs = function () {
    this._createMainDivs();
};

LogPlotLayoutCreator.prototype._createMainDivs = function () {
    //initialWidth, initialHeight

    var plotDiv = $("<!--LP control: width and height-->" + 
    "<div id='logPlot' style='border: solid 1px black; height: " + this._initialHeight + "px; width: " + this._initialWidth + "px;'>" +
        this._getHorizontalSplitterHtml() +
        "<div id='verticalSpaceAtBottom' style='height: 15px;'>&nbsp;</div>" + 
    "</div>");

    plotDiv.appendTo(this._containerDiv);
};

LogPlotLayoutCreator.prototype._getHorizontalSplitterHtml = function () {
    var html = "<div id='horizontalByColumns' style='height: 100%; width: 100%;'>";
    for (var column in this._columnPrefixes) {
        if (this._columnPrefixes.hasOwnProperty(column)) {
            column = this._columnPrefixes[column];

            html += "<div id='" + this._getColumnPaneIdFromPrefix(column) + "'>";
            html += "<div style='height:100%;'>";

            html += this._getVerticalSplitterHtmlForColumn(column);

            html += "</div>";
            html += "</div>";
        }
    }
    html += "</div>";
    return html;
};

LogPlotLayoutCreator.prototype._getColumnPaneIdFromPrefix = function (column) {
    return "full_column_pane_" + column;
};

LogPlotLayoutCreator.prototype._getColumnIdFromPrefix = function (columnPrefix) {
    var columnId = parseInt(columnPrefix.substr('column'.length), 10);
    return columnId;
};

LogPlotLayoutCreator.prototype._getVerticalSplitterHtmlForColumn = function (columnPrefix) {
    var createDiv = function (columnId, paneType) {
        //TODO 15.2 set height to 100%
        return "<div id='columnFirst-column" + columnId + "-pane-" + paneType + "' style='height: 97%; width: 100%;'></div>";
    };

    var columnId = this._getColumnIdFromPrefix(columnPrefix);

    var html = "";

    html += "<div id='" + this._getVerticalSplitterIdForColumn(columnId) + "' style='height: 100%; width: 100%;'>";

    html += "<div id='"+this._getColumnSplitterHeaderPaneId(columnPrefix)+ "' class='header-row-pane'>";
    html += createDiv(columnId, 'header');
    html += "</div>";

    html += "<div id='" + this._getColumnSplitterMainPaneId(columnPrefix) + "' class='main-row-pane'>";
    html += createDiv(columnId, 'main');
    html += "</div>";

    html += "<div id='" + this._getColumnSplitterFooterPaneId(columnPrefix) + "' class='footer-row-pane'>";
    html += createDiv(columnId, 'footer');
    html += "</div>";

    html += "</div>";
    return html;
};

LogPlotLayoutCreator.prototype._getColumnSplitterHeaderPaneId = function (columnPrefix) {
    return "splitter-header-pane-" + columnPrefix;
};

LogPlotLayoutCreator.prototype._getColumnSplitterMainPaneId = function (columnPrefix) {
    return "splitter-main-pane-" + columnPrefix;
};

LogPlotLayoutCreator.prototype._getColumnSplitterFooterPaneId = function (columnPrefix) {
    return "splitter-footer-pane-" + columnPrefix;
};

LogPlotLayoutCreator.prototype._getVerticalSplitterIdForColumn = function (columnId) {
    return "verticalSplitter-column-" + columnId;
};

LogPlotLayoutCreator.prototype.getHeaderDivForRuler = function () {
    return this.getHeaderDivForColumn(0);
};

/** get the div element for the Header of the given column
*/
LogPlotLayoutCreator.prototype.getHeaderDivForColumn = function (columnId) {
    var div = this._containerDiv.find("#columnFirst-column" + columnId + "-pane-header");
    if (div.length !== 1) {
        throw 'problem finding the column header div!';
    }
    return div;
};
/** get the div element for the main display (main canvas) of the Ruler
*/
LogPlotLayoutCreator.prototype.getMainDivForRuler = function () {
    return this.getMainDivForColumn(0);
};

/** get the div element for the main display (main plot canvas) of the given column
*/
LogPlotLayoutCreator.prototype.getMainDivForColumn = function (columnId) {
    var div = this._containerDiv.find("#columnFirst-column"+columnId+"-pane-main");
    if (div.length !== 1) {
        throw 'problem finding the column main div!';
    }
    return div;
};

/** get the div element for the value axis of the given column
*/
LogPlotLayoutCreator.prototype.getValueAxisDivForColumn = function (columnId) {
    var div = this._containerDiv.find("#columnFirst-column" + columnId + "-pane-footer");
    if (div.length !== 1) {
        throw 'problem finding the column footer div!';
    }
    return div;
};

LogPlotLayoutCreator.prototype._createColumnPrefixes = function (rulerPosition) {
    this._columnPrefixes = [];

    for (var i = 1; i < this._columnCount; i++) {
        this._columnPrefixes.push("column" + i);
    }
    
    //to simplify LP view, the ruler will always be column0.
    //we simply swap the column IDs, in order to position the ruler as requested:
    this._columnPrefixes.splice(rulerPosition, 0, "column" + 0);
};

LogPlotLayoutCreator.prototype._createVerticalSplitterAtDiv = function (containerDiv, columnId) {
    var self = this;

    var splitterDivId = this._getVerticalSplitterIdForColumn(columnId);

    var splitterDiv = containerDiv.find("#" + splitterDivId);
    if (splitterDiv.length !== 1) {
        throw "could not find the vertical splitter div!";
    }

    splitterDiv.kendoSplitter({
        orientation: "vertical",
        panes: [
            { collapsible: true, resizable: false, size: "300px" }, //headings
            { collapsible: false, resizable: true }, //main
            { collapsible: true, resizable: false, size: "300px" } //footer
        ],
        expand: function (e) {
            self._onExpandColumnVerticalSplitter(e, this, columnId);
        },
        collapse: function (e) {
            self._onCollapseColumnVerticalSplitter(e, this, columnId);
        }
    });
};

LogPlotLayoutCreator.prototype._onExpandColumnVerticalSplitter = function (e, elem, columnId) {
    console.log("expanded :: Pane <b>#" + e.pane.id + "</b> from splitter <b>#" + elem.element[0].id + "</b> expanded");

    var isHeader = (e.pane.id.indexOf('header') >= 0);
    this._setOtherColumnsAsExpanded(columnId, isHeader, true);
};

LogPlotLayoutCreator.prototype._onCollapseColumnVerticalSplitter = function (e, elem, columnId) {
    console.log("collapsed :: Pane <b>#" + e.pane.id + "</b> from splitter <b>#" + elem.element[0].id + "</b> collapsed");

    var isHeader = (e.pane.id.indexOf('header') >= 0);
    this._setOtherColumnsAsExpanded(columnId, isHeader, false);
};

LogPlotLayoutCreator.prototype._setOtherColumnsAsExpanded = function(columnId, isHeader, isExpanded) {
    var getPaneIdFun = isHeader ? '_getColumnSplitterHeaderPaneId' : '_getColumnSplitterFooterPaneId';
    getPaneIdFun = this[getPaneIdFun];

    //tell the other columns to expand/collapse their splitter:
    for (var otherColumnPrefix in this._columnPrefixes) {
        if (this._columnPrefixes.hasOwnProperty(otherColumnPrefix)) {
            otherColumnPrefix = this._columnPrefixes[otherColumnPrefix];
            var otherColumnId = this._getColumnIdFromPrefix(otherColumnPrefix);
            if (otherColumnId !== columnId) {
                var splitter = this._getVerticalSplitterForColumn(otherColumnId);
                splitter.toggle('#' + getPaneIdFun(otherColumnPrefix), isExpanded);
            }
        }
    }
};

LogPlotLayoutCreator.prototype._getVerticalSplitterForColumn = function (columnId) {
    var splitterDivId = this._getVerticalSplitterIdForColumn(columnId);
    var splitter = this._containerDiv.find('#' + splitterDivId).data('kendoSplitter');
    return splitter;
};

LogPlotLayoutCreator.prototype._createKendoSplitters = function () {
    var self = this;

    //column-first splitter:
    var options = {
        panes: []
    };
    options = {
        panes: [],
        expand: function (e) {
            //self._onExpandMainHorizontal(e, this);
        },
        collapse: function (e) {
            //self._onCollapseMainHorizontal(e, this);
        },
        contentload: function (e) {
            //self._onContentLoadMainHorizontal(e, this);
        },
        resize: function (e) {
            //self._onResizeMainHorizontal(e, this);
        }
    };
    this._addColumnPaneOptions(options.panes, { collapsible: true, resizable: true, scrollable: false });
    var horizSplitterDiv = this._containerDiv.find("#horizontalByColumns");
    horizSplitterDiv.kendoSplitter(options);
    this._horizontalSplitter = horizSplitterDiv.data('kendoSplitter');

    //now create the vertical splitter within each column:
    for (var column in this._columnPrefixes) {
        if (this._columnPrefixes.hasOwnProperty(column)) {
            column = this._columnPrefixes[column];
            var columnId = this._getColumnIdFromPrefix(column);
            this._createVerticalSplitterAtDiv(horizSplitterDiv, columnId);
        }
    }
};

LogPlotLayoutCreator.prototype._fireOnResizeLayout = function() {
    this._callback.fireOnResizeLayout();
};

LogPlotLayoutCreator.prototype._addColumnPaneOptions = function(panes, options) {
    for(var c = 0; c < this._columnCount; c++) {
        panes.push(options);
    };
};

LogPlotLayoutCreator.prototype._makeColumnsDraggable = function () {
    for (var dragColumn in this._columnPrefixes) {
        if (this._columnPrefixes.hasOwnProperty(dragColumn)) {
            dragColumn = this._columnPrefixes[dragColumn];

            for (var dropColumn in this._columnPrefixes) {
                if (this._columnPrefixes.hasOwnProperty(dropColumn)) {
                    dropColumn = this._columnPrefixes[dropColumn];
                    if (dragColumn !== dropColumn) {
                        this._makeColumnDraggable(dragColumn, dropColumn);
                    }
                }
            }
        }
    }
};

LogPlotLayoutCreator.prototype._makeColumnDraggable = function (columnPrefix) {
    var self = this;

    var getDraggableIdFromEvent = function (e) {
        var draggableId;
        if (e.draggable) {
            draggableId = e.draggable.element[0].id;
        } else {
            return null;
        }
        if (typeof (draggableId) === 'undefined') {
            throw 'cannot find id!';
        }
        return draggableId;
    };

    var getDropTargetIdFromEvent = function (e) {
        var dropTargetId;
        if (e.dropTarget) {
            dropTargetId = e.dropTarget[0].id;
        } else {
            return null;
        }
        if (typeof (dropTargetId) === 'undefined') {
            throw 'cannot find id!';
        }
        return dropTargetId;
    };

    var setCursorAsDefault = function () {
        $('body').css('cursor', 'default');
    };
    var setCursorAsDenied = function () {
        $('body').css('cursor', 'not-allowed');
    };
    var setCursorAsDragging = function () {
        $('body').css('cursor', 'move');
    };
    var setCursorAsDroppable = function () {
        $('body').css('cursor', 'move');
    };

    var droptargetOnDragEnter = function (e) {
        var draggableId = getDraggableIdFromEvent(e);
        var dropTargetId = getDropTargetIdFromEvent(e);

        if (draggableId !== dropTargetId) {
            self._containerDiv.find("#" + dropTargetId).addClass("dropTargetEntered");

            setCursorAsDroppable();
        }
    };
    var droptargetOnDragLeave = function (e) {
        var dropTargetId = getDropTargetIdFromEvent(e);

        self._containerDiv.find("#" + dropTargetId).removeClass("dropTargetEntered");

        setCursorAsDragging();
    };
    var getColumnPrefixFromDragDrop = function (id) {
        id = id.split('-')[0];
        return id;
    };
    var restoreMouseAndUi = function (draggableId, dropTargetId) {
        var draggable = self._containerDiv.find("#" + draggableId);
        draggable.removeClass("draggableOnDrag");
        if (dropTargetId) {
            self._containerDiv.find("#" + dropTargetId).removeClass("dropTargetEntered");
        }
        setCursorAsDefault();
    };

    var droptargetOnDrop = function (e) {
        var draggableId = getDraggableIdFromEvent(e);
        var dropTargetId = getDropTargetIdFromEvent(e);

        var draggable = self._containerDiv.find("#" + draggableId);

        restoreMouseAndUi(draggableId, dropTargetId);

        if (!draggableId || !dropTargetId) {
            return;
        }

        if (!draggable.data("kendoDraggable")) {
            // drag ended outside of any droptarget
            return;
        }

        var dropColumnPrefix = getColumnPrefixFromDragDrop(dropTargetId);
        var dragColumnPrefix = getColumnPrefixFromDragDrop(draggableId);

        self._swapColumnContents(dragColumnPrefix, dropColumnPrefix);
    }

    var draggableOnDrag = function (e) {
        var draggableId = getDraggableIdFromEvent(e);

        var isDroppable = e.dropTarget;

        if (isDroppable && draggableId !== e.dropTarget.id) {
            setCursorAsDroppable();
        } else {
            setCursorAsDenied();
        }
    };

    var draggableOnDragStart = function (e) {
        var draggableId = getDraggableIdFromEvent(e);
        if (!draggableId) {
            draggableId = e.currentTarget[0].id;
        }
        if (draggableId) {
            self._containerDiv.find("#" + draggableId).addClass("draggableOnDrag");

            setCursorAsDragging();
        }
    }

    var draggableOnDragEnd = function (e) {
        //not implementing main drop operation here, since e does not contain the drag'n'drop elements!
        var draggableId = getDraggableIdFromEvent(e);
        var dropTargetId = getDropTargetIdFromEvent(e);

        if (!draggableId) {
            draggableId = e.currentTarget[0].id;
        }

        restoreMouseAndUi(draggableId, dropTargetId);
    }

    var dragDropId = columnPrefix + '-pane-heading';

    self._containerDiv.find("#" + dragDropId).kendoDraggable({
        hint: function () {
            return self._containerDiv.find("#" + dragDropId).clone();
        },
        drag: draggableOnDrag,
        dragstart: draggableOnDragStart,
        dragend: draggableOnDragEnd,
        cursorOffset: { top: 10, left: 10 }
    });

    self._containerDiv.find("#" + dragDropId).kendoDropTarget({
        dragenter: droptargetOnDragEnter,
        dragleave: droptargetOnDragLeave,
        drop: droptargetOnDrop
    });
};

LogPlotLayoutCreator.prototype._swapColumnContents = function (sourceColumnPrefix, targetColumnPrefix) {
    if (sourceColumnPrefix === targetColumnPrefix) {
        return;
    }

    for (var rowSuffix in this._rowSuffixesIncludingMain) {
        if (this._rowSuffixesIncludingMain.hasOwnProperty(rowSuffix)) {
            rowSuffix = this._rowSuffixesIncludingMain[rowSuffix];
            var sourcePaneId = sourceColumnPrefix + rowSuffix;
            var targetPaneId = targetColumnPrefix + rowSuffix;

            var sourceDiv = this._containerDiv.find('#' + sourcePaneId);
            var destDiv = this._containerDiv.find('#' + targetPaneId);

            this._swapChildrenOfDivs(sourceDiv, destDiv);
        }
    }
};

LogPlotLayoutCreator.prototype._swapChildrenOfDivs = function (sourceDiv, destDiv) {
    //using detach() NOT remove() to preserve jQuery objects and events
    var sourceChildren = sourceDiv.children().detach();
    var destChildren = destDiv.children().detach();

    sourceChildren.appendTo(destDiv);
    destChildren.appendTo(sourceDiv);
};

LogPlotLayoutCreator.prototype._getPane = function (id) {
    //TODO could cache to optimize
    return this._containerDiv.find('#' + id).data('pane');
};

LogPlotLayoutCreator.prototype._onResizeMainHorizontal = function(e, elem) {
    console.log("Resized :: Splitter <b>#" + elem.element[0].id + "</b>");

    this._fireOnResizeLayout();
};

LogPlotLayoutCreator.prototype._onExpandMainHorizontal = function(e, elem) {
    console.log("Expanded :: Pane <b>#" + e.pane.id + "</b> from splitter <b>#" + elem.element[0].id + "</b> expanded");
};

LogPlotLayoutCreator.prototype._onCollapseMainHorizontal = function(e, elem) {
    console.log("Collapsed :: Pane <b>#" + e.pane.id + "</b> from splitter <b>#" + elem.element[0].id + "</b> collapsed");
};

// ReSharper disable once UnusedParameter
LogPlotLayoutCreator.prototype._onContentLoadMainHorizontal = function(e, elem) {
    console.log("Content loaded in <b>#" + e.pane.id +
        "</b> and starts with <b>" + this._containerDiv.find(e.pane).text().substr(0, 20) + "...</b>");
};
