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


    template.Images.forEach(function (o, i) {
        var extra = "";
        if (o.H != undefined && o.H != null) extra = "height:" + GetPx(o.H) + ";";
        html += AddLayer(o, "<img style='width:" + GetPx(o.W) + ";" + extra + "' src='" + GetURL(o.ImageId) + "'/>");
    });


    html += "<div><img src=\"" + GetURL(template.BgImageId) + "\" id=\"imgBack\" style=\"width: 100%; z-index: 0\" /></div>";
    $("#pMain").html(html);
}

function FillTemplate(t, pis, epitaph) {
    var se = t.SingleEpitaph;
    console.log(t);
    for (var i = 0; i < pis.length; i++) {
        var pi = pis[i];
        //t.Portraits[i].ImageId = pi.ImageId;
        for (var j = 0; j < t.Texts.length; j++) {
            var ttx = t.Texts[j];
            ttx.Text = ttx.Text.replace("{фамилия" + (i + 1) + "}", pi.F.toUpperCase());
            ttx.Text = ttx.Text.replace("{имя" + (i + 1) + "}", pi.I);
            ttx.Text = ttx.Text.replace("{отчество" + (i + 1) + "}", pi.O);
            ttx.Text = ttx.Text.replace("{датар" + (i + 1) + "}", pi.Birth);
            ttx.Text = ttx.Text.replace("{датас" + (i + 1) + "}", pi.Dead);
            if (se)
                ttx.Text = ttx.Text.replace("{эпитафия}",
                    epitaph.replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>"));
            else {
                ttx.Text = ttx.Text.replace("{эпитафия" + (i + 1) + "}", pi.Epitaph.replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>").replace('\n', "<br/>"));
            }
        }
    }
}

function GetURL(id) { return "/Images/GetImage/" + id; };

function GetPx(i) { return px * i + "px" }

function AddLayer(subj, html) {

    return "<div id='Layer" + subj.Num + "' style='z-index: " + subj.Num + ";width:" + GetPx(subj.Area.W) + "; margin-left:" + GetPx(subj.Area.X) + "; margin-top:" + GetPx(subj.Area.Y) + "'  class='Layer'>" + html + "</div>";
};


(function ($) {
    $.fn.imageSelector = function (images, options = {}) {
        const settings = $.extend({
            selectedUrl: null,        // начальная картинка
            onImageSelected: null     // callback(index, imageUrl)
        }, options);

        return this.each(function () {
            const $container = $(this);
            let currentIndex = 0;

            // если передан selectedUrl — ищем индекс этой картинки
            if (settings.selectedUrl) {
                const foundIndex = images.indexOf(settings.selectedUrl);
                if (foundIndex !== -1) {
                    currentIndex = foundIndex;
                }
            }

            // HTML шаблон
            const widget = $(`
        <div class="image-selector-widget">
          <div class="controls">
            <button class="btn btn-primary nav-btn prev">⟨ Назад</button>

            <div class="dropdown">
              <button class="btn btn-outline-secondary dropdown-toggle d-flex align-items-center" type="button" data-bs-toggle="dropdown">
                <img src="${images[currentIndex]}" alt="preview">
              </button>
              <ul class="dropdown-menu"></ul>
            </div>

            <button class="btn btn-primary nav-btn next">Вперёд ⟩</button>
          </div>
        </div>
      `);

            $container.html(widget);

            const $dropdownMenu = $container.find(".dropdown-menu");
            const $dropdownButton = $container.find(".dropdown-toggle");

            // Заполняем список миниатюр
            images.forEach((img, i) => {
                const $item = $(`
          <li>
            <a class="dropdown-item" href="#" data-index="${i}">
              <img src="${img}" alt="thumb"> Изображение ${i + 1}
            </a>
          </li>
        `);
                $dropdownMenu.append($item);
            });

            function updateImage(index) {
                currentIndex = index;
                $dropdownButton.find("img").attr("src", images[index]);
                // Вызов колбэка при выборе
                if (typeof settings.onImageSelected === "function") {
                    settings.onImageSelected(index, images[index]);
                }
            }

            // Обработчики
            $dropdownMenu.on("click", ".dropdown-item", function (e) {
                e.preventDefault();
                const index = parseInt($(this).data("index"));
                updateImage(index);
            });

            $container.find(".prev").on("click", function () {
                currentIndex = (currentIndex - 1 + images.length) % images.length;
                updateImage(currentIndex);
            });

            $container.find(".next").on("click", function () {
                currentIndex = (currentIndex + 1) % images.length;
                updateImage(currentIndex);
            });

            // Вызов события сразу при инициализации (если selectedUrl задан)
            if (typeof settings.onImageSelected === "function") {
                settings.onImageSelected(currentIndex, images[currentIndex]);
            }
        });
    };
})(jQuery);

                /*
$(function() {
  const images = [
    "https://picsum.photos/id/1015/800/400",
    "https://picsum.photos/id/1020/800/400",
    "https://picsum.photos/id/1035/800/400",
    "https://picsum.photos/id/1042/800/400",
    "https://picsum.photos/id/1052/800/400"
  ];

  $("#myImageWidget").imageSelector(images, {
    selectedUrl: "https://picsum.photos/id/1035/800/400",
    onImageSelected: function(index, url) {
      console.log("Выбрана картинка:", index, url);
    }
  });
});                 
            */