/// <reference path="LogPlotHeaderLayoutStrategy.js" />
/// <reference path="LogPlotHeaderLayoutPartsCreator.js" />
/**
 * @constructor 
 * @description Create the layout for the header of 1 series.
 * Can be arranged to be suitable for Top or Bottom position.
 */
var LogPlotSeriesHeaderLayout = function (isAtTop, serieId, serieName, headerConfig) {
    if (typeof isAtTop === "undefined" || typeof serieId === "undefined" ||
        typeof serieName === "undefined" || typeof headerConfig === "undefined") {
        throw "bad args!";
    }

    this._isAtTop = isAtTop;
    this._serieId = serieId;
    this._serieName = serieName;
    this._config = headerConfig;

    this._partsCreator = new LogPlotHeaderLayoutPartsCreator(headerConfig);
    var layoutFactory = new LogPlotHeaderLayoutStrategyFactory(headerConfig, isAtTop, this._partsCreator);
    this._layoutStrategy = layoutFactory.create();
};

LogPlotSeriesHeaderLayout.prototype._getHtmlForNameValueUnits = function () {
    return "<div class='pure-g pure-g-valign-fix' style='height: calc(100% - (" + this._getHeightOfValueAxis() + "px));'>" +
            "<div id='nameDiv' class='pure-u-4-24 logPlotHeaderSmallText logPlotHeaderAutoTruncatedText'>" + this._serieName + "</div>" +
            //TODO make value font take 100% of height
            "<div id='snapshotValueDiv' class='pure-u-18-24 logPlotHeaderCenteredText logPlotHeaderLargeText logPlotHeaderAutoTruncatedText' style='height: 100%;'>" + this._getHtmlForValue() + "</div>" +
            "<div id='unitsDiv' class='pure-u-2-24 logPlotHeaderSmallText' >[C]</div>" +
            "</div>";
};
LogPlotSeriesHeaderLayout.prototype._getHtmlForValue = function() {
    var html = "123.45";
    return html;
};
//tried SVG but does not really scale :(
LogPlotSeriesHeaderLayout.prototype._getHtmlForValueSvg = function () {
    var html = "<svg width='100%' height='100%'" +
    "style='border:none;'" +
    "xmlns='http://www.w3.org/2000/svg' preserveAspectRatio='xMinYMin meet'>" +
    "<g>" +
        "<text y='12.5%' textLength='436' lengthAdjust='spacingAndGlyphs' transform='scale(1,3)'>{value}</text>" +
    "</g>" +
    "</svg>";

    return html;
};
LogPlotSeriesHeaderLayout.prototype.getInitialHtml = function () {
    //TODO refactor to take in a div, add the HTML, and then cache the divs for each part
    //TODO add accessors and use them to set the values etc.

    /*
    name
    value
    units
    metaDataIndicators
    valueAxis
    errorBar
    */

    var html = "";

    html += "<div class='logPlotHeaderSerieCell'>";

    //TODO refactor - extract fun
    //TODO refactor - extract CSS

    var hasTopRow = this._config.getShowName() || this._config.getShowUom() || this._config.getShowSnapshotValue();

    var strategyHtml = this._layoutStrategy.createLayoutHtml();

    html += strategyHtml;


    if (!strategyHtml) {
        //TODO move this HTML stuff down into the strategies ...

        //if we have a top row, then put the metadata indicators on that row (so that value axis has full width)
        if (hasTopRow) {
            html += "<div class='logPlotHeaderSerieCellTopBox'>";
            html += "<div class='pure-g pure-g-valign-fix ' >" +
                "<div class='pure-u-20-24' style='height: 100%; min-height: 25px;'>" + this._getHtmlForNameValueUnits() + "</div>" +
                "<div class='pure-u-4-24' style='height: 100%;'><div class='logPlotMetaDataIndicators'><div>B</div><div>L</div><div>F</div></div></div>"
                + "</div>";

            html += "<div class='pure-g pure-g-valign-fix' >" +
                "<div class='pure-u-24-24' style='height: 100%; min-height: 25px;'>" + this._getHtmlForValueAxis() + "</div>" +
                "</div>";

            html += "</div>";
        } else {
            //put meta data indicators on same row as value axis:
            if (this._config.getShowMetadataIndicators()) {
                html += "<div class='pure-g pure-g-valign-fix logPlotHeaderSerieCellTopBox' >" +
                    "<div class='pure-u-20-24' style='height: 100%; min-height: 25px;'>" + this._getHtmlForValueAxis() + "</div>" +
                    "<div class='pure-u-4-24' style='height: 100%;'><div class='logPlotMetaDataIndicators'><div>B</div><div>L</div><div>F</div></div></div>";
                html += "</div>";
            } else {
                //value axis only:
                html += "<div class='pure-g pure-g-valign-fix logPlotHeaderSerieCellTopBox' >" +
                    "<div class='pure-u-24-24' style='height: 100%; min-height: 25px;'>" + this._getHtmlForValueAxis() + "</div>";
                html += "</div>";
            }
        }
    }
    return html;
};
LogPlotSeriesHeaderLayout.prototype.getInitialEmptyHtml = function () {
    var html = "";

    html += "<div class='logPlotHeaderSerieCell'>";
    //TODO refactor - extract CSS
    html += "<div class='pure-g pure-g-valign-fix'  style='height: 100%;   border: solid 1px blue;' >";
    html += "</div>";

    return html;
};
LogPlotSeriesHeaderLayout.prototype._getHtmlForValueAxis = function () {
    //TODO refactor - extract CSS
    return "<div style='width:100%; height:" + this._getHeightOfValueAxis() + "px; border: solid black 1px;   margin-top: -5px; ' >{value axis here}</div>";
};

LogPlotSeriesHeaderLayout.prototype._getHeightOfValueAxis = function () {
    return 37;
};

LogPlotSeriesHeaderLayout.prototype.onResizeLayout = function () {
    this._hidePartsToMatchHeight();

    this._sizeValueFontToMatchHeight();
};

LogPlotSeriesHeaderLayout.prototype._hidePartsToMatchHeight = function () {
    var minHeightToShowTwoRows = 66;
    //TODO if height < minHeightToShowTwoRows then hide: name, value, units
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
