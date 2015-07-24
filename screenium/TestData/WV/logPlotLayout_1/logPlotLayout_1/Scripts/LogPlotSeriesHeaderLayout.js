/**
 * @constructor 
 * @description Create the layout for the header of 1 series.
 * Can be arranged to be suitable for Top or Bottom position.
 */
var LogPlotSeriesHeaderLayout = function (isAtTop, serieId, serieName, isValueShowing) {
    if (typeof isAtTop === "undefined" || typeof serieId === "undefined" ||
        typeof serieName === "undefined" || typeof isValueShowing === "undefined") {
        throw "bad args!";
    }

    this._isAtTop = isAtTop;
    this._serieId = serieId;
    this._serieName = serieName;
    this._isValueShowing = isValueShowing;
};

LogPlotSeriesHeaderLayout.prototype._getHtmlForNameValueUnits = function () {
    if (!this._isValueShowing) {
        throw "this._isValueShowing - not impl!";
    }

    return "<div class='pure-g pure-g-valign-fix' style='height: 100%;'>" +
        "<div class='pure-u-10-24'>" + this._serieName + "</div>" +

        //TODO make value font take 100% of height
        "<div class='pure-u-10-24' style='  height: 100%;'>123.45</div>" +
        "<div class='pure-u-4-24'>[C]</div>" +
        "</div>";
};
LogPlotSeriesHeaderLayout.prototype.getInitialHtml = function () {

    /*
    name
    value
    units
    metaDataIndicators
    valueAxis
    errorBar
    */

    var html = "";

    html += "<div class='headerSerieCell'>";

    //TODO refactor - extract fun
    //TODO refactor - extract CSS
    html += "<div class='pure-g pure-g-valign-fix'  style='height: 100%;   border: solid 1px blue;' >" +
        "<div class='pure-u-4-5' style='height: calc(100% - (50px));'>" + this._getHtmlForNameValueUnits() + this._getHtmlForValueAxis() + "</div>" +
        "<div class='pure-u-1-5' style='height: 100%;'><div class='logPlotMetaDataIndicators'><div>{B}</div><div>{L}</div><div>{F}</div></div></div>";
    html += "</div>";

    return html;
};
LogPlotSeriesHeaderLayout.prototype.getInitialEmptyHtml = function () {
    var html = "";

    html += "<div class='headerSerieCell'>";
    //TODO refactor - extract CSS
    html += "<div class='pure-g pure-g-valign-fix'  style='height: 100%;   border: solid 1px blue;' >";
    html += "</div>";

    return html;
};
LogPlotSeriesHeaderLayout.prototype._getHtmlForValueAxis = function () {
    return "<div style='width:100%; height:50px; border: solid black 1px;   margin-top: -5px; margin-left: 3px;' >{value axis here}</div>";
};

LogPlotSeriesHeaderLayout.prototype.onResizeLayout = function () {
    this._sizeValueFontToMatchHeight();
};

LogPlotSeriesHeaderLayout.prototype._sizeValueFontToMatchHeight = function () {
    var minWidthToShowValue = 130;

    //TODO if width < minWidthToShowValue then hide the value
    //TODO if width > minWidthToShowValue then show the value (if configured to show)

    //TODO implement me
    /*
    $.fn.shrinkText = function() {

    var $_me = this;
    var $_parent = $_me.parent();

    var int_my_width = $_me.width();
    var int_parent_width = $_parent.width();

    if ( int_my_width > int_parent_width ){

        rl_ratio =   int_parent_width / int_my_width;

        var int_my_fontSize = $_me.css("font-size").replace(/[^-\d\.]/g, '');

        int_my_fontSize = Math.floor(int_my_fontSize * rl_ratio);

        $_me.css("font-size", int_my_fontSize + "px");

    }
    };

    */
};
