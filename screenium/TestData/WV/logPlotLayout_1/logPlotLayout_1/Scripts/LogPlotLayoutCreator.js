/**
 * create layout to be used by the log plot view.
 */
var LogPlotLayoutCreator = function (containerDivId) {
    this._containerDiv = $('#' + containerDivId);

    this._mainVerticalSplitter = null;
    this._headingsHorizontalSplitter = null;
    this._mainHorizontalSplitter = null;
    this._footersHorizontalSplitter = null;

    this._columnPrefixes = [
        'column0', 'column1', 'column2', 'column3'
    ];

    this._rowSuffixes = [
        '-pane-heading', '-pane-footer'
    ];

    this._rowSuffixesIncludingMain = [
        '-pane-heading', '-pane-footer', '-pane-main'
    ];
};

LogPlotLayoutCreator.prototype.construct = function () {
    this._createKendoSplitters();

    //jQuery resizable:
    this._containerDiv.find('#logPlot').resizable({
        //handles: "e"
    });

    this._makeColumnsDraggable();
};


LogPlotLayoutCreator.prototype._createKendoSplitters = function () {
    var self = this;

    this._containerDiv.find("#vertical").kendoSplitter({
        orientation: "vertical",
        panes: [
            { collapsible: true, resizable: true, size: "50px" }, //headings
            { collapsible: false, resizable: true }, //main
            { collapsible: true, resizable: true, size: "50px" } //footer
        ]
    });
    this._mainVerticalSplitter = this._containerDiv.find("#vertical").data("kendoSplitter");

    this._containerDiv.find("#horizontal-headings").kendoSplitter({
        panes: [
            { collapsible: false, resizable: false, scrollable: false },
            { collapsible: false, resizable: false, scrollable: false },
            { collapsible: false, resizable: false, scrollable: false },
            { collapsible: false, resizable: false, scrollable: false }
        ]
    });
    this._headingsHorizontalSplitter = this._containerDiv.find("#horizontal-headings").data("kendoSplitter");

    this._containerDiv.find("#horizontal-main").kendoSplitter({
        panes: [
            { collapsible: true, scrollable: false },
            { collapsible: true, scrollable: false },
            { collapsible: true, scrollable: false },
            { collapsible: true, scrollable: false }
        ],
        expand: function (e) {
            self._onExpandMainHorizontal(e, this);
        },
        collapse: function (e) {
            self._onCollapseMainHorizontal(e, this);
        },
        contentload: function (e) {
            self._onContentLoadMainHorizontal(e, this);
        },
        resize: function (e) {
            self._onResizeMainHorizontal(e, this);
        }
    });
    this._mainHorizontalSplitter = this._containerDiv.find("#horizontal-main").data('kendoSplitter');

    this._containerDiv.find("#horizontal-footers").kendoSplitter({
        panes: [
            { collapsible: false, resizable: false, scrollable: false },
            { collapsible: false, resizable: false, scrollable: false },
            { collapsible: false, resizable: false, scrollable: false },
            { collapsible: false, resizable: false, scrollable: false }
        ]
    });
    this._footersHorizontalSplitter = this._containerDiv.find("#horizontal-footers").data('kendoSplitter');
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
            return self._containerDiv.find("#" + dragDropId).clone(); //TODO review will this work OK with real plots
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

/**adjust the pane sizes, after some resize event.
*unfortunately, kendo ui does not always provide the pane object that was resized.
*so, this function adjusts ALL panes, taking Main as the authority.
*/
LogPlotLayoutCreator.prototype._adjustPaneSizes = function () {
    var self = this;

    var authorityPaneSuffix = '-pane-main';

    var rowSuffixToSplitter = [];

    //using Null Object to handle edge cases during control creation (where 1 or more row splitters not yet created)
    var nullObjectSplitter = {
        size: function () {
        }
    };

    rowSuffixToSplitter[this._rowSuffixes[0]] = this._headingsHorizontalSplitter ? this._headingsHorizontalSplitter : nullObjectSplitter;
    rowSuffixToSplitter[this._rowSuffixes[1]] = this._footersHorizontalSplitter ? this._footersHorizontalSplitter : nullObjectSplitter;

    var resizeOtherPanesInColumn = function (columnPrefix) {
        var authorityPaneId = columnPrefix + authorityPaneSuffix;
        var authorityPane = self._getPane(authorityPaneId);
        var size = authorityPane.size;
        if (authorityPane.collapsed) {
            size = 0;
        }
        if (typeof (size) === 'undefined') {
            //bug in kendo ui?
            size = self._containerDiv.find('#' + authorityPaneId).width();
        }

        for (var rowSuffix in self._rowSuffixes) {
            if (self._rowSuffixes.hasOwnProperty(rowSuffix)) {
                rowSuffix = self._rowSuffixes[rowSuffix];
                var targetPaneId = columnPrefix + rowSuffix;

                var splitter = rowSuffixToSplitter[rowSuffix];
                splitter.size('#' + targetPaneId, size);
            }
        }
    };

    for (var columnPrefix in this._columnPrefixes) {
        if (this._columnPrefixes.hasOwnProperty(columnPrefix)) {
            columnPrefix = this._columnPrefixes[columnPrefix];
            resizeOtherPanesInColumn(columnPrefix);
        }
    }
};

LogPlotLayoutCreator.prototype._getPane = function (id) {
    return this._containerDiv.find('#' + id).data('pane');
};

LogPlotLayoutCreator.prototype._onResizeMainHorizontal = function(e, elem) {
    console.log("Resized :: Splitter <b>#" + elem.element[0].id + "</b>");
    this._adjustPaneSizes();
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
