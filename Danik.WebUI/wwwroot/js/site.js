var px = 0;

function Draw(m) {
    px = document.getElementById("pMain").clientWidth / 1000;

    var html = "";
    var template = structuredClone(m.Template);
    FillTemplate(template, m.PersonInfos, m.Epitaph);
    console.log(template);
    template.Portraits.forEach(function (o, i) {
        html += AddLayer(o, "<img src='" + GetURL(m.PersonInfos[i].ImageId) + "' class='Portrait'>");
    });

    template.Texts.forEach(function (o, i) {
        html += AddLayer(o, "<div style='text-align:" + o.AlignStr + "; font-size:" + GetPx(o.Size) + "; " + (o.Bold ? "font-weight:bold" : "") + "'>" + o.Text + "</div>");
    });


    html += "<div><img src=\"" + GetURL(template.BgImageId) + "\" id=\"imgBack\" style=\"width: 100%; z-index: 0\" /></div>";
    $("#pMain").html(html);
}

function FillTemplate(t, pis, epitaph) {
    for (var i = 0; i < pis.length; i++) {
        var pi = pis[i];
        //t.Portraits[i].ImageId = pi.ImageId;
        for (var j = 0; j < t.Texts.length; j++) {
            var ttx = t.Texts[j];
            ttx.Text = ttx.Text.replace("{фамилия" + (i + 1) + "}", pi.F);
            ttx.Text = ttx.Text.replace("{имя" + (i + 1) + "}", pi.I);
            ttx.Text = ttx.Text.replace("{отчество" + (i + 1) + "}", pi.O);
            ttx.Text = ttx.Text.replace("{датар" + (i + 1) + "}", pi.Birth);
            ttx.Text = ttx.Text.replace("{датас" + (i + 1) + "}", pi.Dead);
            ttx.Text = ttx.Text.replace("{эпитафия}", epitaph.replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>"));
        }
    }
}

function GetURL(id) { return "/Images/GetImage/" + id; };

function GetPx(i) { return px * i + "px" }

function AddLayer(subj, html) {

    return "<div id='Layer" + subj.Num + "' style='z-index: " + subj.Num + ";width:" + GetPx(subj.Area.W) + "; margin-left:" + GetPx(subj.Area.X) + "; margin-top:" + GetPx(subj.Area.Y) + "'  class='Layer'>" + html + "</div>";
};

