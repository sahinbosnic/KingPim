$(document).ready(function () {
    var api_key = "40f2eef1b2884b309ff075ca16508d76";
    $.ajax({
        url: "https://newsapi.org/v1/articles?source=the-next-web&sortBy=latest&apiKey=" + api_key,
        success: function (result) {

            $.each(result.articles/*.slice(0, 3)*/, function (key, value) {

                var article = [
                    ' <div class="card col-3 m-4" style="border:none;">',
                    '<a target="_blank" href="' + value.url + '">',
                    '<img class="card-img" src="' + value.urlToImage + '" alt="Card image cap">',
                    '<div class="card-body">',
                    '<h4>' + value.title + '</h4>',
                    '<p class="card-text">' + value.description + '</p > ',
                    '</div>',
                    '</a>',
                    '</div>'].join("");

                $(".news-feed").append(article);
            });
        }
    });
});